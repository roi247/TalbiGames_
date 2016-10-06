using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MultiplayerFps
{
    public class GameMessanger : MonoBehaviour
    {
        public Text gameInfoText;
        [SerializeField]
        float killInfoDataLifetime;
        IEnumerator AddToGameInfoTextRoutine(string _data, float _dataLifetime)
        {
            gameInfoText.text += _data;
            yield return new WaitForSeconds(_dataLifetime);
            gameInfoText.text = gameInfoText.text.Replace(_data, "");

        }

        public void AddKillsInfoText(string playerShootingID, string playerShotID, string weaponName)
        {
            string _data;
            string playerShootingName = GameManager.GetPlayer(playerShootingID).playerName;
            string playerShotName = GameManager.GetPlayer(playerShotID).playerName;

            if (playerShootingName == playerShotName)
            {
                _data = playerShootingName + " Committed Suicide \n";
            }
            else
            {
                _data = playerShootingName + " Killed " + playerShotName + " With [" + weaponName + "] \n";
            }

            StartCoroutine(AddToGameInfoTextRoutine(_data, killInfoDataLifetime));
        }

        public void AddToGameInfoText(string data, float dataLifetime)
        {
            StartCoroutine(AddToGameInfoTextRoutine(data, dataLifetime));
        }
    }
}

