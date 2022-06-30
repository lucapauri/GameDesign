using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristallo : MonoBehaviour
{
    private GlobalVariables globalVariables;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    private void OnTrigger(Collision other)
    {
        Debug.Log("cristallo");
        if (other.gameObject.layer == 10)
        {
            globalVariables.crystalsNumber++;
            Destroy(gameObject);
        }
    }


}
