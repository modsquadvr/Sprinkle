using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using System.IO;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    [Header("Ink")]
    [SerializeField]
    public TextAsset inkJSONAsset;
    public Story story;
    List<int> choicePath = new List<int>();

    [Space(5)]
    public Color textColour = Color.white;

    [Tooltip("Reveal x letters per second, where 0 is instant.")]
    [Range(0, float.MaxValue)]
    public float textSpeed;

    public int fontSize = 40;
    public float yPad = 5;
    public float xPad = 5;
    public float chunkSpace = 10;


    [Header("Content Objects")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private ObjectPool textPool;
    [SerializeField]
    private ObjectPool choicePool;
    public ScrollRect scrollRect;
    public VerticalLayoutGroup contentLayout;
    public CanvasScaler canvasScaler;
    Text[] textContent;

    [Header("Content Vars")]
    public int maxCharsPerLine = 35;
    public Vector2 scrollBounds = new Vector2(-100f, 100f);
    float viewHeight;
    float contentHeight;
    float choiceHeight;
    public float minContentHeight = 150;
    float[] textContentHeight;
    int contentCount = 0;

    [Range(0, float.MaxValue)]
    public float choiceDelay = 0.5f;

    bool layoutDirty = false;
    bool drawNow = false;

    void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
            instance = this;

        //viewHeight = scrollRect.content.sizeDelta.y / scrollRect.GetComponent<RectTransform>().rect.height;

        choicePool.BuildPool();
        textPool.BuildPool();
        //StartCoroutine(StartStory());
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (inkJSONAsset == null)
            PanelSwap.instance.SwapTo(1); //swap to story listing if there isn't one currently set
    }

    private void LateUpdate()
    {
        if (layoutDirty)
        {
            if (!drawNow)
            {
                drawNow = true;
                return;
            }

            layoutDirty = false;
            StartCoroutine(DrawContent());
        }
    }

    private void OnApplicationQuit()
    {
        SaveChoices();
    }
    
    public void SaveChoices()
    {
        if (inkJSONAsset == null || story == null)
            return;
        
        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);
        else if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = null;
        saveFile = File.Create(Application.persistentDataPath + "/Saves/" + inkJSONAsset.name + ".dat");
        formatter.Serialize(saveFile, choicePath); //story.state.ToJson());
        saveFile.Close();
    }

    public bool LoadChoices()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("Save directory not found! (" + Application.persistentDataPath + ")");
            return false;
        }
        else if(!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Debug.LogError("Save directory not found! (" + Application.persistentDataPath + "/Saves/)");
            return false;
        }

        if (!File.Exists(Application.persistentDataPath + "/Saves/" + inkJSONAsset.name + ".dat"))
        {
            Debug.LogError("Save file not found! (" + Application.persistentDataPath + "/Saves/" + inkJSONAsset.name + ".dat)");
            return false;
        }

        FileStream saveFile = File.OpenRead(Application.persistentDataPath + "/Saves/" + inkJSONAsset.name + ".dat");
        //story.state.LoadJson((string)formatter.Deserialize(saveFile));
        choicePath.Clear();

        try
        {
            choicePath = (List<int>)formatter.Deserialize(saveFile);
        }
        catch
        {
            return false;
        }

        RestartPanel();

        StartCoroutine(LoadContent());

        return true;
    }

    IEnumerator LoadContent()
    {
        float delay = choiceDelay;
        choiceDelay = 0;

        for (int i = 0; i < choicePath.Count; i++)
        {
            RefreshView();

            while (story.currentChoices.Count == 0)
                yield return null;

            story.ChooseChoiceIndex(choicePath[i]);

            yield return null;
        }

        choiceDelay = delay;

        RefreshView();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect.content.rect.height <= viewHeight)
            return;
        if (scrollRect.content.anchoredPosition.y >= scrollBounds.y  && eventData.delta.y > 0)
            return;
        else if (scrollRect.content.anchoredPosition.y <= scrollBounds.x && eventData.delta.y < 0)
            return;
    }

    public void Scrolling()
    {
        if (scrollRect.content.anchoredPosition.y < scrollBounds.x)
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollBounds.x);

        else if (scrollRect.content.anchoredPosition.y > scrollBounds.y)
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollBounds.y);
    }

    public void RestartPanel()
    {
        textColour = Color.white;

        scrollRect.content.anchoredPosition = new Vector2(0, -10);
        textPool.PoolRange();
        PoolContent();

        story = new Story(inkJSONAsset.text);

        if(story.variablesState.Contains("textColour"))
        { 
            story.ObserveVariable("textColour", (string varName, object newValue) =>
            {
                HandleTextColour((string)newValue);
            });
        }

        contentHeight = 0;
    }

    // Creates a new Story object with the compiled story which we can then play!
    public void StartStory()
    {
        RestartPanel();
        RefreshView();
    }

    void HandleTextColour(string newValue)
    {
        //Debug.Log(newValue);
        switch ((newValue).ToLower())
        {
            case "black":
                textColour = Color.black;
                break;
            case "blue":
                textColour = Color.blue;
                break;
            case "cyan":
                textColour = Color.cyan;
                break;
            case "gray":
                textColour = Color.gray;
                break;
            case "green":
                textColour = Color.green;
                break;
            case "magenta":
                textColour = Color.magenta;
                break;
            case "red":
                textColour = Color.red;
                break;
            case "white":
                textColour = Color.white;
                break;
            case "yellow":
                textColour = Color.yellow;
                break;

            default:
                if (!ColorUtility.TryParseHtmlString("#" + newValue, out textColour))
                {
                    textColour = Color.black;
                    Debug.LogWarning("When setting text colour, got: [" + newValue + "] and expected a hex value or: " +
                                     "black, blue, cyan, gray, green, magenta, red, white, yellow");
                }
                break;
        }
    }

    // This is the main function called every time the story changes. It does a few things:
    // Repositions all text elements
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    void RefreshView()
    {
        PoolContent();
        contentCount = scrollRect.content.childCount;
        PrepContent();
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    void PrepContent()
    {
        // Continue gets the next line of the story
        while (story.canContinue)
        {
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!

            string[] paragraphs = text.Split('\n');
            textContent = new Text[paragraphs.Length];
            textContentHeight = new float[paragraphs.Length];
            for (int i = 0; i < paragraphs.Length; i++)
            {
                Text storyText = textPool.NextGameObject().GetComponent<Text>();
                storyText.text = "";
                storyText.color = Color.white;
                storyText.fontSize = fontSize;
                storyText.transform.SetParent(scrollRect.content, false);

                storyText.text = paragraphs[i];
                storyText.gameObject.SetActive(true);

                LayoutRebuilder.ForceRebuildLayoutImmediate(storyText.rectTransform);
                textContent[i] = storyText;
                textContentHeight[i] = storyText.rectTransform.rect.height;

                storyText.rectTransform.sizeDelta = new Vector2(scrollRect.content.rect.width - xPad, storyText.rectTransform.rect.height);
                storyText.rectTransform.anchoredPosition = new Vector2(0, -(contentHeight + (chunkSpace + storyText.rectTransform.rect.height + yPad) * 0.5f));
                //   Debug.Log(i + " < height > " + storyText.rectTransform.rect.height);
                contentHeight += storyText.rectTransform.rect.height + yPad + chunkSpace;
            }
        }

        drawNow = false;
        layoutDirty = true;
    }

    IEnumerator DrawContent()
    {
        if (scrollRect.content.childCount > 0)
        {
            contentHeight = 0;

            for (int i = 0; i < contentCount; i++)
            {
                contentHeight += scrollRect.content.GetChild(i).GetComponent<RectTransform>().rect.height + yPad + chunkSpace;
            }

            for(int i = contentCount; i < scrollRect.content.childCount; i++)
            {
                RectTransform rt = scrollRect.content.GetChild(i).GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(scrollRect.content.rect.width - xPad, rt.rect.height);
                rt.anchoredPosition = new Vector2(0, -(contentHeight + (chunkSpace + rt.rect.height + yPad) * 0.5f));
                //Debug.Log(i + " < height > " + rt.rect.height);
                contentHeight += rt.rect.height + yPad + chunkSpace;

                Text t = scrollRect.content.GetChild(i).GetComponent<Text>();
                t.color = textColour;
            }
        }

        contentHeight += chunkSpace * 2;

        yield return new WaitForSeconds(choiceDelay);

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim());
                button.transform.localScale = Vector3.one;

                // Tell the button what to do when we press it
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });
            }
        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            Button choice = CreateChoiceView("End of story.\nRestart?");
            choice.transform.localScale = Vector3.one;

            choice.onClick.RemoveAllListeners();
            choice.onClick.AddListener(delegate
            {
                StartStory();
                //StartCoroutine(StartStory());
            });
        }

        yield return new WaitForEndOfFrame();

        RectTransform last_rt = scrollRect.content.GetChild(scrollRect.content.childCount - 1).GetComponent<RectTransform>();
        float lastPos = last_rt.anchoredPosition.y;
        //Debug.Log("LastPos: " + lastPos);

        scrollBounds = new Vector2(scrollBounds.x, Mathf.Max(-10, -(canvasScaler.referenceResolution.y * canvas.transform.localScale.y + lastPos + last_rt.sizeDelta.y / 2 + chunkSpace)));  
                                                   //contentHeight * canvas.transform.localScale.y);
    }

    // Creates a button showing the choice text
    IEnumerator CreateContentView()
    {
      //  Debug.Log("printing content");

        // Read all the content until we can't continue any more
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!

            string[] paragraphs = text.Split('\n');

            for (int i = 0; i < paragraphs.Length; i++)
            {
                Text storyText = textPool.NextGameObject().GetComponent<Text>();
                storyText.text = "";
                storyText.color = textColour;
                storyText.fontSize = fontSize;
                storyText.transform.SetParent(scrollRect.content, false);

                storyText.text = paragraphs[i];

                storyText.gameObject.SetActive(true);

                LayoutRebuilder.ForceRebuildLayoutImmediate(storyText.rectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(storyText.rectTransform);

                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                yield return new WaitForEndOfFrame();

                float sizeY = storyText.rectTransform.rect.height;

                // Debug.Log("pref height: " + storyText.preferredHeight);
                // Debug.Log("height: " + storyText.rectTransform.rect.height);


                storyText.rectTransform.anchoredPosition = new Vector2(0, -(contentHeight + (chunkSpace + sizeY) * 0.5f));
                storyText.rectTransform.sizeDelta = new Vector2(scrollRect.content.rect.width - xPad, sizeY);


                if (textSpeed == 0)
                    storyText.text = paragraphs[i];
                else
                {
                    StringBuilder sb = new StringBuilder(paragraphs[i].Length);
                    for (int k = 0; k < paragraphs[i].Length; k++)
                    {
                        sb.Append(paragraphs[i][k]);
                        yield return new WaitForSeconds(textSpeed);
                        storyText.text = sb.ToString();
                    }
                }

                contentHeight += sizeY + chunkSpace;
            }

            yield return null;
        }

        contentHeight += chunkSpace * 2;

        yield return new WaitForSeconds(choiceDelay);

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim());
                button.transform.localScale = Vector3.one;

                // Tell the button what to do when we press it
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });
            }
        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            Button choice = CreateChoiceView("End of story.\nRestart?");
            choice.transform.localScale = Vector3.one;

            choice.onClick.RemoveAllListeners();
            choice.onClick.AddListener(delegate
            {
                StartStory();
                //StartCoroutine(StartStory());
            });
        }

        scrollBounds = new Vector2(scrollBounds.x, contentHeight * canvas.transform.localScale.y);
    }

    // Creates a button showing the choice text
    Button CreateChoiceView(string text)
    {
        // Gets the button from a pool
        Button choice = choicePool.NextGameObject().GetComponent<Button>();
        choice.transform.SetParent(scrollRect.content, false);
        text = text.TrimEnd(' ');
        choice.gameObject.SetActive(true);

        float sizeY = choice.GetComponentInChildren<Text>().font.lineHeight + yPad;

        if ((text.Length * fontSize + xPad * 2) / (canvasScaler.referenceResolution.x - xPad) > 1)
        {
            sizeY += (Mathf.RoundToInt((text.Length * fontSize + xPad)
                       / (canvasScaler.referenceResolution.x - xPad)))
                      * choice.GetComponentInChildren<Text>().font.lineHeight;
        }

        if (sizeY < minContentHeight)
            sizeY = minContentHeight;

        RectTransform rt = choice.GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(0, -(contentHeight + (chunkSpace + sizeY) * 0.5f));
        rt.sizeDelta = new Vector2(canvasScaler.referenceResolution.x - xPad, sizeY);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;
        choiceText.fontSize = fontSize;

        contentHeight += sizeY + chunkSpace;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    void PoolContent()
    {
        if (choicePool.pool.Length == 0)
            return;

        choicePool.PoolRange();
    }

    float CalculateContentHeight(string text, Text content)
    {
        int curWidth = 0;

        CharacterInfo characterInfo = new CharacterInfo();
        content.font.RequestCharactersInTexture(text, content.fontSize, content.fontStyle);

        content.font.GetCharacterInfo(' ', out characterInfo, content.fontSize);
        int spaceSize = characterInfo.advance;

        string[] words = text.Split(' ');
        float sizeY = yPad;
        for (int k = 0; k < words.Length; k++)
        {
            int width = Mathf.CeilToInt(xPad);

            for (int i = 0; i < words[k].Length; i++)
            {
                content.font.GetCharacterInfo(words[k][i], out characterInfo, content.fontSize);
                width += characterInfo.advance;
            }

            // Debug.Log("width: " + width);

            if (curWidth + width > canvasScaler.referenceResolution.x)
            {
                //   Debug.Log("curWidth: " + curWidth);
                sizeY += content.fontSize + content.lineSpacing;
                curWidth = width;
            }
            else
                curWidth += width + spaceSize;
        }

        if (curWidth > 0)
            sizeY += content.fontSize + content.lineSpacing;

        if (sizeY < fontSize + yPad)
            sizeY = fontSize + yPad;

        return sizeY;
    }
}