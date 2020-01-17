using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StoryList : MonoBehaviour
{
    public const string storyListURL = "https://vicstories.ca/api/stories";
    public const string detailURL = "https://vicstories.ca/api/story/detail/";
    public const string storyURL = "https://vicstories.ca/api/story/";

    [Tooltip("Container for story cards.")]
    public ScrollRect scrollRect;
    public CanvasGroup canvasGroup;
    public StoryCard cardPrefab;
    public List<StoryCard> cards = new List<StoryCard>();

    public bool initialized = false;

    public Button welcomeShowList;

    //placeholder for appropriate type

    private void Awake()
    {
        BuildList();
    }

    private void OnEnable()
    {
        scrollRect.content.sizeDelta = new Vector2(0, scrollRect.content.GetComponent<VerticalLayoutGroup>().preferredHeight);
    }

    public void BuildList()
    {
        StartCoroutine(IFetchStories());
    }

    IEnumerator IFetchStories()
    {
        byte[] data;
        
        UnityWebRequest sList = UnityWebRequest.Get(storyListURL);
        UnityWebRequestAsyncOperation ao_get = sList.SendWebRequest();

        //get names
        while(!sList.downloadHandler.isDone)
        {
            if(sList.isNetworkError || sList.isHttpError)
            {
                Debug.LogError(sList.error);
                yield break;
            }

            yield return null;
        }

        data = sList.downloadHandler.data;

        string listing_json = Encoding.UTF8.GetString(data, 0, data.Length);

        JSONObject storyNames = new JSONObject(Encoding.UTF8.GetString(data, 0, data.Length));

        //get details
        for (int i = 0; i < storyNames.list.Count; i++)
        {
            //TODO: check version, load instead of request if version matches
                    //if there's a version mismatch, clear the story save data

            string name = storyNames.list[i].str.Split('.')[0];
            string path = detailURL + name;
            //Debug.Log(path);

            sList.Dispose();
            sList = UnityWebRequest.Get(detailURL + name);
            ao_get = sList.SendWebRequest();

            while (!sList.downloadHandler.isDone)
            {
                if (sList.isNetworkError || sList.isHttpError)
                {
                    Debug.LogError(sList.error);
                    break;
                }

                yield return null;
            }

            StoryCard card = Instantiate(cardPrefab);
            card.transform.SetParent(scrollRect.content, false);
            card.storyName.text = name;
            card.storyName.rectTransform.sizeDelta = new Vector2(card.rectTransform.sizeDelta.x, card.storyName.preferredHeight);
            //instantiate story listings
            if (sList.downloadHandler.isDone)
            {
                data = sList.downloadHandler.data;
                JSONObject details = new JSONObject(Encoding.UTF8.GetString(data, 0, data.Length));
                //Debug.Log(details.str);
                card.details.text = details.str;
            }

            card.details.rectTransform.sizeDelta = new Vector2(card.details.rectTransform.sizeDelta.x, card.details.preferredHeight);

            //get story content
            //Debug.Log(storyURL + name);

            sList.Dispose();
            sList = UnityWebRequest.Get(storyURL + name + ".ink.json");
            ao_get = sList.SendWebRequest();

            while (!sList.downloadHandler.isDone)
            {
                if (sList.isNetworkError || sList.isHttpError)
                {
                    Debug.LogError(sList.error);
                    break;
                }

                yield return null;
            }

            if (sList.downloadHandler.isDone)
            {
                data = sList.downloadHandler.data;
                string content = Encoding.UTF8.GetString(data, 0, data.Length);
                card.storyContent = new TextAsset(content);

                //Debug.Log(content);
            }
            else
                card.playButton.interactable = false;

            //save data to be loaded later

            card.Fold();
        }

        sList.Dispose();

        if (cards.Count > 0)
        {
            StoryManager.instance.inkJSONAsset = cards[0].storyContent;
            StoryManager.instance.story = new Ink.Runtime.Story(cards[0].storyContent.text);
        }

        scrollRect.gameObject.SetActive(false);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;

        PanelSwap.instance.enabled = true;
        welcomeShowList.interactable = true;
    }
}
