using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ChangeText : MonoBehaviour
{
    public GameObject camera;
    private bool ready;
    NewControls controls;
    public SliderMenu sliderMenu;
    private List<string> samples = new List<string>();

    private int counter;

    private void Awake()
    {
        
        controls = new NewControls();
        controls.MenuController.ScrollRx.performed += ctx => { NextCounter(); };
        controls.MenuController.ScrollSx.performed += ctx => { PreviousCounter(); };
        controls.MenuController.Back.performed += ctx => { Back(); };
        controls.MenuController.Select.performed += ctx => { SelectLevel(); };
    }

    private void OnEnable()
    {
        controls.MenuController.Enable();
    }

    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
    }

    private void NextCounter()
    {
        if (counter < (samples.Count - 1) && ready)
        {
            counter++;
            sliderMenu.NextButton();
        }
    }

    private void PreviousCounter()
    {
        if (counter > 0 && ready)
        {
            counter--;
            sliderMenu.PreviousButton();
        }
    }

    private void Back()
    {
        if(ready)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    private void SelectLevel()
    {
        if (ready)
        {
            switch (counter){
                case 0:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
                    break;
                case 1:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Giungla");
                    break;
                case 2:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("DesertoCitta");
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        samples.Add("Hub");
        samples.Add("Vietnam");
        samples.Add("Usa");
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentInChildren<TMPro.TextMeshPro>().text = samples[counter];
        if (camera.transform.rotation.eulerAngles.y < 90 && camera.transform.rotation.eulerAngles.y > 45)
        {
            ready = true;
        }
        else
            ready = false;


    }
}
