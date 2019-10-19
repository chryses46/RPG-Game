using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Inventory;

namespace Core.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        InventorySystem inventory;
        [SerializeField] Canvas dialogueCanvas; 
        [SerializeField] GameObject infoDialogue;
        [SerializeField] GameObject itemInteractDialogue;
        [SerializeField] Text itemUseResult;
        [SerializeField] Text itemDetailsResult;

        bool dialogueCanvasEnabled;

        public int DelayTime = 2;
        public Dictionary<string, string> gameDialogue = new Dictionary<string, string>() 
        {
            {"Start","A mysterious dock. The island beckons you."},
        };
        
        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
        }

        void Update()
        {
            if(Input.GetButtonDown("Cancel"))
            {
                InitiateInfoDialogue("Dialogue Test", 2);
            }
        }

        public void InitiateInfoDialogue(string text, int givenDelay)
        {

            ToggleDialogueBox(true);
            infoDialogue.SetActive(true);
            infoDialogue.GetComponent<Text>().text = text;
            
            if(givenDelay != 0) // if delay is 0, the box will stay
            {
                DelayTime = givenDelay;
                StartCoroutine("HideDialogueBoxDelay");
            }
            
        }

        public void ItemInteractDialogueBox()
        {
            // box with options of Use and Inspect
            if(!dialogueCanvasEnabled) ToggleDialogueBox(true);
            infoDialogue.SetActive(false);
            itemInteractDialogue.SetActive(true);
        }

        public void ToggleDialogueBox(bool option)
        {
            dialogueCanvasEnabled = option;
            
            if(option)
            {
                dialogueCanvas.gameObject.SetActive(option);
            }

            if(!inventory.inventoryActive && !option) // Ensure to always disable inventory before calling this for Dialogue Canvas to close;
            {
                dialogueCanvas.gameObject.SetActive(option);    
            }
        }

        IEnumerator HideDialogueBoxDelay()
        {
            yield return new WaitForSeconds(DelayTime);
            ToggleDialogueBox(false);
        }
    }
}