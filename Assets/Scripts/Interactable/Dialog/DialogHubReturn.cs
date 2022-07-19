using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogHubReturn : MonoBehaviour
{
    NewControls controls;
    public NPCConversation conversation;
    public DialogCamera dialogCamera;

    private GlobalVariables globalVariables;


    private float rightDistance = 7f;
    private bool cameraOn;
    private Vector3 startPos;
    private bool isFirst;

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
        float distance = Mathf.Abs(globalVariables.justin.transform.position.x - transform.position.x);
        bool dialogPos = distance < rightDistance && globalVariables.justin._isGrounded;

        if (dialogPos && isFirst && !cameraOn)
        {
            dialogSequence();
            isFirst = false;
        }
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
        
        ConversationManager.Instance.EndConversation();
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
