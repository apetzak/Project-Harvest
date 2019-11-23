using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private SoundFX() { }

    public static SoundFX Instance { get; } = new SoundFX();

    public AudioClip gunShot;

    void Start()
    {
        Debug.Log(gunShot);
        Instance.gunShot = gunShot;
    }

    void Update()
    {
        
    }

    public void Play()
    {
        //AudioSource a = new AudioSource();
        //Debug.Log(Instance.gunShot);
        //a.clip = Instance.gunShot;
        //a.Play();
    }
}
