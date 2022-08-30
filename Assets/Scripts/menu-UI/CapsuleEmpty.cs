using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleEmpty : MonoBehaviour
{
    private NewControls controls;
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    private bool destroyed;

    private void Awake()
    {
        controls = new NewControls();
        controls.JustinController.OpenTimeCapsule.performed += ctx => Eliminate();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        destroyed = false;
        scritte = FindObjectOfType<Scritte>();
        justin = GameObject.FindGameObjectWithTag("Player");
        isActive = false;
    }

    private void Eliminate()
    {
        if (isActive && !destroyed)
        {
            scritte.setNotActive();
            destroyed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (justin == null)
        {
            justin = GameObject.FindGameObjectWithTag("Player");
        }
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 16 && !destroyed)
        {
            scritte.setActive("Press LB to open the time capsule", null);
            isActive = true;
        }
        if (justin != null && isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 16)
        {
            scritte.setNotActive();
            isActive = false;
        }
    }
    private void OnDestroy()
    {
        controls.JustinController.Disable();
    }

}
