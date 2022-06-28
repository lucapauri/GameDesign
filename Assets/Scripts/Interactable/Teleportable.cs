using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    public GameObject creator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // comportamento per le noci di cocco al livello 1
        if (collision.gameObject.layer == 11)
        {
            Destroy(GameObject.FindGameObjectWithTag("Door"));
            if (creator.GetComponent<CoconutPlant>())
            {
                creator.GetComponent<CoconutPlant>().cancel();
            }
        }

        Destroy(this.gameObject);
        
    }
}
