using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Events.ScriptableObjects;

public class Enemy : NPC
{

    [Header("Broadcasting on")]
    public IntEventChannelSO onEnemyAttacked;
    public struct EnemyStats
    {
        public string Type;
        public int AC;
        public int SimpleDamage;
        public float AwakeDistance;
        public float WeaponRange;
        public float AttackCooldown;
        public float HP;
    }

    protected float decisionRate = 2.0f;
    protected NavMeshAgent agent;
    public GameObject Target { get; set; }

    public EnemyStats enemyStats;

    public Func<int> DmgRoll;


    // Start is called before the first frame update
    void Start()
    {
        this.Target = GameObject.FindGameObjectWithTag("Player");
        InitializeBehaviourTree();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.currBehaviorTree != null)
            this.currBehaviorTree.Run();
    }

    public virtual void InitializeBehaviourTree()
    {
        // TODO but in the children's class
    }

    void CheckPlayerPosition()
    {
        if (Vector3.Distance(this.transform.position, this.Target.transform.position) < enemyStats.AwakeDistance)
        {

            if (Vector3.Distance(this.transform.position, this.Target.transform.position) <= enemyStats.WeaponRange)
            {
                AttackPlayer();
            }

            else
            {
                PursuePlayer();
                Invoke("CheckPlayerPosition", 0.5f);
            }
        }
        else
        {
            Invoke("CheckPlayerPosition", 3.0f);
        }
    }

    public void PursuePlayer()
    {
        if (agent != null)
            this.agent.SetDestination(this.Target.transform.position);
    }

    public void AttackPlayer()
    {
        if (onEnemyAttacked != null)
            onEnemyAttacked.RaiseEvent(this.enemyStats.SimpleDamage);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (agent != null)
            this.agent.SetDestination(targetPosition);
    }
}
