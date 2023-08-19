using System.Collections.Generic;
using System.Linq;
using Characters.Enemies;
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
        public bool hasTimedRespawns = true;
        public bool listensToGeneralEvent = true;
        public LayerMask spawnObstacles;
        public GameObject angelPrefab;
        
        private List<Angel> _spawnedAngels;
        private int _numOfRespawns;
        
        [Header("Listening On")] public VoidEventChannelSO spawnAngelsEvent;
        public VoidEventChannelSO specificSpawnAngelsEvent;
        public void OnEnable()
        {
            if (listensToGeneralEvent)
            {
                spawnAngelsEvent.OnEventRaised += SpawnAngels;
            }
            else
            {
                specificSpawnAngelsEvent.OnEventRaised += SpawnAngels;
            }
        }

        public void OnDisable()
        {
            if (listensToGeneralEvent)
            {
                spawnAngelsEvent.OnEventRaised -= SpawnAngels;
            }
            else
            {
                specificSpawnAngelsEvent.OnEventRaised -= SpawnAngels;
            }
        }

        private void Start()
        {
            _spawnedAngels = new List<Angel>();
        }

        private void SpawnAngels()
        {
            if (_numOfRespawns > 0 && !hasTimedRespawns) return;
            int numAngelsToSpawn;

            _spawnedAngels ??= new List<Angel>();

            if (_spawnedAngels.Count > 0)
            {
                List<Angel> aux = _spawnedAngels.Where(a => a != null && a.gameObject != null).ToList();
                _spawnedAngels = aux;
                numAngelsToSpawn = maxAngels - aux.Count;
            }
            else
            {
                numAngelsToSpawn = maxAngels;
            }
            int numSpawnedAngels = 0;
            int cycleAttempts = 0;

            while (numSpawnedAngels < numAngelsToSpawn && cycleAttempts < 1000)
            {
                Vector2 positionToSpawn = new Vector2(transform.position.x, transform.position.y) + (Random.insideUnitCircle * spawnerRange);
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(positionToSpawn, 0.5f, spawnObstacles);
                if (collider2Ds.Length == 0)
                {
                    GameObject spawnedAngel = Instantiate(angelPrefab, positionToSpawn, Quaternion.identity, transform);
                    Angel angel = spawnedAngel.GetComponentInChildren<Angel>();
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