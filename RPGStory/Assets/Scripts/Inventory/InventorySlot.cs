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
        [SerializeField] GameObject background;
        [SerializeField] GameObject interactBackground;
        [SerializeField] GameObject itemUseSelected;
        [SerializeField] GameObject itemDetailsSelected;
        public bool Occupied = false;
        public bool useSelected = true;
        public bool detailsSelected;
        public bool useAttempted = false;
        public bool isFocus;
        private string slotText;

        void Start()
        {
            slotTextBox = GetComponent<Text>();
            useSelected = true;
        }

        public void Selected(bool isSelected)
        {
            background.SetActive(isSelected);
        }

        public void Interacted(bool interacted)
        {
            interactBackground.SetActive(interacted);
            isFocus = interacted;
        }

        public void SetSlotText(string text)
        {
            slotText = text;
        }

        public void ToggleInteractOptions()
        {
            if(Occupied)
            {
               if(itemUseSelected.activeSelf)
                {
                    itemUseSelected.SetActive(false);
                    useSelected = false;
                    
                    itemDetailsSelected.SetActive(true);
                    detailsSelected = true;
                }
                else
                {
                    itemUseSelected.SetActive(true);
                    useSelected = true;

                    itemDetailsSelected.SetActive(false);
                    detailsSelected = false;
                } 
            }
            
        }

        public void EmptySlot()
        {
            slotTextBox.text = null;
            slotText = null;
            Occupied = false;
            interactBackground.SetActive(false);
            background.SetActive(false);
            useSelected = false;
        }
    }
}
