using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Assets.Scripts.Utils;
using Characters.BehaviorTrees.Trees;

public class LessDumbAngel : Enemy
{
    public Vector3 SignalSpot;
    public int angelBatch;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.info.Type = "OKAngel";
        // TODO decide if we're keeping rndm rolls  
        //        this.DmgRoll = () => RandomHelper.RollD10() + 2;
        this.combat.simpleDamage = 5;
        this.combat.DmgRoll = () => this.combat.simpleDamage;
        this.info.awakeDistance = 10;
        this.combat.attackRange = 1;
        this.combat.AC = 20;
        this.combat.attackCooldown = 2;
        this.combat.cooldownLeft = 0;
        this.MainBehaviourTree = null;
        InitializeBehaviourTree();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.MainBehaviourTree != null)
            this.MainBehaviourTree.Run();
    }

    public override void InitializeBehaviourTree()
    {
        // First Patrol Point is object at town center
        GameObject patrolObject1 = new GameObject();
        patrolObject1.transform.position = Vector3.zero;

        // Second Patrol Point is Angel Spawn Point
        GameObject patrolObject2 = new GameObject();
        patrolObject2.transform.position = this.transform.position;

        List<GameObject> patrolObjects = new List<GameObject>()
        {
                patrolObject1,
                patrolObject2
        };
    }
}
