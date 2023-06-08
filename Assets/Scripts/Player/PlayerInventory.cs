using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.ScriptableObjects;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    { 
        [Header("Menu UI")]       
        public GameObject inventoryCanvas;
        public Transform inventoryMenu;
        private RadialLayout _radialMenu;
        public float rotateTime = 0.3f;
        private bool _isRotating;
        public GameObject inventorySlotPrefab;
        
        [Header("Items")]
        private ItemSO _equippedItemSo;
        public HeldItem heldItem;
        
        [Header("Inventory")]
        private int _currentItem;
        public int maxInventoryItems = 8;
        [HideInInspector] public List<ItemSO> currentItems;
        
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _radialMenu = inventoryMenu.gameObject.GetComponent<RadialLayout>();
            List<ItemSO> auxList = new List<ItemSO>();
            for (int i = 0; i < inventoryMenu.childCount; i++)
            {
                auxList.Add(inventoryMenu.GetChild(i).GetComponent<InventorySlot>().itemSo);
            }
            currentItems = new List<ItemSO>(auxList);
            _currentItem = 0;
            _equippedItemSo = currentItems[_currentItem];
            inventoryCanvas.SetActive(false);
        }

        private void Update()
        {
            inventoryCanvas.SetActive(PlayerEntity.Instance.menuOpen);
            if (inventoryCanvas.activeSelf)
            {
                int children = inventoryMenu.childCount;
                if (_isRotating || children < 2 || currentItems.Count < 2) return;
                
                float x = PlayerEntity.Instance.move.x;
                
                switch (x)
                {
                    case > 0.1f:
                    {
                        _currentItem++;
                        if (_currentItem >= children)
                        {
                            _currentItem = 0;
                        }
                        StartCoroutine(MenuRotateRoutine(1));
                        break;
                    }
                    case < -0.1f:
                    {
                        _currentItem--;
                        if (_currentItem < 0)
                        {
                            _currentItem = children - 1;
                        }
                        StartCoroutine(MenuRotateRoutine(-1));
                        break;
                    }
                }
            }
            else
            {
                heldItem.itemSo = _equippedItemSo;
            }
        }

        public bool AddItem(ItemSO itemSo)
        {
            if (currentItems.Count >= maxInventoryItems || inventoryMenu.childCount >= maxInventoryItems) return false;
            
            currentItems.Add(itemSo);
            GameObject spawnedSlot =
                Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, inventoryMenu);
            InventorySlot slot = spawnedSlot.GetComponent<InventorySlot>();
            slot.itemSo = itemSo;
            _radialMenu.AddItem();
            return true;
        }
        
        public void RemoveItem(ItemSO itemSo)
        {
            if (!currentItems.Contains(itemSo)) return;
            
            currentItems.Remove(itemSo);
            for (int i = 0; i < inventoryMenu.childCount; i++)
            {
                GameObject child = inventoryMenu.GetChild(i).gameObject;
                if (child.GetComponent<InventorySlot>().itemSo == itemSo)
                {
                    child.transform.parent = null;
                    Destroy(child);
                    break;
                }
                    
            }
            _radialMenu.RemoveItem();
            _currentItem--;
            if (_currentItem < 0)
            {
                _currentItem = inventoryMenu.childCount - 1;
            }
            _equippedItemSo = _currentItem > -1 ? currentItems[_currentItem] : null;
            return;
        }

        private IEnumerator MenuRotateRoutine(int direction)
        {
            _isRotating = true;

            int children = inventoryMenu.childCount;
            
            Vector3 originalRotation = inventoryMenu.rotation.eulerAngles;
            Vector3 targetRotation = new Vector3(originalRotation.x, originalRotation.y,
                originalRotation.z + 360.0f / children * direction);

            Vector3 originalCounterRotation = inventoryMenu.GetChild(0).transform.localRotation.eulerAngles;
            Vector3 targetCounterRotation = new Vector3(originalCounterRotation.x, originalCounterRotation.y,
                originalCounterRotation.z + 360.0f / children * direction * -1);

            float elapsedTime = 0.0f;

            while (elapsedTime < rotateTime)
            {
                inventoryMenu.transform.rotation = Quaternion.Euler(Vector3.Lerp(originalRotation, targetRotation, elapsedTime/rotateTime));
                for (int i = 0; i < children; i++)
                {
                    inventoryMenu.GetChild(i).transform.localRotation = Quaternion.Euler(Vector3.Lerp(originalCounterRotation, targetCounterRotation, elapsedTime/rotateTime));
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            inventoryMenu.rotation = Quaternion.Euler(targetRotation);
            for (int i = 0; i < children; i++)
            {
                inventoryMenu.GetChild(i).transform.localRotation = Quaternion.Euler(targetCounterRotation);
            }

            _equippedItemSo = currentItems[_currentItem];
            _isRotating = false;
        }

        public void UseItem()
        {
            switch (_equippedItemSo)
            {
                case HealingItemSO item:
                    UseHealItem(item);
                    break;
                case WeaponSO item:
                {
                    switch (item.weaponType)
                    {
                        case WeaponType.MELEE:
                            UseMeleeWeapon(item);
                            break;
                        case WeaponType.RANGED:
                            UseRangedWeapon(item);
                            break;
                        default:
                            return;
                    }
                    break;
                }
            }
        }
        
        private void UseHealItem(HealingItemSO item)
        {
            //TODO dar freeze temporário ao player
            
         
            PlayerEntity.Instance.health.RestoreHealth(item.hpRestoreValue);
        }
        
        private  void UseMeleeWeapon(WeaponSO weapon)
        {
            _animator.SetTrigger(Animator.StringToHash("MeleeSwing"));
            //RemoveItem(heldItem.item);
        }
        
        private static void UseRangedWeapon(WeaponSO weapon)
        {
            
        }

    }
}