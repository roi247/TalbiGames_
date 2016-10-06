using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize = 6;

    [SerializeField] GameObject[] objectsToEnable;

    [SerializeField] Text ipText;
    // [SerializeField] NetworkManagerHUD hud;
    NetworkManagerHUD hud;
    private string roomName;

	private NetworkManager networkManager;

	void Start ()
	{
        networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
        hud = networkManager.GetComponent<NetworkManagerHUD>();
        if (hud == null)
            Debug.Log("HUD IS NULL!!!!");
        //

    }

	public void SetRoomName (string _name)
	{
		roomName = _name;
	}

	public void CreateRoom ()
	{
		if (roomName != "" && roomName != null)
		{
			Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}

    bool enableLanManagerHUD = true;
    public void ToggleLanManagerHUD()
    {
        hud.showGUI = enableLanManagerHUD;
        ipText.gameObject.SetActive(enableLanManagerHUD);
        if (enableLanManagerHUD)
            networkManager.StopMatchMaker();
        else
            networkManager.StartMatchMaker();
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(!enableLanManagerHUD);
        }
        enableLanManagerHUD = !enableLanManagerHUD;
    }

    public void LoadMainScene(bool allowSceneActivation)
    {
        //SceneManager.LoadSceneAsync("MainLevel03", LoadSceneMode.Additive).allowSceneActivation = allowSceneActivation;
    }

    public void GetLocalIp()
    {
        IPAddress[] IPS = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in IPS)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {

                ipText.text="IP address: " + ip;
            }
        }
    }

}
