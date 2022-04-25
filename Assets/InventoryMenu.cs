using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static bool isMenu = false;
    public GameObject Menu;

    private void Start()
    {
        Menu = GameObject.FindGameObjectWithTag("InventoryMenu");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isMenu == true)
            SetPause();
        else
            Resume();
    }

    public void setMenuTrue()
    {
        isMenu = true;
    }

    public void setMenuFalse()
    {
        isMenu = false;
    }

    private void SetPause()
    {
        Menu.SetActive(true);
    }

    public void Resume()
    {
        Menu.SetActive(false);
    }

    public bool isActive()
    {
        return isMenu;
    }
}