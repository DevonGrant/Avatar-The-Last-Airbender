using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerUI : MonoBehaviour
{
    public static PlatformerUI Instance { get; private set; }
    public Image iconAirBlast;
    public Image iconAirJump;
    public Image iconAirScooter;
    public AnimationCurve iconAirScooterCurve;
    private bool iconAirScooterAnimating;
    private Coroutine iconAirScooterInstance;
    private IEnumerator iconAirJumpInstance;
    public GameObject windowHUD;
    public GameObject windowPause;
    public TMPro.TMP_Text textPause;
    private bool statePause;
    private const string displayPause = "Pause - esc to unpause";
    private const string displayLoss = "You Lost. Press the menu button to exit";
    private const string displayWin = "You Won. Press the menu button to exit";
    private const float opacityActive = 0.1f;
    private const float opacityInactive = 1;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        iconAirJumpInstance = DelayJump();
        windowPause.SetActive(false);
        textPause.text = displayPause;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!statePause && Input.GetKeyDown(KeyCode.Escape))
            ToggleWindowPause(true);
        else if (statePause && Input.GetKeyDown(KeyCode.Escape) && WinstateManager.Instance.KeyState < 4 && WinstateManager.Instance.LossState < 3)
            ToggleWindowPause(false);
    }

    public void ToggleAirScooter(bool active)
    {
        if (active != iconAirScooterAnimating)
        {
            if (iconAirScooterAnimating)
            {
                StopCoroutine(iconAirScooterInstance);
                Color c = iconAirScooter.color;
                c.a = opacityInactive;
                iconAirScooter.color = c;
            }

            else
                iconAirScooterInstance = StartCoroutine(AnimateScooter());

            iconAirScooterAnimating = active;
        }
    }

    public void ToggleAirJump(bool active)
    {
        if (active)
        {
            iconAirJumpInstance = DelayJump();
            StartCoroutine(iconAirJumpInstance);
        }

        else
        {
            StopCoroutine(iconAirJumpInstance);
            Color c = iconAirJump.color;
            c.a = opacityInactive;
            iconAirJump.color = c;
        }
    }

    public void ToggleWindowPause(bool active)
    {
        statePause = active;
        windowPause.SetActive(active);
        Time.timeScale = active ? 0 : 1;
        if (active)
        {
            if (WinstateManager.Instance.KeyState >= 4)
                textPause.text = displayWin;
            else if (WinstateManager.Instance.LossState >= 3)
                textPause.text = displayLoss;
            else
                textPause.text = displayPause;
        }
        if (!active && !CManager.Instance.Running)
            ToggleWindowHUD(true);
        else if (active)
            ToggleWindowHUD(false);
    }

    public void ToggleWindowHUD(bool active)
    {
        if (active && !statePause)
            windowHUD.SetActive(active);
        else if (!active)
            windowHUD.SetActive(active);
    }

    private IEnumerator AnimateScooter()
    {
        float t;
        float animLength;
        while (true)
        {
            t = 0;
            animLength = 2;

            while (t <= animLength)
            {
                Color c = iconAirScooter.color;
                c.a = iconAirScooterCurve.Evaluate(t / animLength);
                iconAirScooter.color = c;
                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator DelayJump()
    {
        yield return new WaitForFixedUpdate();
        Color c = iconAirJump.color;
        c.a = opacityActive;
        iconAirJump.color = c;
    }
}
