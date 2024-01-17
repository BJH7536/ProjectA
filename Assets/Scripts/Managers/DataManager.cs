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

// 테스트를 위해서 MonoBehaviour를 상속받는 버전으로 만듬.
// 이를 없애고 Managers 아래로 넣어야함
public class DataManager : MonoBehaviour
{
    public static DataManager instance;    // Managers 아래로 갈 때 없애야 함
    
    private PlayerData _nowPlayer;
    
    private string _path;    // 저장 경로 
    private string _fileName = "saveData";
    // 실제 위치는 C:\Users\[user name]\AppData\LocalLow\[company name]\[product name] + "/" + _fileName

    private void Awake()    // Managers 아래로 갈 때 없애야 함
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        Init();     
    }

    public void Init()
    {
        _nowPlayer = new PlayerData();
        _path = Application.persistentDataPath + "/";
    }

    public void SaveData()
    {
        // 현재 플레이어의 위치를 가져와서
        _nowPlayer.pos = GameObject.Find("Player").transform.position;
        
        // Json으로 만들어서
        string playData = JsonUtility.ToJson(_nowPlayer);
        
        // 로컬 데이터로 변환한다
        File.WriteAllText(_path + _fileName, playData);
        
    }

    public void LoadData()
    {
        // 저장된 파일로부터 (아마 있을것으로 가정)
        _nowPlayer = JsonUtility.FromJson<PlayerData>(File.ReadAllText(_path + _fileName));
        
        // 이를 Scene의 Player에게 적용
        GameObject.Find("Player").transform.position = _nowPlayer.pos;
    }
    
}
