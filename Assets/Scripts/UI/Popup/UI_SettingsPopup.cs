using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingsPopup : UI_Popup
{
    enum Images
    {
        Background,
    }

    public override bool Init()
    {
        BindImage(typeof(Images));
        
        GetImage((int)Images.Background).gameObject.BindEvent(ClosePopupUI);

        return true;
    }
    
}