using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrittaUsaChiave : MonoBehaviour
{
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    public InventoryMenu inventory;

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
        if(justin == null)
            justin = GameObject.FindGameObjectWithTag("Player");
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 3 
            && inventory.getButtons() > 0)
        {
            scritte.setActive("Press RB to open inventory", null);
            isActive = true;
        }
        if (isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 3)
        {
            scritte.setNotActive();
            isActive = false;
        }
        if (isActive && inventory.getButtons() < 1)
        {
            scritte.setNotActive();
        }
    }
}
