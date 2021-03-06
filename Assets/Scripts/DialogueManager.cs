﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class DialogueManager : MonoBehaviour, IManager
{
    private int currentIndex = -1; //1부터 세기
    List<Line> speeches = new List<Line>();
    private CharacterName charName;

    //public int SceneNumber;
    public Image background;
    public Image speaker;
    public Text printName;
    public Text utterance;

    public GameObject choice2;
    public Text choiceText2;

    public float textDelay;


    public GameObject pointerL;
    public GameObject pointerR;

    public AudioSource bgm;

    private bool allowNext = true;


    private void Start()
    {
        GameManager.instance.currentSceneManager = this;
        //leftSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        //rightSpeaker.sprite = GameManager.instance.SpriteDictionary[SpriteName.None];
        speaker.sprite = GetSprite(CharacterName.None);
        //ParseDialogue(Resources.Load<TextAsset>("Text/Scene" + SceneNumber.ToString()).text);
        if(GameManager.instance != null)
            ParseDialogue(GameManager.instance.rawDialogue);
        NextTalk();
    }

    //public void ParseDialogue(string rawDialogue)
    public void ParseDialogue(List<string> rows)
    {
        speeches.Clear();

        string[] f = rows[0].Split('\t');

        //string[] firstRow = 
        background.sprite = Resources.Load<Sprite>("Image/Background/" + f[0]);

        if (f.Length > 1)
        {
            Debug.Log(f[1]);
            SetCharOrBGM(f[1]);

            if(f.Length > 2)
            {
                SetCharOrBGM(f[2]);
            }
        }

        for(int i = 1; i < rows.Count; i++)
        {
            var lastLine = new Line(rows[i], charName);
            speeches.Add(lastLine);
            if(lastLine.lineType == LineType.enter && lastLine.spriteName != CharacterName.Dowoon)
            {
                charName = lastLine.spriteName;
            }
            else if(lastLine.lineType == LineType.reaction)
            {
                speeches.Remove(lastLine);
                Debug.Log(GameManager.instance.cookedFood);
                var reactionRows = Resources.Load<TextAsset>(GameManager.instance.textPath + lastLine.reactionFile).text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                bool foodFound = false;
                for(int k = 0; k < reactionRows.Length; k++)
                {
                    var splitRow = reactionRows[k].Split(new[] { '\t', ':' });
                    var splitRowList = new List<string>(splitRow);
                    Debug.Log(splitRow[0]);
                    if(Enum.TryParse(splitRow[0], out FoodName fn))
                    {
                        if (foodFound) break;
                        if (reactionRows[k].Contains(GameManager.instance.cookedFood.ToString()))
                        foodFound = true;
                        if (!GameManager.instance.impressedPeople.Contains(charName))
                        {
                            GameManager.instance.impressedPeople.Add(charName);
                        }
                    }
                    else if(splitRow[0] == "else")
                    {
                        
                        if (foodFound) break;
                        foodFound = true;
                        if (GameManager.instance.impressedPeople.Contains(charName))
                        {
                            GameManager.instance.impressedPeople.Remove(charName);
                        }
                    }
                    else if(foodFound)
                    {
                        lastLine = new Line(reactionRows[k], charName);
                        speeches.Add(lastLine);
                        if (lastLine.lineType == LineType.enter && lastLine.spriteName != CharacterName.Dowoon)
                        {
                            charName = lastLine.spriteName;
                        }
                    }
                }
            }
        }
    }

    private void SetCharOrBGM(string input)
    {
        if (Enum.TryParse(input, out CharacterName cn))
            charName = cn;
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("SoundFiles/" + input);
            if (clip != null)
            {
                bgm.clip = clip;
                bgm.Play();
            }
        }
    }

    public void NextTalk()
    {
        if (!allowNext)
        {
            return;
        }
        if (++currentIndex >= speeches.Count)
        {
            EndTalk();
        }

        else
        {
            //Debug.Log(currentIndex);
            var curLine = speeches[currentIndex];

            utterance.raycastTarget = false;
            choice2.SetActive(false);
            utterance.alignment = TextAnchor.UpperLeft;
            background.raycastTarget = true;
            //pointerL.SetActive(false);
            //pointerR.SetActive(false);

            switch (curLine.lineType)
            {
                case LineType.speech:
                    //Debug.Log(curLine.utterance);
                    if (!curLine.MC)
                        speaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    
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
                    if (!curLine.MC)
                        speaker.sprite = GetSprite(curLine.spriteName, curLine.emotion);
                    //charName = curLine.spriteName;
                    NextTalk();
                    break;

                case LineType.exit:
                    //if (leftSpeaker.sprite.name.Contains(curLine.spriteName.ToString()))
                    speaker.sprite = GetSprite(CharacterName.None);
                    Debug.Log(charName + " 퇴장");
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
                    background.raycastTarget = false;
                    break;

                case LineType.ending:
                    int impressedN = GameManager.instance.impressedPeople.Count;
                    if(impressedN < 2)
                    {
                        GameManager.instance.LoadNewDialogue(curLine.endings[0]);
                    }
                    else if (impressedN < 4)
                    {
                        GameManager.instance.LoadNewDialogue(curLine.endings[1]);
                    }
                    else
                    {
                        GameManager.instance.LoadNewDialogue(curLine.endings[2]);
                    }
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
            yield return new WaitForSeconds(textDelay);
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
        public bool MC;

        public LineType lineType;

        public string choice1;
        public string choice2;

        public string[] result;

        public string reactionFile;

        public string[] endings;

        public Line(string row, CharacterName charName)
        {
            char[] trimmers = { ':', '\t' };
            string[] s = row.Split(trimmers);
            MC = false;
            spriteName = charName;
            emotion = "";
            utterance = "";
            printName = "";
            choice1 = "";
            choice2 = "";
            result = new string[] { "", ""};
            reactionFile = "";
            endings = new string[] { "", "", "" };

            s[0] = s[0].Trim();
            if (s[0].StartsWith("/"))
            {
                lineType = LineType.narration;
                utterance = s[0];
                utterance = utterance.Replace("/", "");
            }
            else if (s[0].Contains("enter"))
            {
                lineType = LineType.enter;

                if (s[0] != "enter")
                    s = s[0].Split(' ');
                spriteName = (CharacterName) Enum.Parse(typeof(CharacterName), s[1]);
                if (s.Length > 2) emotion = s[2];

                if (spriteName == CharacterName.Dowoon)
                {
                    MC = true;
                }
            }
            else if(s[0].Contains("exit"))
            {
                if (s[0] != "exit")
                    s = s[0].Split(' ');
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

            else if (s[0] == "reaction")
            {
                lineType = LineType.reaction;
                reactionFile = s[1];
            }

            else if (s[0] == "ending")
            {
                lineType = LineType.ending;
                endings[0] = s[1];
                endings[1] = s[2];
                endings[2] = s[3];
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


                if (printName == "도운" || printName == "주인공" || printName == "Main character" || printName == "Dawn")
                {
                    MC = true;
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
            return Resources.Load<Sprite>("Image/Character/" + charName);
        }
        if(GameManager.instance.Emotion.ContainsKey(emotion))
            emotion = GameManager.instance.Emotion[emotion];

        Sprite s = Resources.Load<Sprite>("Image/Character/" + charName + "_" + emotion);
        if(s == null) return Resources.Load<Sprite>("Image/Character/" + charName);
        Debug.Log(charName);
        return s;
    }

    private int choiceN;
    public void Choice(int num)
    {
        var nextLine = speeches[currentIndex + 1];

        if(nextLine.lineType == LineType.result)
        {
            choiceN = num;
            GameManager.instance.LoadNewDialogue(nextLine.result[choiceN - 1]);
        }
        else
        {
            NextTalk();

        }
    }

    public void ChangeLanguage(Language selectedLangugae)
    {

    }
}
