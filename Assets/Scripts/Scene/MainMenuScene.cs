using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.MainMenuScene;
        Managers.UI.ShowSceneUI<UI_MainMenu>();
        Debug.Log("MainMenu");
        return true;
    }
}
