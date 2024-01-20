using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.InGameScene;
        //Managers.UI.ShowSceneUI<UI_InGame>();
        Debug.Log("InGame");
        
        if (Managers.Game.thisGameis == Define.ThisGameis.LoadedGame)
        {
            Managers.Data.LoadData();
        }
        else if (Managers.Game.thisGameis == Define.ThisGameis.NewGame)
        {
            // 저장된 데이터 지우기
        }
        
        return true;
    }
}
