using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public Justin justin;

    public int currentTimeline;

    public List<simpleEnemy> enemies = new List<simpleEnemy>();


    public int justinLife;

    public int enemyLife;

    public Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    public Dictionary<string, string> agingSets = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        enemyLife = 0;
        currentTimeline = 1;
        justinLife = 3;
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

    }


}
