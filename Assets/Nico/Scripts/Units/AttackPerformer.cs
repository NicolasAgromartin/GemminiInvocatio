using UnityEngine;
using System.Collections;
using System;

public class AttackPerformer : MonoBehaviour
{
    
    public event Action OnAttackEnded;
    public event Action OnComboEnabled;
    public event Action OnHurtEnded;
    public static event Action OnDeathAnimationEnd;

    public int Damage { get; private set; }
    private GameObject parent;
    private GameObject impacted;
    private SphereCollider attackCollider;

    [SerializeField] private LayerMask damageableLayer;






    private void Awake()
    {
        attackCollider = GetComponent<SphereCollider>();
        parent = transform.root.gameObject;

        //Damage = parent.GetComponent<Unit>().Stats.Attack;
        Damage = 10;
    }





    // reproducir sfx (woosh y si impacta, de impacto)
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & damageableLayer.value) == 0) return; // si el other esta dentro de la layer damageable

        impacted = other.transform.root.gameObject;

        //Debug.Log(impacted.name);

        if (parent.CompareTag("Player") || parent.CompareTag("PlayerMinion"))
        {
            if (impacted.CompareTag("Enemy"))
            {
                Debug.Log("Attacked an enemy");
                impacted.GetComponent<Unit>().RecieveDamage(Damage);
            }
        }
        else
        {
            if (impacted.CompareTag("Player") || impacted.CompareTag("PlayerMinion"))
            {
                impacted.GetComponent<Unit>().RecieveDamage(Damage);
            }
        }

    }




    // las primeras dos se ejecutan desde animation events del player
    public void EnableAttackArea()
    {
        attackCollider.enabled = true;
    }
    public void DisableAttackArea()
    {
        attackCollider.enabled = false;
        OnComboEnabled?.Invoke();
    }
    public void EndAttack()
    {
        OnAttackEnded?.Invoke();
    }



    public IEnumerator PerformAttack(float damageWindowTime)
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(damageWindowTime);
        attackCollider.enabled = false;
    }


    #region Animations End
    public void EndHurtAnimation() => OnHurtEnded?.Invoke();
    public void EndDeathAnimation() => OnDeathAnimationEnd?.Invoke();
    #endregion
}
