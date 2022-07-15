using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleEmpty : MonoBehaviour
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
        if (justin == null)
        {
            justin = GameObject.FindGameObjectWithTag("Player");
        }
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 16)
        {
            scritte.setActive("Premi X per usare la capsula del tempo", null);
            isActive = true;
        }
        if (isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 16)
        {
            scritte.setNotActive();
            isActive = false;
        }
        if (isActive && Input.GetKeyDown(KeyCode.X))
        {
            scritte.setNotActive();
            Destroy(gameObject);
        }
    }
}
