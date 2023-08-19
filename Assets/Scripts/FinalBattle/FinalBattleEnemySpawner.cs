using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace FinalBattle
{
    public class FinalBattleEnemySpawner : MonoBehaviour
    {
        public int angelsToSpawn;
        public int angelsPerSpawn;
        public int timeBetweenSpawns;
        private float _cooldownLeft;
        
        public int spawnerRange;
        public LayerMask spawnObstacles;

        
        public GameObject angelPrefab;
        private List<Angel> _spawnedAngels;

        private void Start()
        {
            _spawnedAngels = new List<Angel>();
        }

        private void Update()
        {
            _cooldownLeft-= Time.deltaTime;
            if (_cooldownLeft < 0 && _spawnedAngels.Count < angelsToSpawn)
            {
                SpawnAngels();
                _cooldownLeft = timeBetweenSpawns;
            }

            if (_spawnedAngels.Count >= angelsToSpawn)
            {
                CheckAllDead();
            }
        }
        
        private void SpawnAngels()
        {
            
            int numSpawnedAngels = 0;
            int cycleAttempts = 0;

            while (numSpawnedAngels < angelsPerSpawn && cycleAttempts < 1000)
            {
                Vector2 positionToSpawn = new Vector2(transform.position.x, transform.position.y) + (Random.insideUnitCircle * spawnerRange);
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(positionToSpawn, 0.5f, spawnObstacles);
                if (collider2Ds.Length == 0)
                {
                    GameObject spawnedAngel = Instantiate(angelPrefab, positionToSpawn, Quaternion.identity, transform);
                    Angel angel = spawnedAngel.GetComponentInChildren<Angel>();
                    angel.angelBatch = -1;
                    _spawnedAngels.Add(angel);
                    numSpawnedAngels++;
                }
                cycleAttempts++;
            }
        }

        private void CheckAllDead()
        {
            bool oneAlive = _spawnedAngels.Any(t => t != null && t.gameObject != null);
            if (!oneAlive) SceneManager.LoadScene(3);

        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, spawnerRange);
        }
    }
}