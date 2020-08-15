using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioBtn : MonoBehaviour
{
    public Sprite play;
    public Sprite stop;
    public AudioSource music;
    private bool isPlay = true;

    public void ChangeMode()
    {
        isPlay = !isPlay;
        if (isPlay)
        {
            gameObject.GetComponent<Image>().sprite = play;
            music.Play();
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = stop;
            music.Stop();
        }
    }
}