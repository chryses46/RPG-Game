using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Dialogue;
using Core.Battle;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        DialogueSystem dialogue;
        public Sprite currentAreaBackgroundImage;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
            StartCoroutine("LoadGame");
        }

        private IEnumerator LoadGame()
        {
            yield return new WaitForSeconds(2);
            //dialogue.InitiateInfoDialogue(dialogue.gameDialogue["Start"], 4); Disabled for now
        }

        void Update()
        {
        }
        
    }
}
