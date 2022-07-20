using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMenu : MonoBehaviour
{
    public Animator textAnim;
    public int TransitionNum;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(transitionCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        textAnim.SetTrigger("TextScale");
    }

    private IEnumerator transitionCoroutine()
    {
        yield return new WaitForSeconds(4f);
        if (TransitionNum == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn1");
        }
 
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HubReturn2");
        }
    }
}
