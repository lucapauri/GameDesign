using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ChiaveEmpty : MonoBehaviour
{
    //per scritte
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    private bool isTaken;
    //per dialogo
    public NPCConversation conversation;
    public DialogCamera dialogCamera;
    private GlobalVariables globalVariables;
    private bool cameraOn;
    private Vector3 startPos;
    public Camera fourthCamera;

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

    // Update is called once per frame
    void Update()
    {
        if(justin == null)
        {
            justin = GameObject.FindGameObjectWithTag("Player");
        }
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10 && !isTaken)
        {
            scritte.setActive("Premi X per raccogliere la chiave", null);
            isActive = true;
        }
        if (isActive && Vector3.Distance(gameObject.transform.position, justin.transform.position) > 10)
        {
            scritte.setNotActive();
            isActive = false;
        }
        if (isActive && Input.GetKeyDown(KeyCode.X))
        {
            scritte.setNotActive();
            isTaken = true;
            StartCoroutine(conversationWaitCoroutine(2f));
        }
        if (cameraOn && Input.GetKeyDown(KeyCode.Return))
        {
            ConversationManager.Instance.PressSelectedOption();
            cameraOn = false;
            endDialog();
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

        ConversationManager.Instance.EndConversation();
        GameObject canvas = GameObject.FindGameObjectWithTag("Dialog");
        Canvas can = canvas.GetComponent<Canvas>();
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
