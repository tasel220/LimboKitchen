using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private int currentIndex = 0; //1부터 세기
    private int length = 0;
    List<Speech> speeches = new List<Speech>();
    private SpriteName charName;

    //public int SceneNumber;
    public Image background;
    public Image leftSpeaker;
    public Image rightSpeaker;
    public Text printName;
    public Text utterance;

    private bool allowNext = true;


    private void Start()
    {
        //leftSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        leftSpeaker.sprite = GetSprite(SpriteName.None);
        rightSpeaker.sprite = GetSprite(SpriteName.None);
        //ParseDialogue(Resources.Load<TextAsset>("Text/Scene" + SceneNumber.ToString()).text);
        if(GameManager.instance != null)
            ParseDialogue(GameManager.instance.rawDialogue);
        NextTalk();
    }

    //public void ParseDialogue(string rawDialogue)
    public void ParseDialogue(List<string> rows)
    {
        //string[] rows = rawDialogue.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        var f = rows[0].Split('\t');
        background.sprite = Resources.Load<Sprite>("Image/Background/" + f[0]);
        //Debug.Log(f[0]);
        charName = (SpriteName) Enum.Parse(typeof(SpriteName), f[1]);

        length = rows.Count - 1;
        for(int i = 1; i < rows.Count; i++)
        {
            speeches.Add(new Speech(rows[i], charName));
        }
    }

    public void NextTalk()
    {
        if (!allowNext) return;
        if (++currentIndex >= length) EndTalk();
        else
        {
            var curSpeech = speeches[currentIndex];

            if (curSpeech.right)
                //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[curSpeech.spriteName];
                rightSpeaker.sprite = GetSprite(curSpeech.spriteName, curSpeech.emotion);
            else
                leftSpeaker.sprite = GetSprite(curSpeech.spriteName, curSpeech.emotion);
            printName.text = curSpeech.printName;
            utteranceToPrint = curSpeech.utterance;
            StartCoroutine(Type());
            //utterance.text = "";
            //utterance.DOText(curSpeech.utterance, 0.5f);
        }
    }

    private string utteranceToPrint;
    private IEnumerator Type()
    {
        allowNext = false;
        utterance.text = "";
        for(int i = 0; i < utteranceToPrint.Length; i++)
        {
            utterance.text += utteranceToPrint[i];
            yield return new WaitForSeconds(0.01f);
        }
        allowNext = true;
    }
    
    public void EndTalk()
    {
        GameManager.instance.rawDialogue.Clear();
        GameManager.instance.Proceed();
    }

    struct Speech
    {
        public string printName;
        public SpriteName spriteName;
        public string emotion;
        public string utterance;
        public bool right;

        public Speech(string row, SpriteName charName)
        {
            char[] trimmers = { ':', '\t' };
            string[] s = row.Split(trimmers);
            right = false;
            spriteName = charName;
            emotion = "";

            if (s[0].Contains("/"))
            {
                printName = "";
                //spriteName = SpriteName.None;
                utterance = s[0];
                utterance = utterance.Replace("/", " ");
                //Debug.Log(utterance);
            }
            else
            {
                printName = s[0].Trim();
                //spriteName = (SpriteName)Enum.Parse(typeof(SpriteName), s[0].Trim());
                utterance = s[1];
                string[] u = utterance.Split(')');
                if(u.Length > 1)
                {
                    //Debug.Log(u[0]);
                    //Debug.Log(u[1]);
                    utterance = u[1];
                    emotion = u[0].Trim();
                    emotion = emotion.Remove(0,1);
                }

                utterance.Trim();

                if (printName == "도운" || printName == "주인공")
                {
                    right = true;
                    spriteName = SpriteName.Dowoon;
                }
            }
        }
    }

    private Sprite GetSprite(SpriteName charName, string emotion = "")
    {
        if(emotion == "") return Resources.Load<Sprite>("Image/Character/" + charName);

        emotion = GameManager.instance.Emotion[emotion];
        Sprite s = Resources.Load<Sprite>("Image/Character/" + charName + "_" + emotion);
        if(s == null) return Resources.Load<Sprite>("Image/Character/" + charName);
        return s;
    }

}
