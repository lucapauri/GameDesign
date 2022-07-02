using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutPlant : MonoBehaviour
{
    private Vector3 fallingPosition = new Vector3(46f, 99f, -23f);

    private GlobalVariables globalVariables;

    public GameObject coconut;

    private float triggerDistance = 15f;
    private bool triggered;
    private bool ableToGenerate;


    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        triggered = false;
        ableToGenerate = true;


        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(globalVariables.justin.transform.position, transform.position) < triggerDistance && !triggered && ableToGenerate)
        {
            InvokeRepeating("generateCoconut", 1f, 4f);
            triggered = true;

        }

        if (triggered && Vector3.Distance(globalVariables.justin.transform.position, transform.position) > triggerDistance + 2f && ableToGenerate)
        {
            triggered = false;
        }
        
    }

    private void generateCoconut()
    {
        Instantiate(coconut, fallingPosition, Quaternion.identity);
        coconut.GetComponent<Teleportable>().creator = this.gameObject;
    }

    public void cancel()
    {
        CancelInvoke("generateCoconut");
        ableToGenerate = false;
    }



}
