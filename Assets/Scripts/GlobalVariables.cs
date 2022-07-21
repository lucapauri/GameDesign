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


    // Start is called before the first frame update
    void Start()
    {
        totCrystals = FindObjectsOfType<Cristallo>().Length;
        crystalsNumber = 0;
        currentTimeline = 1;
        justinLife = 5;
        upPlaneHeight = GameObject.FindGameObjectWithTag("PlaneUp").transform.position.y;
        downPlaneHeight = GameObject.FindGameObjectWithTag("PlaneDown").transform.position.y;
        setsStart();

        soundtrackSource = justin.GetComponentsInChildren<AudioSource>()[1];
        switch (lv)
        {
            case livello.hub:
                AudioClip track1 = Resources.Load("Audio/Soundtrack/Hub") as AudioClip;
                soundtrackSource.clip = track1;
                soundtrackSource.pitch = 1;
                soundtrackSource.loop = true;
                soundtrackSource.Play();
                Debug.Log("HubSoundtrackOn");
                justin.inputMul = 1;
                break;
            case livello.vietnam:
                AudioClip track2 = Resources.Load("Audio/Soundtrack/Giungla") as AudioClip;
                soundtrackSource.clip = track2;
                soundtrackSource.pitch = 1;
                soundtrackSource.loop = true;
                soundtrackSource.Play();
                Debug.Log("VietSoundtrackOn");
                justin.inputMul = -1;
                break;
            case livello.usa:
                AudioClip track3 = Resources.Load("Audio/Soundtrack/Citta") as AudioClip;
                soundtrackSource.clip = track3;
                soundtrackSource.pitch = 1;
                soundtrackSource.loop = true;
                soundtrackSource.Play();
                Debug.Log("UsaSoundtrackOn");
                justin.inputMul = 1;
                break;
        }


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
        Debug.Log(justinLife.ToString());


        if (justinLife == 0)
        {
            AudioClip track = Resources.Load("Audio/Justin/Damage") as AudioClip;
            justin.source.clip = track;
            justin.source.pitch = 2;
            justin.source.Play();
            Debug.Log("audioDamageOn");
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
        Debug.Log("audioDamageOn");
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
                UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn1");
                break;
            case livello.usa:
                UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn2");
                break;
        }
    }


}
