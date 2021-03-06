﻿using UnityEngine;
using Core.Dialogue;
using Core.Inventory;

namespace Core.Interactables
{
    public class OpenSeseme : MonoBehaviour
    {
        InventorySystem inventory;
        DialogueSystem dialogue;
        SpriteRenderer spriteRenderer;
        CircleCollider2D circleCollider2D;
        [SerializeField] Sprite openChestSprite;
        [SerializeField] string chestContents;
        [SerializeField] string contentDescription;

        Item chestItem = new Item();

        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
            dialogue = FindObjectOfType<DialogueSystem>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            chestItem.itemName = chestContents;
            chestItem.itemDescription = contentDescription;

            dialogue.gameDialogue.Add(name,"You received the " + chestContents + "!");
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
                Interact();
        }

        void Interact()
        {
            if(Input.GetButton("Submit"))
            {
                spriteRenderer.sprite = openChestSprite;
                dialogue.InitiateInfoDialogue(dialogue.gameDialogue[name], 3);
                
                inventory.AddItemToInventory(chestItem);
                circleCollider2D.enabled = false;
            }
        }
    }
}