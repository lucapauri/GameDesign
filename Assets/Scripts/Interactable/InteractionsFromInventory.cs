using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsFromInventory : MonoBehaviour
{ 

    public Dictionary<string, float> interactionMap = new Dictionary<string, float>();
    GameObject[] interactionPoints;

    private float rightDistance = 15f;

    // Start is called before the first frame update
    void Start()
    {
        interactionPoints = GameObject.FindGameObjectsWithTag("interactionPoints");

        foreach (GameObject child in interactionPoints)
        {
            interactionMap.Add(child.name, child.transform.position.x);
        }
        
    }

    public void checkInteractions(GameObject go, Vector3 instPosition)
    {
        switch (go.name)
        {
            case "BabyDino ":
                float rightPos = interactionMap["BabyDinoPoint"];
                if (Mathf.Abs(rightPos - instPosition.x) < rightDistance)
                {
                    Debug.Log("checkInteractions");
                    go.GetComponent<DinoEgg>().enabled = true;
                }
                break;
        }

    }
}
