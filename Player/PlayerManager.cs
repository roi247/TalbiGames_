using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

    public bool isLocalHost = false;
    public string playerName = " ";
    public static PlayerManager instance;

    void Awake()
    {
        //if (isLocalPlayer)
            instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
