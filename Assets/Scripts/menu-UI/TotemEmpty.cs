using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemEmpty : MonoBehaviour
{
    private GameObject justin;
    private Scritte scritte;

    // Start is called before the first frame update
    void Start()
    {
        justin = GameObject.FindGameObjectWithTag("Player");
        scritte = FindObjectOfType<Scritte>();
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 1)
            scritte.setActive("Premi X per piantare il baobab");
        if (scritte.active() && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 1)
            scritte.setNotActive();
    }
}
