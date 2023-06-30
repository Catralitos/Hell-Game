using System.Collections.Generic;
using System.Linq;
using Events.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;
using Inventory;
using Inventory.ScriptableObjects;

namespace Management
{
    public class ItemSpawner : MonoBehaviour
    {
        public int spawnerRange;
        public int maxItems;
        public int itemBatch;
        public bool hasTimedRespawns = true;
        public bool listensToGeneralEvent = true;
        public LayerMask spawnObstacles;
        public List<ItemSO> itemsToSpawn;
        public GameObject pickupPrefab;
        
        private List<CollectableItem> _spawnedItems;
        private int _numOfRespawns;
        
        [Header("Listening On")] public VoidEventChannelSO spawnItemEvent;
        public VoidEventChannelSO specificSpawnItemEvent;
        public void OnEnable()
        {
            if (listensToGeneralEvent)
            {
                Debug.Log(gameObject.name + " subscribed to general event.");
                spawnItemEvent.OnEventRaised += SpawnItems;
            }
            else
            {
                Debug.Log(gameObject.name + " subscribed to specific event.");
                specificSpawnItemEvent.OnEventRaised += SpawnItems;
            }
        }

        public void OnDisable()
        {
            if (listensToGeneralEvent)
            {
                spawnItemEvent.OnEventRaised -= SpawnItems;
            }
            else
            {
                specificSpawnItemEvent.OnEventRaised -= SpawnItems;
            }
        }

        private void Start()
        {
            _spawnedItems = new List<CollectableItem>();
        }

        private void SpawnItems()
        {
            if (_numOfRespawns > 0 && !hasTimedRespawns) return;
            int numItemsToSpawn;

            if (_spawnedItems == null)
                _spawnedItems = new List<CollectableItem>();

            if (_spawnedItems.Count > 0)
            {
                List<CollectableItem> aux = _spawnedItems.Where(a => a != null && a.gameObject != null).ToList();
                _spawnedItems = aux;
                numItemsToSpawn = maxItems - aux.Count;
            }
            else
            {
                numItemsToSpawn = maxItems;
            }
            int numSpawnedItems = 0;
            int cycleAttempts = 0;

            while (numSpawnedItems < numItemsToSpawn && cycleAttempts < 1000)
            {
                Vector2 positionToSpawn = new Vector2(transform.position.x, transform.position.y) + (Random.insideUnitCircle * spawnerRange);
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(positionToSpawn, 0.5f, spawnObstacles);
                if (collider2Ds.Length == 0)
                {
                    int itemId = listensToGeneralEvent?
                                 Mathf.RoundToInt(Random.Range(0, itemsToSpawn.Count)) :
                                 0;

                    GameObject spawnedItem = Instantiate(pickupPrefab, positionToSpawn, Quaternion.identity, transform);
                    CollectableItem item = spawnedItem.GetComponent<CollectableItem>();
                    item.currentItem = itemsToSpawn[itemId];
                    _spawnedItems.Add(item);
                    numSpawnedItems++;
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