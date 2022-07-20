using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    NewControls controls;
    public static bool isMenu = false;
    public GameObject menu;
    public GameObject buttonPrefab;
    public InteractionsFromInventory interactions;
    private List<GameObject> buttons = new List<GameObject>();
    private GlobalVariables globalVariables;
    private int activeButton;
    public bool openTimeCapsule;
    public AudioSource source;

    private void Awake()
    {
        controls = new NewControls();
        controls.JustinController.ScrollInvRx.performed += ctx => ScrollRx();
        controls.JustinController.ScrollInvSx.performed += ctx => ScrollSx();
        controls.JustinController.EscInv.performed += ctx => Return();
        controls.JustinController.SelectInv.performed += ctx => SelectItem();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    private void Start()
    {
        interactions = FindObjectOfType<InteractionsFromInventory>();
        openTimeCapsule = false;
        menu = GameObject.FindGameObjectWithTag("InventoryMenu");
        buttons = new List<GameObject>();
        globalVariables = FindObjectOfType<GlobalVariables>();
        activeButton = 0;
    }

    private void ScrollRx()
    {
        if (activeButton < buttons.Count - 1)
        {
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", false);
            activeButton++;
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);
            source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
            AudioClip track = Resources.Load("Audio/Oggetti/UI") as AudioClip;
            source.clip = track;
            source.Play();
        }
    }

    private void ScrollSx()
    {
        if (activeButton > 0)
        {
            Debug.Log(buttons.Count);
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", false);
            activeButton--;
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);
            source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
            AudioClip track = Resources.Load("Audio/Oggetti/UI") as AudioClip;
            source.clip = track;
            source.Play();
        }
    }

    private void Return()
    {
        if (isActive())
        {
            setMenuFalse();
            openTimeCapsule = false;
            AudioClip track = Resources.Load("Audio/Oggetti/UI") as AudioClip;
            source.clip = track;
            source.Play();
        }
    }

    private void SelectItem()
    {
        if (menu.activeInHierarchy)
        {
            //controllo se il menù è stato aperto per interagire con la capsula del tempo o per posizionare un oggetto
            if (openTimeCapsule)
            {
                string oldKey = buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
                string newKey = globalVariables.agingSets[oldKey];
                globalVariables.inventoryAging(oldKey);
                setMenuFalse();
                buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = newKey;
                AudioClip track = Resources.Load("Audio/Oggetti/Capsula") as AudioClip;
                source.clip = track;
                source.Play();
            }
            else
            {
                string buttonText = buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
                Vector3 instPos = new Vector3();
                Quaternion instRot = new Quaternion();
                if (buttonText.Equals("key") || buttonText.Equals("rustyKey"))
                {
                    instPos = globalVariables.justin.transform.position + globalVariables.justin.transform.forward * 30f +
                        globalVariables.justin.transform.up * 5f;
                    instRot = Quaternion.LookRotation(-Vector3.up, Vector3.right);
                }
                else
                {
                    instPos = globalVariables.justin.transform.position + globalVariables.justin.transform.forward * 3f;
                    instRot = Quaternion.LookRotation(-Vector3.up, Vector3.right);
                }
                string name = "Meshes/" + buttonText;
                GameObject goInstance = Instantiate(Resources.Load(name) as GameObject, instPos, Quaternion.identity);
                goInstance.name = buttonText;
                Destroy(buttons[activeButton]);
                buttons.Remove(buttons[activeButton]);
                interactions.checkInteractions(goInstance, instPos);
                globalVariables.inventory.Remove(buttonText);
                setMenuFalse();
                List<string> names = new List<string>();
                buttons.ForEach(b => {
                    names.Add(b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
                    Destroy(b);
                });
                buttons = new List<GameObject>();
                names.ForEach(n => addButton(n));
                AudioClip track = Resources.Load("Audio/Oggetti/Instantiate") as AudioClip;
                source.clip = track;
                source.Play();
            }
            openTimeCapsule = false;
        }
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
        if (buttons.Count > 0) { 
        isMenu = true;
        globalVariables.justin.enabled = false;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = false;
        }
            source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
            if (openTimeCapsule)
            {
                AudioClip track = Resources.Load("Audio/Oggetti/Capsula") as AudioClip;
                source.clip = track;
                source.Play();
            }
            else
            {
                AudioClip track = Resources.Load("Audio/Oggetti/UI") as AudioClip;
                source.clip = track;
                source.Play();
            }
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
        buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);
    }

    public void Resume()
    {
        menu.SetActive(false);
    }

    public bool isActive()
    {
        return isMenu;
    }

    public void setBehaviour()
    {
        openTimeCapsule = true;
}

    public void addButton(string name)
    {
        GameObject go = Instantiate(buttonPrefab);
        buttons.Add(go);
        go.transform.SetParent(menu.transform, false);
        Vector3 buttonPos = new Vector3();
        if (buttons.Count < 4)
        {
            buttonPos = new Vector3(menu.transform.position.x - 960, menu.transform.position.y + 200, menu.transform.position.z);
            buttonPos.x = buttonPos.x + buttons.Count * 1920 / 4;
        }else if(buttons.Count < 7)
        {
            buttonPos = new Vector3(menu.transform.position.x - 960, menu.transform.position.y - 200, menu.transform.position.z);
            buttonPos.x = buttonPos.x + (buttons.Count - 3) * 1920 / 4;
        }
        else { }
        go.GetComponent<RectTransform>().position = buttonPos;   
        go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;
    }

    public int getButtons()
    {
        return buttons.Count;
    }

}