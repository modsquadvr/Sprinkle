using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ContentSizeFitter))]
[RequireComponent(typeof(Text))]
public class ContentSizeFitterHelper : MonoBehaviour
{
    public  RectTransform rt;
    public Text text;

    void Awake()
    {
        rt.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
        gameObject.SetActive(false);
    }
}