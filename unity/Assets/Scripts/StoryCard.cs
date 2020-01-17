using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI for displaying story details when browsing.
/// </summary>
public class StoryCard : MonoBehaviour
{
    public Text storyName;
    public Text details;
    public TextAsset storyContent;
    public RectTransform rectTransform;
    public Button playButton;

    public void Fold()
    {
        details.gameObject.SetActive(!details.gameObject.activeSelf);

        float y = storyName.rectTransform.sizeDelta.y + 110;

        if (details.gameObject.activeSelf)
            y += details.rectTransform.sizeDelta.y;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, y);
    }

    public void Set(string name, string details, TextAsset storyContent)
    {
        storyName.text = name;
        this.details.text = details;
        this.storyContent = storyContent;
    }

    //manage story states and launch story
    public void PlayStory()
    {
        Debug.Log("playing " + name);

        StoryManager.instance.SaveChoices();

        StoryManager.instance.inkJSONAsset = storyContent;
        StoryManager.instance.story = new Story(storyContent.text);

        PanelSwap.instance.SwapTo(2);

        if (!StoryManager.instance.LoadChoices())
        {
            Debug.Log("Starting " + name);
            StoryManager.instance.StartStory(); //StoryManager.instance.StartCoroutine(StoryManager.instance.StartStory());
        }
        else
            Debug.Log("Loaded " + name);
    }
}
