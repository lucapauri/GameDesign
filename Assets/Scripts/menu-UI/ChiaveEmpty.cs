using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ChiaveEmpty : MonoBehaviour
{
    NewControls controls;
    //per scritte
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    private bool isTaken;
    //per dialogo
    public Camera dialogcamera;
    public NPCConversation conversation;
    public DialogCamera dialogCamera;
    private GlobalVariables globalVariables;
    private bool cameraOn;
    private Vector3 startPos;
    public Camera fourthCamera;

    private void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Grab.performed += ctx => Grab();
        controls.JustinController.SkipDialog.performed += ctx => Skip();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //per dialogo
        startPos = dialogCamera.transform.position;
        cameraOn = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
        //per scritte
        scritte = FindObjectOfType<Scritte>();
        justin = GameObject.FindGameObjectWithTag("Player");
        isActive = false;
        isTaken = false;
    }

    private void Grab()
    {
        if (isActive)
        {
            scritte.setNotActive();
            isTaken = true;
            StartCoroutine(conversationWaitCoroutine(1.5f));
        }
    }

    private void Skip()
    {
        if (cameraOn)
        {
            ConversationManager.Instance.PressSelectedOption();
            cameraOn = false;
            endDialog();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(justin == null)
        {
            justin = GameObject.FindGameObjectWithTag("Player");
        }
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10 && !isTaken)
        {
            scritte.setActive("Press Y to grab the key", null);
            isActive = true;
        }
        if (justin != null && isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 10)
        {
            scritte.setNotActive();
            isActive = false;
        }
    }

    private void OnDestroy()
    {
        controls.JustinController.Disable();
    }

    private void endDialog()
    {
        globalVariables.justin.GetComponent<Justin>().enabled = true;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = true;
        }

        FindObjectOfType<UpCamera>().gameObject.GetComponent<Camera>().enabled = true;
        FindObjectOfType<DownCamera>().gameObject.GetComponent<Camera>().enabled = true;
        dialogCamera.GetComponent<Camera>().enabled = false;

        dialogCamera.transform.position = startPos;

        ConversationManager.Instance.EndConversation();
        GameObject canvas = GameObject.FindGameObjectWithTag("Dialog");
        Canvas can = canvas.GetComponent<Canvas>();
        can.worldCamera = fourthCamera;
    }

    private void dialogSequence()
    {
        globalVariables.justin.GetComponent<Justin>().enabled = false;
        globalVariables.justin.GetComponent<Animator>().SetBool("Walk", false);
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = false;
        }

        dialogCamera.GetComponent<Camera>().enabled = true;
        FindObjectOfType<UpCamera>().gameObject.GetComponent<Camera>().enabled = false;
        FindObjectOfType<DownCamera>().gameObject.GetComponent<Camera>().enabled = false;

        Camera c = dialogCamera.GetComponent<Camera>();
        c.rect = new Rect(0,0,1,1);
        dialogCamera.moveCamera();
        
        StartCoroutine(conversationCoroutine(dialogCamera.moveTime));
    }

    private IEnumerator conversationWaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        dialogSequence();


    }

    private IEnumerator conversationCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        cameraOn = true;
        ConversationManager.Instance.StartConversation(conversation);


    }
}
