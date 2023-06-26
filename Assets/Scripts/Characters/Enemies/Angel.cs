using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Assets.Scripts.Utils;
using Characters.BehaviorTrees.Trees;

public class Angel : Enemy
{
    public Vector3 SignalSpot;
    public int angelBatch;

    public Angel()
    {
        this.enemyStats.Type = "Angel";
        this.enemyStats.AC = 14;
        this.enemyStats.HP = 15;
        // TODO decide if we're keeping rndm rolls  
        this.DmgRoll = () => RandomHelper.RollD10() + 2;
        this.enemyStats.SimpleDamage = 5;
        this.enemyStats.AwakeDistance = 10;
        this.enemyStats.WeaponRange = 3;
        this.MainBehaviourTree = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Target = GameObject.FindGameObjectWithTag("Player");
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

        //        GameObject patrolObject1 = GameObject.Find("Patrol").gameObject;

        // Second Patrol Point is Angel Spawn Point
        GameObject patrolObject2 = new GameObject();
        patrolObject2.transform.position = this.transform.position;

        List<GameObject> patrolObjects = new List<GameObject>()
        {
                patrolObject1,
                patrolObject2
        };

        this.MainBehaviourTree = new AngelTree(this, this.Target, patrolObjects, null);
        this.currBehaviorTreeType = BehaviorTreeType.Main;
        this.currBehaviorTree = this.MainBehaviourTree;

    }
}
