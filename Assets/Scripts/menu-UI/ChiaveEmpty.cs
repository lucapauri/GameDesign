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
    //per dialogo
    public Animator justinAnim;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(justin == null)
        {
            justin = GameObject.FindGameObjectWithTag("Player");
        }
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10)
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
            cameraOn = true;
        }
        if (cameraOn && Input.GetKeyDown(KeyCode.Return))
        {
            ConversationManager.Instance.PressSelectedOption();
            cameraOn = false;
        }
    }

}
