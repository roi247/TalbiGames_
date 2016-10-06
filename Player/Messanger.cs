using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Messanger : NetworkBehaviour {

    [Command]
    public void CmdSendChat(string data,string senderName)
    {
        RpcSendChat(senderName,data);
    }

    [ClientRpc]
    void RpcSendChat(string data,string senderName)
    {
        ChatController.instance.AddText(senderName,data);
        ChatController.instance.DragDownScrollBar();
    }
}
