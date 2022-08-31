using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.InputSystem;

public class DialoPointHub : MonoBehaviour
{
    NewControls controls;
    public NPCConversation conversation;
    public DialogCamera dialogCamera;

    private GlobalVariables globalVariables;


    private float rightDistance = 7f;
    private bool cameraOn;
    private Vector3 startPos;
    private bool isFirst;
    public Camera secondCamera;
    public GameObject scritta;

    void Awake()
    {
        controls = new NewControls();
        controls.JustinController.SkipDialog.performed += ctx => Skip();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    void Skip()
    {
        if (cameraOn)
        {
            ConversationManager.Instance.PressSelectedOption();
            endDialog();
            cameraOn = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isFirst = true;
        startPos = dialogCamera.transform.position;
        cameraOn = false;
        globalVariables = FindObjectOfType<GlobalVariables>();

    }

    // Update is called once per frame
    void Update()
    {
        if (globalVariables.justin != null)
        {
            float distance = Mathf.Abs(globalVariables.justin.transform.position.x - transform.position.x);
            bool dialogPos = distance < rightDistance && globalVariables.justin._isGrounded;

            if (dialogPos && isFirst && !cameraOn)
            {
                dialogSequence();
                isFirst = false;
            }
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
        GameObject canvas = GameObject.FindGameObjectWithTag("Dialog");
        Canvas can = canvas.GetComponent<Canvas>();
        can.worldCamera = secondCamera;
        ConversationManager.Instance.EndConversation();
        if(scritta != null)
            scritta.SetActive(true);
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
}
