using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Game() { }

    public static Game Instance { get; } = new Game();

    public static int screenHeight;
    public static int screenWidth;
    public List<Troop> unitPrefabs;
    public List<Troop> troops { get; set; }
    public Pea peaPrefab;
    public Blueberry bbPrefab;
    public List<Worker> workers { get; set; }
    public Unit selectedUnit;
    public List<Farm> farms { get; set; }
    public GameObject selectorBox;
    public bool holdingDown = false;
    public Vector3 boxPoint;
    public bool selectionChanged = false;
    public int selectionCount = 0;

    void Start()
    {
        //Application.targetFrameRate = 30;
        Instance.troops = new List<Troop>();
        Instance.farms = new List<Farm>();
        Instance.workers = new List<Worker>();

        AddSquad(20, 3);
        AddWorkers();

        var rt = selectorBox.GetComponent<RectTransform>();
        rt.transform.localScale = new Vector3(0, 0, 0);
    }

    void AddWorkers()
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

    void AddSquad(int rows, int columns)
    {
        for (int z = 0; z < 10; z++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int q = 0; q < columns; q++)
                {
                    int j = i * 10;

                    AddUnit(z, i * 10, q * 10 + (12 * z * columns));
                }
            }
        }
    }

    public void AddUnit(int index, int x, int z)
    {
        Vector3 pos = new Vector3(x + 20, 10f, z + 10);
        Troop u = Instantiate(unitPrefabs[index], pos, Quaternion.identity);
        u.ToggleSelected(false);
        u.attacking = true;
        Instance.troops.Add(u);
    }

    public void AddWorker(int index, int x, int z)
    {
        Vector3 pos = new Vector3(x + 20, 10f, z + 10);
        Worker w = Instantiate(peaPrefab, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.workers.Add(w);

        pos = new Vector3(x + 30, 10f, z + 10);
        w = Instantiate(bbPrefab, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.workers.Add(w);
    }

    public void ChangeSelection(int selectedCount)
    {
        if (selectedCount != 1)
            Instance.selectedUnit = null;
        Instance.selectionCount = selectedCount;
        //Debug.Log("changeSelection");
        Instance.selectionChanged = true;
    }

    void Update()
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
