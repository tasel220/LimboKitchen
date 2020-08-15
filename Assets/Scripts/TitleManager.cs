using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour, IManager
{
    public bool openAllChapters;

    public static TitleManager instance;

    public Text[] buttonTexts;
    private string[] koreanButtonNames = { "새로운 시작", "챕터 선택", "만든 사람", "종료" };
    private string[] englishButtonNames = { "New Game", "Select Chapter", "Credit", "Quit" };

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.currentSceneManager = this;
        ChangeLanguage(GameManager.instance.language);
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

    public void ChangeLanguage(Language selectedLanguage)
    {
        if(selectedLanguage == Language.English)
            for(int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].text = englishButtonNames[i];
            }

        else
            for (int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].text = koreanButtonNames[i];
            }
    }
}
