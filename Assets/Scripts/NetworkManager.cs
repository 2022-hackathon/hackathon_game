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

    public List<TDRank> rankInfo = new List<TDRank>();
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
    }


    public void connectToMasterFunc()
    {
        PhotonNetwork.ConnectUsingSettings();

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

    
        
        

        UnityWebRequest www = UnityWebRequest.Get("http://192.168.72.124:8080/gamegetuser?id=" +id);
        yield return www.SendWebRequest();

        string response = www.downloadHandler.text;
        Debug.Log(response);
        string status = response.Substring(10, 3);
        Debug.Log(status);
        response = response.Substring(21, response.Length-22);
        Debug.Log(response);

        TDUser user = JsonUtility.FromJson<TDUser>(response);

        if (user.nickname.Length != 0)
        {
            Debug.Log(user.money);
            Debug.Log(user.nickname);
            GameManager.userData.nickname = user.nickname;
            GameManager.userData.money = Int32.Parse(user.money);
            GameManager.userData.id = id;
            PhotonNetwork.LocalPlayer.NickName = GameManager.userData.nickname;

            SceneManager.LoadScene("RoomScene");
        }
    }

    public void DataSave()
    {
        StartCoroutine(saveCo());
    }
    public IEnumerator saveCo()
    {
        yield return null;




        WWWForm form = new WWWForm();
        form.AddField("Id", GameManager.userData.id);
        form.AddField("nickname", GameManager.userData.nickname);
        form.AddField("Money", GameManager.userData.money);

        Debug.Log("MONEY " + GameManager.userData.money);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.72.124:8080/savemoney", form);
        yield return www.SendWebRequest();


        string a =  www.downloadHandler.text;

        Debug.Log(a);

    }

    public void LoadRank()
    {
        StartCoroutine(rankCo());
    }

    public IEnumerator rankCo()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://192.168.72.124:8080/getrank");

        yield return www.SendWebRequest();

        string response = www.downloadHandler.text;

        rankInfo.Clear();

        var rank = JsonHelper.FromJson<TDRank>(response);

        foreach (var item in rank)
        {
            rankInfo.Add(item);
        }
    }


}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
}