using UnityEngine;
using Core.Dialogue;
using System.Collections;

namespace Core.Interactables
{
    public class HintObject : MonoBehaviour
    {
        CircleCollider2D circleCollider2D;
        DialogueSystem dialogue;

        [SerializeField] string hintText;
        [SerializeField] bool enablesAnItem;
        [SerializeField] GameObject itemToEnable;

        Camera cam;
        int camMovementSpeed = 2;
        Vector3 originalCameraPosition;
        float camPanSpeed;
        bool panCamToItem = false;
        bool panCamBack = false;

        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            dialogue = FindObjectOfType<DialogueSystem>();
            cam = Camera.main;
        }

        void Update()
        {
            camPanSpeed = camMovementSpeed * Time.deltaTime;

            if(panCamToItem)
            {
                CameraPanToItemToEnable();
            }
            else if(panCamBack)
            {
                CameraPanBack();
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            dialogue.InitiateInfoDialogue(hintText, 0);

            if(enablesAnItem)
            {
                if(Input.GetButtonDown("Submit"))
                {
                    itemToEnable.SetActive(true);
                    panCamToItem = true;
                }
            }
        }

        void CameraPanToItemToEnable()
        {   
            if(originalCameraPosition == null)
                originalCameraPosition = cam.transform.position;
            
            cam.transform.position = Vector2.MoveTowards(originalCameraPosition, itemToEnable.transform.position, camPanSpeed);

            if(cam.transform.position == itemToEnable.transform.position)
            {
                panCamToItem = false;
                panCamBack = true;
            }
            
        }

        void CameraPanBack()
        {
            cam.transform.position = Vector2.MoveTowards(cam.transform.position, originalCameraPosition, camPanSpeed);

            if(cam.transform.position == originalCameraPosition)
            {
                panCamBack = false;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            dialogue.ToggleDialogueBox(false);
        }
    }
}

