using UnityEngine;


public class PlayerContext 
{
    public Stats Stats { get; private set; }
    public Animator Animator { get; private set; }
    public Inventory Inventory { get; private set; }
    public Transform Transform { get; private set; }
    public Necromancy Necromancy { get; private set; }
    public MinionOwner MinionOwner { get; private set; }
    public EnemyDetector EnemyDetector {  get; private set; }
    public AnimationEvents AnimationEvents { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public CinemachineController CinemachineController { get; private set; }

    public float MovementSpeed { get; private set; } = 6f;
    public float RotationSpeed { get; private set; } = 4f;





    public PlayerContext(CharacterController characterController, Animator animator, Transform transform, Inventory inventory, Necromancy necromancy,
        EnemyDetector enemyDetector, MinionOwner minionOwner, AnimationEvents animationEvents, Stats stats, CinemachineController cinemachineController)
    {
        Animator = animator;
        Inventory = inventory;
        Transform = transform;
        Necromancy = necromancy;
        EnemyDetector = enemyDetector;
        CharacterController = characterController;
        MinionOwner = minionOwner;
        AnimationEvents = animationEvents;
        Stats = stats;
        CinemachineController = cinemachineController;
    }


}
