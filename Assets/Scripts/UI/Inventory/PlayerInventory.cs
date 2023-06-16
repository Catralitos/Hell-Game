using System.Collections;
using Events.ScriptableObjects;
using Gameplay.ScriptableObjects;
using Inputs;
using Inventory;
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
        private ItemSO _equippedItemSo;
        public HeldItem heldItem;
        
        [Header("Inventory")]
        private int _currentItem;

        private Animator _animator;

        public InputReader inputReader;
        public InventorySO currentInventory;

        private Vector2 _menuInput;

        [Header("Listening on")] public VoidEventChannelSO updateInventoryEvent;
        
        //TODO passar estes dois eventos para a parte de usar items
        [Header("Broadcasting on")]
        //To remove the correct amount on the inventory
        public ItemEventChannelSO useItemEvent;
        //To restore health
        public IntEventChannelSO restoreHealth;
        //To attack
        public VoidEventChannelSO attackEvent;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _radialMenu = inventoryMenu.gameObject.GetComponent<RadialLayout>();
            _currentItem = 0;
            _equippedItemSo = currentInventory.items[_currentItem];
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
            //Clear all children
            for (int i = 0; i < inventoryMenu.childCount; i++)
            {
                _radialMenu.RemoveItem();
            }

            //Create the new ones
            foreach (ItemSO item in currentInventory.items)
            {
                GameObject spawnedSlot =
                    Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, inventoryMenu);
                InventorySlot slot = spawnedSlot.GetComponent<InventorySlot>();
                slot.itemSo = item;
                slot.itemImage.sprite = item.itemSprite;
                switch (item)
                {
                    case HealingItemSO heal:
                        slot.leftText.text = heal.hpRestoreValue.ToString();
                        slot.rightText.text = "HP";
                        break;
                    case WeaponSO weapon:
                        slot.leftText.text = weapon.damage.ToString();
                        slot.rightText.text = weapon.usesLeft.ToString();
                        break;
                }
                _radialMenu.AddItem();
            }
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

            _equippedItemSo = currentInventory.items[_currentItem];
            _isRotating = false;
        }

        private void UseItem()
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
            //TODO dar freeze temporário ao player/tocar animação
            Debug.Log("Used healing item");
            restoreHealth.RaiseEvent(item.hpRestoreValue);
            useItemEvent.RaiseEvent(item);
        }
        
        private void UseMeleeWeapon(WeaponSO weapon)
        {
            _animator.SetTrigger(Animator.StringToHash("MeleeSwing"));
            attackEvent.RaiseEvent();
            useItemEvent.RaiseEvent(weapon);
        }
        
        private void UseRangedWeapon(WeaponSO weapon)
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