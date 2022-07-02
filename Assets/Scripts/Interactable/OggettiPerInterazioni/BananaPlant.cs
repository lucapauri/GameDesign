using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPlant : MonoBehaviour
{

    GameObject[] go = new GameObject[2];

    GlobalVariables globalVariables;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        go = GameObject.FindGameObjectsWithTag("BananaInteractions");

        foreach (GameObject inst in go)
        {
            if (inst.GetComponent<simpleEnemy>())
            {
                globalVariables.enemies.Remove(inst.GetComponent<simpleEnemy>());
            }
            Destroy(inst);
        }
        
    }

    
}
