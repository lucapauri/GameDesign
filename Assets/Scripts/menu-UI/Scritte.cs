using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scritte : MonoBehaviour
{
    private NewControls controls;
    private GameObject panel;
    public bool isActive;
    private GameObject activeGo;

    void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Grab.performed += ctx => Grab();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //inizializzo
        panel = GameObject.FindGameObjectWithTag("ScrittePanel");
        isActive = false;
        panel.SetActive(false);
    }

    private void Grab()
    {
        if (isActive == true)
        {
            
            isActive = false;
            setNotActive();
            Destroy(activeGo);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnDestroy()
    {
        controls.JustinController.Disable();
    }

    public void setActive(string text, GameObject gameObject)
    {
        isActive = true;
        panel.SetActive(true);
        panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        activeGo = gameObject;
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
