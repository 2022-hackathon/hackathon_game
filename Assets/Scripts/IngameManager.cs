using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI    ;

public class IngameManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public static IngameManager Instance;
    [Header("Chat")]
    public GameObject chatPanel;
    public TMP_InputField chatInputfield;
    public TMP_Text[] chatTexts;

    public static bool isChat;
    public Coroutine chatCor;

    public PlayerController myController;

    public TMP_Text coinText;
    [Header("Rank")]
    public GameObject rankingPanel;
    public GameObject[] ranks;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
    }

    private void Update()
    {

        if (PlayerController._isActivity)
        {
            chatPanel.SetActive(false);
            return;
        }

        coinText.text = GameManager.userData.money.ToString();

        if (Input.GetKeyDown(KeyCode.Return))
        {
                chatInputfield.ActivateInputField();


            if (chatInputfield.gameObject.activeSelf)
            {
                chatInputfield.gameObject.SetActive(false);

                if (chatInputfield.text.Length != 0)
                {
                    myController.Chatting("\n" + chatInputfield.text);
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
        yield return new WaitForSeconds(5);

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
        StartCoroutine(chatCo());
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("SendChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "¥‘¿Ã ≈¿Â«œºÃΩ¿¥œ¥Ÿ</color>");
        StartCoroutine(chatCo());
    }

    public void rank()
    {
        NetworkManager.Instance.LoadRank();


        for (int i = 0; i < NetworkManager.Instance.rankInfo.Count; i++)
        {
            ranks[i].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = NetworkManager.Instance.rankInfo[i].nickname;
            ranks[i].gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = NetworkManager.Instance.rankInfo[i].money.ToString();
        }
    }



    public void ShowIt(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void HideIt(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void exitRank()
    {
        PlayerController._isActivity = false;
    }

    public void ExitRoom()
    {
        PlayerController._isActivity = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("RoomScene");
    }
}
