using UnityEngine;
using System.Collections;

public class AttackPerformer : MonoBehaviour
{
    [Header("Attack Collider")]
    [SerializeField] private SphereCollider attackCollider;



    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("DamageableArea")) return;
        if (other.gameObject == this.gameObject || other.transform.root == transform.root) return; // evita colisiones consigo mismo/padre/hermanos
        if (other.transform == transform || other.transform.IsChildOf(transform.root)) return;
        if (transform.parent.gameObject.CompareTag(other.gameObject.tag)) return;// evita colisiones con mismo tipo de tag
        


        if(transform.root.gameObject.CompareTag("PlayerMinion") || transform.root.gameObject.CompareTag("Player"))
        { // evita colisiones entre minion-minion o minion-jugador
            if(other.gameObject.transform.root.gameObject.CompareTag("PlayerMinion") || other.gameObject.transform.root.gameObject.CompareTag("Player"))
            {
                return;
            }
        }

        if (other.gameObject.CompareTag("DamageableArea"))
        {
            //Debug.Log(other.gameObject.name);
            CombatSystem.AttackPerformed(attacker: transform.parent.gameObject, reciever: other.transform.root.gameObject);
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
    }
    public IEnumerator PerformAttack(float damageWindowTime)
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(damageWindowTime);
        attackCollider.enabled = false;
    }

}
