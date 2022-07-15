using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public Justin justin;

    public int currentTimeline;
    public int crystalsNumber;

    public float upPlaneHeight;
    public float downPlaneHeight;

    public List<simpleEnemy> enemies = new List<simpleEnemy>();


    public int justinLife;

    public int enemyLife;

    public Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    public Dictionary<string, string> agingSets = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        crystalsNumber = 0;
        enemyLife = 0;
        currentTimeline = 1;
        justinLife = 20;
        upPlaneHeight = GameObject.FindGameObjectWithTag("PlaneUp").transform.position.y;
        downPlaneHeight = GameObject.FindGameObjectWithTag("PlaneDown").transform.position.y;
        setsStart();


    }

    private void Update()
    {
        
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


}
