using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TroopClass : MonoBehaviour
{
    private TroopClass() { }

    public static TroopClass Instance { get; } = new TroopClass();
    public List<TroopClass> list;

    public string name;
    public int index;
    public bool fruit = true;
    public float speed;
    public float maxHealth;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float lineOfSight;
    public List<AudioClip> sounds;

    void Start()
    {
        Instance.list = GetProperties();
        //Debug.Log(Instance.list);
    }

    private List<AudioClip> GetClips(params string[] arr)
    {
        var list = new List<AudioClip>();
        foreach (string s in arr)
            list.Add(Resources.Load<AudioClip>($"Audio/{s}"));
        return list;
    }

    public List<TroopClass> GetProperties()
    {
        var list = new List<TroopClass>();
        list.Add(new TroopClass()
        {
            name = "Apple",
            index = 2,
            fruit = true,
            speed = 7,
            maxHealth = 150,
            attackDamage = 15,
            attackRange = 8,
            attackSpeed = 1.25f,
            lineOfSight = 30,
            sounds = GetClips("swing")
        });
        list.Add(new TroopClass()
        {
            name = "Carrot",
            index = 3,
            fruit = false,
            speed = 13,
            maxHealth = 90,
            attackDamage = 30,
            attackRange = 15,
            attackSpeed = 1f,
            lineOfSight = 30,
            sounds = GetClips("swing")
        });
        list.Add(new TroopClass()
        {
            name = "Banana",
            index = 4,
            fruit = true,
            speed = 6,
            maxHealth = 90,
            attackDamage = 10,
            attackRange = 40,
            attackSpeed = .5f,
            lineOfSight = 30,
            sounds = GetClips("ak47")
        });
        list.Add(new TroopClass()
        {
            name = "Brocolli",
            index = 5,
            fruit = false,
            speed = 5,
            maxHealth = 85,
            attackDamage = 6,
            attackRange = 30,
            attackSpeed = .25f,
            lineOfSight = 30,
            sounds = GetClips("ak47")
        });
        list.Add(new TroopClass()
        {
            name = "Pear",
            index = 6,
            fruit = true,
            speed = 5,
            maxHealth = 100,
            attackDamage = 40,
            attackRange = 90,
            attackSpeed = 4.5f,
            lineOfSight = 30,
            sounds = GetClips("rocket_whistle")
        });
        list.Add(new TroopClass()
        {
            name = "Corn",
            index = 7,
            fruit = false,
            speed = 3,
            maxHealth = 120,
            attackDamage = 50,
            attackRange = 100,
            attackSpeed = 5,
            lineOfSight = 30,
            sounds = GetClips("crossbow")
        });
        list.Add(new TroopClass()
        {
            name = "Strawberry",
            index = 8,
            fruit = true,
            speed = 4,
            maxHealth = 120,
            attackDamage = 25,
            attackRange = 90,
            attackSpeed = 3,
            lineOfSight = 30,
            sounds = GetClips("crossbow")
        });
        list.Add(new TroopClass()
        {
            name = "Asparagus",
            index = 9,
            fruit = false,
            speed = 4,
            maxHealth = 110,
            attackDamage = 30,
            attackRange = 110,
            attackSpeed = 4,
            lineOfSight = 30,
            sounds = GetClips("sniper_rifle")
        });
        list.Add(new TroopClass()
        {
            name = "Watermelon",
            index = 10,
            fruit = true,
            speed = 3,
            maxHealth = 175,
            attackDamage = 25,
            attackRange = 20,
            attackSpeed = 1.5f,
            lineOfSight = 30,
            sounds = GetClips("swing")
        });
        list.Add(new TroopClass()
        {
            name = "Onion",
            index = 11,
            fruit = false,
            speed = 5,
            maxHealth = 130,
            attackDamage = 30,
            attackRange = 40,
            attackSpeed = 2.5f,
            lineOfSight = 30,
            sounds = GetClips("shotgun")
        });
        return list;
    }
}