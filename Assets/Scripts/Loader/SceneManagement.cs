using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene order in build settings should match enum order
/// </summary>
[Serializable]
public enum SceneType
{
    None = -1,
    Preload = 0,
    Main = 1,
    Gameplay = 2
}

[Serializable]
public class SceneManagement : LoadableManager<SceneManagement>
{
    private int sceneBuildCount;

    private void Validate()
    {
        sceneBuildCount = SceneManager.sceneCountInBuildSettings;
        var sceneEnumCount = EnumExtensions.GetEnumLength<SceneType>() - 1;
        AssertUtil.AreEqual(sceneEnumCount, sceneBuildCount, 
            $"Mismatched scene count. sceneEnumCount: {sceneEnumCount} != sceneBuildCount: {sceneBuildCount}");
    }

    protected override IEnumerator InitRoutine()
    {
        Validate();
        yield return null;
    }

    public void LoadSceneAsync(SceneType sceneType, Action callback = null)
    {
        var sceneIndex = (int)sceneType;
        if (sceneIndex < 0)
        {
            Debug.LogError($"Cannot load scene at index {sceneIndex}");
            return;
        }

        if (sceneIndex >= sceneBuildCount)
        {
            Debug.LogError($"Cannot load scene at index {sceneIndex}. Max SceneBuildCount: {sceneBuildCount}");
            return;
        }

        StopCoroutine(nameof(LoadScene));
        StartCoroutine(LoadScene(sceneIndex, callback));
    }

    private IEnumerator LoadScene(int sceneIndex, Action callback = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return null;
        callback?.Invoke();
    }
}
