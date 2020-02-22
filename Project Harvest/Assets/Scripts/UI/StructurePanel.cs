using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructurePanel : MonoBehaviour
{
    public List<Structure> prefabs;
    public List<Structure> fruitPrefabs;
    public List<Structure> veggiePrefabs;
    private List<Button> buttons = new List<Button>();
    private bool placing = false;
    private Structure placingObject;
    public Material matWhite;
    public Material matRed;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var btn = transform.GetChild(i).GetComponent<Button>();
            if (btn == null)
                continue;

            Text t = btn.transform.GetChild(0).GetComponent<Text>();
            t.fontSize = 10;
            t.text = btn.name.Replace("btn", "");
            btn.onClick.AddListener(() => { OnButtonClick(t.text); });
            buttons.Add(btn);
        }
    }

    private void Update()
    {
        if (!placing)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            placing = false;
            Destroy(placingObject.gameObject);
            placingObject = null;
            return;
        }

        if (OverlapsExistingStructure())
        {
            placingObject.ToggleSelectorColor(matRed);
        }
        else
        {
            Material m = placingObject.fruit ? TroopClass.Instance.materials[0] : TroopClass.Instance.materials[1];

            placingObject.ToggleSelectorColor(m);

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
                return;
            }
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit)) // todo: || (hit.collider is MeshCollider))
            return;

        Vector3 v = new Vector3(hit.point.x, 10, hit.point.z);
        placingObject.transform.position = v;
        placingObject.SetBounds();
    }

    private void PlaceObject()
    {
        if (placingObject.fruit)
            Game.Instance.fruitStructures.Add(placingObject);
        else
            Game.Instance.veggieStructures.Add(placingObject);

        TypeSpecificInit();
        placing = false;
        placingObject.isPlaced = true;
        placingObject = null;
    }

    private void TypeSpecificInit()
    {
        if (placingObject is Farm)
            (placingObject as Farm).FindRallyPoint();
        else if (placingObject is RallyPoint)
            (placingObject as RallyPoint).SetPointOnFarms();
        else if (placingObject is WaterTower)
            (placingObject as WaterTower).ActivateSprinklers();
        else if (placingObject is Sprinkler)
            (placingObject as Sprinkler).SetSource();
    }

    private bool OverlapsExistingStructure()
    {
        foreach (Structure s in Game.Instance.fruitStructures)
        {
            if (StructureOverlaps(s))
                return true;
        }
        foreach (Structure s in Game.Instance.veggieStructures)
        {
            if (StructureOverlaps(s))
                return true;
        }
        return false;
    }

    private bool StructureOverlaps(Structure s)
    {
        if (s.maxX < placingObject.minX ||
            s.minX > placingObject.maxX ||
            s.maxY < placingObject.minY ||
            s.minY > placingObject.maxY)
            return false;
        return true;
    }

    private void CreatePlacingObject(Structure prefab)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit))
            return;

        Structure s = Instantiate(prefab, hit.point, Quaternion.identity);
        s.fruit = Game.Instance.fruit;
        s.CreateSelector();
        s.ToggleRing();
        s.ToggleSelector();
        placingObject = s;
    }

    public void OnButtonClick(string s)
    {
        if (s == "War")
        {
            EntityUtils.War();
            return;
        }
        else if (s == "Team") // swap teams
        {
            Game.Instance.fruit = !Game.Instance.fruit;
            return;
        }

        placing = true;

        Structure prefab = prefabs[0];
        int i = 0;

        if (s.Contains("Farm") || s.Contains("Hub"))
        {
            if (s.Contains("Farm"))
                i = System.Convert.ToInt16(s.Replace("Farm", ""));
            prefab = Game.Instance.fruit ? fruitPrefabs[i] : veggiePrefabs[i];               
        }
        else
        {
            s = s.Replace(" ", "");
            foreach (Structure structure in prefabs)
            {
                if (s.Contains(structure.name))
                {
                    prefab = structure;
                    break;
                }
            }
        }

        CreatePlacingObject(prefab);
    }
}
