using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

/// 저장할 데이터
public class PlayerData
{
    // 플레이 데이터
    public Vector3 pos;     // 위치
}

public class DataManager
{
    private PlayerData _nowPlayer;
    
    private string _path;    // 저장 경로 
    private string _fileName = "saveData";
    // 실제 위치는 C:\Users\[user name]\AppData\LocalLow\[company name]\[product name] + "/" + _fileName
    
    public void Init()
    {
        _nowPlayer = new PlayerData();
        _path = Application.persistentDataPath + "/";
    }

    public void SaveData()
    {
        if (Managers.Scene.CurrentSceneType != Define.Scene.InGameScene) return;
        
        _nowPlayer.pos = GameObject.Find("Player").transform.position;  // 현재 플레이어의 위치를 가져와서
        string playData = JsonUtility.ToJson(_nowPlayer);               // Json으로 만들어서
        File.WriteAllText(_path + _fileName, playData);     // 로컬 데이터로 변환한다
    }

    public void LoadData()
    {
        // 정해진 경로에서 데이터를 읽어온다, 없으면 새로 만들어서
        try
        {
            _nowPlayer = JsonUtility.FromJson<PlayerData>(File.ReadAllText(_path + _fileName));
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            Debug.Log("There is no Saved Data! so, This is new Game!");
            _nowPlayer = new PlayerData(){ pos = new Vector3(0,0,0) };
        }
        
        // 이를 Scene의 Player에게 적용
        GameObject.Find("Player").transform.position = _nowPlayer.pos;
    }
}
