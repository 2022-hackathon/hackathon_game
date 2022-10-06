using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jump_GameManager : MonoBehaviour
{
    static Jump_GameManager instance;

    public static Jump_GameManager Instance { get { return instance; } }


    public float gameTime;

    public TMP_Text timeText;

    public bool isGameover;

    public GameObject resultObject;
    public TMP_Text resultSecond;
    public TMP_Text resultMoney;
    
    public List<GameObject> blocks = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameover)
            return;

        gameTime += Time.deltaTime;
        timeText.text = gameTime.ToString("0.00");
    }

    public void jumpGameover()
    {
        isGameover = true;
        timeText.gameObject.SetActive(false);
        resultObject.SetActive(true);
        resultSecond.text = gameTime.ToString("0.00") + "√ ";
        resultMoney.text = "µ∑: +" +((int)gameTime * 14) .ToString();
    }
    public void jumpGameExit()
    {
        foreach (GameObject block in blocks)
        {
            Destroy(block);
        }
        PlayerController._isActivity = false;
        GameManager.userData.money += (int)gameTime * 14;
        NetworkManager.Instance.DataSave();


        SceneManager.UnloadScene("JumpGameScene");
    }

    
}
