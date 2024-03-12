using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EthanMovementController : MonoBehaviour
{
    private PlayerControls playerControls;

    public bool invertContorls = false;

    Rigidbody m_Rigidbody;
    public float m_Speed = 5f;
    public Vector3 movement;
    public float gravityFactor;

    public static bool isGameActive = false;

    [Space][Header("Attacks")][Space]

    public bool arePlayersColliding = false;

    [SerializeField] private int baseDamage;

    PlayerHealthClass enemyPHC = null;

    Animator controllerAnim;

    public Transform hitCastPoint;

    public float playerMoveX;

    public bool isEnemyInRange = false;
    public float hitRange = 10f;
    public float overlapRange = 0.4f;

    public CombatClass playerCombatClass;

    public bool isBlocking;
    public bool isPunching;
    public bool playerKnockedBack = false;

    [SerializeField] private AnimationClip punchClip;
    [SerializeField] private float punchClipTime;
    [SerializeField] private float hitCheckDelay;
    [SerializeField] private float pushbackForce = 1f;

    [Space][Header("DEBUG VARS")][Space]

    public bool disablePlayerInput;
    public KeyCode lightAttackKey;
    public KeyCode blockAttackKey;



    [Space][Header("CharacterController")][Space]

    public CharacterController controller;

    public KeyCode jumpKey;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [SerializeField] private Vector3 move;

    Vector3 Velocity;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        playerControls = new PlayerControls();
        controllerAnim = gameObject.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        punchClipTime = punchClip.length;

        // Seems to make character go the other way if they're player two
        if (invertContorls) 
        { 
            controllerAnim.SetFloat("walkLeftSpeed", -1);
            controllerAnim.SetFloat("walkRightSpeed", -1);
        }
    }

    private void FixedUpdate()
    {
        if (!disablePlayerInput)
        {
            RaycastHit overlap;
            if (Physics.Raycast(hitCastPoint.position, hitCastPoint.forward, out overlap, overlapRange))
            {
                if (overlap.transform.gameObject.tag == "Player")
                {
                    arePlayersColliding = true;
                }
            }
            else
            {
                arePlayersColliding = false;

            }
        }

        if (!disablePlayerInput) 
        {
            RaycastHit hit;
            if (Physics.Raycast(hitCastPoint.position, hitCastPoint.forward, out hit, hitRange))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    isEnemyInRange = true;
                    if (enemyPHC == null)
                    {
                        enemyPHC = hit.transform.gameObject.GetComponent<PlayerHealthClass>();
                    }
                    Debug.Log("Is Enemy In Range: " + isEnemyInRange + " - " + gameObject.name);
                }
            }
            else
            {
                isEnemyInRange = false;
                enemyPHC = null;
                Debug.Log("Is Enemy In Range: " + isEnemyInRange);
            }
        } 
    }

    void Update()
    {
        if (isGameActive)
        {
            

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }

            move = transform.forward * playerMoveX;

            // Changed
            // Call for lateral movement
            if (isGrounded)
            {
                controllerAnim.SetFloat("WalkDirection", playerMoveX);
            }

            else if (playerMoveX > -0.3 && playerMoveX < 0.3 && isGrounded && !isPunching)
            {
                FightStance();
            }

            if (invertContorls)
            {
                if (!arePlayersColliding)
                {
                    controller.Move(-move * speed * Time.deltaTime);
                }
                else if (arePlayersColliding && playerMoveX > 0)
                {
                    controller.Move(-move * speed * Time.deltaTime);
                }
            }
            else
            {
                if (!arePlayersColliding)
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
                else if (arePlayersColliding && playerMoveX < 0)
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
            }
        }
        
        Velocity.y += gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }
    
    private void OnJump()
    {
        Debug.Log("Jump");
        if (isGrounded)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void OnMove(InputValue inputValue)
    {
        Vector2 inputValVec = inputValue.Get<Vector2>();
        playerMoveX = inputValVec.x;
    }

    private void OnLightAttack() 
    {
        Debug.Log("LightAttackCont");
        if (!isPunching)
        {
            isPunching = true;
            Attack();
            Invoke("makePunchingFalse", punchClipTime);
            Invoke("SendHitCheck", hitCheckDelay);
        }
    }

    private void OnBlock(InputValue inputValue) 
    {
        if (inputValue.Get<float>() > 0) 
        {
            isBlocking = true;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }

        else 
        {
            isBlocking = false;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }

    }

    public void ApplyKnockback() 
    {
        if (invertContorls) 
        {
            Debug.Log("Knockback - " + gameObject.name);
            Velocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);
            playerKnockedBack = true;
            Velocity.x = Mathf.Sqrt(pushbackForce * -2f * gravity);
            Invoke("ResetXVelocity", 0.3f);
        }
        else 
        {
            Debug.Log("Knockback - " + gameObject.name);
            Velocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);
            playerKnockedBack = true;
            Velocity.x = -Mathf.Sqrt(pushbackForce * -2f * gravity);
            Invoke("ResetXVelocity", 0.3f);
        }
        
    }

    private void ResetXVelocity() 
    {
        Velocity.x = 0;
        playerKnockedBack = false;
    }

    public void DamageAnim() 
    {
        controllerAnim.SetTrigger("TakeDamage");
    }

    public void FightStance() 
    {
        controllerAnim.SetBool("isWalkingRight", false);

        controllerAnim.SetBool("isWalkingLeft", false);
    }
    public void WalkLeft()
    {
        controllerAnim.SetBool("isWalkingLeft", true);
    }

    public void WalkRight()
    {
        controllerAnim.SetBool("isWalkingRight", true);
    }

    public void Attack()
    {
        controllerAnim.SetTrigger("Punching");
        isBlocking = false;
        controllerAnim.SetBool("isBlocking", isBlocking);
    }


    public void makePunchingFalse() 
    {
        isPunching = false;
    }

    public void SendHitCheck() 
    {
        if(enemyPHC != null)
        {
            if (isGrounded) 
            {
                enemyPHC.TakeDamage(HitType.Chest, baseDamage);
            }
            else
            {
                enemyPHC.TakeDamage(HitType.Head, baseDamage);
            }
        }
        else 
        {
            return;
        }
    }

    public void SetBlocking()
    {
        if (isBlocking)
        {
            isBlocking = false;
        }
        else
        {
            isBlocking = true;
        }
        controllerAnim.SetBool("isBlocking", isBlocking);
    }
    public void KillPlayer() 
    {
        isGameActive = false;
        controllerAnim.SetBool("isDead", true);
    }

}