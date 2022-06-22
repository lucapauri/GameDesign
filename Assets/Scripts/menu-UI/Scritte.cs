using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scritte : MonoBehaviour
{
    private GameObject panel;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        //inizializzo
        panel = GameObject.FindGameObjectWithTag("ScrittePanel");
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive == true && Input.GetKeyDown(KeyCode.X))
            isActive = false;
        if (isActive == false)
            setNotActive();
    }

    public void setActive(string text)
    {
        isActive = true;
        panel.SetActive(true);
        panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }

    public void setNotActive()
    {
        panel.SetActive(false);
    }

    public bool active()
    {
        return isActive;
    }
}
