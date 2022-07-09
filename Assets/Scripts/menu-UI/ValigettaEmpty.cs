using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValigettaEmpty : MonoBehaviour
{
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    private bool isTaken;
    public GameObject valigetta;
    private GameObject camera1;
    private GameObject camera2;

    // Start is called before the first frame update
    void Start()
    {
        justin = GameObject.FindGameObjectWithTag("Player");
        scritte = FindObjectOfType<Scritte>();
        isActive = false;
        isTaken = false;
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        camera2 = GameObject.FindGameObjectWithTag("SecondCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 15 && !isTaken)
        {
            scritte.setActive("Premi X per prendere la valigetta", valigetta);
            isActive = true;
        }
        if (scritte.active() && (Vector3.Distance(gameObject.transform.position, justin.transform.position) > 15 || isTaken))
        {
            scritte.setNotActive();
            isActive = false;
        }
        if (isActive && Input.GetKeyDown(KeyCode.X))
        {
            isTaken = true;
            camera1.GetComponent<Camera>().rect = new Rect(0,0.5f,1,1);
            camera2.GetComponent<Camera>().rect = new Rect(0,-0.5f,1,1);
        }
    }

    public bool taken()
    {
        return isTaken;
    }
}
