using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }

    public enum Scene
    {
        Unknown,
        MainMenuScene,
        InGameScene,
    }

    public enum ThisGameis
    {
        NewGame,
        LoadedGame,
    }
}
