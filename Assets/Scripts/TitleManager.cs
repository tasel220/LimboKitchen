using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public bool openAllChapters;

    public static TitleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Continue()
    {
        if (Saver.LoadFile(ref GameManager.instance))
            GameManager.instance.Proceed();
        else
            NewGame();
    }

    public void SelectChapter(int index)
    {
        Debug.Log(index);
        if (PlayerPrefs.GetInt("sceneNumber") < index && !openAllChapters) return;

        GameManager.instance.currentSceneNumber = index;

        GameManager.instance.Proceed();
    }

    public void NewGame()
    {
        //Saver.Delete(0);
        //GameManager.instance.currentSceneNumber = 1;
        PlayerPrefs.SetInt("sceneNumber", 1);
        GameManager.instance.Proceed();
    }
}
