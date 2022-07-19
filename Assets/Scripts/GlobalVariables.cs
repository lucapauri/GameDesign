using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public Justin justin;
    public GameObject deathMenuUp;
    public GameObject deathMenuDown;
    public GameObject UiCristalli;
    public GameObject UiVite;

    public int currentTimeline;
    public int crystalsNumber;
    private int totCrystals;

    public float upPlaneHeight;
    public float downPlaneHeight;
    public enum livello
    {
        hub,vietnam,usa
    }

    public livello lv;
    public List<simpleEnemy> enemies = new List<simpleEnemy>();


    public int justinLife;

    public int enemyLife;

    public Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    public Dictionary<string, string> agingSets = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        totCrystals = FindObjectsOfType<Cristallo>().Length;
        crystalsNumber = 0;
        enemyLife = 0;
        currentTimeline = 1;
        justinLife = 1;
        upPlaneHeight = GameObject.FindGameObjectWithTag("PlaneUp").transform.position.y;
        downPlaneHeight = GameObject.FindGameObjectWithTag("PlaneDown").transform.position.y;
        setsStart();


    }

    private void Update()
    {
        string num = crystalsNumber + " / " + totCrystals;
        UiCristalli.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num;

        if(crystalsNumber == totCrystals)
        {
            NextLevel();
        }
        string life = justinLife.ToString();
        UiVite.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = life;


        if (justinLife == 0)
        {
            foreach (simpleEnemy enemy in enemies)
            {
                enemy.enabled = false;
            }
            deathMenuUp.SetActive(true);
            deathMenuDown.SetActive(true);
        }
        
    }

    public void inventoryAging(string key)
    {
        GameObject go = inventory[key];
        string name = agingSets[key];
        inventory.Remove(key);
        inventory.Add(name, go);
    }

    public void setsStart()
    {
        agingSets.Add("SphereInteractable", "OldSphere");
        agingSets.Add("NewSphere", "OldNewSphere");
        agingSets.Add("DinoEgg ", "BabyDino ");
        agingSets.Add("Tdevice", "TDeviceAGED");
        agingSets.Add("key", "rustyKey");

    }

    private void NextLevel() {
        switch (lv)
        {
            case livello.vietnam:
                UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn1");
                break;
            case livello.usa:
                UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn2");
                break;
        }
    }


}
