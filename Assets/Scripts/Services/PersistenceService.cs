using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class PersistenceService
{
    private readonly string saveFileName = "PlayerData.json";
    private readonly string saveFilePath;

    public PersistenceService()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    public void Save(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public PlayerData Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            return playerData;
        }

        Debug.LogWarning($"PersistenceJsonService.Load() - Save file {saveFileName} not found");
        return null;
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }

}
