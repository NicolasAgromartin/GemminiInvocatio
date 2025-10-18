using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;




public class Enemy : Fiend
{
    private Outline outline;
    

    public bool isFinalBoos = false;



    new private void Awake()
    {
        base.Awake();
        name = data.name;
        outline = GetComponent<Outline>();
        GetComponent<BehaviorGraphAgent>().enabled = true;
    }
    private void OnEnable()
    {
        animationEvents.OnDeathAnimationEnd += MakeRemains;
    }
    private void OnDisable()
    {
        animationEvents.OnDeathAnimationEnd -= MakeRemains;
    }





    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }




    protected override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        if (Stats.CurrentHealth <= 0)
        {

        }
    }
    public void MarkEnemy()
    {
        outline.enabled = true;
    }
    public void DismarkEnemy()
    {
        outline.enabled = false;
    }





    private void MakeRemains()
    {
        tag = "Remains";
        name += " - Remains";

        GetComponent<CapsuleCollider>().enabled = false;

        Remains remains = this.AddComponent<Remains>();
        remains.SetRemainsData(data);

        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);
        Destroy(outline);
        Destroy(this);
    }

}
