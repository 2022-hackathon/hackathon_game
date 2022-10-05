using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI    ;

public class IngameManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    [Header("Chat")]
    public GameObject chatPanel;
    public TMP_InputField chatInputfield;
    public TMP_Text[] chatTexts;

    public static bool isChat;
    public Coroutine chatCor;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        chatInputfield.ActivateInputField();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
                chatInputfield.ActivateInputField();


            if (chatInputfield.gameObject.activeSelf)
            {
                chatInputfield.gameObject.SetActive(false);

                if (chatInputfield.text.Length != 0)
                {

                    PV.RPC("SendChatRPC", RpcTarget.All, PhotonNetwork.NickName + ": " + chatInputfield.text);

                    if (chatCor != null)
                        StopCoroutine(chatCor);

                    chatCor = StartCoroutine(chatCo());
                    chatInputfield.text = "";
                }

                isChat = false;

            }
            else
            {


                if (chatCor != null)
                    StopCoroutine(chatCor);



                isChat = true;
                chatInputfield.gameObject.SetActive(true);
                chatPanel.SetActive(true);
            }

        }
    }

    IEnumerator chatCo()
    {
        yield return new WaitForSeconds(8);

        if(chatPanel.activeSelf)
            chatPanel.SetActive(false);

        for (int i = 0; i < chatTexts.Length; i++)
        {
            chatTexts[i].text = "";
        }
        chatCor = null;
    }
    [PunRPC]
    public void SendChatRPC(string chat)
    {
        chatPanel.SetActive(true);
        bool isInput = false;

        
        for (int i = 0; i < chatTexts.Length; i++)
        {
            if (chatTexts[i].text == "")
            {
                isInput = true;
                chatTexts[i].text = chat;
                break;
            }
        }
        if (!isInput)
        {
            for (int i =0; i < chatTexts.Length-1; i++)
                chatTexts[i].text = chatTexts[i+1].text;

                chatTexts[chatTexts.Length - 1].text = chat;
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("SendChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "¥‘¿Ã ¬¸∞°«œºÃΩ¿¥œ¥Ÿ</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("SendChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "¥‘¿Ã ≈¿Â«œºÃΩ¿¥œ¥Ÿ</color>");
    }
}
