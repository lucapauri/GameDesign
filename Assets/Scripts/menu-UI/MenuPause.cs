using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public GameObject menu;
    private GlobalVariables globalVariables;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMenuTrue()
    {
        if (!isActive && globalVariables.justin.enabled == true)
        {
            isActive = true;
            menu.SetActive(true);
            Time.timeScale = 0;
            globalVariables.justin.enabled = false;
            foreach (simpleEnemy enemy in globalVariables.enemies)
            {
                enemy.GetComponent<simpleEnemy>().enabled = false;
            }
        }
    }

    public void SetMenuFalse()
    {
        if (isActive)
        {
            isActive = false;
            menu.SetActive(false);
            Time.timeScale = 1;
            globalVariables.justin.enabled = true;
            foreach (simpleEnemy enemy in globalVariables.enemies)
            {
                enemy.GetComponent<simpleEnemy>().enabled = true;
            }
        }
    }
}
