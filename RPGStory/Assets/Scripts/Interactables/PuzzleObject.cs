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
        CameraPan cameraPan;
        Animator animator;
        CircleCollider2D circleCollider2D;
        [SerializeField] string interactHint;
        [SerializeField] string puzzleItem;
        [SerializeField] string winDialogue;
        [SerializeField] Sprite solveSprite;
        [SerializeField] bool enablesAnItem;
        [SerializeField] GameObject itemToEnable;
        [SerializeField] bool isPuzzle;
        [SerializeField] string animationToTrigger;
        [SerializeField] bool isSolved;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
            inventory = FindObjectOfType<InventorySystem>();
            cameraPan = FindObjectOfType<CameraPan>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            if(gameObject.GetComponent<Animator>())
            {
                animator = gameObject.GetComponent<Animator>();
            }

            dialogue.gameDialogue.Add(name, interactHint);
        }

        void Update()
        {
            
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(isPuzzle & !isSolved & other.gameObject.tag == "Player")
            {
                if(GameManager.instance.autoHintsEnabled && interactHint != "") dialogue.InitiateInfoDialogue(dialogue.gameDialogue[name], 0);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(isPuzzle)
            {
                inventory.inRangeOfPuzzle = true;

                inventory.SetActivePuzzle(this); 
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if(!isSolved)
            {
                dialogue.ToggleDialogueBox(false);

                inventory.inRangeOfPuzzle = false;

                inventory.ClearActivePuzzle();
            }
        }

        public void CheckItemEligability()
        {
            Debug.Log("CheckItemEligability sees puzzleItem as: " + puzzleItem);
            InventorySlot slot = inventory.currentlySelectedSlot;

            if(slot.useSelected && slot.useAttempted)
            {
                if(inventory.currentInventory[inventory.currentlySelectedSlot].itemName == puzzleItem)
                {
                    PuzzleSolved(slot);
                }
                else
                {
                    Debug.Log(inventory.currentInventory[inventory.currentlySelectedSlot].itemName + " does not match " + puzzleItem);
                    dialogue.InitiateInfoDialogue("That does nothing here.", 3);
                }

                slot.useAttempted = false;
            }  
        }

        private void PuzzleSolved(InventorySlot slot)
        {
            if(animator) animator.SetBool(animationToTrigger, true);

            inventory.RemoveItem(slot);

            if(winDialogue != "") dialogue.InitiateInfoDialogue(winDialogue, 3);

            inventory.inRangeOfPuzzle = false;

            isPuzzle = false;

            inventory.ClearActivePuzzle();

            isSolved = true;

            GetComponent<SpriteRenderer>().sprite = solveSprite;

            GetComponent<PolygonCollider2D>().enabled = false;

            if(enablesAnItem)
            {
                EnableItem();
            }
        }


        private void EnableItem()
        {
            itemToEnable.SetActive(true);

            cameraPan.InitiatePan(itemToEnable.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

            enablesAnItem = false;
        }
    }
}
