using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    public GameObject selector;
    public int index;
    public bool fruit = true;
    public bool selected;
    public Vector3 destination;
    public Vector3 velocity;
    public bool moving;
    public float speed;
    public float currentSpeed;
    public float facingAngle;
    public float health;
    public float maxHealth;
    public float lineOfSight;
    public bool dying = false;
    public int deathTimer = 150;
    private int clickTimer;
    private bool clickedOnce = false;
    public Troop target;
    private static float rad = 180.0f / Mathf.PI;
    private Collider coll;

    public void Start()
    {
        coll = GetComponent<Collider>();
    }

    public virtual void Update()
    {
        if (moving)
            Move();
    }

    public virtual void RightClick()
    {

    }

    public void SelectType()
    {
        int selectedCount = 0;
        foreach (Unit u in Game.Instance.troops)
        {
            if (selectedCount > 84)
                break;

            // to do: only select units in screen
            if (index == u.index)
            {
                selectedCount++;
                u.ToggleSelected(true);
            }
        }
        foreach (Unit u in Game.Instance.workers)
        {
            if (selectedCount > 84)
                break;

            // to do: only select units in screen
            if (index == u.index)
            {
                selectedCount++;
                u.ToggleSelected(true);
            }
        }
        //Debug.Log("select type " + selectedCount);
        Game.Instance.ChangeSelection(selectedCount);
    }

    int ss = 0;
    public virtual void OnMouseOver()
    {
        //Debug.Log("unit");
        if (Input.GetMouseButtonDown(0))
        {
            if (clickedOnce == true)
                SelectType();
            clickedOnce = true;
        }
        else if (Input.GetMouseButtonDown(1))
            RightClick();

        if (clickedOnce)
            clickTimer++;

        if (clickTimer == 20)
        {
            clickTimer = 0;
            clickedOnce = false;
        }
    }

    public void ToggleSelected(bool b)
    {
        var mr = selector.GetComponentInChildren<MeshRenderer>();
        mr.enabled = selected = b;
    }

    void OnMouseDown()
    {
        //Debug.Log("mouse down");
        foreach (Unit u in Game.Instance.troops)
            u.ToggleSelected(false);

        foreach (Unit u in Game.Instance.workers)
            u.ToggleSelected(false);

        ToggleSelected(true);
        Game.Instance.selectedUnit = this;
        Game.Instance.selectionCount = 1;
        Game.Instance.selectionChanged = true;
        Game.Instance.holdingDown = false;
    }

    public void Move()
    {
        Vector3 v = transform.position - destination;
        if (Mathf.Abs(v.x) < 2 && Mathf.Abs(v.z) < 2)
            StopMoving();
        else
            transform.Translate(velocity * currentSpeed / 10, Space.World);

        coll.enabled = false;
        coll.enabled = true;
    }

    public void SetDestination(Vector3 v)
    {
        currentSpeed = speed;
        destination = v;
        Vector3 diff = transform.position - v;
        velocity = GetVelocity(diff.x, diff.z);
        RotateTowards(diff.x, diff.z);
        moving = true;
    }

    public Vector3 GetVelocity(float x, float z)
    {
        return new Vector3(-x, 0, -z).normalized;
    }

    public void StopMoving()
    {
        moving = false;
        velocity = new Vector3();
        destination = transform.position;
    }

    public virtual void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, diff, 0, Space.Self);
        facingAngle += diff;
    }

    public float GetAngle(float x, float z)
    {
        return Mathf.Atan2(x, z) * rad - facingAngle;
    }
}