using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Dialogue;

namespace Core.Interactables
{
    public class PuzzleObject : MonoBehaviour
    {
        DialogueSystem dialogue;
        CircleCollider2D circleCollider2D;
        [SerializeField] string interactHint;

        public bool isPuzzleItem;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            dialogue.gameDialogue.Add(name, interactHint);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log(other.name + " can interact with " + name);
            if(isPuzzleItem)
            {
                dialogue.InitiateInfoDialogue(dialogue.gameDialogue[name], 0);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            dialogue.ToggleDialogueBox(false);
        }
    }
  
}
