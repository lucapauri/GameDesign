using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public Justin justin;
    public GameObject deathMenuUp;
    public GameObject deathMenuDown;
    public GameObject UiCristalli;
    public GameObject UiVite;
    public AudioSource soundtrackSource;
    public float gravity;
    public float jumpHeight;

    public int currentTimeline;
    public int crystalsNumber;
    private int totCrystals;

    public float upPlaneHeight;
    public float downPlaneHeight;
    public enum livello
    {
        hub,vietnam,usa
    }

    public livello lv;
    public List<simpleEnemy> enemies = new List<simpleEnemy>();


    public int justinLife;

    public Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    public Dictionary<string, string> agingSets = new Dictionary<string, string>();

    private void Awake()
    {
        switch (lv)
        {
            case livello.hub:
                gravity = -40f;
                jumpHeight = 25f;
                break;
            case livello.usa:
                gravity = -9.81f;
                jumpHeight = 5f;
                break;
            case livello.vietnam:
                gravity = -9.81f;
                jumpHeight = 5f;
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totCrystals = FindObjectsOfType<Cristallo>().Length;
        crystalsNumber = 0;
        currentTimeline = 1;
        justinLife = 7;
        upPlaneHeight = GameObject.FindGameObjectWithTag("PlaneUp").transform.position.y;
        downPlaneHeight = GameObject.FindGameObjectWithTag("PlaneDown").transform.position.y;
        setsStart();

    }

    private void Update()
    {
        string num = crystalsNumber + " / " + totCrystals;
        UiCristalli.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num;

        if(crystalsNumber == totCrystals)
        {
            NextLevel();
        }

        UiVite.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = " " + justinLife;


        if (justinLife == 0)
        {
            AudioClip track = Resources.Load("Audio/Justin/Damage") as AudioClip;
            justin.source.clip = track;
            justin.source.pitch = 2;
            justin.source.Play();
            foreach (simpleEnemy enemy in enemies)
            {
                enemy.enabled = false;
            }
            deathMenuUp.SetActive(true);
            deathMenuDown.SetActive(true);
        }
        
    }



    public void justinDamage()
    {
        justinLife--;
        AudioClip track = Resources.Load("Audio/Justin/Damage") as AudioClip;
        justin.source.clip = track;
        justin.source.pitch = 2;
        justin.source.Play();
    }

    public void inventoryAging(string key)
    {
        GameObject go = inventory[key];
        string name = agingSets[key];
        inventory.Remove(key);
        inventory.Add(name, go);
    }

    public void setsStart()
    {
        agingSets.Add("SphereInteractable", "OldSphere");
        agingSets.Add("NewSphere", "OldNewSphere");
        agingSets.Add("DinoEgg ", "BabyDino ");
        agingSets.Add("Tdevice", "TDeviceAGED");
        agingSets.Add("key", "rustyKey");

    }

    private void NextLevel() {
        switch (lv)
        {
            case livello.vietnam:
                UnityEngine.SceneManagement.SceneManager.LoadScene("TransitonScene1");
                break;
            case livello.usa:
                UnityEngine.SceneManagement.SceneManager.LoadScene("TransitionScene2");
                break;
        }
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    public void ReLoad()
    {
        switch (lv)
        {
            case livello.hub:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
                break;
            case livello.vietnam:
                UnityEngine.SceneManagement.SceneManager.LoadScene("giungla");
                break;
            case livello.usa:
                UnityEngine.SceneManagement.SceneManager.LoadScene("DesertoCitta");
                break;
        }
        Time.timeScale = 1;
    }
}
