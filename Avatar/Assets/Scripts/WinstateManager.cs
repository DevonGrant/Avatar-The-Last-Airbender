using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinstateManager : MonoBehaviour
{
    private int keyState;
    private int lossState;
    public static WinstateManager Instance { get; private set; }
    public int KeyState
    {
        get { return keyState; }
        set
        {
            if (value <= 5)
                keyState = value;
            for (int i = 0; i < keyState; i++)
                keys[i].color = Color.white;
        }
    }
    public int LossState
    {
        get { return lossState; }
        set
        {
            if (value >= lossState)
                lossState = value;
            for (int i = 0; i < lives.Length; i++)
                if (lossState > i)
                    lives[i].enabled = false;
        }
    }

    public Image[] keys;
    public Image[] lives;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    public void TriggerWin()
    {
        if (KeyState >= 5 && lossState < 3)
            PlatformerUI.Instance.ToggleWindowPause(true);
    }
}
