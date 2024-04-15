using UnityEngine;

public static class AnimHash
{
    public static int IsWalk = Animator.StringToHash("IsWalk");
    public static int VelocityX = Animator.StringToHash("VelocityX");
    public static int VelocityZ = Animator.StringToHash("VelocityZ");
    public static int IsAiming = Animator.StringToHash("IsAiming");
    public static int SpecialIdle = Animator.StringToHash("SpecialIdle");
    public static int Dead = Animator.StringToHash("Dead");
    public static int Fire1 = Animator.StringToHash("Fire1");
    public static int Fire2 = Animator.StringToHash("Fire2");
    public static int Fire3 = Animator.StringToHash("Fire3");
}