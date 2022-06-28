using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsFromInventory : MonoBehaviour
{ 

    public Dictionary<string, float> interactionMap = new Dictionary<string, float>();
    GameObject[] interactionPoints;

    private float rightDistanceDino = 15f;
    private float rightDistanceBanana = 5f;

    // Start is called before the first frame update
    void Start()
    {
        interactionPoints = GameObject.FindGameObjectsWithTag("interactionPoints");

        foreach (GameObject child in interactionPoints)
        {
            interactionMap.Add(child.name, child.transform.position.x);
            Debug.Log(interactionMap.Count);
        }
        
    }

    public void checkInteractions(GameObject go, Vector3 instPosition)
    {
        switch (go.name)
        {
            case "BabyDino ":
                float rightPos1 = interactionMap["BabyDinoPoint"];
                if (Mathf.Abs(rightPos1 - instPosition.x) < rightDistanceDino)
                {
                    Debug.Log("checkInteractions");
                    go.GetComponent<DinoEgg>().enabled = true;
                }
                break;
            case "Banana ":
                float rightPos2 = interactionMap["BananaPoint"];
                if (Mathf.Abs(rightPos2 - instPosition.x) < rightDistanceBanana)
                {
                    Debug.Log("checkInteractions");
                    go.GetComponent<BananaPlant>().enabled = true;
                }
                break;
        }

    }
}
