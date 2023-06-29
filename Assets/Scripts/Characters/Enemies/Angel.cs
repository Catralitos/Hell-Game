using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Assets.Scripts.Utils;
using Pathfinding;

public class Angel : Enemy
{
    public Vector3 SignalSpot;
    public int angelBatch;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.info.Type = "Angel";
        // TODO decide if we're keeping rndm rolls  
        //        this.DmgRoll = () => RandomHelper.RollD10() + 2;
        this.combat.simpleDamage = 5;
        this.combat.DmgRoll = () => this.combat.simpleDamage;
        this.info.awakeDistance = 10;
        this.combat.attackRange = 1;
        this.combat.AC = 20;
        this.combat.attackCooldown = 2;
        this.combat.cooldownLeft = 0;
    }

    public void Update()
    {
        if (this.ai.reachedDestination)
        {
            AttackPlayer();
        }
    }
}
