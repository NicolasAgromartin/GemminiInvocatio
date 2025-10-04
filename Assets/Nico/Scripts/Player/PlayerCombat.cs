using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using System.Linq;


public class PlayerCombat : MonoBehaviour
{
    #region Events
    public event Action<int> OnBasicAttack;
    #endregion

    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemiesNearby = new();
    [SerializeField] private SphereCollider attackTrigger;
    public GameObject FocusedTarget { get; private set; }


    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyDetector detector;


    public int CurrentAttack { get; private set; }
    private int clickCounter;
    private bool canClick;
    private int lastIndexedTarget = 0;








    #region Life Cykle
    private void Awake()
    {
        animator = GetComponent<Animator>();

        clickCounter = 0;
        canClick = true;
    }
    private void OnEnable()
    {
        InputManager.OnBasicAttackPerformed += BasicAttack;
        InputManager.OnSwitchTargetButtonPressed += ChangeFocusedTarget;

        //detector.OnEnemyDetected += UpdateEnemyList;
        //detector.OnEnemyUndetected += UpdateEnemyList;
    }
    private void OnDisable()
    {
        InputManager.OnBasicAttackPerformed -= BasicAttack;
        InputManager.OnSwitchTargetButtonPressed -= ChangeFocusedTarget;

        //detector.OnEnemyDetected -= UpdateEnemyList;
        //detector.OnEnemyUndetected -= UpdateEnemyList;
    }
    #endregion



    #region Basic Attack
    private void BasicAttack()
    {
        if (canClick)
        {
            clickCounter++;
        }


        if(clickCounter == 1)
        {
            animator.SetInteger("AttackNumber", 1);
            CurrentAttack = 1;
        }
    }
    public void ComboCheck()
    {
        canClick = false;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack1") && clickCounter == 1)
        {
            animator.SetInteger("AttackNumber", 0);
            clickCounter = 0;
            canClick = true;
            CurrentAttack = 1;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack1") && clickCounter >= 2)
        {
            animator.SetInteger("AttackNumber", 2);
            canClick = true;
            CurrentAttack = 1;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack2") && clickCounter == 2)
        {
            animator.SetInteger("AttackNumber", 0);
            clickCounter = 0;
            canClick = true;
            CurrentAttack = 2;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack2") && clickCounter >= 3)
        {
            animator.SetInteger("AttackNumber", 3);
            canClick = true;
            CurrentAttack = 2;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack3"))
        {
            animator.SetInteger("AttackNumber", 0);
            clickCounter = 0;
            canClick = true;
            CurrentAttack = 3;
        }
    }
    public void ActivateAttackTrigger()
    {
        OnBasicAttack?.Invoke(CurrentAttack);
        attackTrigger.enabled = true;
    }
    public void DeactivateAttackTrigger()
    {
        attackTrigger.enabled = false;
    }
    #endregion


    private void UpdateEnemyList(GameObject enemy)
    {
        if (enemiesNearby.Contains(enemy))
        {
            enemiesNearby.Remove(enemy);
        }
        else
        {
            enemiesNearby.Add(enemy);
        }
    }
    private void ChangeFocusedTarget()
    {
        if (enemiesNearby.Count == 0) return;

        lastIndexedTarget++;

        if (lastIndexedTarget >= enemiesNearby.Count)
        {
            lastIndexedTarget = 0;
        }

        FocusedTarget = enemiesNearby[lastIndexedTarget];
    }
}

