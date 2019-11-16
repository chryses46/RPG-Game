using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CameraPan : MonoBehaviour
    {
        FollowPlayer followPlayer;

        public bool cameraIsPanning = false;

        private Vector3 destination;
        private Vector3 originalPosition;
        const float movementSpeed = 2;
        float speed;
        bool returningHome = false;
        bool movingToDestination;
        bool isWaiting;

        // Update is called once per frame
        void Update()
        {
            speed = movementSpeed * Time.deltaTime;
            
            if(!isWaiting)
            {
                MoveToDestination();
            }
        }

        public void InitiatePan(Vector2 panDestination, Vector2 playerPosition)
        {
            originalPosition = playerPosition;
            destination = panDestination;
            movingToDestination = true;
            cameraIsPanning = true;
        }

        private void MoveToDestination()
        {
            if(movingToDestination)
            {
                if(transform.position != destination)
                {
                    Debug.Log("moving to " + destination);
                    transform.position = Vector2.MoveTowards(transform.position, destination, speed);

                }
                else if(transform.position == destination && !returningHome)
                {
                    movingToDestination = false;
                    isWaiting = true;
                    
                    StartCoroutine(Wait(2));
                }
                else if(transform.position == originalPosition && returningHome)
                {
                    cameraIsPanning = false;
                    movingToDestination = false;
                    returningHome = false;
                }
            }    
        }

        IEnumerator Wait(float waitTime)
        {
            destination = originalPosition;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;
            movingToDestination = true;
            returningHome = true;
        }
    }
}

