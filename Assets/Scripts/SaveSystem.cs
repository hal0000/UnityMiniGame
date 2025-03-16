using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string _filePath = Application.persistentDataPath + "/playerData.dat";

    public static void Save(PlayerModel data)
    {
        try
        {
            using FileStream file = File.Create(_filePath);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
        }
        catch (Exception ex)
        {
            throw new Exception($"SaveSystem Error: {ex.Message}");
        }
    }

    public static PlayerModel Load()
    {
        if (!File.Exists(_filePath))
            return new PlayerModel(); // New player if file doesn't exist

        FileInfo fileInfo = new FileInfo(_filePath);
        if (fileInfo.Length == 0)
        {
            Debug.LogWarning("Save file is empty. Returning new PlayerData.");
            return new PlayerModel();
        }
        try
        {
            using (FileStream file = File.Open(_filePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (PlayerModel)bf.Deserialize(file);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Load Error: {ex.Message}");
            return new PlayerModel(); // Prevent crash if file is corrupt
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
        else
            Debug.LogError("Delete requested but file doesn't exist at:" + _filePath);
    }
}