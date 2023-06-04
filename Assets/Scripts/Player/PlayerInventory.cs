using System.Collections;
using System.Collections.Generic;
using Inventory;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Transform itemHinge;

        public InventoryItem testItem;
        private InventoryItem _equippedItem;

        private Animator _animator;

        public GameObject inventoryCanvas;
        private int _currentItem = 0;
        public int maxInventoryItems = 8;
        [HideInInspector] public List<InventoryItem> currentItems;

        public Transform inventoryMenu;
        private RadialLayout _radialMenu;
        public float rotateTime = 0.3f;
        private bool _isRotating;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _radialMenu = inventoryMenu.gameObject.GetComponent<RadialLayout>();
            _equippedItem = testItem;
            inventoryCanvas.SetActive(false);
        }

        private void Update()
        {
            inventoryCanvas.SetActive(PlayerEntity.Instance.menuOpen);
            if (inventoryCanvas.activeSelf)
            {
                if (_isRotating) return;
                
                float x = PlayerEntity.Instance.move.x;
                int children = inventoryMenu.childCount;
                
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
            
            Debug.Log("///////////////////////////");
            Debug.Log(originalRotation);
            Debug.Log(targetRotation);
            Debug.Log(originalCounterRotation);
            Debug.Log(targetCounterRotation);

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
            _isRotating = false;
        }

        public void UseItem()
        {
            switch (_equippedItem)
            {
                case HealingItem item:
                    UseHealItem(item);
                    break;
                case Weapon item:
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
        
        private void UseHealItem(HealingItem item)
        {
            //TODO dar freeze temporário ao player
            
         
            PlayerEntity.Instance.health.RestoreHealth(item.hpRestoreValue);
        }
        
        private  void UseMeleeWeapon(Weapon weapon)
        {
            _animator.SetTrigger(Animator.StringToHash("MeleeSwing"));
        }
        
        private static void UseRangedWeapon(Weapon weapon)
        {
            
        }

    }
}