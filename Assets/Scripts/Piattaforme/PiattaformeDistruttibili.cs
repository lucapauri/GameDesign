using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformeDistruttibili : MonoBehaviour
{
    private float radius = 5f;
    private float movSpeed = 2f;
    [SerializeField] private LayerMask _playerMask;
    private PlatRespawn manager;
    private bool shaking;



    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<PlatRespawn>();
        shaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, _playerMask) && !shaking)
        {
            triggerShake();
        }

        if (shaking)
        {
            Vector3 movVec = Vector3.up * movSpeed * Time.deltaTime;
            transform.Translate(movVec);
        }

    }

    private void triggerShake()
    {
        Debug.Log("shaking");
        shaking = true;
        StartCoroutine(shakeCoroutine());
        StartCoroutine(lateDestroyCoroutine());
        manager.respawnPlat(transform.position, transform.rotation, transform.localScale);
    }

    private IEnumerator shakeCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        movSpeed = movSpeed * -1;
        StartCoroutine(shakeCoroutine());
    }


    private IEnumerator lateDestroyCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }


}
