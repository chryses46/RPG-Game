using UnityEngine;
using Core.Dialogue;
using System.Collections;

namespace Core.Interactables
{
    public class HintObject : MonoBehaviour
    {
        CircleCollider2D circleCollider2D;
        DialogueSystem dialogue;

        CameraPan cameraPan;

        [SerializeField] string hintText;
        [SerializeField] bool enablesAnItem;
        [SerializeField] GameObject itemToEnable;

        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            dialogue = FindObjectOfType<DialogueSystem>();
            cameraPan = FindObjectOfType<CameraPan>();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            dialogue.InitiateInfoDialogue(hintText, 0);

            if(enablesAnItem)
            {
                if(Input.GetButtonDown("Submit"))
                {
                    itemToEnable.SetActive(true);
                    
                    cameraPan.InitiatePan(itemToEnable.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            dialogue.ToggleDialogueBox(false);
        }
    }
}

