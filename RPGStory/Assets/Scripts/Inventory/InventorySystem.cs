using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Inventory
{    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] Canvas inventoryCanvas;
        [SerializeField] InventorySlot[] inventorySlots;
        public Dictionary<InventorySlot, Item> currentInventory = new Dictionary<InventorySlot, Item>();

        public void DisplayInventory()
        {
            if(!inventoryCanvas.gameObject.activeSelf)
            {
                inventoryCanvas.gameObject.SetActive(true);
            }
            else
            {
                inventoryCanvas.gameObject.SetActive(false);
            }
        }

        private int AvailableSlot()
        {
            int slotNumber = 0;

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if(!inventorySlots[i].Occupied)
                {
                    slotNumber = inventorySlots[i].Order;
                    break;
                }
            }

            return slotNumber; 
        }

        public void AddItemToInventory(Item item)
        {
            InventorySlot slot = inventorySlots[AvailableSlot()];
            currentInventory.Add(slot, item);
            UpdateSlot(item, slot);
        }

        private void UpdateSlot(Item item, InventorySlot slot)
        {
            slot.gameObject.SetActive(true);
            slot.GetSlotText = currentInventory[slot].itemName;
            slot.Occupied = true;
        }
    }
    
    public class Item
    {
        public string itemName;
        public string itemDescription;
    }
}
