using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoEmpty : MonoBehaviour
{
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        scritte = FindObjectOfType<Scritte>();
        justin = GameObject.FindGameObjectWithTag("Player");
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10)
        {
            scritte.setActive("Premi la barra per saltare", null);
            isActive = true;
        }
        if (isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 10)
        {
            scritte.setNotActive();
            isActive = false;
        }
        if(isActive && Input.GetKeyDown(KeyCode.Space))
        {
            scritte.setNotActive();
            Destroy(gameObject);
        }
    }
}
