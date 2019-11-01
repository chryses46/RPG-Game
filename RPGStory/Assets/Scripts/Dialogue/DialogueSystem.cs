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

        public int DelayTime = 2;
        public Dictionary<string, string> gameDialogue = new Dictionary<string, string>() 
        {
            {"Start","A mysterious dock. The island beckons you."},
        };
        
        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
        }

        public void InitiateInfoDialogue(string text, int givenDelay)
        {
            if(dialogueCanvas.gameObject.activeSelf)
            {
                if(DelayTime > 0)
                {
                    StopCoroutine("HideDialogueBoxDelay");

                    SetInfoDialogue(text, givenDelay);
                }
                else
                {
                    SetInfoDialogue(text, givenDelay);
                }
            }
            else
            {
                ToggleDialogueBox(true);

                SetInfoDialogue(text, givenDelay);  
            } 
        }

        private void SetInfoDialogue(string text, int givenDelay)
        {
            infoDialogue.GetComponent<Text>().text = text;

            CloseDialogueBoxOnDelay(givenDelay);
        }

        private void CloseDialogueBoxOnDelay(int givenDelay)
        {
            DelayTime = givenDelay;

            if (DelayTime > 0) // if delay is 0, the box will stay
            {
                StartCoroutine("HideDialogueBoxDelay");
            }
        }

        public void ToggleDialogueBox(bool option)
        {
            dialogueCanvas.gameObject.SetActive(option);
        }

        IEnumerator HideDialogueBoxDelay()
        {
            yield return new WaitForSeconds(DelayTime);

            ToggleDialogueBox(false);

            DelayTime = 0;
        }
    }
}