using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        GameManager gameManager;
        [SerializeField] Canvas battleCanvas;
        [SerializeField] Image battleBackground;
        [SerializeField] Text enemy1Name;
        [SerializeField] Text enemy2Name;
        [SerializeField] Text enemy3Name;
        [SerializeField] Image enemy1Sprite;
        [SerializeField] Image enemy2Sprite;
        [SerializeField] Image enemy3Sprite;

        public bool inBattle = false;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Need at least 1 enemy name, 1 enemy sprite, and battle background.
        public void InitiateBattle(string enemyName, Sprite enemySprite)
        {
            inBattle = true;

            battleCanvas.gameObject.SetActive(true);

            battleBackground.sprite = gameManager.currentAreaBackgroundImage;

            enemy1Name.text = enemyName;
            enemy1Sprite.sprite = enemySprite;
        }
    }
}

