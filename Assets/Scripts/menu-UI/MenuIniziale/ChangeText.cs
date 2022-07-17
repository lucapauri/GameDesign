using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    private List<string> samples = new List<string>();

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        samples.Add("Hub");
        samples.Add("Vietnam");
        samples.Add("Usa");
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentInChildren<TMPro.TextMeshPro>().text = samples[counter];

        if (Input.GetKeyDown(KeyCode.LeftArrow) && counter > 0)
        {
            counter--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && counter < (samples.Count -1))
        {
            counter++;
        }

    }
}
