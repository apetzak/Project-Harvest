using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Game() { }

    public static Game Instance { get; } = new Game();

    public static int screenHeight;
    public static int screenWidth;
    public Pea peaPrefab;
    public Blueberry bbPrefab;
    public List<Troop> unitPrefabs;
    public List<Troop> fruits { get; set; }
    public List<Troop> veggies { get; set; }
    public List<Worker> blueberries { get; set; }
    public List<Worker> peas { get; set; }
    public List<Structure> fruitStructures { get; set; }
    public List<Structure> veggieStructures { get; set; }
    public List<Unit> selectedUnits { get; set; }
    public List<Structure> selectedStructures { get; set; }
    public List<Resource> resources { get; set; }
    public int fruitResourceWater;
    public int fruitResourceWood;
    public int fruitResourceStone;
    public int fruitResourceGold;
    public int fruitResourceFertilizer;
    public int veggieResourceWater;
    public int veggieResourceWood;
    public int veggieResourceStone;
    public int veggieResourceGold;
    public int veggieResourceFertilizer;
    public Entity selectedEntity;
    public GameObject selectorBox;
    public bool holdingDown = false;
    public Vector3 boxPoint;
    public bool selectionChanged = false;
    public bool workerIsSelected = false;
    public bool troopIsSelected = false;
    public bool fruit = true;

    private void Start()
    {
        InstantiateGlobalProperties();
        GetObjectsInScene();
        //AddSquad(10, 2);
        //AddWorkers();
        selectorBox.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    private void InstantiateGlobalProperties()
    {
        Instance.peaPrefab = peaPrefab;
        Instance.bbPrefab = bbPrefab;
        Instance.unitPrefabs = unitPrefabs;
        Instance.fruits = new List<Troop>();
        Instance.veggies = new List<Troop>();
        Instance.blueberries = new List<Worker>();
        Instance.peas = new List<Worker>();
        Instance.fruitStructures = new List<Structure>();
        Instance.veggieStructures = new List<Structure>();
        Instance.selectedUnits = new List<Unit>();
        Instance.resources = new List<Resource>();
        Instance.selectedStructures = new List<Structure>();
    }

    private void GetObjectsInScene()
    {
        foreach (Resource r in GameObject.FindObjectsOfType(typeof(Resource)))
            Instance.resources.Add(r);

        foreach (Structure s in GameObject.FindObjectsOfType(typeof(Structure)))
        {
            s.isPlaced = true;
            if (s.fruit)
                Instance.fruitStructures.Add(s);
            else
                Instance.veggieStructures.Add(s);
        }

        #region Set Rally Points
        foreach (Structure s in Instance.fruitStructures)
            if (s is Farm)
                (s as Farm).FindRallyPoint();
        foreach (Structure s in Instance.veggieStructures)
            if (s is Farm)
                (s as Farm).FindRallyPoint();
        #endregion
    }

    private void AddWorkers()
    {
        for (int i = 0; i < 10; i++)
        {
            int j = i * 10;

            AddWorker(0, j + 200, 0);
            AddWorker(0, j + 200, 10);
            AddWorker(1, j + 200, 20);
            AddWorker(1, j + 200, 30);
        }
    }

    private void AddSquad(int rows, int columns)
    {
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int q = 0; q < columns; q++)
                    AddUnit(z, i * 10, q * 10 + (12 * z * columns));
            }
        }
    }

    public void AddUnit(int index, int x, int z)
    {
        Vector3 pos = new Vector3(x + 20, 10f, z + 10);
        Troop u = Instantiate(unitPrefabs[index], pos, Quaternion.identity);
        u.ToggleSelected(false);
        //u.attacking = true;
    }

    public void AddWorker(int index, int x, int z)
    {
        Vector3 pos = new Vector3(x + 20, 10f, z + 10);
        Worker w = Instantiate(peaPrefab, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.blueberries.Add(w);

        pos = new Vector3(x + 30, 10f, z + 10);
        w = Instantiate(bbPrefab, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.peas.Add(w);
    }

    /// <summary>
    /// Set/clear selectedEntity, toggle selectionChanged flag, set workerIsSelected and troopIsSelected
    /// </summary>
    public void ChangeSelection()
    {
        if (selectedUnits.Count == 1)
            selectedEntity = Instance.selectedUnits[0];
        else if (selectedStructures.Count == 1)
            selectedEntity = Instance.selectedStructures[0];
        else
            selectedEntity = null;

        selectionChanged = true;
        workerIsSelected = troopIsSelected = false;

        if (selectedUnits.Count > 0)
        {
            foreach (Unit u in selectedUnits)
            {
                if (u is Worker)
                {
                    workerIsSelected = true;
                    break;
                }
            }

            foreach (Unit u in selectedUnits)
            {
                if (u is Troop)
                {
                    troopIsSelected = true;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit = new RaycastHit();

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.collider.gameObject.name);
        //    }
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{

        //}

        //int count = Instance.units.Count;

        //for (int i = 0; i < count; i++)
        //{
        //    for (int j = i + 1; j < count; j++)
        //    {
        //        float xDiff = Instance.units[i].transform.position.x - Instance.units[j].transform.position.x;
        //        float zDiff = Instance.units[i].transform.position.z - Instance.units[j].transform.position.z;
        //        float xAbs = Mathf.Abs(xDiff);
        //        float zAbs = Mathf.Abs(zDiff);


        //        if (xAbs < 1 && zAbs < 1)
        //        {
        //            Debug.Log(xDiff + " " + zDiff);

        //            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff))
        //            {
        //                if (xDiff < 0)
        //                    Instance.units[i].transform.Translate(2, 0, 0, Space.World);
        //                else
        //                    Instance.units[i].transform.Translate(-2, 0, 0, Space.World);

        //            }
        //            else
        //            {
        //                if (zDiff < 0)
        //                    Instance.units[i].transform.Translate(0, 0, 2, Space.World);
        //                else
        //                    Instance.units[i].transform.Translate(0, 0, -2, Space.World);
        //            }

        //        }
        //    }
        //}

        //for (int i = 0; i < Instance.troops.Count; i++)
        //{
        //    for (int j = i + 1; j < Instance.troops.Count; j++)
        //    {
        //        var c1 = Instance.troops[i].GetComponent<Collider>();
        //        var c2 = Instance.troops[j].GetComponent<Collider>();

        //        if (c1.bounds.Intersects(c2.bounds))
        //        {

        //            //Debug.Log($"collision {i} {j}");
        //        }
        //    }
        //}
    }
}
