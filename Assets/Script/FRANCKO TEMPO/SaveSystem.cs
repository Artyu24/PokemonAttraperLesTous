using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public static class SaveSystem
{
    public static void InitSaveData()
    {
        SaveData data = new SaveData(new SaveData.GameData(), new SaveData.SettingData());

        string path = Application.persistentDataPath + "/Game.save";

        if (!File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Save init successful");
        }
    }

    #region GAME
    public static void SaveGameData(string name)
    {
        SaveData.GameData saveGame = new SaveData.GameData(name, 0);
        SaveData data = new SaveData(saveGame, LoadSettingData());

        string path = Application.persistentDataPath + "/Game.save";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save game successful");
    }

    public static SaveData.GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/Game.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Load game successful");

            return data.MyGameData;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            ResetGameData();
            return LoadGameData();
        }
    }

    public static void ResetGameData()
    {
        SaveData data = new SaveData(new SaveData.GameData(), LoadSettingData());

        string path = Application.persistentDataPath + "/Game.save";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
        
        Debug.Log("Reset game successful");
    }

    #endregion

    #region SETTING
    public static void SaveSettingData()
    {
        SaveData.SettingData saveSetting = new SaveData.SettingData();
        SaveData data = new SaveData(LoadGameData(), saveSetting);

        string path = Application.persistentDataPath + "/Game.save";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save setting successful");
    }

    public static SaveData.SettingData LoadSettingData()
    {
        string path = Application.persistentDataPath + "/Game.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Load setting successful");

            return data.MySettingData;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            ResetSettingData();
            return LoadSettingData();
        }
    }

    public static void ResetSettingData()
    {
        SaveData data = new SaveData(LoadGameData(), new SaveData.SettingData());

        string path = Application.persistentDataPath + "/Game.save";
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
        
        Debug.Log("Reset setting successful");
    }

    #endregion

#if UNITY_EDITOR
    public static int GetLocalId(UnityEngine.Object obj)
    {
        PropertyInfo inspectorModeInfo =
        typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        SerializedObject serializedObject = new SerializedObject(obj);
        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

        SerializedProperty localIdProp =
            serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

        return localIdProp.intValue;
    }
#endif
}
