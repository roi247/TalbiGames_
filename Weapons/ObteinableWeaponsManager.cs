using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

public class ObteinableWeaponsManager : NetworkBehaviour
{
    //public PlayerWeapon temp;
    public static List<ObteinableWeaponData> obteinableWeaponsList;
    public WeaponSpawnPoint[] spawnPoints;
    public PlayerWeapon[] aviableWeapons; 

    public static ObteinableWeaponsManager instance;
    public int[] indexList;
    public string[] nameList;

    [SyncVar]
    public string data;

    [SyncVar]
    public bool hasStarted;

    public void Remove(int _spawnPointIndex)
    {
        WeaponLocationDataParser.Remove(ref data, _spawnPointIndex);
    }


    IEnumerator StartSpawnerAI()
    {
        Debug.Log("Start >>>>> StartSpawnerAI");
        yield return new WaitForSeconds(WeaponSpawnerData.spawnInitialDelay);
        while (true)
        {
            int _freePointIndex;
            try
            {
                string _nextWeapon;
                //Debug.Log(">>>>>><<<<<<<<<");
                _freePointIndex = WeaponSpawnerData.GetFreeSpot(spawnPoints);
                if (spawnPoints[_freePointIndex].prefferableWeapon == null) //no prefferableWeapon for this spawn point
                    _nextWeapon = WeaponSpawnerData.GetNextWeapon(aviableWeapons);
                else
                    _nextWeapon = spawnPoints[_freePointIndex].prefferableWeapon.weaponName;
                CmdSpawnNewWeapon(_freePointIndex, _nextWeapon);
            }
            catch (System.OverflowException)
            {
                Debug.Log("Spawn Points List is full!");
            }
            catch (System.ArgumentException)
            {
                Debug.Log("max weapons aviable is invalid");
            }
            catch (System.Exception)
            {
                Debug.Log("Another Exception trying to get free spawn point");
            }

            yield return new WaitForSeconds(WeaponSpawnerData.GetNextDelay());
        }
    }

    [Command]
    public void CmdSpawnNewWeapon(int _spawnPointIndex,string _name)
    {
        Debug.Log("New Weapon is spawned. -> Weapon: " + _name + " At Spawn point number: " + _spawnPointIndex.ToString());
        RpcSpawnNewWeapon(_spawnPointIndex, _name);
    }

    [ClientRpc]
    public void RpcSpawnNewWeapon(int _spawnPointIndex, string _name)
    {
        WeaponLocationDataParser.Add(ref data, _spawnPointIndex, _name);
        PlayerWeapon _weaponIns = Instantiate(GetWeaponWithName(_name), spawnPoints[_spawnPointIndex].transform.position, Quaternion.identity) as PlayerWeapon;
        spawnPoints[_spawnPointIndex].containedWeapon = _weaponIns;
        spawnPoints[_spawnPointIndex].isTaken = true;
        _weaponIns.transform.SetParent(spawnPoints[_spawnPointIndex].transform);
    }

    // Use this for initialization
    void Awake()
    {
        obteinableWeaponsList = new List<ObteinableWeaponData>();
        if (instance == null)
            instance = this;

        //string data = "";

        //WeaponLocationDataParser.Extract(data, ref indexList, ref nameList);

        Debug.Log("before --> " +data);
        //data = WeaponLocationDataParser.Remove(data, 2);
        //WeaponLocationDataParser.Extract(data, ref indexList, ref nameList);
        //Debug.Log(data);
    }

    void Start()
    {
        //InvokeRepeating("bla", 4, 4);
        if (data == null || data.Length == 0)
        {
            //StartTHIS();
            /*
           WeaponLocationDataParser.Add(ref data, 0, "ShotGun");
           WeaponLocationDataParser.Add(ref data, 1, "Sniper");
           WeaponLocationDataParser.Add(ref data, 2, "RocketLuncher");
           WeaponLocationDataParser.Add(ref data, 3, "ShotGun");
           */
            ;
        }
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(StartSpawnerAI());
        }
            

    }

    PlayerWeapon GetWeaponWithName(string _weaponName)
    {
        foreach (PlayerWeapon _weapon in aviableWeapons)
        {
            if (_weapon.weaponName==_weaponName)
            {
                return _weapon;
            }
        }
        Debug.LogError("NO WEAPON WITH THAT NAME: " +_weaponName);
        return null;
    }


    public void SpawnWeaponsForNewPlayer()
    {
        WeaponLocationDataParser.Extract(data, ref indexList, ref nameList);
        for (int i = 0; i < indexList.Length; i++)
        {
            PlayerWeapon _weapon = GetWeaponWithName(nameList[i]);
            WeaponSpawnPoint _point = spawnPoints[indexList[i]];
            PlayerWeapon _weaponIns =Instantiate(_weapon, _point.transform.position, Quaternion.identity) as PlayerWeapon;
            _point.containedWeapon = _weaponIns;
            //_point.spawnPointIndex = indexList[i];
            _weaponIns.transform.SetParent(_point.transform);
            _point.isTaken = true;
        }
    }

    [Command]
    public void CmdChangeBlaa(string addedData)
    {
        data += addedData;
    }


    public void printBlaaa()
    {
        GameManager.instance.t.text += " [ " + data + " ] \n" ;
    }

    void bla2()
    {
       
    }

    void bla()
    {
        /*
        Instantiate(temp, new Vector3(Random.Range(0, 4), 1.2f, 0), Quaternion.identity);
        //NetworkServer.Spawn(weap.gameObject);
        CmdChangeBlaa(" _BB ");
        */
    }

    public static void Remove(PlayerWeapon weapon)
    {
        foreach (ObteinableWeaponData data in obteinableWeaponsList)
        {
            if (data.weapon == weapon)
            {
                try
                {
                    obteinableWeaponsList.Remove(data);
                }
                catch (System.Exception)
                {
                    Debug.Log("EXCEPTION AT REMOVING FROM obteinableWeaponsList ");
                }
            }
        }
    }

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            printToVisualDebuggerAllWeapons();
        }
    }
    */

    void printToVisualDebuggerAllWeapons()
    {
        foreach (PlayerWeapon data in GameManager.instance.worldWeapons)
        {
            GameManager.instance.t.text += "[ " + data.weaponName + " ] ";
        }

        /*
        foreach (ObteinableWeaponData data in obteinableWeaponsList)
        {
             GameManager.instance.t.text+= "[ " + data.weapon.weaponName + " ] ";
        }

    */
        GameManager.instance.t.text += "\n";
    }

    public static void Add(PlayerWeapon weapon,Transform location)
    {
       obteinableWeaponsList.Add(new ObteinableWeaponData(weapon, location));
    }


    public static void InstantiateWeapons()
    {
        foreach (ObteinableWeaponData data in obteinableWeaponsList)
        {
            Instantiate(data.weapon, data.location.position, Quaternion.identity);
        }
    }

}



[System.Serializable]
public class ObteinableWeaponData
{
    public PlayerWeapon weapon;
    public Transform location;

    public ObteinableWeaponData(PlayerWeapon _weap, Transform _loc)
    {
        weapon = _weap;
        location = _loc;
    }

}


[System.Serializable]
public class WeaponLocationDataParser
{
    public int[] spawnPoints;
    public string weaponNames;

    public static void Extract(string data, ref int[] indexList,ref string[] nameList)
    {
        indexList=null;
        nameList = null;
        string[] pieces = data.Split('/');
        indexList = new int[pieces.Length-1];
        nameList = new string[pieces.Length-1];
        int i = 0;
        foreach (string str in pieces)
        {
            if (str == null || str.Length < 2)
                continue;
            string[] parts = str.Split(',');
            int currentNum = int.Parse(parts[0]);
            string currentName = parts[1];
            indexList[i] = currentNum;
            nameList[i] = currentName;
            // Debug.Log("Number is: " + currentNum.ToString() + " Wapon Name: " + currentName);
            ++i;
        }
    }
    public static void Remove(ref string data,int index)
    {
        //Debug.Log("Data was: " + data + "Removing spawn point number : " + index.ToString());

        string[] pieces = data.Split('/');
        int[] indexList = new int[pieces.Length - 1];
        string[] nameList = new string[pieces.Length - 1];
        int i = 0;
        foreach (string str in pieces)
        {
            if (str == null || str.Length < 2)
                continue;
            string[] parts = str.Split(',');
            int currentNum = int.Parse(parts[0]);
            string currentName = parts[1];
            indexList[i] = currentNum;
            nameList[i] = currentName;
            //Debug.Log("Number is: " + currentNum.ToString() + " Wapon Name: " + currentName);
            ++i;
        }
        //NOW ADD ALL BACK BUT THE INDEX REMOVED
        i = 0;
        string newData = "";
        foreach (int ind in indexList)
        {
            if (ind!=index)
            {
                 Add(ref newData, ind, nameList[i]);
            }
            ++i;
        }
        data= newData;
    }

    public static void Add(ref string data, int spawnPointIndex,string weaponName)
    {
        //Debug.Log("Data was: " + data + "Adding weapon: " +weaponName +" at spawn point: " + spawnPointIndex.ToString());
        string rv = data+ "/" + spawnPointIndex.ToString() + "," + weaponName;
        //Debug.Log("NOW DATA IS: " + rv);
        data = rv;
    }
}

public class WeaponSpawnerData
{
    public static float spawnInitialDelay=2f;
    public static float spawDelayMin=5f;//8
    public static float spawDelayMax=10f;//12

    public static int maxWeaponsAtTheMoment = 9;

    public static float GetNextDelay()
    {
        //in the meantime - a pretty simple spawning function
        float randDelay = Random.Range(spawDelayMin, spawDelayMax);
        return randDelay;
    }

    public static string GetNextWeapon(PlayerWeapon[] _weaponList)
    {
        //in the meantime - a pretty simple spawning function
        int _rand = Random.Range(0,_weaponList.Length);
        return (_weaponList[_rand].weaponName);
    }

    public static int GetFreeSpot(WeaponSpawnPoint[] _spawnPoints)
    {
        //First check if list is full
        int takedPointsCount=0;
        //bool isListFull = true;
        foreach (WeaponSpawnPoint _point in _spawnPoints)
        {
            if (_point.isTaken)
            {
                ++takedPointsCount;
                //isListFull = false;
               //break;
            }
            //return _point.spawnPointIndex;
        }
        //spawn point list is full
        if (takedPointsCount > maxWeaponsAtTheMoment)
            throw (new System.OverflowException());
        
        if (_spawnPoints.Length < maxWeaponsAtTheMoment)
            throw (new System.ArgumentException());


        int _rand = Random.Range(0, _spawnPoints.Length);

        while (_spawnPoints[_rand].isTaken)
        {
            _rand = Random.Range(0, _spawnPoints.Length);
        }
        return _spawnPoints[_rand].spawnPointIndex;

    }


};
