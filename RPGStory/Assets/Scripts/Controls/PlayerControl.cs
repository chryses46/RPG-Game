using UnityEngine;
using Core.Inventory;
using System;

namespace Core.Controls
{
    public class PlayerControl : MonoBehaviour
    {
        InventorySystem inventory;
        Animator animator;
        CameraPan cameraPan;
        SpriteRenderer spriteRenderer;
        [SerializeField] int movementSpeed = 2;
        enum Position {Forward, Backward, Right, Left}
        Position currentPostion = Position.Forward;
        string currentIdleSprite;

        bool dpadActive = false;
        bool playerIdle = false;

        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
            animator = GetComponent<Animator>();
            cameraPan = FindObjectOfType<CameraPan>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if(!cameraPan.cameraIsPanning)
            {
                RespondToInput();
            }
            else
            {
                SetIdleSprite();
            }
            
        }

        private void RespondToInput()
        {
            if(Input.GetButtonDown("Inventory"))
            {
                inventory.DisplayInventory();
            }

            if(inventory.inventoryActive)
            {
                InventoryControls();
                SetIdleSprite();
            }
            else
            {
                MovePlayer();
            }
        }

        private void MovePlayer()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if(Mathf.Abs(verticalInput) > 0 && Mathf.Abs(horizontalInput) > 0)
            {
                if(Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
                {
                    horizontalInput = 0;
                }
                else
                {
                    verticalInput = 0;
                }
            }

            Vector3 newDestination = new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);

            transform.position = transform.position + newDestination;

            UpdateCharacterSprites(horizontalInput, verticalInput);
        }

        private void UpdateCharacterSprites(float horizontalInput, float verticalInput)
        {
            animator.ResetTrigger(currentIdleSprite);

            if (verticalInput > 0)
            {
                animator.SetTrigger("Forward");
                currentPostion = Position.Forward;
            }
            else if(verticalInput < 0)
            {
                animator.SetTrigger("Backward");
                currentPostion = Position.Backward;
            }
            else if(horizontalInput > 0)
            {
                animator.SetTrigger("Right");
                currentPostion = Position.Right;
            }
            else if(horizontalInput < 0)
            {
                animator.SetTrigger("Left");
                currentPostion = Position.Left;
            }
            else
            {
                SetIdleSprite();
            }
        }

        private void SetIdleSprite()
        {
            switch(currentPostion)
            {
                case Position.Forward:
                    animator.SetTrigger("ForwardIdle");
                    currentIdleSprite = "ForwardIdle";
                    break;
                case Position.Backward:
                    animator.SetTrigger("BackwardIdle");
                    currentIdleSprite = "BackwardIdle";
                    break;
                case Position.Right:
                    animator.SetTrigger("RightIdle");
                    currentIdleSprite = "RightIdle";
                    break;
                case Position.Left:
                    animator.SetTrigger("LeftIdle");
                    currentIdleSprite = "LeftIdle";
                    break;
                default:
                    animator.SetTrigger("ForwardIdle");
                    currentIdleSprite = "ForwardIdle";
                    break;
            }
        }

        void InventoryControls()
        {
            
            if(inventory.isFocus)
            {
                ProcessInventoryNavigation();

                ProcessInventoryInteraction();
            }
            else
            {
                ProcessItemInteraction();
            }
        }

        private void ProcessInventoryInteraction()
        {
            if (Input.GetButtonDown("Cancel"))
                inventory.DisplayInventory();

            if (inventory.currentlySelectedSlot)
            {
                if (Input.GetButtonDown("Submit"))
                    inventory.CallItemInteractBox(true);
            }
        }

        private void ProcessInventoryNavigation()
        {
            if(!dpadActive && Mathf.Abs(Input.GetAxis("DPad Y")) == 1)
            {
                dpadActive = true;

                if(inventory.currentInventory.Count < 1) 
                    return;

                if (Input.GetAxisRaw("DPad Y") < 0)
                {
                    inventory.SelectInventorySlot(inventory.selectedSlotOrder + 1);
                }
                else if (Input.GetAxisRaw("DPad Y") > 0 )
                {
                    inventory.SelectInventorySlot(inventory.selectedSlotOrder - 1);
                }
                
            }
            else if (dpadActive && Mathf.Abs(Input.GetAxis("DPad Y")) == 0)
            {
                dpadActive = false;
            }
        }

        private void ProcessItemInteraction()
        {

            if (inventory.currentlySelectedSlot.isFocus)
            {

                if(!dpadActive && Mathf.Abs(Input.GetAxis("DPad X")) == 1)
                {
                    dpadActive = true;
                    inventory.CycleItemInteract(Input.GetAxis("DPad X")); 

                }
                else if(dpadActive && Mathf.Abs(Input.GetAxis("DPad X")) == 0)
                {
                    dpadActive = false;
                }

                if (Input.GetButtonDown("Submit"))
                {
                    inventory.ItemInteract();
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    inventory.CallItemInteractBox(false);
                }
            }
        }

        
    }

    

    // May need this later but for now; may use DPad for navigating inventory, or moving through text.
    public class DPadButtons : MonoBehaviour
    {
        InventorySystem inventory;
        public static bool IsLeft, IsRight, IsUp, IsDown;
        private float _LastX, _LastY;
 
        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
        }

        private void Update()
        {
            float x = Input.GetAxis("DPad X");
            float y = Input.GetAxis("DPad Y");

            IsLeft = false;
            IsRight = false;

            IsUp = false;
            IsDown = false;

            if (_LastX != x)
            {
                if (x == -1)
                {
                    IsLeft = true;
                }
                else if (x == 1)
                {
                    IsRight = true;
                }
                    
            }

            if (_LastY != y)
            {
                if (y == -1)
                {
                    IsDown = true;
                }
                else if (y == 1)
                {
                    IsUp = true;
                }
            }

            _LastX = x;
            _LastY = y;
        }
    }
}