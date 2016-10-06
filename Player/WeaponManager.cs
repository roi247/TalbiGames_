using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SyncVar]
    public string currentWeaponName;

    //[SyncVar]
    //public SyncListString myList =new SyncListString();
    //[SyncVar]
    // public string weapons;

    //[SyncVar]
    //public SyncListString playerWeapons;
    //SyncListString playerWeapons;

    public GameObject redBall;
    public GameObject blueBall;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    public PlayerWeapon primaryWeapon;

    public PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    public AudioSource currentWeaponAudio;

    public List<PlayerWeapon> playerWeaponsList;

    public List<PlayerWeapon> playerAviableWeaponsList;

    private PlayerWeapon weaponIns = null;

    public PlayerWeapon nextWeapon;

    void Start()
    {
        if (isLocalPlayer)
        {
            nextWeapon = primaryWeapon;
            ////CmdEquipWeapon();
            EquipWeapon(nextWeapon);
        }
        else
        {
            //Player playerScriptRef = GetComponent<Player>();
            nextWeapon = FindCurrentWeapon();
            /////Debug.Log("remotePlayersNextWeapon- " + remotePlayersNextWeapon.weaponName);
            EquipWeapon(nextWeapon);

        }
        camOldFieldOfView = GetComponent<Player>().playerCamera.fieldOfView;

    }

    void OnJoinedRoom()
    {
        //GameManager.instance.messanger.AddToGameInfoText(transform.name + "JOINDED THE GAME" , 3f);
        Debug.Log("OnJoinedRoom() :" + transform.name + " Has Joined a Room");

    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public bool HasWeapon(string _weaponName)
    {
        foreach (PlayerWeapon _weapon in playerWeaponsList)
        {
            if (_weapon.weaponName == _weaponName)
                return true;
        }
        return false;
    }

    public void SwitchWeapon()
    {
        CmdEquipWeapon(FindNextWeaponName(currentWeapon.weaponName));
    }

    float camOldFieldOfView;


    void EquipWeapon(PlayerWeapon _weapon)
    {
        if (_weapon == null)
        {
            Debug.Log("_weapon is NULL !!!");
            return;
        }

        if (weaponIns != null)
        {
            weaponIns.GetComponentInChildren<Renderer>().enabled = false;
            Destroy(weaponIns.gameObject);
        }


        //currentWeapon = _weapon;
        //GameManager.changeWeapon(GetComponent<Player>(), nextWeapon);
        currentWeaponName = _weapon.weaponName;
        weaponIns = Instantiate(_weapon, weaponHolder.position, weaponHolder.rotation) as PlayerWeapon;
        weaponIns.transform.SetParent(weaponHolder);
        currentWeapon = weaponIns;

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
            Debug.LogError("No WeaponGraphics component on the weapon object: " + weaponIns.name);

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns.gameObject, LayerMask.NameToLayer(weaponLayerName));
            //StringListManager.Add(ref weapons, _weapon.weaponName);
            //Debug.Log("WEAPONS --------->>>>> " + weapons);
        }
        //StringListManager.Get(weapons, 0);
        //printSyncList();

        GetComponent<AudioSource>().clip = currentWeapon.weaponAudio.clip;


        /////int weaponInd = (playerWeaponsList.IndexOf(_weapon) + 1) % playerWeaponsList.Count;
        //// nextWeapon = playerWeaponsList[weaponInd];
        //  playerWeapons.Add(_weapon.weaponName);

        currentWeapon.SetPlayerShooting(GetComponent<PlayerShoot>());


    }

    [Command]
    void CmdEquipWeapon(string _name)
    {
        RpcEquipWeapon(_name);
        //RpcWeaponChangeGameManager();
    }

    [ClientRpc]
    void RpcEquipWeapon(string _name)
    {
        nextWeapon = GetWeaponByName(_name);
        EquipWeapon(nextWeapon);
    }

    [Command]
    public void CmdDebugTempBall(int num)
    {
        RpcDebugTempBall(num);
    }

    public void DebugTempBall(int op)
    {
        if (op == 1)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y * 4, transform.position.z);
            Instantiate(blueBall, pos, Quaternion.identity);
        }
        else if (op == 2)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y * 3, transform.position.z);
            Instantiate(redBall, pos, Quaternion.identity);
        }
    }

    [ClientRpc]
    void RpcDebugTempBall(int op)
    {
        //DEBUG
        if (op == 1)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y * 4, transform.position.z);
            Instantiate(blueBall, pos, Quaternion.identity);
        }
        else if (op == 2)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y * 3, transform.position.z);
            Instantiate(redBall, pos, Quaternion.identity);
        }

    }

    public void RemoveLastWeapon()
    {
        string weaponNameToRemove = "";
        List<PlayerWeapon> _playerWeapons = new List<PlayerWeapon>(playerWeaponsList);
        if (playerWeaponsList.Count < 2)
            return;
        else if (currentWeaponName == primaryWeapon.name)
        {
            //FIXXXXXXXXXXXX
            Debug.Log(transform.name + " IS HOLDING PRIMARY WEAPON!!!");
            playerWeaponsList.Clear();
            for (int i = 0; i < _playerWeapons.Count - 1; i++)
            {
                playerWeaponsList.Add(_playerWeapons[i]);
            }
            SwitchWeapon();
            return;
        }
        else
        {
            //Remove the weapon he is currently holding
            weaponNameToRemove = currentWeaponName;
        }


        playerWeaponsList.Clear();

        foreach (PlayerWeapon _weapon in _playerWeapons)
        {
            if (_weapon.weaponName != weaponNameToRemove)
            {
                try
                {
                    playerWeaponsList.Add(_weapon);
                }
                catch (System.Exception)
                {
                    Debug.Log("EXCEPTION WHILE REMOVING WEAPON");
                }
                //Debug.Log("REMOVED WEAPON: " + _weapon.weaponName);
            }
        }
        SwitchWeapon();
    }

    public void RemoveAllWeapons()
    {
        if (playerWeaponsList.Count < 2)
            return;

        try
        {
            playerWeaponsList.Clear();
            playerWeaponsList.Add(primaryWeapon);

        }
        catch (System.Exception)
        {
            Debug.Log("EXCEPTION WHILE REMOVING WEAPON");
        }
        SwitchWeapon();
    }

    [Command]
    public void CmdAddWeapon(string name)
    {
        RpcAddWeapon(name);
    }

    [ClientRpc]
    void RpcAddWeapon(string name)
    {
        //currentWeaponName = name;
        //DebugTempBall(Color.blue);
        foreach (PlayerWeapon playerWeapon in playerAviableWeaponsList)
        {
            if (playerWeapon.weaponName == name)
            {
                if (!playerWeaponsList.Contains(playerWeapon))
                {
                    Debug.Log("FOUND WEAPON!!! - " + playerWeapon.weaponName);
                    playerWeaponsList.Add(playerWeapon);
                    //nextWeapon = playerWeapon;
                    // CmdEquipWeapon(nextWeapon.weaponName);
                    //playerAviableWeaponsList.Remove(playerWeapon);
                }
            }
        }
        CmdEquipWeapon(name);
    }

    PlayerWeapon FindCurrentWeapon()
    {
        if (currentWeaponName == null || currentWeaponName.Length == 0)
        {
            //GameManager.instance.t.text += "RETURN - primaryWeapon !!!";
            return primaryWeapon;

        }
        foreach (PlayerWeapon _weapon in playerAviableWeaponsList)
        {
            if (_weapon.weaponName == currentWeaponName)
            {
                Debug.Log("WEAPON RETURNED: " + _weapon.weaponName);
                return _weapon;
                // returnedWeapon = _weapon;
            }
        }
        return primaryWeapon;
    }

    PlayerWeapon GetWeaponByName(string _name)
    {
        foreach (PlayerWeapon _weapon in playerAviableWeaponsList)
        {
            if (_weapon.weaponName == _name)
            {
                Debug.Log("WEAPON RETURNED: " + _weapon.weaponName);
                return _weapon;
                // returnedWeapon = _weapon;

            }
        }
        Debug.LogError("NO WEAPON WITH THAT NAME!! ->" + _name);
        return null;
    }

    public string FindNextWeaponName(string currentWeapon)
    {
        bool hasFoundCurrent = false;
        foreach (PlayerWeapon _weapon in playerWeaponsList)
        {
            if (_weapon.weaponName == currentWeapon || hasFoundCurrent)
            {
                if (hasFoundCurrent)
                {
                    Debug.Log("WEAPON RETURNED: " + _weapon.weaponName);
                    return _weapon.weaponName;
                }
                hasFoundCurrent = true;
                // returnedWeapon = _weapon;

            }
        }
        Debug.Log("COUNDT FIND NEXT WEAPON OR NEXT ONE IS FIRST");
        return playerWeaponsList[0].weaponName;
    }

    /*
    void printSyncList()
    {
        GameManager.instance.t.text += "[ ";
        foreach (string weap in myList)
        {
            GameManager.instance.t.text += weap.ToString() + " ";
        }
        GameManager.instance.t.text += " ]";
    }
    */

}


public class StringListManager
{
    public static void Add(ref string list, string weap)
    {
        if (!list.Contains(weap))
        {
            list += "/" + weap;
        }
    }
    public static string Get(string list, int index)
    {
        string[] pieces = list.Split('/');
        /*
        int i = 0;
        foreach (string str in pieces)
        {
            Debug.Log("i=" + i.ToString() + "-> "+str);
            i++;
        }
        */

        if (pieces.Length > (index + 1))
            return pieces[index + 1];
        else
            return " ";


    }

    public static void Switch(ref string list)
    {

    }


}


/*
* 
* 
* 
* 
* 
* 
* 
* 
*     PlayerWeapon FindCurrentWeapon()
{
    PlayerWeapon returnedWeapon = null;
    bool exsistInplayerAviableWeaponsList = false;
    bool exsistInPlayerWeaponsList = false;
    //GameManager.instance.t.text += "FindCurrentWeapon: " + currentWeaponName.ToString();
    if (currentWeaponName == null || currentWeaponName.Length == 0)
    {
        //GameManager.instance.t.text += "RETURN - primaryWeapon !!!";
        return primaryWeapon;
           
    }


    foreach (PlayerWeapon _weapon in playerWeaponsList)
    {
        if (_weapon.weaponName == currentWeaponName)
        {
            Debug.Log("WEAPON RETURNED: " + _weapon.weaponName);
            exsistInPlayerWeaponsList = true;
            if (!playerWeaponsList.Contains(_weapon))
                CmdAddWeapon(_weapon.weaponName);
            return _weapon;
            // returnedWeapon = _weapon;
              
        }
    }

    foreach (PlayerWeapon _weapon in playerAviableWeaponsList)
    {
        if (_weapon.weaponName==currentWeaponName)
        {
            Debug.Log("WEAPON RETURNED: " + _weapon.weaponName);
            //return _weapon;
            returnedWeapon = _weapon;
            if (!playerWeaponsList.Contains(_weapon))
                CmdAddWeapon(_weapon.weaponName);
            exsistInplayerAviableWeaponsList = true;
            return returnedWeapon;
        }
    }

    return primaryWeapon;
}
* 
* 
* 
* 
* 
* 
* 
* 
* 
* */
