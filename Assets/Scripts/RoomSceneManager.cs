using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    [Header("ETC")]
    [SerializeField] private TMP_InputField roomNameInputField; // �� �̸� �Է� �ʵ�
    [SerializeField] private TMP_InputField maxPlayerInputField; // �� �̸� �Է� �ʵ�


    [SerializeField] private Button[] roomButtons; // �� ��ư
    [SerializeField] private Button previousButton; // ���� ��ư
    [SerializeField] private Button nextButton; // ���� ��ư

    List<RoomInfo> roomList = new List<RoomInfo>(); // �� ����Ʈ

    int currentPage = 1; // ���� �� ������
    int multiple; // �� ��°
    int maxPage; // �ִ� �� ������


    private void Start()
    {
        //RoomListRenewal();
    }
    private void Update()
    {
    }
    
    #region ��ư

    /// <summary>
    /// ������ ��ư�� ������ �� ����Ǵ� �Լ�
    /// </summary>
    public void OnExitButton()
    {
        PhotonNetwork.Disconnect(); // ���� ���� ����
        Application.Quit(); // ���� ����
    }

    /// <summary>
    /// ���� ��ư�� ������ �� ����Ǵ� �Լ�
    /// </summary>
    public void OnCreateRoomButton()
    {
        if (roomNameInputField.text.Length == 0 || maxPlayerInputField.text.Length == 0)
            return;
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = byte.Parse(maxPlayerInputField.text) });
    }

    public void ShowIt(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void HideIt(GameObject obj)
    {
        obj.SetActive(false);
    }
    #endregion

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Create or Join");
        PhotonNetwork.LoadLevel("GameScene");
    }


    #region ��

    /// <summary>
    /// �� ����Ʈ�� ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="num">������ ��ȣ</param>
    public void OnRoomList(int num)
    {
        // ����ư -2 , ����ư -1 , �� ����
        if (num == -2) --currentPage; // �� ����Ʈ �������� �ѱ��
        else if (num == -1) ++currentPage; // �� ����Ʈ ���������� �ѱ��
        else PhotonNetwork.JoinRoom(roomList[multiple + num].Name); // �濡 ����
        RoomListRenewal(); // �� ����Ʈ ����
    }

    /// <summary>
    /// �� ����Ʈ�� �����ϴ� �Լ�
    /// </summary>
    public void RoomListRenewal()
    {
        // �ִ�������
        maxPage = (roomList.Count % roomButtons.Length == 0) ? roomList.Count / roomButtons.Length : roomList.Count / roomButtons.Length + 1;

        // ����, ������ư
        previousButton.interactable = (currentPage <= 1) ? false : true;
        nextButton.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * roomButtons.Length;
        for (int i = 0; i < roomButtons.Length; i++)
        {
            roomButtons[i].interactable = (multiple + i < roomList.Count) ? true : false;
            roomButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < roomList.Count) ? roomList[multiple + i].Name : "";
            roomButtons[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < roomList.Count) ? roomList[multiple + i].PlayerCount + "/" + roomList[multiple + i].MaxPlayers : "";
        }
    }

    /// <summary>
    /// ���� ������Ʈ ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Update");

        int roomCount = roomList.Count; // �� ���� �ʱ�ȭ
        Debug.Log(roomCount);
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!this.roomList.Contains(roomList[i])) this.roomList.Add(roomList[i]);
                else this.roomList[this.roomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (this.roomList.IndexOf(roomList[i]) != -1) this.roomList.RemoveAt(this.roomList.IndexOf(roomList[i]));
        }
        RoomListRenewal(); // �� ����Ʈ ����
    }

    #endregion 
}
    