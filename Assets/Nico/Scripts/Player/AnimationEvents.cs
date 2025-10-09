using System;
using UnityEngine;





public class AnimationEvents : MonoBehaviour
{
    public static event Action OnDeathAnimationEnd;
    public static event Action OnHurtAnimationEnd;

    public void DeathAnimationEnd() => OnDeathAnimationEnd?.Invoke();
    public void HurtAnimationEnd() => OnHurtAnimationEnd?.Invoke();
}
