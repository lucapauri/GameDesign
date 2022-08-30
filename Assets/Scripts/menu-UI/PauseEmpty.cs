using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEmpty : MonoBehaviour
{
    private Scritte scritte;
    public string text;
    private GlobalVariables globalVariables;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        scritte = FindObjectOfType<Scritte>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globalVariables.justin != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, globalVariables.justin.transform.position);

            if (globalVariables.justin != null && distance < 10)
            {
                scritte.setActive(text, null);
                StartCoroutine(ScritteEndingCoroutine(1f));
            }
            if (scritte.isActive && distance > 10)
                scritte.setNotActive();
        }
    }

    private IEnumerator ScritteEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        scritte.setNotActive();
        Destroy(gameObject);
    }
}
