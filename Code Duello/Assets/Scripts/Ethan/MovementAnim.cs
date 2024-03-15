using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnim : MonoBehaviour
{
    private Vector3 previousPos;
    private Vector3 velocity;

    private float lerpedVel;

    public Animator chaAnimator;

    private void Awake()
    {
        chaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the movement direction of the character.
        // x = horizontal
        // y = vertical
        velocity = (transform.position - previousPos) / Time.deltaTime;
        previousPos = transform.position;

        lerpedVel = Mathf.Lerp(lerpedVel, velocity.normalized.x, 10f * Time.deltaTime);
        Debug.Log(lerpedVel);

        chaAnimator.SetFloat("E_LateralDir", lerpedVel);
    }
}
