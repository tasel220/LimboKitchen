using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelection : MonoBehaviour
{
    public int totalSceneCount;
    public Button buttonPrefab;
    public Button[] buttons;
    public Image[] images;

    private void Awake()
    {
        buttons = new Button[totalSceneCount];
        images = new Image[totalSceneCount];
        int lastIndex = PlayerPrefs.GetInt("sceneNumber");
        Debug.Log(lastIndex);
        for(int i = 1; i <= totalSceneCount; i++)
        {
            Button newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(transform);
            newButton.transform.GetChild(0).GetComponent<Text>().text = "Chapter " + i.ToString();

            int x = i;

            newButton.transform.localScale = Vector3.one;
            buttons[x - 1] = newButton;
            images[x - 1] = newButton.GetComponent<Image>();
            newButton.onClick.AddListener(() => {
                TitleManager.instance.SelectChapter(x);
            });
            if (x > lastIndex)
                DeactivateButton(x);
            else
                ActivateButton(x);
        }

        transform.parent.gameObject.SetActive(false);
    }

    private void ActivateButton(int index)
    {
        buttons[index - 1].interactable = true;
        images[index - 1].color = Color.white;
    }
    private void DeactivateButton(int index)
    {
        //buttons[index].interactable = false;
        images[index - 1].color = Color.grey;
    }
}
