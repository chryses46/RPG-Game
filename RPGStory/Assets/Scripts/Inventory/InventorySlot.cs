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

        void Start()
        {
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
            slotTextBox = GetComponent<Text>();
            slotTextBox.text = text;
        }

        public void ToggleInteractOptions(float x)
        {
            if(Occupied)
            {
                switch(x)
                {
                    case 1:
                        itemUseSelected.SetActive(false);
                        useSelected = false;
                        itemDetailsSelected.SetActive(true);
                        detailsSelected = true;
                        break;
                    case -1:
                        itemUseSelected.SetActive(true);
                        useSelected = true;
                        itemDetailsSelected.SetActive(false);
                        detailsSelected = false;
                        break;
                    default:
                        itemUseSelected.SetActive(true);
                        useSelected = true;
                        break;
                }
            }
            
        }

        public void EmptySlot()
        {
            slotTextBox = GetComponent<Text>();
            slotTextBox.text = null;
            Occupied = false;
            interactBackground.SetActive(false);
            background.SetActive(false);
            useSelected = false;
        }
    }
}
