using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class EnemyContext
{
    public Stats Stats { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Transform { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public TargetsDetector TargetsDetector { get; private set; }
    public AnimationEvents AnimationEvents { get; private set; }
    public List<Transform> PatrollingPoints { get; private set; }   


    public float PatrolSpeed { get; private set; }
    public float ChaseSpeed { get; private set; }





    public EnemyContext(Animator animator, Transform transform, AnimationEvents animationEvents, Stats stats,
        TargetsDetector targetsDetector, NavMeshAgent agent, List<Transform> patrollingPoints, float patrolSpeed, float chaseSpeed)
    {
        Stats = stats;
        Animator = animator;
        NavMeshAgent = agent;
        Transform = transform;
        AnimationEvents = animationEvents;
        TargetsDetector = targetsDetector;
        PatrollingPoints = patrollingPoints;

        PatrolSpeed = patrolSpeed;
        ChaseSpeed = chaseSpeed;
    }

}
