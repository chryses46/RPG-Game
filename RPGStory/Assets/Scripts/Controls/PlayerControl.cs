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
        [SerializeField] int movementSpeed = 2;
        enum Position {Forward, Backward, Right, Left}
        Position currentPostion = Position.Forward;

        bool dPadActive = false;

        void Start()
        {
            inventory = FindObjectOfType<InventorySystem>();
            animator = GetComponent<Animator>();
            cameraPan = FindObjectOfType<CameraPan>();
        }

        void Update()
        {
            if(!cameraPan.cameraIsPanning)
            {
                RespondToInput();
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

        void InventoryControls()
        {
            if(inventory.isFocus)
            {
                ProcessInventoryNavigation();
            }
            else
            {
                ProcessItemInteraction();
            }
        }

        private void ProcessInventoryNavigation()
        {
            if (Input.GetAxisRaw("DPad Y") < 0 || Input.GetButtonDown("InventoryDown"))
            {
                if (inventory.currentInventory.Count < 1)
                {
                    // Debug.Log("No Inventory");
                }
                else
                {
                    // get the last d-pad direction store in a var
                    // if the previous d-pad direction = 

                    // or (wile navigating d-pad) (boolean)
                        // lock d-pad

                    inventory.SelectInventorySlot(inventory.selectedSlotOrder + 1);
                }
            }

            if (Input.GetAxisRaw("DPad Y") > 0 || Input.GetButtonDown("InventoryUp"))
            {
                if (inventory.currentInventory.Count < 1)
                {
                    // Debug.Log("No Inventory");
                }
                else
                {
                    inventory.SelectInventorySlot(inventory.selectedSlotOrder - 1);
                }
            }

            if (Input.GetButtonDown("Cancel"))
            {
                inventory.DisplayInventory();
            }

            if(inventory.currentlySelectedSlot)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    inventory.CallItemInteractBox(true);
                    Debug.Log("call CallItemInteractBox from Player Control");
                }
            }
        }

        private void ProcessItemInteraction()
        {
            if (inventory.currentlySelectedSlot.isFocus)
            {
                if (Input.GetAxisRaw("DPad X") > 0 || Input.GetButtonDown("Horizontal"))
                {
                    inventory.CycleItemInteract();
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




        // Can this be moved out of control??

        private void UpdateCharacterSprites(float horizontalInput, float verticalInput)
        {

            if(verticalInput == 0)
            {
                Debug.Log("Vertical Input is 0");
            }
            else if(horizontalInput == 0)
            {
                Debug.Log("Horizontal Input is 0");
            }

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
                ProcessIdlePosition();
            }
        }

        private void ProcessIdlePosition()
        {
            switch(currentPostion)
            {
                case Position.Forward:
                    animator.SetTrigger("ForwardIdle");
                    break;
                case Position.Backward:
                    animator.SetTrigger("BackwardIdle");
                    break;
                case Position.Right:
                    animator.SetTrigger("RightIdle");
                    break;
                case Position.Left:
                    animator.SetTrigger("LeftIdle");
                    break;
                default:
                    animator.SetTrigger("ForwardIdle");
                    break;
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