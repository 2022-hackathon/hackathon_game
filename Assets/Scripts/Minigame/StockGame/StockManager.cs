using JetBrains.Annotations;
using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        moneyText.text = GameManager.userData._money.ToString();
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
            if(GameManager.userData._money >= cnt * stocks[curIndex]._money)
            {
                stockCount[curIndex] += Int32.Parse(popup_InputField.text);
                GameManager.userData._money -= (uint)cnt * stocks[curIndex]._money;
            }
        }
        else
        {
            //Sell
            if (stockCount[curIndex] >= cnt)
            {
                stockCount[curIndex] -= Int32.Parse(popup_InputField.text);
                GameManager.userData._money += (uint)(stocks[curIndex]._money * cnt);
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
        SceneManager.UnloadScene("StockGameScene");
    }
}
