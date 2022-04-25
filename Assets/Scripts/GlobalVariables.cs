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

    public Dictionary<string, GameObject> inventory= new Dictionary<string, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        enemyLife = 0;
        currentTimeline = 1;
        justinLife = 3;
        
    }

    private void Update()
    {
        if (justin == null)
        {
            justin = FindObjectOfType<Justin>();
        }

    }


}
