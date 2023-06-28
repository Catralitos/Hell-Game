using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Enemies;
using Gameplay;

public class Enemy : NPC
{
    public struct EnemyInfo
    {
        public string Type;
        public float awakeDistance;
    }

    public EnemyInfo info;

    [HideInInspector] public EnemyCombat combat;
    [HideInInspector] public EnemyHealth health;

    protected float decisionRate = 2.0f;
    protected NavMeshAgent agent;
    public GameObject Target { get; set; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        this.combat = GetComponent<EnemyCombat>();
        this.health = GetComponent<EnemyHealth>();
        this.info = new EnemyInfo();
        this.Target = GameObject.FindGameObjectWithTag("Player");

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
        if (Vector3.Distance(this.transform.position, this.Target.transform.position) < info.awakeDistance)
        {

            if (Vector3.Distance(this.transform.position, this.Target.transform.position) <= combat.attackRange)
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
        combat.Attack();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (agent != null)
            this.agent.SetDestination(targetPosition);
    }
}
