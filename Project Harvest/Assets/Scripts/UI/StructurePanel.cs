using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructurePanel : UIElement
{
    public List<Structure> prefabs;
    public List<Structure> fruitPrefabs;
    public List<Structure> veggiePrefabs;
    private List<Button> buttons = new List<Button>();
    private bool placing = false;
    private Structure placingObject;
    public Material matWhite;
    public Material matRed;
    private Vector3 startPoint = new Vector3();
    private Vector3 endPoint = new Vector3();
    private bool facingDirectionChanged = true;
    private string lastClicked;

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
            Destroy(placingObject.gameObject);
            Reset();
            return;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit)) // todo: || (hit.collider is MeshCollider))
            return;

        Vector3 v = new Vector3(hit.point.x, 10, hit.point.z);

        if (startPoint != new Vector3())
        {
            var diff = (startPoint - v);
            float f = Mathf.Atan2(diff.x, diff.z) * (180.0f / Mathf.PI);

            Debug.Log(placingObject.transform.rotation);

            if (f < 45 && f >= -45)
            {
                placingObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                //Debug.Log("down " + f);
            }
            else if (Mathf.Abs(f) > 135)
            {
                placingObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                //Debug.Log("up " + f);
            }
            else if (f < 135 && f >= 45)
            {
                placingObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                //Debug.Log("left " + f);
            }
            else
            {
                placingObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                //Debug.Log("right " + f);
            }

            //placingObject.transform.Rotate(0, f, 0);
            // n 180, s 0, e -90, w 90
            //Debug.Log(diff.magnitude + " " + f + " " + placingObject.transform.rotation.y);
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
                if (placingObject is Wall || placingObject is Bridge)
                {
                    startPoint = placingObject.transform.position;
                }
                else
                {
                    PlaceObject();
                    return;
                }
            }
        }

        placingObject.transform.position = v;
        placingObject.SetBounds();
    }

    private void PlaceObject()
    {
        if (placingObject.fruit)
            Game.Instance.fruitStructures.Add(placingObject);
        else
            Game.Instance.veggieStructures.Add(placingObject);

        placingObject.isPlaced = true;
        placingObject.health = 1;

        if (!(placingObject is Farm))
        {
            placingObject.transform.GetChild(0).transform.Translate(0, -placingObject.height, 0, Space.World);
            placingObject.SendWorkersToBuild();
        }
        else
        {
            (placingObject as Farm).FindRallyPoint();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            CreatePlacingObject(GetPrefab(lastClicked));
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        placing = false;
        placingObject = null;
        startPoint = endPoint = new Vector3();
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
            s.maxZ < placingObject.minZ ||
            s.minZ > placingObject.maxZ)
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

        lastClicked = s;
        placing = true;
        CreatePlacingObject(GetPrefab(s));
    }

    private Structure GetPrefab(string s)
    {
        Structure prefab = prefabs[0];

        if (s.Contains("Farm") || s.Contains("Hub"))
        {
            int i = 0;
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

        return prefab;
    }
}
