﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserManager : MonoBehaviour{

    public string url;
    //public delegate void testCallBack(int id, string name, string add_time, int count_all, int count_collect);
    //public delegate void testCallBack(UserData);

    public static UserManager Instance
    {
        get; private set;
    }

    [Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string add_time;
        public int count_all;
        public int count_correct;
    }

    UserData userData;
    // Use this for initialization
    void Awake () {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //Debug.Log("unko");
        url = "http://192.168.179.5/quiz/index.php";
    }


    public IEnumerator RequestUserData(Action delegateMethod,int myUserId)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("user_id", myUserId);
        wwwForm.AddField("type", "user_data");        
        WWW result = new WWW(url, wwwForm);
        //Debug.Log("test");
        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            Debug.LogError("www Error:" + result.error);
        }
        else
        {
            Debug.Log(result.text);
            userData = JsonUtility.FromJson<UserData>(result.text);
            Debug.Log(userData.id);
            Debug.Log(userData.name);
            Debug.Log(userData.add_time);
            delegateMethod();
        }
    }

    //シーンが有効化されたときに呼び出す
    public void ShowUserData()
    {
        StartCoroutine(RequestUserData(SendUserData, 1));
    }
    /*public void TestUserRequest()
    {
        StartCoroutine(RequestUserData(SendUserData, 1));
    }*/

    public void SendUserData()
    {

        GameObject UIManager = GameObject.Find("UIManager");
        UIManager.GetComponent<UserDataController>().UserDataReceived(userData.id, userData.name, userData.add_time, userData.count_all, userData.count_correct);
        Debug.Log("test");
    }
}