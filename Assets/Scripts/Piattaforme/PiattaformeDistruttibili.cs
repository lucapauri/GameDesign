using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformeDistruttibili : MonoBehaviour
{
    private float radius = 2f;
    private float movSpeed = 2f;
    [SerializeField] private LayerMask _playerMask;
    private PlatRespawn manager;
    private bool shaking;

    public GameObject dustExp;



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

        Vector3 explosionPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        if (dustExp != null)
        {
            GameObject go = Instantiate(dustExp, explosionPoint, Quaternion.Euler(-90f, 0f, 0f));
            go.transform.localScale = new Vector3(0.05f, 0.03f, 0.05f);
        }

    }


}
