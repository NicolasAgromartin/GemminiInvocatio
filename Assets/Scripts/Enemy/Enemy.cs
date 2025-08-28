using Unity.Behavior;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public class Enemy : Fiend
{
    public delegate void EnemyEvent();
    public event EnemyEvent OnDamageRecievd;



    new private void Awake()
    {
        base.Awake();

    }

    /*
     * 
        tipo de unidad    

        vida
        defensa
        ataque
        
        velocidad de movimiento
        rango de ataque
        tiempo entre ataques

        tiempo entre patrullaje
        zonas de patrullaje
        
     */

    /*
     cuando el enemigo muere
       
        remuevo este script, el behavior agent, el targets detector
        y le agrego un script de remains, seteando sus valores segun los valores del enemigo muerto (cruzando los datos con un diccionario)


        

     */


    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        OnDamageRecievd?.Invoke();

        if(Stats.Health <= 0)
        {
            Debug.Log("Ded");
            MakeRemains();
        }
    }

    private void MakeRemains()
    {
        tag = "Remains";
        name += " - Remains";
        Destroy(GetComponent<BehaviorGraphAgent>());
        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);
        Remains remains = this.AddComponent<Remains>();
        remains.SetRemainsData(data);
        Destroy(this);
    }
}
