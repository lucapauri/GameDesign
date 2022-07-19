using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRustyKey : MonoBehaviour
{
    private Animator animator;
    private GameObject end;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("rotate");
        end = GameObject.FindGameObjectWithTag("EndLevel");
        end.GetComponent<Animator>().SetTrigger("fade");
        StartCoroutine(endLevelCoroutine(2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator endLevelCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Giungla");
    }
}
