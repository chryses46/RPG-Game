using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Dialogue;

namespace Core.Inventory
{    public class InventorySystem : MonoBehaviour
    {
        DialogueSystem dialogue;
        [SerializeField] Canvas inventoryCanvas;
        [SerializeField] InventorySlot[] inventorySlots;
        public Dictionary<InventorySlot, Item> currentInventory = new Dictionary<InventorySlot, Item>();

        public bool inventoryActive = false;

        public bool isFocus;
        
        public bool inRangeOfPuzzle;

        public int selectedSlotOrder;

        public InventorySlot currentlySelectedSlot;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
        }

        public void DisplayInventory()
        {
            
            if(currentlySelectedSlot) currentlySelectedSlot.Interacted(false);
            GetCurrentInventory(0);
            isFocus = true;

            if(!inventoryCanvas.gameObject.activeSelf)
            {
                inventoryCanvas.gameObject.SetActive(true);
                inventoryActive = true;
                
                selectedSlotOrder = 0;
            }
            else
            {
                inventoryCanvas.gameObject.SetActive(false);
                inventoryActive = false;
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
            slot.SetSlotText(currentInventory[slot].itemName);
            slot.Occupied = true;
        }

        public void SelectInventorySlot(int updatedSelection)
        {
            if (updatedSelection == currentInventory.Count)
            {
                // Debug.Log("End of inventory"); // either return or do something here later like animate a jarring "down" movement of the slection sprite.
            }
            else if (updatedSelection == -1)
            {
                // Debug.Log("Top of inventory"); // either return or do something here later like animate a jarring "up" movement of the slection sprite.
            }
            else
            {
                GetCurrentInventory(updatedSelection);
                selectedSlotOrder = updatedSelection;
            }
        }

        public void GetCurrentInventory(int requestedSlotOrder)
        {
            if(currentlySelectedSlot != null && requestedSlotOrder != currentlySelectedSlot.Order) currentlySelectedSlot.Selected(false);

            foreach(KeyValuePair<InventorySlot,Item> slot in currentInventory)
            {
                if(slot.Key.Order == requestedSlotOrder)
                {
                    slot.Key.Selected(true);
                    currentlySelectedSlot = slot.Key;
                }
            }
        }

        public void RemoveItem(InventorySlot slot)
        {
            slot.EmptySlot();
            Item item = currentInventory[slot];
            currentInventory.Remove(slot);
            slot.gameObject.SetActive(false);
            isFocus = true;
        }

        public void CallItemInteractBox(bool isCalled)
        {
            currentlySelectedSlot.Interacted(isCalled);

            if(isCalled)
            {
                isFocus = false;
            }
            else
            {
                isFocus = true;
            }
        }

        public void ItemInteract()
        {
            if(currentlySelectedSlot.useSelected)
            {
                if(!inRangeOfPuzzle)
                {
                    dialogue.InitiateInfoDialogue("You can't use that here", 3);
                }
                else
                {
                    currentlySelectedSlot.useAttempted = true;
                }
            }
            else
            {
                dialogue.InitiateInfoDialogue(currentInventory[currentlySelectedSlot].itemDescription, 3);
            }
        }

        public void CycleItemInteract()
        {
            currentlySelectedSlot.ToggleInteractOptions();
        }

    }
    
    public class Item
    {
        public string itemName;
        public string itemDescription;
    }
}
