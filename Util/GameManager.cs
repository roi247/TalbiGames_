using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

/*
[Serializable]
public class PlayerData
{
    public Player player;
    public PlayerWeapon weapon;

    public PlayerData(Player _player, PlayerWeapon _playerWeapon)
    {
        player = _player;
        weapon = _playerWeapon;
    }

}
*/

/*
[Serializable]
public class PlayerDataList
{
    public List<PlayerData> dataList;

    public void Add(Player _player, PlayerWeapon _playerWeapon)
    {
        dataList.Add(new PlayerData(_player, _playerWeapon));
    }

    public void Remove(Player _player)
    {
        try
        {
            foreach (PlayerData playerdata in dataList)
            {
                if (playerdata.player == _player)
                {
                    dataList.Remove(playerdata);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("PlayerDataList EXCEPTION ! - Remove function. msg= " + e.ToString());
        }
    }

    public PlayerWeapon getPlayerWeapon(Player _player)
    {
        try
        {
            foreach (PlayerData playerdata in dataList)
            {
                if (playerdata.player == _player)
                {
                    Debug.Log("getPlayerWeapon ---> " + _player.name + " HAS " + playerdata.weapon.name);
                    return (playerdata.weapon);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("PlayerDataList EXCEPTION ! - getPlayerWeapon function. msg= " + e.ToString());
        }
        return null;

    }

    public void setPlayerWeapon(Player _player, PlayerWeapon _playerNewWeapon)
    {
        try
        {
            foreach (PlayerData playerdata in dataList)
            {
                if (playerdata.player == _player)
                {
                    Debug.Log("setPlayerWeapon for player---> " + _player.name);
                    playerdata.weapon = _playerNewWeapon;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("PlayerDataList EXCEPTION ! - setPlayerWeapon function. msg= " + e.ToString());
        }
    }
}
*/

namespace MultiplayerFps
{

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        public MatchSettings matchSettings;

        public List<PlayerWeapon> worldWeapons;
        public List<Player> playersList;
        // public List<PlayerWeapon> weaponsList;

        //public PlayerDataList playerDataList;

        [SerializeField]
        private GameObject sceneCamera;
        public GameMessanger messanger;

        public Text t;



        /*
        public PlayerWeapon debugWeap;


        public void setDebugWeap()
        {
            playerDataList.setPlayerWeapon(playersList[0], debugWeap);
        }

        public void getWeap()
        {
            playerDataList.getPlayerWeapon(playersList[0]);
        } 

        public void RemovePlayer()
        {
            playerDataList.Remove(playersList[0]);
        }


        /*
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                setDebugWeap();
            }
        }
        */



        // public static void changeWeapon(Player _player,PlayerWeapon playerWeapon)
        //{
        //  instance.playerDataList.setPlayerWeapon(_player, playerWeapon);
        //}

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one GameManager in scene.");
            }
            else
            {
                instance = this;
            }

        }


        public void SetSceneCameraActive(bool isActive)
        {
            if (sceneCamera == null)
                return;

            sceneCamera.SetActive(isActive);
        }

        #region Player tracking

        private const string PLAYER_ID_PREFIX = "Player ";

        private static Dictionary<string, Player> players = new Dictionary<string, Player>();

        public static void RegisterPlayer(string _netID, Player _player, string playerName)
        {
            string _playerID = PLAYER_ID_PREFIX + _netID;
            players.Add(_playerID, _player);
            _player.transform.name = _playerID;
            instance.playersList.Add(_player);

            //instance.worldWeapons.Add(_player.GetComponent<WeaponManager>().primaryWeapon);
            //instance.weaponsList.Add(_player.GetComponent<WeaponManager>().currentWeapon);

            // instance.playerDataList.Add(_player, _player.GetComponent<WeaponManager>().primaryWeapon);
        }

        public static void UnRegisterPlayer(string _playerID)
        {
            try
            {
                //instance.playerDataList.Remove(GetPlayer(_playerID));
                players.Remove(_playerID);
                // instance.playersList.Remove(GetPlayer(_playerID));
            }
            catch (Exception e)
            {
                Debug.Log("UnRegisterPlayer Exception::-" + e.ToString());
            }

        }

        public static Player GetPlayer(string _playerID)
        {
            return players[_playerID];
        }

        public Player FindLocalPlayer()
        {
            if (playersList.Count == 1)
                return playersList[0];
            foreach (Player _player in playersList)
            {
                if (_player.isLocalPlayer)
                {
                    return _player;
                }
            }
            Debug.LogError("NO LOCAL PLAYER!");
            return null;
        }
        /*
        public void printListVisualDeubegger()
        {
            foreach (PlayerData playerData in playerDataList.dataList)
            {
                if (playerData != null)
                {
                    t.text += "[ " + playerData.player.name + ", Weapon: " + playerData.weapon.weaponName +" ]";
                }
            }
        }
        */

        /*
        void OnGUI ()
        {
           GUILayout.BeginArea(new Rect(200, 200, 200, 500));
           GUILayout.BeginVertical();

           foreach (string _playerID in players.Keys)
            {
                GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
            }

           GUILayout.EndVertical();
           GUILayout.EndArea();
        }
        */

    }

    #endregion

}
