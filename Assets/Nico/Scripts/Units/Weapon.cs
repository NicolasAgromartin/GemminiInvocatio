using UnityEngine;




public class Weapon : MonoBehaviour
{
    public int Damage = 10;
    public bool IsEnemy => isEnemy;
    [SerializeField] bool isEnemy = false;
    [SerializeField] private LayerMask damageableLayer;
    private GameObject impacted;





    private void Awake()
    {
        //Damage = parent.GetComponent<Unit>().Stats.Attack;
        //Damage = 10;
    }





    private void OnTriggerEnter(Collider other)
    {
        //if (((1 << other.gameObject.layer) & damageableLayer.value) == 0) return; // si el other esta dentro de la layer damageable

        //impacted = other.transform.parent.gameObject;


        //if (!isEnemy)
        //{
        //    if (impacted.transform.parent.CompareTag("Enemy"))
        //    {
        //        Debug.Log("Attacked an enemy");
        //        impacted.transform.parent.GetComponent<Unit>().RecieveDamage(Damage);
        //    }
        //}
        //else
        //{
        //    if (impacted.CompareTag("Player") || impacted.CompareTag("PlayerMinion"))
        //    {
        //        impacted.GetComponent<Unit>().RecieveDamage(Damage);
        //    }
        //}
    }









    public void SetIsEnemy(bool isEnemy) => this.isEnemy = isEnemy;

}
