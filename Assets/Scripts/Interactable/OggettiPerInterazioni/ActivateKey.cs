using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ActivateKey : MonoBehaviour
{
    private Camera dialogcamera;
    private NPCConversation conversation;
    private DialogCamera dialogCamera;
    private GlobalVariables globalVariables;
    private bool cameraOn;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("DialogCamera");
        GameObject conv = GameObject.FindGameObjectWithTag("Conversation");
        dialogcamera = camera.GetComponent<Camera>();
        dialogCamera = camera.GetComponent<DialogCamera>();
        conversation = conv.GetComponent<KeyDialogue>().getConversation();
        startPos = dialogCamera.transform.position;
        cameraOn = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
        dialogSequence();
    }

    // Update is called once per frame
    void Update()
    {
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

        Camera c = dialogCamera.GetComponent<Camera>();
        c.rect = new Rect(0, 0, 1, 1);
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
