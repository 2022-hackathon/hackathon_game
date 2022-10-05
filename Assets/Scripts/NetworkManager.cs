using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get { return _instance; }
    }
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
        PhotonNetwork.LocalPlayer.NickName = "A" + Random.Range(0, 1000).ToString();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Master");
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
        form.AddField("pw", pw);
        
        

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.154.124:8080/login", form);
        yield return www.SendWebRequest();

        string response = www.downloadHandler.text;
        Debug.Log(response);


    }
}
