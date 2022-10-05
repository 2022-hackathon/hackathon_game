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
    [SerializeField] private TMP_InputField roomNameInputField; // 방 이름 입력 필드
    [SerializeField] private TMP_InputField maxPlayerInputField; // 방 이름 입력 필드


    [SerializeField] private Button[] roomButtons; // 방 버튼
    [SerializeField] private Button previousButton; // 이전 버튼
    [SerializeField] private Button nextButton; // 다음 버튼

    List<RoomInfo> roomList = new List<RoomInfo>(); // 방 리스트

    int currentPage = 1; // 현재 방 페이지
    int multiple; // 방 번째
    int maxPage; // 최대 방 페이지


    private void Start()
    {
        //RoomListRenewal();
    }
    private void Update()
    {
    }
    
    #region 버튼

    /// <summary>
    /// 나가기 버튼을 눌렀을 때 실행되는 함수
    /// </summary>
    public void OnExitButton()
    {
        PhotonNetwork.Disconnect(); // 서버 연결 종료
        Application.Quit(); // 게임 종료
    }

    /// <summary>
    /// 연결 버튼을 눌렀을 때 실행되는 함수
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


    #region 방

    /// <summary>
    /// 방 리스트를 눌렀을 때 실행되는 함수
    /// </summary>
    /// <param name="num">구별할 번호</param>
    public void OnRoomList(int num)
    {
        // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
        if (num == -2) --currentPage; // 방 리스트 왼쪽으로 넘기기
        else if (num == -1) ++currentPage; // 방 리스트 오른쪽으로 넘기기
        else PhotonNetwork.JoinRoom(roomList[multiple + num].Name); // 방에 접속
        RoomListRenewal(); // 방 리스트 갱신
    }

    /// <summary>
    /// 방 리스트를 갱신하는 함수
    /// </summary>
    public void RoomListRenewal()
    {
        // 최대페이지
        maxPage = (roomList.Count % roomButtons.Length == 0) ? roomList.Count / roomButtons.Length : roomList.Count / roomButtons.Length + 1;

        // 이전, 다음버튼
        previousButton.interactable = (currentPage <= 1) ? false : true;
        nextButton.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * roomButtons.Length;
        for (int i = 0; i < roomButtons.Length; i++)
        {
            roomButtons[i].interactable = (multiple + i < roomList.Count) ? true : false;
            roomButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < roomList.Count) ? roomList[multiple + i].Name : "";
            roomButtons[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < roomList.Count) ? roomList[multiple + i].PlayerCount + "/" + roomList[multiple + i].MaxPlayers : "";
        }
    }

    /// <summary>
    /// 방이 업데이트 됐을 때 실행되는 함수
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Update");

        int roomCount = roomList.Count; // 방 개수 초기화
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
        RoomListRenewal(); // 방 리스트 갱신
    }

    #endregion 
}
    