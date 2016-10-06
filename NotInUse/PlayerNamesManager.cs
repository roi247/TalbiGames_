using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerNamesManager : NetworkBehaviour
{
    public string[] playerIDs;
    public string[] playerNames;

    public static PlayerNamesManager instance;
    [SyncVar]
    public string data;
    // Use this for initialization

    void Awake()
    {
        instance = this;
    }

    public void Add(string playerID, string playerName)
    {
        PlayerNameDataParser.Add(ref data, playerID, playerName);
    }

    public void Remove(string playerID)
    {
        PlayerNameDataParser.Remove(ref data, playerID);
    }
}


[System.Serializable]
public class PlayerNameDataParser
{
    public static void Extract(string data, ref string[] playerIDs, ref string[] playerNames)
    {
        playerIDs = null;
        playerNames = null;

        string[] pieces = data.Split('/');
        playerIDs = new string [pieces.Length - 1];
        playerNames = new string [pieces.Length - 1];
        int i = 0;
        foreach (string str in pieces)
        {
            if (str == null || str.Length < 2)
                continue;
            string[] parts = str.Split(',');
            string currentID = parts[0];
            string currentName = parts[1];
            playerIDs[i] = currentID;
            playerNames[i] = currentName;
            // Debug.Log("Number is: " + currentNum.ToString() + " Wapon Name: " + currentName);
            ++i;
        }
    }
    public static void Remove(ref string data, string playerID)
    {
        //Debug.Log("Data was: " + data + "Removing spawn point number : " + index.ToString());

        string[] pieces = data.Split('/');
        string[] playerIDs = new string[pieces.Length - 1];
        string[] playerNames = new string[pieces.Length - 1];
        int i = 0;
        foreach (string str in pieces)
        {
            if (str == null || str.Length < 2)
                continue;
            string[] parts = str.Split(',');
            string currentID =parts[0];
            string currentName = parts[1];
            playerIDs[i] = currentID;
            playerNames[i] = currentName;
            //Debug.Log("Number is: " + currentNum.ToString() + " Wapon Name: " + currentName);
            ++i;
        }
        //NOW ADD ALL BACK BUT THE INDEX REMOVED
        i = 0;
        string newData = "";
        foreach (string id in playerIDs)
        {
            if (id != playerID)
            {
                Add(ref newData, id, playerNames[i]);
            }
            ++i;
        }
        data = newData;
    }

    public static void Add(ref string data, string playerID, string playerName)
    {
        //Debug.Log("Data was: " + data + "Adding weapon: " +weaponName +" at spawn point: " + spawnPointIndex.ToString());
        string rv = data + "/" + playerID + "," + playerName;
        //Debug.Log("NOW DATA IS: " + rv);
        data = rv;
    }
}