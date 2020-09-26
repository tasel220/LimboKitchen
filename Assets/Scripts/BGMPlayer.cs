using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour, IPause
{
    public void Start()
    {
        GameManager.instance.pausers.Add(this);
    }

    public AudioSource source;

    public void OnPause()
    {
        source.Pause();
    }
    public void OnResume()
    {
        source.UnPause();
    }
}
