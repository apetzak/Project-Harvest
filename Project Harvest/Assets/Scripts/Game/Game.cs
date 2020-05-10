using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private Game() { }

    public static Game Instance { get; } = new Game();

    public static int screenHeight;
    public static int screenWidth;
    public List<Troop> fruits { get; set; }
    public List<Troop> veggies { get; set; }
    public List<Farm> fruitFarms { get; set; }
    public List<Farm> veggieFarms { get; set; }
    public List<Worker> blueberries { get; set; }
    public List<Worker> peas { get; set; }
    public List<Structure> fruitStructures { get; set; }
    public List<Structure> veggieStructures { get; set; }
    /// <summary>
    /// Units currently selected by player. Contains troops and workers
    /// </summary>
    public List<Unit> selectedUnits { get; set; }
    /// <summary>
    /// Structures currently selected by player. Always null when units are selected
    /// </summary>
    public List<Structure> selectedStructures { get; set; }
    /// <summary>
    /// List of neutral Resource entities scattered throughout zone
    /// </summary>
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
    public GameObject gameOverText;
    public bool holdingDown = false;
    public bool mouseOverUI = false;
    public bool selectionChanged = false;
    /// <summary>
    /// At least one worker is selected by player
    /// </summary>
    public bool workerIsSelected = false;
    /// <summary>
    /// At least one troop is selected by player
    /// </summary>
    public bool troopIsSelected = false;
    /// <summary>
    /// The player is controlling the fruit team
    /// </summary>
    public bool fruit = true;

    private void Awake()
    {
        InstantiateGlobalProperties();
        GetObjectsInScene();
        //AddSquad(10, 2);
        //AddWorkers();
        //QualitySettings.vSyncCount = 0; // VSync must be disabled.
        //Application.targetFrameRate = 60;
        selectorBox.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        var hubPosition = Instance.fruit ? Instance.fruitStructures[0].transform.position : Instance.veggieStructures[0].transform.position;
        float y = Camera.main.transform.position.y;
        Camera.main.transform.position = new Vector3(hubPosition.x, y, hubPosition.z - 70);
        Instance.veggieResourceWood = 2000;
        Instance.fruitResourceWood = 2000;
        Instance.veggieResourceGold = 3000;
        Instance.fruitResourceGold = 3000;
        Instance.veggieResourceStone = 3200;
        Instance.fruitResourceStone = 3200;
    }

    private void InstantiateGlobalProperties()
    {
        Instance.fruits = new List<Troop>();
        Instance.veggies = new List<Troop>();
        Instance.fruitFarms = new List<Farm>();
        Instance.veggieFarms = new List<Farm>();
        Instance.blueberries = new List<Worker>();
        Instance.peas = new List<Worker>();
        Instance.fruitStructures = new List<Structure>();
        Instance.veggieStructures = new List<Structure>();
        Instance.selectedUnits = new List<Unit>();
        Instance.resources = new List<Resource>();
        Instance.selectedStructures = new List<Structure>();
    }

    /// <summary>
    /// Adds all entities in scene to Instance collections
    /// </summary>
    private void GetObjectsInScene()
    {
        foreach (Resource r in GameObject.FindObjectsOfType(typeof(Resource)))
            Instance.resources.Add(r);

        foreach (Structure s in GameObject.FindObjectsOfType(typeof(Structure)))
        {
            if (s.GetType() == typeof(Gold) || s.GetType() == typeof(Stone) || s.GetType() == typeof(Tree))
                continue; // skip resources

            s.isPlaced = s.isBuilt = true;

            if (s is Farm)
            {
                if (s.fruit)
                    Instance.fruitFarms.Add(s as Farm);
                else
                    Instance.veggieFarms.Add(s as Farm);
            }
            else
            {
                if (s.fruit)
                    Instance.fruitStructures.Add(s);
                else
                    Instance.veggieStructures.Add(s);
            }
        }

        foreach (Farm f in Instance.fruitFarms)
            f.FindRallyPoint();
        foreach (Farm f in Instance.veggieFarms)
            f.FindRallyPoint();
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
        if (Instance.fruits.Count == 0 && Instance.blueberries.Count == 0 && Instance.fruitStructures.Count == 0 && Instance.fruitFarms.Count == 0)
            ShowText("Veggies");
        else if (Instance.veggies.Count == 0 && Instance.peas.Count == 0 && Instance.veggieStructures.Count == 0 && Instance.veggieFarms.Count == 0)
            ShowText("Fruits");
    }

    private void ShowText(string s)
    {
        Text t = gameOverText.GetComponent<Text>();
        t.text = s + " Win";
        t.enabled = true;
    }

    #region Spawn squads of units (unused) 

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
        Troop u = Instantiate(Assets.Instance.troops[index], pos, Quaternion.identity);
        u.ToggleSelected(false);
    }

    public void AddWorker(int index, int x, int z)
    {
        Vector3 pos = new Vector3(x + 20, 10f, z + 10);
        Worker w = Instantiate(Assets.Instance.bb, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.blueberries.Add(w);

        pos = new Vector3(x + 30, 10f, z + 10);
        w = Instantiate(Assets.Instance.pea, pos, Quaternion.identity);
        w.ToggleSelected(false);
        Instance.peas.Add(w);
    }

    #endregion
}
