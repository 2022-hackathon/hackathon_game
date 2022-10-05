using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginSceneManager : MonoBehaviour
{

    public TMP_InputField idInputField;
    public TMP_InputField pwInputField;


    public void EnterLogin()
    {
        if (idInputField.text.Length == 0 || pwInputField.text.Length == 0)
            return;

        Debug.Log(idInputField.text +" " +  pwInputField.text);
        NetworkManager.Instance.LoginFunc(idInputField.text,pwInputField.text);
    }
}
