using JetBrains.Annotations;
using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StockManager : MonoBehaviour
{

    static StockManager instance;
    public static StockManager Instance { get { return instance; } }
    public Stock[] stocks;

    public List<int> stockCount = new List<int>() ;

    public TMP_Text moneyText;
    [Header("Chart")]
    public GameObject chartPanel;
    public TMP_Text chartStockTitle;
    public TMP_Text chartStockMoney;
    public TMP_Text chartStockPercent;
    public TMP_Text chartStockCount;

    public List<GameObject> chartObjList = new List<GameObject>();
    public GameObject linePrefab;
    public Transform LineGroup;

    public int curIndex;
    
    [Header("Buy/Sell")]
    public TMP_Text popup_Title;
    public TMP_InputField popup_InputField;
    public bool isBuy;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for(int i =0;i<9;i++)
        stockCount.Add(0);
    }
    void Start()
    {
    }

    void Update()
    {
        moneyText.text = "돈: " + GameManager.userData.money.ToString();
    }

    public void ShowChart(int index)
    {
        for (int i = 0; i < stocks.Length; i++)
        {
            if(i == index)
                stocks[i].isChoose = true;
            else
                stocks[i].isChoose = false;
        }
        chartStockTitle.text = stocks[index]._stockName;
        chartStockMoney.text = stocks[index]._money.ToString();
        chartStockPercent.text = stocks[index].percentText.text;
        chartStockPercent.color = stocks[index].percentText.color;
        chartStockMoney.color = stocks[index].percentText.color;
        chartStockCount.text = "보유: " + stockCount[index];
        curIndex = index;
        chartPanel.SetActive(true);
        
        List<GameObject> objs = chartObjList;

        if (chartObjList.Count > 0)
        {
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
        }
        DrawChart(stocks[index].moneyFluctuations);

    }

    public void DrawChart(List<int> dotList)
    {
        // ---
        // 격차 최대값 등...
        // ---

        List<GameObject> objs = chartObjList;

        if (chartObjList.Count > 0)
        {
            foreach (GameObject obj in chartObjList)
            {
                Destroy(obj);
            }
        }

        chartObjList.Clear();

        Vector2 prevDotPos = Vector2.zero;
        // 이전 점과 현재 점을 이어야하므로 이전 점의 위치를 저장한다.
        List<GameObject> objs2 = new List<GameObject>();
        for (int i = 0; i < dotList.Count; i++)
        {
            // ---
            // 점 찍기
            // ---

            if (i == 0)
            {
                prevDotPos =new Vector2(-930, 0);
                continue;
            }
            // 최초 점을 찍을 땐 연결할 선이 없으므로 넘긴다.

            GameObject line = Instantiate(linePrefab, LineGroup, true);
            line.transform.localScale = Vector3.one;

            objs2.Add(line);
            RectTransform lineRT = line.GetComponent<RectTransform>();
            Image lineImage = line.GetComponent<Image>();

            float lineWidth = Vector2.Distance(prevDotPos, new Vector2(prevDotPos.x, (dotList[i]- stocks[curIndex].firstPrice) / 2));
            float xPos = (-930 + 30 * i);
            
            float yPos = (prevDotPos.y + (dotList[i] - stocks[curIndex].firstPrice) / 2) / 2;

            if (dotList[i-1] > dotList[i])
                lineImage.color = Color.blue;
            else
                lineImage.color = Color.red;
            // 그림으로 설명..
            lineRT.localRotation = Quaternion.Euler(0f, 0f, 90f);

            lineRT.anchoredPosition = new Vector2(xPos, yPos);
            lineRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);

            /*Vector2 dir = (new Vector2(-930 + 30 * i, dotList[i] - stocks[curIndex].firstPrice) - prevDotPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            lineRT.anchoredPosition = new Vector2(xPos, yPos);
            lineRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            // 두 점 사이의 각도를 tan를 이용하여 구한다.
            // atan를 이용해 라디안 값을 구하고 Rad2Deg를 이용해 라디안을 각도로 변환해준다.
            */
            // 마스크 패널 생성

            prevDotPos = new Vector2(xPos, yPos);
            // 이전 점 좌표 업데이트
        }

        chartObjList = objs2;
    }

    public void createPopup(int i)
    {
        popup_Title.gameObject.transform.parent.gameObject.SetActive(true);
        if(i == 0)
        {
            popup_Title.text = "매수";
            isBuy = true;
        }
        else
        {
            popup_Title.text = "매도";
            isBuy = false ;

        }
    }

    public void EnterBuySell()
    {

        //TODO 조건 추가
        int cnt = Int32.Parse(popup_InputField.text);

        if(isBuy)
        {
            // Buy
            if(GameManager.userData.money >= cnt * stocks[curIndex]._money)
            {
                stockCount[curIndex] += Int32.Parse(popup_InputField.text);
                GameManager.userData.money -= (int)(cnt * stocks[curIndex]._money);
            }
        }
        else
        {
            //Sell
            if (stockCount[curIndex] >= cnt)
            {
                stockCount[curIndex] -= Int32.Parse(popup_InputField.text);
                GameManager.userData.money += (int)(stocks[curIndex]._money * cnt);
            }
        }

        chartStockCount.text = "보유: " + stockCount[curIndex];
        popup_InputField.text ="";
    }

    public void ShowIt(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void HideIt(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void GameExit()
    {
        PlayerController._isActivity = false;
        NetworkManager.Instance.DataSave();
        SceneManager.UnloadScene("StockGameScene");
    }
}
