using System.Collections.Generic;
using System.Linq;
using Events.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
    public class EnemySpawner : MonoBehaviour
    {
        public int spawnerRange;
        public int maxAngels;
        public int angelBatch;
        public bool respawns = true;
        public LayerMask spawnObstacles;
        public GameObject angelPrefab;
        
        private List<Angel> _spawnedAngels;
        private int _numOfRespawns;
        
        [Header("Listening On")] public VoidEventChannelSO spawnAngelsEvent;

        public void OnEnable()
        {
            spawnAngelsEvent.OnEventRaised += SpawnAngels;
        }

        public void OnDisable()
        {
            spawnAngelsEvent.OnEventRaised -= SpawnAngels;
        }

        private void Start()
        {
            _spawnedAngels = new List<Angel>();
        }

        private void SpawnAngels()
        {
            if (_numOfRespawns > 0 && !respawns) return;
            List<Angel> aux = _spawnedAngels.Where(a => a != null && a.gameObject != null).ToList();
            _spawnedAngels = aux;
            int numAngelsToSpawn = maxAngels - aux.Count;
            int numSpawnedAngels = 0;
            int cycleAttempts = 0;

            while (numSpawnedAngels < numAngelsToSpawn && cycleAttempts < 1000)
            {
                Vector2 positionToSpawn = Random.insideUnitCircle * spawnerRange;
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(positionToSpawn, 0.5f, spawnObstacles);
                if (collider2Ds.Length == 0)
                {
                    GameObject spawnedAngel = Instantiate(angelPrefab, positionToSpawn, Quaternion.identity, transform);
                    Angel angel = spawnedAngel.GetComponent<Angel>();
                    angel.angelBatch = angelBatch;
                    _spawnedAngels.Add(angel);
                    numSpawnedAngels++;
                }
                cycleAttempts++;
            }

            _numOfRespawns++;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, spawnerRange);
        }
    }
}