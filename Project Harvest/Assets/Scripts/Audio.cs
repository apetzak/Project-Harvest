﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private Audio() { }

    public static Audio Instance { get; } = new Audio();

    private List<GameObject> objects;
    public int currentIndex = 0;

    void Start()
    {
        BuildAudioSourceObjects();
    }

    void Update() { }

    void BuildAudioSourceObjects()
    {
        var v = Resources.LoadAll<AudioClip>("Audio");
        Instance.objects = new List<GameObject>();
        for (int i = 0; i < 30; i++)
        {
            GameObject o = new GameObject();
            AudioSource s = o.AddComponent<AudioSource>();
            Instance.objects.Add(o);
        }
        Debug.Log(Instance.objects);
    }

    public void PlaySound(AudioClip clip)
    {
        Debug.Log(clip);
        Debug.Log(Instance.objects);
        Debug.Log(Instance.objects[currentIndex].GetComponent<AudioSource>());

        Instance.objects[currentIndex].GetComponent<AudioSource>().clip = clip;
        Instance.objects[currentIndex++].GetComponent<AudioSource>().Play();

        if (currentIndex == 30)
            currentIndex = 0;
    }

    public void PlayDeath(int index)
    {
        PlaySound(Resources.Load<AudioClip>($"Audio/wilhelm"));
    }

    public void PlayAttack(int index)
    {
        //Instance.objects[currentIndex].GetComponent<AudioSource>().clip = TroopClass.Instance.list[index - 2].sounds[0];
        //Instance.objects[currentIndex++].GetComponent<AudioSource>().Play();

        //if (currentIndex == 30)
        //    currentIndex = 0;
        PlaySound(TroopClass.Instance.list[index - 2].sounds[0]);
    }
}
