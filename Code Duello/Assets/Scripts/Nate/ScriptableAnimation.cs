using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ScriptableAnimation", menuName = "ScriptableObjects/CreateNewScriptableAnimation")]

public class ScriptableAnimation : ScriptableObject
{
    public AnimationClip lightAttackClip;
    public AnimationClip takeDamageClip;
    public AnimationClip heavyAttackClip;
    public AnimationClip walkFowardClip;
    public AnimationClip walkBackClip;
    public AnimationClip blockClip;
    public AnimationClip deathClip;
    // public AnimationClip jumpClip;
    // public AnimationClip idleClip????;
}
