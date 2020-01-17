using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Ink.Runtime;

public class StoryPackage : ScriptableObject
{
    public Story story;
    public Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();
    public Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

    public void AddImage(string key, Texture2D image)
    {
        if (images.ContainsKey(key))
            images[key] = image;
        else
            images.Add(key, image);
    }

    public void AddSFX(string key, AudioClip clip)
    {
        if (sfx.ContainsKey(key))
            sfx[key] = clip;
        else
            sfx.Add(key, clip);
    }
}
