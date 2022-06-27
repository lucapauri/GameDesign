using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{
    private GlobalVariables globalVariables;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.FindObjectOfType<GlobalVariables>();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            globalVariables.justinLife = 0;
            Debug.Log("die Justin!!");
        }
        else if (collision.gameObject.layer != 6)
        {
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
