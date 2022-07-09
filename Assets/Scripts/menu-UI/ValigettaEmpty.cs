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

    // Start is called before the first frame update
    void Start()
    {
        justin = GameObject.FindGameObjectWithTag("Player");
        scritte = FindObjectOfType<Scritte>();
        isActive = false;
        isTaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 15)
        {
            scritte.setActive("Premi X per prendere la valigetta", valigetta);
            isActive = true;
            Debug.Log("si");
        }
        if (scritte.active() && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 15)
        {
            scritte.setNotActive();
            isActive = false;
            Debug.Log("no");
        }
        if (isActive && Input.GetKeyDown(KeyCode.X))
            isTaken = true;
    }
}
