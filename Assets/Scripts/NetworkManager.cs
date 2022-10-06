using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get { return _instance; }
    }

    public bool isConnectMaster;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }


    public override void OnConnectedToMaster()
    {
        isConnectMaster = true;
        PhotonNetwork.JoinLobby();
    }

    public void LoginFunc(string id, string pw)
    {
        StartCoroutine(LoginCo(id, pw));
    }
    public IEnumerator LoginCo(string id, string pw)
    {

        yield return null;

        Debug.Log(id + "/" + pw);
    
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        
        

        UnityWebRequest www = UnityWebRequest.Get("http://192.168.72.124:8080/gamegetuser?id=" +id);
        yield return www.SendWebRequest();

        string response = www.downloadHandler.text;
        Debug.Log(response);

        response = response.Substring(21, response.Length-22);
        Debug.Log(response);

        TDUser user = JsonUtility.FromJson<TDUser>(response);

        Debug.Log(user.money);
        Debug.Log(user.nickname);
        GameManager.userData.nickName = user.nickname;
        GameManager.userData._money = (uint)Int32.Parse(user.money);

        PhotonNetwork.LocalPlayer.NickName = GameManager.userData.nickName;

        SceneManager.LoadScene("RoomScene");
    }

}
