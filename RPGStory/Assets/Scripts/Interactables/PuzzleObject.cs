using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Dialogue;
using Core.Inventory;

namespace Core.Interactables
{   
    public class PuzzleObject : MonoBehaviour
    {
        DialogueSystem dialogue;
        InventorySystem inventory;
        CircleCollider2D circleCollider2D;
        [SerializeField] string interactHint;
        [SerializeField] string puzzleItem;
        [SerializeField] string winDialogue;
        [SerializeField] Sprite solveSprite;

        public bool isPuzzle;
        public bool isSolved;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
            inventory = FindObjectOfType<InventorySystem>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            dialogue.gameDialogue.Add(name, interactHint);
        }

        void Update()
        {
            if(inventory.inRangeOfPuzzle && inventory.currentlySelectedSlot) CheckItemEligability();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(isPuzzle & !isSolved)
            {
                dialogue.InitiateInfoDialogue(dialogue.gameDialogue[name], 0);

                inventory.inRangeOfPuzzle = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if(!isSolved)
            {
                dialogue.ToggleDialogueBox(false);
                inventory.inRangeOfPuzzle = false;
            }  
        }

        public void CheckItemEligability()
        {
            InventorySlot slot = inventory.currentlySelectedSlot;

            if(slot.useSelected && slot.useAttempted)
            {
                if(inventory.currentInventory[inventory.currentlySelectedSlot].itemName == puzzleItem)
                {
                    PuzzleSolved(slot);
                }
                else
                {
                    dialogue.InitiateInfoDialogue("That does nothing here.", 3);
                }

                slot.useAttempted = false;
            }  
        }

        private void PuzzleSolved(InventorySlot slot)
        {

            inventory.RemoveItem(slot);

            dialogue.InitiateInfoDialogue(winDialogue, 3);
            inventory.inRangeOfPuzzle = false;
            isSolved = true;
            GetComponent<SpriteRenderer>().sprite = solveSprite;
            GetComponent<PolygonCollider2D>().enabled = false;

        }
    }
}
