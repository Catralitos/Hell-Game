using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Characters;
using Enemies;
using Gameplay;
using CharacterPathfinding;

public class Enemy : PathfindingEntity
{
    public struct EnemyInfo
    {
        public string Type;
        public float awakeDistance;
    }

    public EnemyInfo info;

    [HideInInspector] public EnemyCombat combat;
    [HideInInspector] public EnemyHealth health;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.combat = GetComponent<EnemyCombat>();
        this.health = GetComponent<EnemyHealth>();
        this.info = new EnemyInfo();
        this.AIDestinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public void AttackPlayer()
    {
        combat.Attack();
    }
}
