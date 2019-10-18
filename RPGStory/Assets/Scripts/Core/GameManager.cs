using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Dialogue;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        DialogueSystem dialogue;

        void Start()
        {
            dialogue = FindObjectOfType<DialogueSystem>();
            StartCoroutine("LoadGame");
        }

        private IEnumerator LoadGame()
        {
            yield return new WaitForSeconds(2);
            dialogue.InitiateBox(dialogue.gameDialogue["Start"], 4);
        }
    }
}
