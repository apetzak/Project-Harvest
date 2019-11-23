using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    MeshRenderer mesh;
    public float speed = .0015f;
    public int burstTime = 10;
    public int burstTimer = 10;
    public bool popping = false;
    private bool isTemp = false;
    Vector3 initSize;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        initSize = transform.localScale;
    }

    public void Spawn()
    {
        isTemp = true;
        mesh = GetComponent<MeshRenderer>();
        speed = .4f;
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
        transform.localScale += new Vector3(speed, speed, speed);
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
