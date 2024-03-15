using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnim : MonoBehaviour
{
    private Vector3 previousPos;
    private Vector3 velocity;

    private float lerpedVel;

    public Animator chaAnimator;
    public MovementController chaMove;

    private void Awake()
    {
        chaAnimator = GetComponent<Animator>();
        chaMove = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the movement direction of the character.
        // x = horizontal
        // y = vertical
        velocity = (transform.position - previousPos) / Time.deltaTime;
        previousPos = transform.position;

        // Invert for player 2
        if (chaMove.invertContorls == true)
        {
            velocity.x = -velocity.x;
        }

        lerpedVel = Mathf.Lerp(lerpedVel, velocity.normalized.x, 10f * Time.deltaTime);
        
        chaAnimator.SetFloat("E_LateralDir", lerpedVel);
    }
}
