﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : Structure
{
    public GameObject head;
    public GameObject waterDrop;
    private List<GameObject> drops = new List<GameObject>();
    private int timeBeforeShot = 20;
    public bool turnedOn = false;
    public bool hasSource = false;

    protected override void Start()
    {
        OnBuilt();
        head.transform.Rotate(0, Random.Range(0, 360), 0);
        turnedOn = false;
        base.Start();
    }

    /// <summary>
    /// Periodically spawn water drops, move all water drops, rotate head
    /// </summary>
    protected override void Update()
    {
        if (!isBuilt || !turnedOn || !hasSource)
            return;

        timeBeforeShot--;
        if (timeBeforeShot < 0)
            SpawnWaterDrop();

        MoveDrops();

        head.transform.Rotate(0, 1, 0);
    }

    /// <summary>
    /// Turn on sprinkler if ally WaterTower is in range
    /// </summary>
    protected override void OnBuilt()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s is WaterTower)
            {
                float dist = (transform.position - s.transform.position).magnitude;
                if (dist < 160) // water tower in range
                {
                    (s as WaterTower).sprinklers.Add(this);
                    turnedOn = (s as WaterTower).turnedOn;
                    hasSource = true;
                    return;
                }
            }
        }
        hasSource = false;
    }

    /// <summary>
    /// Spawn drop in a 20 degree range from spout, reset spawn timer
    /// </summary>
    private void SpawnWaterDrop()
    {
        GameObject o = Instantiate(waterDrop, waterDrop.transform.position, waterDrop.transform.rotation);
        o.transform.localScale *= 5;
        o.transform.Rotate(0, Random.Range(-10, 10), 0, Space.Self);
        drops.Add(o);
        timeBeforeShot = Random.Range(2, 12);
    }

    /// <summary>
    /// Propel drops towards destination (forward and slightly down)
    /// Then destroy once below ground.
    /// </summary>
    private void MoveDrops()
    {
        float dropDist = Random.Range(.05f, .15f);

        for (int i = drops.Count - 1; i >= 0; i--)
        {
            drops[i].transform.Translate(1, 0, 0, Space.Self);
            drops[i].transform.Translate(0, -dropDist, 0, Space.World);

            if (drops[i].transform.localPosition.y < 0)
            {
                Destroy(drops[i].gameObject);
                drops.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// If health is below zero, destroy sprinkler and all water drops
    /// </summary>
    public override void TakeDamage()
    {
        if (health <= 0)
            TurnOff();

        base.TakeDamage();
    }

    public void TurnOff()
    {
        turnedOn = false;
        hasSource = false;
        foreach (GameObject o in drops)
            Destroy(o);
        drops.Clear();
    }
}