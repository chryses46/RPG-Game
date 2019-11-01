using UnityEngine;
using Core.Dialogue;

namespace Core.Interactables
{
    public class HintObject : MonoBehaviour
    {
        CircleCollider2D circleCollider2D;
        DialogueSystem dialogue;

        [SerializeField] string hintText;

        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            dialogue = FindObjectOfType<DialogueSystem>();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            dialogue.InitiateInfoDialogue(hintText, 0);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            dialogue.ToggleDialogueBox(false);
        }
    }
}

