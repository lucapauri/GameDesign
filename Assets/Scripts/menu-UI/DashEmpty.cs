using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEmpty : MonoBehaviour
{
    private Scritte scritte;
    public string text;
    private GlobalVariables globalVariables;
    private bool isFirst;
    NewControls controls;
    private GameObject justin;
    private bool isActive;

    private void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Dash.performed += ctx =>
        {
            isFirst = false;
        };
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        isFirst = true;
        isActive = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
        scritte = FindObjectOfType<Scritte>();
        justin = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(justin == null)
            justin = GameObject.FindGameObjectWithTag("Player");
        
            float distance = Vector3.Distance(gameObject.transform.position, justin.transform.position);
            if (isFirst && distance < 10)
            {
            isActive = true;
                scritte.setActive(text, null);
            }
            if (isActive && distance > 10)
            {
                scritte.setNotActive();
            }
    }
}
