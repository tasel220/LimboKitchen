using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public void Continue()
    {
        if (Saver.LoadFile(ref GameManager.instance))
            GameManager.instance.Proceed();
        else
            NewGame();
    }

    public void NewGame()
    {
        Saver.Delete(0);
        GameManager.instance.Proceed();
    }
}
