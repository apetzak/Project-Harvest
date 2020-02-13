using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : Structure
{
    public GameObject head;
    public GameObject waterDrop;
    private List<GameObject> drops = new List<GameObject>();
    private int timeBeforeShot = 20;

    protected override void Update()
    {
        timeBeforeShot--;
        if (timeBeforeShot < 0)
            SpawnWaterDrop();

        MoveDrops();

        head.transform.Rotate(0, 1, 0); // spin around
    }

    private void SpawnWaterDrop()
    {
        GameObject o = Instantiate(waterDrop, waterDrop.transform.position, waterDrop.transform.rotation);
        o.transform.localScale *= 5;
        o.transform.Rotate(0, Random.Range(-10, 10), 0, Space.Self);
        drops.Add(o);
        timeBeforeShot = Random.Range(2, 12);
    }

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

    public override void TakeDamage()
    {
        if (health <= 0)
        {
            foreach (GameObject o in drops)
                Destroy(o);
        }

        base.TakeDamage();
    }
}