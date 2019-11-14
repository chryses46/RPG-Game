using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Battle
{
    public class Enemy : MonoBehaviour
    {
        BattleSystem battleSystem;
        GameManager gameManager;
        SpriteRenderer spriteRenderer;
        Animator animator;

        [SerializeField] string enemyName;
        [SerializeField] int health;
        [SerializeField] int power;
        [SerializeField] float speed;
        [SerializeField] Sprite battleSprite;

            // Patrol AI variables
        enum FacingDirection {Forward, Backward, Right, Left}
        FacingDirection currentFacingDirection;
        FacingDirection previousFacingDirection;
        int currentRandomFacingDirectionInt = 0;
        [SerializeField] float patrolPauseLength = 2f;
        [SerializeField] int minTravelDistance = 2;
        [SerializeField] int maxTravelDistance = 4;
        float travelDistance;
        Vector3 destination;
        List<Vector3> noFlyZones = new List<Vector3>();
        bool movementPaused = false;
        List<ContactPoint2D> collisionPoints = new List<ContactPoint2D>();
        bool movingToDestination = false;
        float step;
            // End Patrol AI variables

        bool playerInRange;
        GameObject player;
        const float distanceBuffer = 1f;
        
        void Start()
        {
            battleSystem = FindObjectOfType<BattleSystem>();
            gameManager = FindObjectOfType<GameManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            currentFacingDirection = SetRandomFacingDirection();
            travelDistance = GetRandomTravelDistance();
        }

        void Update ()
        {
            step = speed * Time.deltaTime;

            if(playerInRange)
            {
                MoveToPlayer();
            }
            else if(!movementPaused)
            {
                PatrolTowardDestination();
            }
        }

        void MoveToPlayer()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
            // animate movement

            if(distanceToPlayer <= distanceBuffer)
                battleSystem.InitiateBattle(enemyName, battleSprite);
        }

        void PatrolTowardDestination()
        {   
            if(!movingToDestination)
            {   
                DetermineDestination();

                movingToDestination = true;
            }
            else
            {
                if(transform.position == destination)
                {
                    movingToDestination = false;
                    StartCoroutine(PatrolPause());
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, destination, step);
                }
            }
        }

        private void DetermineDestination()
        {
            ChangeDirection();

            travelDistance = GetRandomTravelDistance();
            
            SetDestination();

            Vector2 myPosition = transform.position;

            if(collisionPoints.Count > 0)
            {
                for (int i = 0; i < collisionPoints.Count; i++)
                {
                    Vector2 collisionPosition = collisionPoints[i].point;

                    if (Vector2.Distance(myPosition, collisionPosition) < Vector2.Distance(myPosition, destination))
                    {
                        SetDestination();
                    }

                    collisionPoints.Remove(collisionPoints[i]);
                }
            }

            SetSpriteDirection();

        }

        private void SetDestination()
        {
            switch (currentFacingDirection)
            {
                case FacingDirection.Forward:
                    destination = new Vector2(transform.position.x, (transform.position.y + travelDistance));
                    break;
                case FacingDirection.Backward:
                    destination = new Vector2(transform.position.x, (transform.position.y - travelDistance));
                    break;
                case FacingDirection.Right:
                    destination = new Vector2((transform.position.x + travelDistance), transform.position.y);
                    break;
                case FacingDirection.Left:
                    destination = new Vector2((transform.position.x - travelDistance), transform.position.y);
                    break;
            }    
        }

        private void SetSpriteDirection()
        {
            switch (currentFacingDirection)
            {
                case FacingDirection.Forward:
                    animator.SetTrigger("Forward");
                    break;
                case FacingDirection.Backward:
                    animator.SetTrigger("Backward");
                    break;
                case FacingDirection.Right:
                    animator.SetTrigger("Right");
                    break;
                case FacingDirection.Left:
                    animator.SetTrigger("Left");
                    break;
            }
        }

        private void ChangeDirection()
        {
            FacingDirection newFacingDirection = SetRandomFacingDirection();

            previousFacingDirection = currentFacingDirection;

            currentFacingDirection = newFacingDirection;
        }

        IEnumerator PatrolPause()
        {
            movementPaused = true;
            ProcessIdlePosition();
            yield return new WaitForSeconds(patrolPauseLength);
            DetermineDestination();
            movementPaused = false;
        }

        FacingDirection SetRandomFacingDirection()
        {
            int newRandomFacingDirectionInt;

            do
            {
                newRandomFacingDirectionInt = Random.Range(0, 4);
            }
            while(newRandomFacingDirectionInt == currentRandomFacingDirectionInt);

            currentRandomFacingDirectionInt = newRandomFacingDirectionInt;

            Debug.Log(currentRandomFacingDirectionInt);

                // Explicitly cast FacingDirection as randomFacingDirectionInt to obtain FacingDirection enum.
            FacingDirection newFacingDirection = (FacingDirection)currentRandomFacingDirectionInt;

            return newFacingDirection;
        }

        float GetRandomTravelDistance()
        {
            float newTravelDistance = Random.Range(minTravelDistance, maxTravelDistance);

            return newTravelDistance;
        }

        private void ProcessIdlePosition()
        {
            switch(currentFacingDirection)
            {
                case FacingDirection.Forward:
                    animator.SetTrigger("ForwardIdle");
                    break;
                case FacingDirection.Backward:
                    animator.SetTrigger("BackwardIdle");
                    break;
                case FacingDirection.Right:
                    animator.SetTrigger("RightIdle");
                    break;
                case FacingDirection.Left:
                    animator.SetTrigger("LeftIdle");
                    break;
                default:
                    animator.SetTrigger("ForwardIdle");
                    break;
            }
        }

        void OnDrawGizmosSelected()
        {
            if(movingToDestination)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, destination);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                player = other.gameObject;
                playerInRange = true;
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {   
            Debug.Log("Points colliding: " + other.contacts.Length);
            Debug.Log("First point that collided: " + other.contacts[0].point);

            if(!movementPaused)
                StartCoroutine(PatrolPause());
        }

        void OnCollisionStay2D(Collision2D other)
        {
            if(other.contacts.Length >= 3)
            {
                if(!movementPaused)
                {
                    for(int i = 0; i < other.contacts.Length; i++)
                    {
                        collisionPoints.Add(other.contacts[i]);
                    } 

                    StartCoroutine(PatrolPause());                   
                }
            }
        }
    }
}