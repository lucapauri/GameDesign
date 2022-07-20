using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemEmpty : MonoBehaviour
{
    private Scritte scritte;
    public string text;
    private GlobalVariables globalVariables;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        scritte = FindObjectOfType<Scritte>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globalVariables.justin != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, globalVariables.justin.transform.position);

            if (globalVariables.justin != null && distance < 2)
                scritte.setActive(text, gameObject);
            if (scritte.isActive && distance > 3 && distance < 5)
                scritte.setNotActive();
        }
    }
}
