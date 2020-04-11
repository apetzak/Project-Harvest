using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Global object that provides easy access to all prefabs, materials, and images
/// </summary>
public class Assets : MonoBehaviour
{
    public static Assets Instance { get; } = new Assets();

    public Pea pea;
    public Blueberry bb;
    public List<Troop> troops;
    public List<Structure> structures;
    public List<Farm> farms;
    public List<Resource> resources; // unused
    public List<Material> materials;
    public List<Texture2D> unitSprites; // unused
    public List<Sprite> structureSprites;
    public List<Texture2D> cursors; // unused

    private void Awake()
    {
        InstantiateGlobalProperties();
        LoadUnits();
        LoadStructures();
        LoadFarms();
        LoadResources();
        LoadMaterials();
        LoadUnitSprites();
        LoadStructureSprites();
        LoadCursors();
    }

    private void InstantiateGlobalProperties()
    {
        Instance.troops = new List<Troop>();
        Instance.structures = new List<Structure>();
        Instance.farms = new List<Farm>();
        Instance.resources = new List<Resource>();
        Instance.materials = new List<Material>();
        Instance.unitSprites = new List<Texture2D>();
        Instance.structureSprites = new List<Sprite>();
        Instance.cursors = new List<Texture2D>();
    }

    #region Load Objects From Resource Folder

    private void LoadUnits()
    {
        foreach (GameObject o in Resources.LoadAll("Prefabs/Units"))
        {
            string n = o.name;

            if (n == "Blueberry")
                Instance.bb = o.GetComponent<Blueberry>();
            else if (n == "Pea")
                Instance.pea = o.GetComponent<Pea>();
            else if (o.GetComponent<Troop>() != null)
                Instance.troops.Add(o.GetComponent<Troop>());
        }
    }

    private void LoadStructures()
    {
        foreach (GameObject o in Resources.LoadAll("Prefabs/Structures"))
        {
            if (o.GetComponent<Structure>() != null)
                Instance.structures.Add(o.GetComponent<Structure>());
        }
    }

    private void LoadFarms()
    {
        foreach (GameObject o in Resources.LoadAll("Prefabs/Farms"))
        {
            if (o.GetComponent<Farm>() != null)
                Instance.farms.Add(o.GetComponent<Farm>());
        }
    }

    private void LoadResources()
    {
        foreach (GameObject o in Resources.LoadAll("Prefabs/Resources"))
        {
            if (o.GetComponent<Resource>() != null)
                Instance.resources.Add(o.GetComponent<Resource>());
        }
    }

    private void LoadMaterials()
    {
        foreach (Material m in Resources.LoadAll("Materials"))
            Instance.materials.Add(m);
    }

    private void LoadUnitSprites()
    {
        foreach (Texture2D t in Resources.LoadAll("Sprites/Units"))
            Instance.unitSprites.Add(t);
    }

    private void LoadStructureSprites()
    {
        foreach (Object o in Resources.LoadAll("Sprites/Structures"))
        {
            if (!(o is Texture2D))
                continue;
            Texture2D t = o as Texture2D;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2());
            s.name = t.name;
            Instance.structureSprites.Add(s);
        }
    }

    private void LoadCursors()
    {
        foreach (Texture2D t in Resources.LoadAll("Cursors"))
            Instance.cursors.Add(t);
    }

    #endregion

    #region Get Specific Item

    public static Troop GetTroop(string name)
    {
        foreach (Troop t in Instance.troops)
        {
            if (t.name == name)
                return t;
        }
        return new Troop();
    }

    public static Structure GetStructure(string name)
    {
        foreach (Structure s in Instance.structures)
        {
            if (s.name == name)
                return s;
        }
        foreach (Farm f in Instance.farms)
        {
            if (f.name == name)
                return f;
        }
        return new Structure();
    }

    public static Resource GetResource(string name)
    {
        foreach (Resource r in Instance.resources)
        {
            if (r.name == name)
                return r;
        }
        return Instance.resources[0];
    }

    public static Material GetMaterial(string name)
    {
        foreach (Material m in Instance.materials)
        {
            if (m.name.Replace("Mat", "") == name)
                return m;
        }
        return Instance.materials[0];
    }

    public static Texture2D GetUnitSprite(string name)
    {
        foreach (Texture2D i in Instance.unitSprites)
        {
            if (i.name == name)
                return i;
        }
        return Instance.unitSprites[0];
    }

    public static Sprite GetStructureSprite(string name)
    {
        foreach (Sprite s in Instance.structureSprites)
        {
            //Debug.Log(name + " " + s.name);
            if (name == s.name)
                return s;
        }
        return Instance.structureSprites[0];
    }

    #endregion
}
