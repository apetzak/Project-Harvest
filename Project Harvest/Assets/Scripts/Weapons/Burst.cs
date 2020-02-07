using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    private MeshRenderer mesh;
    public float expansionRate = .0015f;
    public int burstTime = 10;
    public int burstTimer = 10;
    public bool popping = false;
    private bool isTemp = false;
    private Vector3 initSize;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        initSize = transform.localScale;
    }

    public void Spawn()
    {
        isTemp = true;
        mesh = GetComponent<MeshRenderer>();
        expansionRate = .4f;
        burstTime = 35;
        Pop();
    }

    public void Pop()
    {
        burstTimer = burstTime;
        mesh.enabled = true;
        popping = true;
    }

    void Update()
    {
        if (!popping)
            return;
        burstTimer--;
        transform.localScale += new Vector3(expansionRate, expansionRate, expansionRate);
        if (burstTimer <= 0)
        {
            mesh.enabled = false;
            transform.localScale = initSize;
            burstTimer = burstTime;
            popping = false;
            if (isTemp)
                Destroy(gameObject);
        }
    }
}
