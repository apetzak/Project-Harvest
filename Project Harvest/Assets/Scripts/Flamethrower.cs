using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// todo
/// </summary>
public class Flamethrower : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        GetComponent<ParticleSystem>().Play();

        Debug.Log(GetComponent<ParticleSystem>().isPlaying);
    }
}
