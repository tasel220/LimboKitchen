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
    List<Line> speeches = new List<Line>();
    private CharacterName charName;

    //public int SceneNumber;
    public Image background;
    public Image leftSpeaker;
    public Image rightSpeaker;
    public Text printName;
    public Text utterance;

    public GameObject choice2;
    public Text choiceText2;

    public GameObject pointerL;
    public GameObject pointerR;

    private bool allowNext = true;


    private void Start()
    {
        //leftSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        leftSpeaker.sprite = GetSprite(CharacterName.None);
        rightSpeaker.sprite = GetSprite(CharacterName.None);
        //ParseDialogue(Resources.Load<TextAsset>("Text/Scene" + SceneNumber.ToString()).text);
        if(GameManager.instance != null)
            ParseDialogue(GameManager.instance.rawDialogue);
        NextTalk();
    }

    //public void ParseDialogue(string rawDialogue)
    public void ParseDialogue(List<string> rows)
    {
        //string[] rows = rawDialogue.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        background.sprite = Resources.Load<Sprite>("Image/Background/" + rows[0]);
        length = rows.Count - 1;
        for(int i = 1; i < rows.Count; i++)
        {
            speeches.Add(new Line(rows[i], charName));
        }
    }

    public void NextTalk()
    {
        if (!allowNext) return;
        if (++currentIndex >= length) EndTalk();
        
        else
        {
            var curLine = speeches[currentIndex];

            utterance.raycastTarget = false;
            choice2.SetActive(false);
            utterance.alignment = TextAnchor.UpperLeft;
            //pointerL.SetActive(false);
            //pointerR.SetActive(false);
            
            switch(curLine.lineType)
            {
                case LineType.speech:
                    if (curLine.right)
                        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[curSpeech.spriteName];
                        rightSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    else
                        leftSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    printName.text = curLine.printName;
                    utteranceToPrint = curLine.utterance;
                    StartCoroutine(Type());
                    //utterance.text = "";
                    //utterance.DOText(curSpeech.utterance, 0.5f);
                    break;

                case LineType.narration:

                    printName.text = curLine.printName;
                    utteranceToPrint = curLine.utterance;
                    StartCoroutine(Type());
                    //utterance.text = "";
                    //utterance.DOText(curSpeech.utterance, 0.5f);
                    break;
                case LineType.enter:
                    if (curLine.right)
                        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[curSpeech.spriteName];
                        rightSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    else
                        leftSpeaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);

                    NextTalk();
                    break;

                case LineType.exit:
                    //if (leftSpeaker.sprite.name.Contains(curLine.spriteName.ToString()))
                    leftSpeaker.sprite = GetSprite(CharacterName.None);
                    //else if (rightSpeaker.sprite.name.Contains(curLine.spriteName.ToString()))
                    //    rightSpeaker.sprite = GetSprite(CharacterName.None);

                    NextTalk();
                    break;

                case LineType.choice:
                    choice2.SetActive(true);
                    utterance.text = curLine.choice1;
                    choiceText2.text = curLine.choice2;
                    utterance.raycastTarget = true;
                    utterance.alignment = TextAnchor.UpperCenter;
                    break;

                case LineType.result:
                    GameManager.instance.Result(curLine.result[choiceN - 1]);
                    break;
            }

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

    struct Line
    {
        public string printName;
        public CharacterName spriteName;
        public string emotion;
        public string utterance;
        public bool right;

        public LineType lineType;

        public string choice1;
        public string choice2;

        public string[] result;

        public Line(string row, CharacterName charName)
        {
            char[] trimmers = { ':', '\t' };
            string[] s = row.Split(trimmers);
            right = false;
            spriteName = charName;
            emotion = "";
            utterance = "";
            printName = "";
            choice1 = "";
            choice2 = "";
            result = new string[] { "", ""};

            s[0] = s[0].Trim();
            if (s[0].StartsWith("/"))
            {
                lineType = LineType.narration;
                utterance = s[0];
                utterance = utterance.Replace("/", "");
            }
            else if (s[0] == "enter")
            {
                lineType = LineType.enter;

                if (printName == "도운" || printName == "주인공")
                {
                    right = true;
                    spriteName = CharacterName.Dowoon;
                }
                else
                {
                    charName = (CharacterName)Enum.Parse(typeof(CharacterName), s[1]);
                    if(s.Length > 2) emotion = s[2];
                }
            }
            else if(s[0] == "exit")
            {
                lineType = LineType.exit;
            }

            else if(s[0] == "choice")
            {
                lineType = LineType.choice;
                choice1 = s[1].Trim();
                choice2 = s[2].Trim();
            }
            
            else if (s[0] == "result")
            {
                lineType = LineType.result;
                result[0] = s[1];
                result[1] = s[2];
            }

            else
            {
                lineType = LineType.speech;
                printName = s[0].Trim();
                utterance = s[1];
                string[] u = utterance.Split(')');
                if(u.Length > 1)
                {
                    utterance = u[1];
                    emotion = u[0].Trim();
                    emotion = emotion.Remove(0,1);
                }


                if (printName == "도운" || printName == "주인공")
                {
                    right = true;
                    spriteName = CharacterName.Dowoon;
                }
            }

            utterance = utterance.Trim();
        }
    }

    private Sprite GetSprite(CharacterName charName, string emotion = "")
    {
        if (emotion == "")
        {
            //Debug.Log(Resources.Load<Sprite>("Image/Character/" + charName));
            return Resources.Load<Sprite>("Image/Character/" + charName);
        }

        emotion = GameManager.instance.Emotion[emotion];
        Sprite s = Resources.Load<Sprite>("Image/Character/" + charName + "_" + emotion);
        if(s == null) return Resources.Load<Sprite>("Image/Character/" + charName);
        return s;
    }

    private int choiceN;
    public void Choice(int num)
    {
        NextTalk();
        choiceN = num;
    }
}
