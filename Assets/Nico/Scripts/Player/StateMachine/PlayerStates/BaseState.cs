using System;
using System.Collections.Generic;
using UnityEngine;





public abstract class BaseState
{
    public abstract event Action<TransitionEvent> OnEventOccurred;

    protected PlayerStateMachine stateMachine;
    protected PlayerContext playerContext;

    protected Stats stats;

    protected Animator animator;
    protected Inventory inventory;
    protected Transform transform;
    protected Necromancy necromancy;
    protected MinionOwner minionOwner;
    protected EnemyDetector enemyDetector;
    protected AnimationEvents animationEvents;
    protected CharacterController characterController;

    protected CinemachineController cinemachineController;

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
        animationEvents = playerContext.AnimationEvents;
        characterController = playerContext.CharacterController;
        cinemachineController =playerContext.CinemachineController;
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
        detected = null;

        //Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * 1.5f + Vector3.up * 1f, 
        //    new Vector3(1f, 2.5f, 1f), transform.rotation, LayerMask.GetMask("Interactable"));

        Vector3 center = transform.TransformPoint(new Vector3(0.0175f, 0.9129f, 0.1656f));
        Vector3 halfExtents = new Vector3(1.0662f, 1.8248f, 0.4272f) * 0.5f;

        Collider[] colliders = Physics.OverlapBox(
            center,
            halfExtents,
            transform.rotation,
            LayerMask.GetMask("Interactable")
        );

        foreach (Collider collider in colliders)
        {
            detected = collider.gameObject;

            if (detected.GetComponent<IInteractable>() == null) return;

            detected.GetComponent<IInteractable>().Interact(transform.gameObject);
        }
    }
    
}



