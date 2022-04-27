using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static bool isMenu = false;
    public GameObject menu;
    public GameObject buttonPrefab;
    private List<GameObject> buttons;

    private void Start()
    {
        menu = GameObject.FindGameObjectWithTag("InventoryMenu");
        buttons = new List<GameObject>();
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
        menu.SetActive(true);
    }

    public void Resume()
    {
        menu.SetActive(false);
    }

    public bool isActive()
    {
        return isMenu;
    }

    public void addButton(string name)
    {
        GameObject go = Instantiate(buttonPrefab);
        go.transform.SetParent(menu.transform, false);
        go.GetComponent<RectTransform>().Translate(new Vector3(0, 100, 0));
        go.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
        buttons.Add(go);
    }
}