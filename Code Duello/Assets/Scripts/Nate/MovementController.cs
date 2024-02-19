using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public float m_Speed = 5f;

    [Space][Header("Attacks")][Space]
    
    [SerializeField] private int baseDamage;

    PlayerHealthClass enemyPHC = null;

    Animator controllerAnim;

    public Transform hitCastPoint;

    public bool isEnemyInRange = false;
    public float hitRange = 10f;

    public CombatClass playerCombatClass;

    public bool isBlocking;
    public bool isPunching;
    public bool playerKnockedBack = false;

    [SerializeField] private AnimationClip punchClip;
    [SerializeField] private float punchClipTime;
    [SerializeField] private float hitCheckDelay;
    [SerializeField] private float pushbackForce = 1f;

    [Space]
    [Header("DEBUG VARS")]
    [Space]

    

    public bool disablePlayerInput;
    public KeyCode lightAttackKey;
    public KeyCode blockAttackKey;



    [Space][Header("CharacterController")][Space]

    public CharacterController controller;

    public KeyCode jumpKey;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 4.5f;

    [SerializeField] private Vector3 move;

    Vector3 Velocity;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        controllerAnim = gameObject.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(hitCastPoint.position, hitCastPoint.forward, out hit, hitRange))
        {
            if (!disablePlayerInput) 
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
            if (!disablePlayerInput)
            {
                isEnemyInRange = false;
                enemyPHC = null;
                Debug.Log("Is Enemy In Range: " + isEnemyInRange);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(lightAttackKey) && !isPunching)
        {
            isPunching = true;
            Attack();
            Invoke("makePunchingFalse", punchClipTime);
            Invoke("SendHitCheck", punchClipTime);
        }
        if (Input.GetKeyDown(blockAttackKey) && !isPunching)
        {
            isBlocking = true;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }
        if (Input.GetButtonUp("Fire2") && !isPunching)
        {
            isBlocking = false;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }

        #region CC Move

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
        

        float x = Input.GetAxis("Horizontal");

        float z = 0;

        move = transform.forward * x;
        move.z = z;
        if (x < -0.3 && isGrounded && !isPunching)
        {
            WalkLeft();
        }
        else if (x > 0.3 && isGrounded && !isPunching)
        {
            WalkRight();
        }
        else if (x > -0.3 && x < 0.3 && isGrounded && !isPunching)
        {
            FightStance();
        }

        controller.Move(move * speed * Time.deltaTime);


        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        #endregion
        Velocity.y += gravity * Time.deltaTime;

        controller.Move(Velocity * Time.deltaTime);
    }
    
    public void ApplyKnockback() 
    {
        Debug.Log("Knockback - " + gameObject.name);
        Velocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);
        playerKnockedBack = true;
        Velocity.x = Mathf.Sqrt(pushbackForce * -2f * gravity);
        Invoke("ResetXVelocity", 0.3f);
    }

    private void ResetXVelocity() 
    {
        Velocity.x = 0;
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

}
