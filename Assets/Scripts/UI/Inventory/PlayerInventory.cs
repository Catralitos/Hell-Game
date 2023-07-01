using System.Collections;
using System.Collections.Generic;
using Events.ScriptableObjects;
using Gameplay.ScriptableObjects;
using Inputs;
using Inventory;
using Inventory.InstancedItems;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace UI.Inventory
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
        public HeldItem heldItem;
        
        [Header("Inventory")]
        private int _currentItem;

        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        public InputReader inputReader;
        public InventorySO currentInventory;

        private Vector2 _menuInput;

        [Header("Listening on")] public VoidEventChannelSO updateInventoryEvent;
        
        [Header("Broadcasting on")]
        //To remove the correct amount on the inventory
        public ItemEventChannelSO useItemEvent;
        //To restore health
        public IntEventChannelSO restoreHealth;
        //To attack
        public VoidEventChannelSO attackEvent;
        
        private static readonly int Heal = Animator.StringToHash("Heal");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _radialMenu = inventoryMenu.gameObject.GetComponent<RadialLayout>();
            _currentItem = 0;
            currentInventory.equippedItem = currentInventory.items[_currentItem];
            UpdateUI();
            inventoryCanvas.SetActive(false);
        }

        private void OnEnable()
        {
            inputReader.MoveEvent += onMoveSelection;
            inputReader.OpenRadialMenuEvent += OpenMenu;
            inputReader.CloseRadialMenuEvent += CloseMenu;
            inputReader.UseItemEvent += UseItem;
            updateInventoryEvent.OnEventRaised += UpdateUI;
        }
        
        private void OnDisable()
        {
            inputReader.MoveEvent -= onMoveSelection;
            inputReader.OpenRadialMenuEvent -= OpenMenu;
            inputReader.CloseRadialMenuEvent -= CloseMenu;
            updateInventoryEvent.OnEventRaised -= UpdateUI;

        }
        
        private void UpdateUI()
        {
            if (gameObject == null) return;
            //Clear all children
            for (int i = inventoryMenu.childCount - 1; i >= 0; i--)
            {
                Transform c = inventoryMenu.GetChild(i);
                c.SetParent(null);
                Destroy(c.gameObject);
            }

            //Create the new ones
            foreach (Item item in currentInventory.items)
            {
                GameObject spawnedSlot =
                    Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, inventoryMenu);
                InventorySlot slot = spawnedSlot.GetComponent<InventorySlot>();
                slot.item = item;
                slot.itemImage.sprite = item.itemSprite;
                switch (item)
                {
                    case HealingItem heal:
                        slot.leftText.text = heal.hpRestoreValue.ToString();
                        slot.rightText.text = "HP";
                        break;
                    case Weapon weapon:
                        slot.leftText.text = weapon.damage.ToString();
                        slot.rightText.text = weapon.usesLeft.ToString();
                        break;
                }
            }

            if (currentInventory.items.Count == 0)
            {
                currentInventory.equippedItem = null;
                heldItem.item = null;
            }
            _radialMenu.CalculateRadial();
        }
 
        private void Update()
        {
            if (inventoryCanvas.activeSelf)
            {
                int children = inventoryMenu.childCount;
                if (_isRotating || children < 2 || currentInventory.items.Count < 2) return;
                
                float x = _menuInput.x;
                switch (x)
                {
                    case > 0.1f:
                    {
                        _currentItem--;
                        if (_currentItem < 0)
                        {
                            _currentItem = currentInventory.items.Count - 1;
                        }
                        StartCoroutine(MenuRotateRoutine(1));
                        break;
                    }
                    case < -0.1f:
                    {
                        _currentItem++;
                        if (_currentItem >= currentInventory.items.Count)
                        {
                            _currentItem = 0;
                        }
                        StartCoroutine(MenuRotateRoutine(-1));
                        break;
                    }
                }
            }
            else
            {
                heldItem.item = currentInventory.equippedItem;
            }
        }
        
        private IEnumerator MenuRotateRoutine(int direction)
        {
            Debug.Log(_currentItem);
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

            currentInventory.equippedItem = currentInventory.items[_currentItem];
            heldItem.item = currentInventory.equippedItem;
            _isRotating = false;
        }

        private void UseItem()
        {
            if (currentInventory.equippedItem == null) return;
            switch (currentInventory.equippedItem)
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
            _animator.SetTrigger(Heal);
            restoreHealth.RaiseEvent(item.hpRestoreValue);
            useItemEvent.RaiseEvent(item);
        }
        
        private void UseMeleeWeapon(Weapon weapon)
        {
            _animator.SetTrigger(Attack);
            attackEvent.RaiseEvent();
            useItemEvent.RaiseEvent(weapon);
        }
        
        private void UseRangedWeapon(Weapon weapon)
        {
            attackEvent.RaiseEvent();
            useItemEvent.RaiseEvent(weapon);
        }
        
        //Event listener for input

        private void onMoveSelection(Vector2 value)
        {
            _menuInput = value;
        }
        
        private void OpenMenu()
        {
            if (inputReader.gameStateManager.currentGameState == GameState.Gameplay)
            {
                inventoryCanvas.SetActive(true);
            }
        }

        private void CloseMenu()
        {
            if (inputReader.gameStateManager.currentGameState == GameState.Gameplay)
            {
                inventoryCanvas.SetActive(false);
            }
        }

    }
}