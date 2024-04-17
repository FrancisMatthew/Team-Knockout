using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwapAnimations : MonoBehaviour
{
    public ScriptableAnimation characterAnims;

    public Weapon[] weapons;

    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    protected int weaponIndex;

    protected AnimationOverrides clipOverrides;
    public void Start()
    {
        animator = GetComponent<Animator>();
        weaponIndex = 0;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);


        clipOverrides["LightBase-T"] = characterAnims.lightAttackClip;
        clipOverrides["HeavyBase-T"] = characterAnims.heavyAttackClip;
        clipOverrides["BlockBase-T"] = characterAnims.blockClip;
        clipOverrides["DeathBase-T"] = characterAnims.deathClip;
        clipOverrides["ImpactBase-T"] = characterAnims.takeDamageClip;
        clipOverrides["WalkFowardBase-T"] = characterAnims.walkFowardClip;
        clipOverrides["WalkBackBase-T"] = characterAnims.walkBackClip;
        clipOverrides["IdleBase-T"] = characterAnims.idleClip;
        animatorOverrideController.ApplyOverrides(clipOverrides);

        MovementController mc = GetComponent<MovementController>();

        mc.attackSound = characterAnims.attackSound;
        mc.takeDamageSound = characterAnims.impactSound;
        //mc.lightAttackClipTime = characterAnims.lightAttackClip.length;
        //mc.heavyAttackClipTime = characterAnims.heavyAttackClip.length;
    }


}
public class AnimationOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

public class Weapon
{
    public AnimationClip singleAttack;
    public AnimationClip comboAttack;
    public AnimationClip dashAttack;
    public AnimationClip smashAttack;
}


