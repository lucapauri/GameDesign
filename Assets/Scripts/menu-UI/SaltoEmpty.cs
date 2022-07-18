using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaltoEmpty : MonoBehaviour
{
    private bool done;
    NewControls controls;
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;

    void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Jump.performed += ctx => Eliminate();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        done = false;
        scritte = FindObjectOfType<Scritte>();
        justin = GameObject.FindGameObjectWithTag("Player");
        isActive = false;
    }

    private void Eliminate()
    {
        if (isActive)
        {
            scritte.setNotActive();
            done = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!done && justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10)
        {
            scritte.setActive("Press A to jump", null);
            isActive = true;
        }
        if (isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 10)
        {
            scritte.setNotActive();
            isActive = false;
        }
    }
}
