using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Gamepad/GPControls")] [SerializeReference] [Space]

    [SerializeField] private Gamepad playerGP;
    public int playerVal;
    public bool invertContorls = false;
    public static bool isGameActive = false;
    private Coroutine stopRumbleAfterTimeCoroutine;

    [Header("Attacks/Animations")][Space]

    public bool isBlocking;
    public bool arePlayersColliding = false;
    [SerializeField] private int baseDamage;
    [SerializeField] private PlayerHealthClass enemyPHC = null;
    [SerializeField] private Animator controllerAnim;
    [SerializeField] private Transform hitCastPoint;
    [SerializeField] private float playerMoveX;
    [SerializeField] private bool isEnemyInRange = false;
    [SerializeField] private float hitRange = 10f;
    [SerializeField] private float overlapRange = 0.4f;
    [SerializeField] private bool isPunching;
    [SerializeField] private bool playerKnockedBack = false;
    [SerializeField] private AnimationClip punchClip;
    [SerializeField] private AnimationClip walkFowardClip;
    [SerializeField] private AnimationClip walkBackClip;
    [SerializeField] private float punchClipTime;
    [SerializeField] private float hitCheckDelay;
    [SerializeField] private float pushbackForce = 1f;

    [Header("Audio")][Space]

    [SerializeField] private AudioSource speaker;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackVolume;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private float takeDamageVolume;
    private Coroutine playSoundAfterTimeCoroutine;

    [Header("CharacterController")][Space]

    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Vector3 move;
    [SerializeField] private Vector3 Velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Debug Vars")][Space]

    [SerializeField] private bool disablePlayerInput_Debug = false;

    void Start()
    {
        speaker = gameObject.GetComponent<AudioSource>();
        playerGP = Gamepad.all[playerVal];
        controllerAnim = gameObject.GetComponent<Animator>();
        controllerAnim.SetBool("isGameActive", true);
        
        punchClipTime = punchClip.length;
        if (invertContorls) 
        { 
            controllerAnim.SetFloat("walkLeftSpeed", -1);
            controllerAnim.SetFloat("walkRightSpeed", -1);
        }
    }

    private void FixedUpdate()
    {
        if (!disablePlayerInput_Debug)
        {
            RaycastHit overlap;
            if (Physics.Raycast(hitCastPoint.position, hitCastPoint.forward, out overlap, overlapRange))                        //Raycast checks to see if the players are colliding
            {
                if (overlap.transform.gameObject.tag == "Player")
                {
                    arePlayersColliding = true;
                    Debug.Log("Colliding");
                }
            }
            else
            {
                arePlayersColliding = false;
            }
        }

        if (!disablePlayerInput_Debug) 
        {
            RaycastHit hit;
            if (Physics.Raycast(hitCastPoint.position, hitCastPoint.forward, out hit, hitRange))                // Raycast checks to see if player is 
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    isEnemyInRange = true;
                    if (enemyPHC == null)
                    {
                        enemyPHC = hit.transform.gameObject.GetComponent<PlayerHealthClass>();
                    }
                }
            }
            else
            {
                isEnemyInRange = false;
                enemyPHC = null;
            }
        } 
    }

    void Update()
    {
        if (isGameActive)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);                 // Checks if player is Grounded

            if (isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;                                                                               // applies gravity if player is not grounded
            }

            move = transform.forward * playerMoveX;                                                             // moves player


            if (playerMoveX < -0.3 && isGrounded && !isPunching)
            {
                WalkLeft();
            }
            else if (playerMoveX > 0.3 && isGrounded && !isPunching)
            {
                WalkRight();
            }
            else if (playerMoveX > -0.3 && playerMoveX < 0.3 && isGrounded && !isPunching)
            {
                FightStance();
            }

            if (invertContorls)
            {
                if (!arePlayersColliding)                                                                       // If players are colliding then the player can only walk backwards so that they can't push the other player
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
                if (!arePlayersColliding)                                                                       // If players are colliding then the player can only walk backwards so that they can't push the other player
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
                else if (arePlayersColliding && playerMoveX < 0)
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
            }
        }
        else 
        {
            if (controllerAnim.GetBool("isGameActive") == true)                                                 // When game finishes a check is run to see if the isGameActive animation bool is true and sets it to false if it is true
            {
                controllerAnim.SetBool("isGameActive", false);
                FightStance();                                                                                  // sets the player anim back to the fight stance
            }
            
        }
        Velocity.y += gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }
    
    private void OnJump()                                                                                       // OnJump function from the input system
    {
        if (isGrounded && isGameActive)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void OnMove(InputValue inputValue)                                                                  // OnMove function from the input system
    {
        Vector2 inputValVec = inputValue.Get<Vector2>();                                                        // Gets controller joystick input
        playerMoveX = inputValVec.x;
    }

    private void OnLightAttack()                                                                                // OnLightAttack function from the input system
    {
        if (!isPunching && isGameActive)
        {
            isPunching = true;                                                                                  // Sets isPunching bool to true so players can't repeatedly spam light Attacks
            Attack();                                                                                           // Triggers Attack animation function
            playSoundAfterTimeCoroutine = StartCoroutine(PlaySound(hitCheckDelay, attackVolume, attackSound));  // Plays sound after a specific amount of time
            Invoke("makePunchingFalse", punchClipTime);                                                         // Triggers makePunchingFalse function after a specific amount of time
            Invoke("SendHitCheck", hitCheckDelay);                                                              // Triggers SendHitCheck function after a specific amount of time
        }
    }

    private void OnBlock(InputValue inputValue)                                                                 // OnBlock function from the input system
    {
        if (inputValue.Get<float>() > 0 && isGameActive) 
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

    public void ApplyKnockback()                                                                                // OnLightAttack function from the input system
    {
        if (invertContorls)                                                                                     // Checks if conrtols are inverted
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);                                              // applys knockback to player getting hit
            playerKnockedBack = true;
            Velocity.x = Mathf.Sqrt(pushbackForce * -2f * gravity);
            Invoke("ResetXVelocity", 0.3f);
        }
        else 
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);
            playerKnockedBack = true;
            Velocity.x = -Mathf.Sqrt(pushbackForce * -2f * gravity);
            Invoke("ResetXVelocity", 0.3f);
        }
    }

    private void ResetXVelocity()                                                                               // Resets X velocity after player is knocked back
    {
        Velocity.x = 0;
        playerKnockedBack = false;
    }

    public void DamageAnim()                                                                                                           
    {
        controllerAnim.SetTrigger("TakeDamage");                                                                // Triggers TakeDamage animation
        playSoundAfterTimeCoroutine = StartCoroutine(PlaySound(0, takeDamageVolume, takeDamageSound));          // Plays sound after certain amount of time       
        RumblePulse(0.7f, 0.7f, 0.3f);                                                                          // Triggers function to vibrate controller       
    }

    public void FightStance()                                                                                   // Activates fighStance animation       
    {
        controllerAnim.SetBool("isWalkingRight", false);
        controllerAnim.SetBool("isWalkingLeft", false);
    }
    public void WalkLeft()                                                                                       // Activates WalkLeft animation    
    {
        controllerAnim.SetBool("isWalkingLeft", true);
    }

    public void WalkRight()                                                                                      // Activates WalkRight animation    
    {
        controllerAnim.SetBool("isWalkingRight", true);
    }

    public void Attack()                                                                                         // Activates Attack animation    
    {
        controllerAnim.SetTrigger("Punching");
        isBlocking = false;
        controllerAnim.SetBool("isBlocking", isBlocking);
    }


    public void makePunchingFalse() 
    {
        isPunching = false;
    }

    public void SendHitCheck()                                                                                  // If other player is in range then they'll take damage depending on the type of hit    
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

    public void SetBlocking()                                                                                  // sets blocking to true or flase   
    {
        if (isBlocking)
        {
            isBlocking = false;
        }
        else
        {
            isBlocking = true;
        }
        controllerAnim.SetBool("isBlocking", isBlocking);                                                    // Sets the aniomation bool to the same as isBlocking                                                       
    }


    public void KillPlayer()                                                                                 // Sets isGameActive to false and the isDead animation bool to true
    {
        isGameActive = false;
        controllerAnim.SetBool("isDead", true);
    }

    private void RumblePulse(float lowFrequency, float highFrequency, float duration)                       // Starts vibrating controller and starts coroutine to stop the vibration after a certain time
    {
        playerGP.SetMotorSpeeds(lowFrequency, highFrequency);
        stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumble(0.3f));
    }

    private IEnumerator StopRumble(float duration)                                                         // stop the vibration after a certain time
    {
        float elapsedTime = 0f;
        while(elapsedTime < duration) 
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerGP.SetMotorSpeeds(0f, 0f);
    }
 
    private IEnumerator PlaySound(float delay, float volume,AudioClip clip)                             // Plays a certain sound at the desired volume after a certain time
    {
        float elapsedTime = 0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        speaker.clip = clip;
        speaker.volume = volume;
        speaker.Play();
    }
}
