using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatRespawn : MonoBehaviour
{
    private float respawnTime = 4f;
    public GameObject plat; //da aggiungere un prefab per le piattaforme nel deserto

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void respawnPlat(Vector3 platPos, Quaternion platRot)
    {
        StartCoroutine(respawnPlatCoroutine(platPos, platRot));
    }

    public IEnumerator respawnPlatCoroutine(Vector3 platPos, Quaternion platRot)
    {
        Debug.Log("respawn");
        yield return new WaitForSeconds(respawnTime);
        Instantiate(plat, platPos, platRot);

    }



}
