using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stock : MonoBehaviour
{

    public string _stockName;
    public uint _money;
    public Sprite _stockImg;


    public Image _stockIcon;
    public TMP_Text nameText;
    public TMP_Text moneyText;
    public TMP_Text percentText;

    public uint firstPrice;
    public List<uint> moneyFluctuations = new List<uint>();

    public bool isChoose;

    private void Start()
    {
        nameText.text = _stockName;
        moneyFluctuations.Add(_money);
        StartCoroutine(StockCo());
    }

    private void Update()
    {
        
    }

    IEnumerator StockCo()
    {
        while (true)
        {
            yield return null;


            bool updown = (Random.Range(0, 2) == 1) ? true : false;
            if (updown)
            {
                
                percentText.text = "+ ";
                moneyText.color = Color.red;
                percentText.color = Color.red;
            }
            else
            {
                percentText.text = "- ";
                moneyText.color = Color.blue;
                percentText.color = Color.blue;

            }
            float percent = Random.Range(1.0f, 8.0f);

            percentText.text += percent.ToString("0.00") + "%";

            Debug.Log(percent);
            _money = (updown) ? _money + (uint)(_money * (percent / 100)) : _money - (uint)(_money * (percent / 100));
            moneyText.text = _money.ToString();

            if (isChoose)
            {
                StockManager.Instance.chartStockMoney.text = _money.ToString();

                if (updown)
                {

                StockManager.Instance.chartStockPercent.text = "+ " + percent.ToString("0.00");
                StockManager.Instance.chartStockMoney.color = Color.red;
                StockManager.Instance.chartStockPercent.color = Color.red;
                }
                else
                {
                    StockManager.Instance.chartStockPercent.text = "- " + percent.ToString("0.00");
                    StockManager.Instance.chartStockMoney.color = Color.blue;
                    StockManager.Instance.chartStockPercent.color = Color.blue;
                }
            }
            moneyFluctuations.Add(_money);

            yield return new WaitForSeconds(5);

        }
    }

}
