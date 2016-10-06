using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChatController : NetworkBehaviour {
    public static ChatController instance;
    [SerializeField] Text chatText;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] Scrollbar chatScrollbar;
    [SerializeField] InputField chatInput;
    [SerializeField] Animator chatWindowAnimator;
    public GameObject chatWindow;

    public static bool isOn;
    void Awake()
    {
        Censor.InitCensor();
        isOn = false;
        instance = this;
    }
    public void Send(string data)
    {
        if (data.Length == 0)
            return;
        string _data = Censor.CensorData(data);
        playerUI.controller.GetComponent<Messanger>().CmdSendChat(GameManager.GetPlayer(playerUI.controller.transform.name).playerName, _data);
    }

    public void DragDownScrollBar()
    {
        chatScrollbar.value = 0f;
    }

    public void CleanChatText()
    {
        chatInput.text = "";
    }

    [Command]
    void CmdSendMessage(string data)
    {
        RpcChat(data);
    }

    [ClientRpc]
    void RpcChat(string _data)
    {
        chatText.text += playerUI.controller.transform.name + ": " + _data + "\n";
    }

    public void AddText(string senderName,string _data)
    {
        chatText.text += senderName + ": " + _data + "\n";
    }

    public void ToggleChaWindow()
    {
        //chatInput.gameObject.SetActive(!chatInput.gameObject.activeSelf);
        Debug.Log("ToggleChaWindow");
        //PauseMenu.IsOn = pauseMenu.activeSelf;

        if (chatInput.gameObject.activeSelf)
        {
            isOn = false;
            playerUI.playerMotor.enabled = true;
            chatWindowAnimator.SetTrigger("Fold");
            StartCoroutine(DragDownScrollBarRoutine(0.5f));
            //DragDownScrollBar();
           // Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            isOn = true;
            playerUI.playerMotor.enabled = false;
            chatWindowAnimator.SetTrigger("Open");
           // Cursor.lockState = CursorLockMode.None;
            StartCoroutine(DragDownScrollBarRoutine(0.7f));
            //Cursor.visible = true;
        }
        
        //playerMotor.StopMovingAndRotating();
    }

    IEnumerator DragDownScrollBarRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("DRAG NOW");
        SetFocusInputField();
        DragDownScrollBar();
    }

    public void SetFocusInputField()
    {
        chatInput.ActivateInputField();
    }
}



public class Censor
{
    public static List<string> dirtyWords;
    //public static Censor instance;

    public static void InitCensor()
    {
        Debug.Log("InitCensor");
        dirtyWords = new List<string>();
        dirtyWords.Add("bitch");
        dirtyWords.Add("fuck");
        dirtyWords.Add("cock");
        dirtyWords.Add("tits");
        dirtyWords.Add("shit");
    }
    /*
    public static string CensorData(string data)
    {
        foreach (string word in dirtyWords)
        {
            data=data.Replace(word, "****");
            //Debug.Log("WORD IS: " +word +" NOW DATA IS: " + data);
        }
        // SO FAR NO SENSORING
        //Debug.Log(" DATA  RETURNED IS: " + data);
        return data;
    }
    */
    public static string CensorData(string data)
    {
        foreach (string word in dirtyWords)
        {
            data = Regex.Replace(data, word, "****", RegexOptions.IgnoreCase);
            //Debug.Log("WORD IS: " +word +" NOW DATA IS: " + data);
        }
        // SO FAR NO SENSORING
        //Debug.Log(" DATA  RETURNED IS: " + data);
        return data;
    }
}
