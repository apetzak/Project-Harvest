using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioPlayer() { }

    public static AudioPlayer Instance { get; } = new AudioPlayer();

    private List<GameObject> objects;
    private List<AudioSource> sources;
    public int currentIndex = 0;

    void Start()
    {
        BuildAudioSourceObjects();
    }

    private void Update() { }

    private void BuildAudioSourceObjects()
    {
        //var v = Resources.LoadAll<AudioClip>("Audio");
        GameObject parent = new GameObject();
        parent.name = "Audio Objects";
        Instance.objects = new List<GameObject>();
        Instance.sources = new List<AudioSource>();
        for (int i = 0; i < 30; i++)
        {
            GameObject o = new GameObject();
            o.name = "Audio Source " + i;
            AudioSource s = o.AddComponent<AudioSource>();
            s.volume *= .1f;
            o.transform.SetParent(parent.transform);
            Instance.sources.Add(s);
            Instance.objects.Add(o);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        //Instance.objects[currentIndex].GetComponent<AudioSource>().clip = clip;
        //Instance.objects[currentIndex++].GetComponent<AudioSource>().Play();

        Instance.sources[currentIndex].clip = clip;
        Instance.sources[currentIndex++].Play();

        if (currentIndex == 30)
            currentIndex = 0;
    }

    public void PlayDeath(int index)
    {
        PlaySound(Resources.Load<AudioClip>($"Audio/wilhelm"));
    }

    public void PlayExplosion()
    {
        PlaySound(Resources.Load<AudioClip>($"Audio/explosion"));
    }

    public void PlayAttack(int index)
    {
        if (currentIndex == 30)
            currentIndex = 0;
        PlaySound(TroopClass.Instance.list[index - 2].sounds[0]);
    }
}
