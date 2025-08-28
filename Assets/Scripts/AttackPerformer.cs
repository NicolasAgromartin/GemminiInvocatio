using UnityEngine;
using System.Collections;

public class AttackPerformer : MonoBehaviour
{
    [Header("Attack Collider")]
    [SerializeField] private SphereCollider attackCollider;

    private bool isAttacking;

    // las primeras dos se ejecutan desde animation events del player
    public void EnableAttackArea() => attackCollider.enabled = true;
    public void DisableAttackArea() => attackCollider.enabled = false;
    public IEnumerator PerformAttack(float damageWindowTime)
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(damageWindowTime);
        attackCollider.enabled = false;
    }



    // detecta la colision en 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.gameObject || other.transform.root == transform.root) return; // evita colisiones consigo mismo/padre/hermanos
        if (other.transform == transform || other.transform.IsChildOf(transform.root)) return;

        if (transform.parent.gameObject.CompareTag(other.gameObject.tag)) return;// evita colisiones con mismo tipo de tag

        if(transform.root.gameObject.CompareTag("PlayerMinion") || transform.root.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.transform.root.gameObject.CompareTag("PlayerMinion") || other.gameObject.transform.root.gameObject.CompareTag("Player"))
            {
                return;
            }
        }

        if (other.gameObject.CompareTag("DamageableArea"))
        {
            CombatSystem.AttackPerformed(attacker: transform.parent.gameObject, reciever: other.transform.root.gameObject);
        }
    }


}
