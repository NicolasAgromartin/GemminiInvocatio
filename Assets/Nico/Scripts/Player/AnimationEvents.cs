using System;
using UnityEngine;





public class AnimationEvents : MonoBehaviour
{
    public event Action OnDeathAnimationEnd;
    public event Action OnHurtAnimationEnd;
    public event Action OnAttackAnimationEnd;
    public event Action OnAttackAnimationStarts;
    public event Action OnResurrectAnimationEnd;
    public event Action OnComboEnabled;

    private SphereCollider attackCollider;
    private GameObject parent;




    private void Awake()
    {
        attackCollider = GetComponentInChildren<SphereCollider>();
        parent = transform.parent.gameObject;

        //Debug.Log(attackCollider.gameObject.name);
    }




        
    public void DeathAnimationEnd() => OnDeathAnimationEnd?.Invoke();
    public void HurtAnimationEnd() => OnHurtAnimationEnd?.Invoke();
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
        if (parent.CompareTag("Player")) OnComboEnabled?.Invoke();
    }
    public void AttackAnimationStarts() => OnAttackAnimationStarts?.Invoke();
    public void AttackAnimationEnd() => OnAttackAnimationEnd?.Invoke();
    public void ResurrectAnimationEnd() => OnResurrectAnimationEnd?.Invoke();
}
