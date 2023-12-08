using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //用伪单例的方式写
    public static AudioManager instance;

    AudioSource au;
    List<AudioClip> audioClips = new List<AudioClip>();

    void Awake()
    {
        instance = this;
        for(int i = 0; i < 21; i++)
        {
            audioClips.Add(Resources.Load<AudioClip>("Audio/Voice"+i));

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
    }

    public void PlayAudio(int index)
    {
        au.clip = audioClips[index];
        au.Play();
    }
   
}
