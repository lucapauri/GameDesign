using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static bool isMenu = false;
    public GameObject menu;
    public GameObject buttonPrefab;
    private List<GameObject> buttons;
    private GlobalVariables globalVariables;
    private int activeButton;

    private void Start()
    {
        menu = GameObject.FindGameObjectWithTag("InventoryMenu");
        buttons = new List<GameObject>();
        globalVariables = FindObjectOfType<GlobalVariables>();
        activeButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenu == true)
            SetPause();
        else
            Resume();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(buttons[activeButton].GetComponentInChildren<UnityEngine.UI.Text>().text);
            string oldKey = buttons[activeButton].GetComponentInChildren<UnityEngine.UI.Text>().text;
            string newKey = globalVariables.agingSets[oldKey];
            globalVariables.inventoryAging(oldKey);
            setMenuFalse();
            buttons[activeButton].GetComponentInChildren<UnityEngine.UI.Text>().text = newKey;
        }

        if (isActive() && Input.GetKeyDown(KeyCode.Escape))
            setMenuFalse();

    }

    public void setMenuTrue()
    {
        isMenu = true;
        globalVariables.justin.enabled = false;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = false;
        }
    }

    public void setMenuFalse()
    {
        isMenu = false;
        globalVariables.justin.enabled = true;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = true;
        }
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
        go.GetComponent<RectTransform>().Translate(new Vector3(0, 100, 0));   /**globalVariables.inventory.Keys.Count*/
        go.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
        buttons.Add(go);
    }
}