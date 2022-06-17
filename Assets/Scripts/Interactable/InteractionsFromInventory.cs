using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsFromInventory : MonoBehaviour
{ 

    public Dictionary<string, Vector3> interactionMap = new Dictionary<string, Vector3>();
    GameObject[] interactionPoints;

    private float rightDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        interactionPoints = GameObject.FindGameObjectsWithTag("interactionPoints");

        foreach (GameObject child in interactionPoints)
        {
            interactionMap.Add(child.name, child.transform.position);
        }
        
    }

    public void checkInteractions(GameObject go, Vector3 instPosition)
    {
        switch (go.name)
        {
            case "BabyDino ":
                Vector3 rightPos = interactionMap["BabyDinoPoint"];
                if (Vector3.Distance(rightPos, instPosition) < rightDistance)
                {
                    Debug.Log("checkInteractions");
                    go.GetComponent<DinoEgg>().enabled = true;
                }
                break;
        }

    }
}
