using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject o;
    public MeshRenderer mesh;
    private Vector3 velocity;
    private float launchDistance = 0;
    public bool hit = false;
    public Vector3 hitLocation;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (launchDistance > 0 && o != null)
            Propel();
    }

    public void Propel()
    {
        o.transform.Translate(velocity * 1.5f, Space.World);
        launchDistance -= velocity.magnitude * 1.5f;
        if (launchDistance <= 0)
        {
            hit = true;
            launchDistance = 0;
            hitLocation = o.transform.position;
            Destroy(o);
        }
    }

    public void Spawn(Vector3 destination)
    {
        Vector3 diff = transform.position - destination;
        launchDistance = Mathf.RoundToInt(diff.magnitude);
        velocity = new Vector3(-diff.x, 0, -diff.z).normalized;
        o = Instantiate(gameObject, transform.position, Quaternion.identity);
        o.transform.localScale = transform.lossyScale;
        o.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
