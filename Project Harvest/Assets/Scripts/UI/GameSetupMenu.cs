using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSetupMenu : MonoBehaviour
{
    public Button btnFruit;
    public Button btnVeggie;

    void Start()
    {
        Text t1 = btnFruit.transform.GetChild(0).GetComponent<Text>();
        Text t2 = btnVeggie.transform.GetChild(0).GetComponent<Text>();

        btnFruit.onClick.AddListener(() => { OnButtonClick(t1.text); });
        btnVeggie.onClick.AddListener(() => { OnButtonClick(t2.text); });
    }

    void Update()
    {
        
    }

    public void OnButtonClick(string s)
    {
        Game.Instance.fruit = s == "Fruits";
        SceneManager.LoadScene("MainScene");
    }
}
