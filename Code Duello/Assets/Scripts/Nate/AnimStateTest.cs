using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateTest : MonoBehaviour
{
    Animator controllerAnim;

    public bool isBlocking;

    // Start is called before the first frame update
    void Start()
    {
        controllerAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
        if (Input.GetButton("Fire2"))
        {
            isBlocking = true;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isBlocking = false;
            controllerAnim.SetBool("isBlocking", isBlocking);
        }

    }

    public void Attack() 
    {
        controllerAnim.SetTrigger("Punching");
        isBlocking = false;
        controllerAnim.SetBool("isBlocking", isBlocking);
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
