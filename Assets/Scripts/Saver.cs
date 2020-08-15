using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Saver
{

    public static void SaveFile(GameManager gm, int id = 0)
    {
        PlayerPrefs.SetInt("file" + id, 1);
        //foreach (string name in Enum.GetNames(typeof(Parameters)))
        //{
        //    Parameters sp = (Parameters)Enum.Parse(typeof(Parameters), name);
        //}
        if(PlayerPrefs.GetInt("sceneNumber") < gm.currentSceneNumber)
            PlayerPrefs.SetInt("sceneNumber", gm.currentSceneNumber);
        PlayerPrefs.SetString("cookedFood", gm.cookedFood.ToString());
        SaveList("impressedPeople", gm.impressedPeople);

        PlayerPrefs.Save();
    }

    public static bool LoadFile(ref GameManager dh, int id = 0)
    {
        if (!PlayerPrefs.HasKey("file" + id)) return false;

        //foreach (string name in Enum.GetNames(typeof(Parameters)))
        //{
        //    Parameters sp = (Parameters)Enum.Parse(typeof(Parameters), name);
        //    //dh.SetStateParameter(sp, PlayerPrefs.GetInt(sp.ToString() + id));
        //}

        dh.currentSceneNumber = PlayerPrefs.GetInt("sceneNumber");
        dh.cookedFood = (FoodName) Enum.Parse(typeof(FoodName), PlayerPrefs.GetString("cookedFood"));
        dh.impressedPeople = LoadList("impressedPeople");
        
        return true;
    }

    public static void SetLanguage(Language language)
    {
        PlayerPrefs.SetString("language", language.ToString());
        PlayerPrefs.Save();
    }

    private static void SaveDictionary<Key, Value>(string dictionaryName, Dictionary<Key, Value> dictionary)
    {
        string data = "";
        foreach (Key key in dictionary.Keys)
        {
            data += key + " " + dictionary[key] + '\n';
        }
        PlayerPrefs.SetString(dictionaryName, data);
    }

    private static void LoadDictionary<Key, Value>(string dictionaryName, ref Dictionary<Key, Value> dictionary)
    {
        string[] keyValuePairs = PlayerPrefs.GetString(dictionaryName).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        for (int i = 0; i < keyValuePairs.Length - 1; i++)
        {
            string[] keyValuePair = keyValuePairs[i].Split(' ');
            Key key = (Key)Enum.Parse(typeof(Key), keyValuePair[0]);
            Value value = (Value)Enum.Parse(typeof(Value), keyValuePair[1]);

            dictionary[key] = value;
        }
    }

    private static void SaveList(string listName, List<CharacterName> list)
    {
        string data = "";
        for (int i = 0; i < list.Count; i++)
        {
            data += list[i].ToString() + '\n';
        }
        PlayerPrefs.SetString(listName, data);
    }

    private static void SaveArray(string arrayName, float[] array)
    {
        string data = "";
        for (int i = 0; i < array.Length; i++)
        {
            string s = array[i].ToString();
            data += array[i].ToString() + '\n';
        }
        PlayerPrefs.SetString(arrayName, data);
    }


    private static List<CharacterName> LoadList(string listName)
    {
        List<CharacterName> list = new List<CharacterName>();
        string[] values = PlayerPrefs.GetString(listName).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None); ;
        for (int i = 0; i < values.Length - 1; i++)
        {
            list.Add((CharacterName) Enum.Parse(typeof(CharacterName), values[i]));
        }
        return list;
    }

    public static void Delete(int id)
    {
        if (PlayerPrefs.HasKey("file" + id))
            PlayerPrefs.DeleteKey("file" + id);
    }
}
