using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SettingsMenu : Singleton<SettingsMenu>
{
    void Start()
    {
        Application.targetFrameRate = 120;
        DontDestroyOnLoad(gameObject);
    }
}
