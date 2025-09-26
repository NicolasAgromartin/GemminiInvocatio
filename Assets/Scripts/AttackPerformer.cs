using UnityEngine;
using System.Collections;

public class AttackPerformer : MonoBehaviour
{
    [Header("Attack Collider")]
    [SerializeField] private SphereCollider attackCollider;

    [SerializeField] private LayerMask damageableLayer;
    

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & damageableLayer.value) == 0) return; // si el other esta dentro de la layer damageable


        if (transform.root.CompareTag("Player") || transform.root.CompareTag("PlayerMinion"))
        {
            Debug.Log($"{transform.root.gameObject.name} is attacking");
            // comparo con damageableArea de enemy unicamente
            if (other.transform.root.CompareTag("Enemy"))
            {
                Debug.Log("An enemy");

                CombatSystem.AttackPerformed(attacker: transform.parent.gameObject, reciever: other.transform.root.gameObject);
            }
        }
        else
        {
            // comparo con damageableArea de player y playerMinion
            if (other.transform.root.CompareTag("Player") || other.transform.root.CompareTag("PlayerMinion"))
            {
                CombatSystem.AttackPerformed(attacker: transform.parent.gameObject, reciever: other.transform.root.gameObject);
            }
        }

        //if (!other.gameObject.CompareTag("DamageableArea")) return; // 
        //if (other.gameObject == gameObject || other.transform.root == transform.root) return; // evita colisiones consigo mismo/padre/hermanos
        //if (other.transform == transform || other.transform.IsChildOf(transform.root)) return;
        //if (transform.parent.gameObject.CompareTag(other.gameObject.tag)) return;// evita colisiones con mismo tipo de tag



        //if (transform.root.gameObject.CompareTag("PlayerMinion") || transform.root.gameObject.CompareTag("Player"))
        //{ // evita colisiones entre minion-minion o minion-jugador
        //    if (other.gameObject.transform.root.gameObject.CompareTag("PlayerMinion") || other.gameObject.transform.root.gameObject.CompareTag("Player"))
        //    {
        //        return;
        //    }
        //}

        //if (other.gameObject.CompareTag("DamageableArea"))
        //{
        //    //Debug.Log(other.gameObject.name);
        //    CombatSystem.AttackPerformed(attacker: transform.parent.gameObject, reciever: other.transform.root.gameObject);
        //}
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
