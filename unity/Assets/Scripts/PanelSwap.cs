using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class PanelSwap : MonoBehaviour
{
    public static PanelSwap instance;

    int cur = 0;
    public GameObject[] panels;
    public GameObject listingPanel;
    KeyCode showListing;
    KeyCode cycle;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
            instance = this;

#if UNITY_EDITOR || UNITY_STANDALONE
        showListing = KeyCode.Escape;
        cycle = KeyCode.Backspace;
#elif UNITY_ANDROID
        showListing = KeyCode.Menu;
        cycle = KeyCode.Escape;
#endif

        enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(showListing))
            ToggleListing();

        if (Input.GetKeyDown(cycle))
            Cycle();
    }

    void ToggleListing()
    {
        listingPanel.SetActive(!listingPanel.activeSelf);
    }

    void Cycle()
    {
        panels[cur].SetActive(false);
        cur = (cur + 1) % panels.Length;
        panels[cur].SetActive(true);
    }

    public void SwapTo(int i)
    {
        if (i < 0)
            i = 0;

        i %= panels.Length;

        panels[cur].SetActive(false);
        panels[i].SetActive(true);
        cur = i;

        listingPanel.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}