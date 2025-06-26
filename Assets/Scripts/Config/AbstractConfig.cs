using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractConfig : ScriptableObject
{
    public IEnumerator LoadConfigFileAsync()
    {
        LoadConfigFile();
        yield return null;
    }

    protected abstract void LoadConfigFile();
}
