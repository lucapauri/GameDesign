using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ValigettaEmpty : MonoBehaviour
{
    private NewControls controls;
    public NPCConversation conversation;
    public DialogCamera dialogCamera;
    private GlobalVariables globalVariables;
    private bool cameraOn;
    private Vector3 startPos;
    public Justin justin;
    private Scritte scritte;
    private bool isActive;
    private bool isTaken;
    public GameObject valigetta;
    private GameObject camera1;
    private GameObject camera2;
    private bool finished;
    public Camera thirdCamera;
    public AudioSource source;
    private bool isFirst;

    void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Grab.performed += ctx => Grab();
        controls.JustinController.SkipDialog.performed += ctx => Skip();
        controls.JustinController.TimeTravel.performed += ctx => Eliminate();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        isFirst = false;
        finished = false;
        scritte = FindObjectOfType<Scritte>();
        isActive = false;
        isTaken = false;
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        camera2 = GameObject.FindGameObjectWithTag("SecondCamera");
        startPos = dialogCamera.transform.position;
        cameraOn = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    private void Grab()
    {
        if (isActive)
        {
            Animator anim = justin.GetComponent<Animator>();
            anim.SetTrigger("Grab");
            StartCoroutine(grabEndingCoroutine(4 / 4));
            justin.valigettaTaken = true;
            StartCoroutine(conversationWaitCoroutine(1.5f));
        }
    }

    private void Skip()
    {
        if (cameraOn)
        {
            ConversationManager.Instance.PressSelectedOption();
            endDialog();
            cameraOn = false;
            finished = true;
        }
    }

    private void Eliminate()
    {
        if (isTaken && finished)
        {
            scritte.setNotActive();
            finished = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 15 && !isTaken)
        {
            isFirst = true;
            scritte.setActive("Press Y to grab the case",null);
            isActive = true;
        }
        if (isFirst && justin != null && scritte.active() && (Vector3.Distance(gameObject.transform.position, justin.transform.position) > 15 || isTaken))
        {
            scritte.setNotActive();
            isActive = false;
        }

        if(isTaken && finished)
        {
            scritte.setActive("Press X to change timeline", null);
        }

    }

    public bool taken()
    {
        return isTaken;
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

        //sdoppio camera
        camera1.GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 1);
        camera2.GetComponent<Camera>().rect = new Rect(0, -0.5f, 1, 1);
        ConversationManager.Instance.EndConversation();
        GameObject canvas = GameObject.FindGameObjectWithTag("Dialog");
        Canvas can = canvas.GetComponent<Canvas>();
        can.worldCamera = thirdCamera;
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

        dialogCamera.moveCamera();

        StartCoroutine(conversationCoroutine(dialogCamera.moveTime));
    }

    private IEnumerator conversationCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        cameraOn = true;
        ConversationManager.Instance.StartConversation(conversation);


    }

    private IEnumerator conversationWaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        dialogSequence();


    }

    private IEnumerator grabEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        source = justin.GetComponent<AudioSource>();
        AudioClip track = Resources.Load("Audio/Justin/Grab") as AudioClip;
        source.clip = track;
        source.pitch = 1;
        source.Play();
        isTaken = true;
        Destroy(valigetta);
    }
}
