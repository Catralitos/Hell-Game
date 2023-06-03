using System;
using Inventory;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Transform itemHinge;

        public InventoryItem testItem;
        private InventoryItem _equippedItem;

        private Animator _animator;
        private static readonly int MeleeSwing = Animator.StringToHash("MeleeSwing");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _equippedItem = testItem;
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
            _animator.SetTrigger(MeleeSwing);
        }
        
        private static void UseRangedWeapon(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }

    }
}
