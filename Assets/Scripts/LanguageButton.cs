using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public Language language;

    private void Awake()
    {
        transform.GetChild(0).GetComponent<Text>().text = language == Language.Korean ? "한" : "Eng";
        GetComponent<Button>().onClick.AddListener(() => { GameManager.SetLanguage(language); });
    }
}
