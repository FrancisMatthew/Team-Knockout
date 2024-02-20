using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [Space][Header("Attacks")][Space]
    Animator controllerAnim;

    public CombatClass playerCombatClass;

    public bool isBlocking;
    public bool isPunching;

    [SerializeField] private AnimationClip punchClip;
    [SerializeField] private float punchClipTime;

    [SerializeField] private float hitCheckDelay;

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

        punchClipTime = punchClip.length;
    }


    void Update()
    {

        if (Input.GetButtonDown("Fire1") && !isPunching)
        {
            isPunching = true;
            Attack();
            Invoke("makePunchingFalse", punchClipTime);
            Invoke("SendHitCheck", hitCheckDelay);
        }
        if (Input.GetButton("Fire2") && !isPunching)
        {
            isBlocking = true;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }
        if (Input.GetButtonUp("Fire2") && !isPunching)
        {
            isBlocking = false;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = 0;

        move = transform.forward * x ;
        move.z = z;
        if(x < -0.3 && isGrounded && !isPunching) 
        {
            WalkLeft();
        }
        else if(x > 0.3 && isGrounded && !isPunching) 
        {
            WalkRight();    
        }
        else if(x > -0.3 && x < 0.3 && isGrounded && !isPunching)
        {
            FightStance();
        }

        controller.Move(move * speed * Time.deltaTime);
        
        

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Velocity.y += gravity * Time.deltaTime;

        controller.Move(Velocity * Time.deltaTime);



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
        playerCombatClass.DamageHitCkeck();
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
