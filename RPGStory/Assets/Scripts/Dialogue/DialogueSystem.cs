using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] Canvas dialogueCanvas; 
        [SerializeField] Text dialogueText;
        public int DelayTime = 2;
        public Dictionary<string, string> gameDialogue = new Dictionary<string, string>() 
        {
            {"Start","A mysterious dock. The island beckons you."},
        };
        
        void Update()
        {
            if(Input.GetButtonDown("Cancel"))
            {
                InitiateBox("Dialogue Test", 2);
            }
        }

        public void InitiateBox(string text, int givenDelay)
        {
            dialogueCanvas.gameObject.SetActive(true);
            dialogueText.text = text;
            DelayTime = givenDelay;
            StartCoroutine("HideBox");
        }

        IEnumerator HideBox()
        {
            yield return new WaitForSeconds(DelayTime);
            dialogueCanvas.gameObject.SetActive(false);
        }
    }
}