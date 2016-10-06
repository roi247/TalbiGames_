using UnityEngine;
using System.Collections;
using MultiplayerFps;

public class PlayerNameTextControl : MonoBehaviour {
    [SerializeField] Player player;
    Player localPlayer;
    Vector3 pivotVector;
	// Use this for initialization

	void Start ()
    {
        localPlayer = GameManager.instance.FindLocalPlayer();
        pivotVector = new Vector3(0, 180, 0);
    }

    void Update()
    {
        //SET THE NAME
        if (player.nameText.text != player.playerName)
        {
            player.nameText.text = player.playerName;
        }
        //Face Local Player camera
        if (!player.isLocalPlayer)
        {
            transform.LookAt(localPlayer.transform);
            transform.Rotate(pivotVector);
        }
    }
}
