using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameActiveObject : ActiveObject
{

    public enum gameType
    {
        jump,
        stock
    }
    public gameType type;
    public override void Active()
    {
        base.Active();
        switch (type)
        {
            case gameType.jump:
        SceneManager.LoadScene("JumpGameScene", LoadSceneMode.Additive);
                break;
            case gameType.stock:
        SceneManager.LoadScene("StockGameScene", LoadSceneMode.Additive);
                break;
            default:
                break;
        }
    }
}
