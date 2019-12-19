using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink;

public class ObservableString : ScriptableObject
{
    public string varName;
    public string value;

    public void ObserveValue(string value)
    {
        this.value = value;
    }
}
