using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;





public abstract class BaseState
{
    public virtual event Action<TransitionEvent> OnEventOccurred;

    protected PlayerStateMachine stateMachine;
    protected PlayerContext playerContext;

    protected Stats stats;

    protected Animator animator;
    protected Inventory inventory;
    protected Transform transform;
    protected Necromancy necromancy;
    protected MinionOwner minionOwner;
    protected EnemyDetector enemyDetector;
    protected AttackPerformer attackPerformer;
    protected CharacterController characterController;

    protected GameObject detected;



    public BaseState(PlayerStateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
        playerContext = stateMachine.PlayerContext;

        animator = playerContext.Animator;
        transform = playerContext.Transform;
        inventory = playerContext.Inventory;
        necromancy = playerContext.Necromancy;
        minionOwner = playerContext.MinionOwner;
        enemyDetector = playerContext.EnemyDetector;
        attackPerformer = playerContext.AttackPerformer;
        characterController = playerContext.CharacterController;
        stats = playerContext.Stats;
    }




    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public virtual void OnTriggerEnter(Collider other) { }



    protected void UsePotion()
    {
        if (stats.CurrentHealth == stats.MaxHealth) return;

        List<Item> potions = Inventory.GetItems(ItemType.Potion);
        if (potions.Count > 0)
        {
            potions[0].Use(transform.GetComponent<Player>());
            inventory.RemoveItem(ItemType.Potion, potions[0]);
        }
    }
    
    protected void Interact()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(0f, 1f, .5f), new Vector3(1f, 2.5f, 1f), 
            Quaternion.identity, LayerMask.GetMask("Interactable"));

        foreach (Collider collider in colliders)
        {
            //GameObject detected = collider.transform.root.gameObject;
            detected = collider.gameObject;


            if (detected.GetComponent<IInteractable>() == null) return;

            detected.GetComponent<IInteractable>().Interact(transform.gameObject);



        }
    }
    
}



