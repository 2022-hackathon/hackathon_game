using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveObject : MonoBehaviour
{
    public GameObject infoText;
    Coroutine infoCor;
    public GameObject miniGameObject;
    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void Active()
    {
        Debug.Log("Active!");
        Instantiate(miniGameObject);
    }

}
