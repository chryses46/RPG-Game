using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class FollowPlayer : MonoBehaviour
    {
   
        GameObject player;
        CameraPan cameraPan;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cameraPan = GetComponent<CameraPan>();
        }

        void Update()
        {
            if(!cameraPan.cameraIsPanning)
            {
                transform.position = player.transform.position;
            }
        }
    }
}

