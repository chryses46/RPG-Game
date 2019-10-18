using System;
using UnityEngine;
using UnityEngine.UI;


namespace Core.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        InventorySystem inventory;
        Text slotTextBox;
        public int Order;
        public bool Occupied = false;
        private string slotText;
        public string GetSlotText {
            set{slotText=value;} 
            get{return slotText;}
        }

        void Start()
        {
            slotTextBox = GetComponent<Text>();
            slotTextBox.text = GetSlotText;
        }
    }
}
