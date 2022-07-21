using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JustinManagerMenu : MonoBehaviour
{

    public Transform wayroot;
    private Sequence walkingSequence;

    public Animator anim;

    public GameObject valigettaPrefab;
    public GameObject fulminePrefab;
    public GameObject pistolaPrefab;
    public GameObject logoPanel;
    public Animator camera;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 startScale;
    private AnimationClip idle;


    private float movingTime = 3f; 
    private AnimationClip[] clips;
    private float throwTime;
    private float fireTime;
    private bool readyToAnim;



    // Start is called before the first frame update
    void Start()
    {

        startScale = transform.localScale;
        startPos = transform.position;
        startRot = transform.rotation;
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        logoPanel = GameObject.FindGameObjectWithTag("ScritteCanvas");
        wayroot = GameObject.FindGameObjectWithTag("Respawn").transform;

        clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {

                case "Armature|throw":
                    throwTime = clip.length;
                    break;

                case "Armature|sparo":
                    fireTime = clip.length;
                    break;
                case "Armature|Idle":
                    idle = clip;
                    break;

                default:
                    break;
            }
        }
        searchPath();
        readyToAnim = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void searchPath()
    {

        if (wayroot != null && wayroot.childCount > 0)
        {
            anim.SetBool("Walk", true);
            readyToAnim = false;
            Vector3[] pathPositions = new Vector3[wayroot.childCount];
            for (int i = 0; i < wayroot.childCount; i++)
            {
                pathPositions[i] = wayroot.GetChild(i).position;
                pathPositions[i].y = transform.position.y;
            }
            walkingSequence = DOTween.Sequence();

            walkingSequence.Append(transform.DOPath(pathPositions, movingTime, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.Linear).SetLookAt(0.01f)
                .SetId("walking").OnComplete(
                () =>
                {
                    anim.SetBool("Walk", false);
                    logoPanel.GetComponent<Animator>().SetTrigger("ScaleUp");
                    readyToAnim = true;
                }

                ));

        }
    }

    public void throwInst()
    {
        if (readyToAnim)
        {
            GameObject valigetta = Instantiate(valigettaPrefab, transform.position, transform.rotation);
            valigetta.transform.localScale = transform.localScale;

            GameObject fulmine = Instantiate(fulminePrefab, transform.position, transform.rotation);
            fulmine.transform.localScale = transform.localScale;

            valigetta.transform.SetParent(transform);
            fulmine.transform.SetParent(transform);

            StartCoroutine(throwEndCoroutine(throwTime / 4));

        }

    }

    private IEnumerator throwEndCoroutine(float timeToEndAnim)
    {
        yield return new WaitForSeconds(timeToEndAnim);
        camera.SetFloat("Animate", 1);
        if (transform.Find("fulmine_unity(Clone)") != null && transform.Find("valigetta_unity(Clone)") != null)
        {
            Destroy(transform.Find("fulmine_unity(Clone)").gameObject);
            Destroy(transform.Find("valigetta_unity(Clone)").gameObject);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = startRot;
        }
    }

    public void fireInst()
    {
        if (readyToAnim)
        {
            GameObject pistola = Instantiate(pistolaPrefab, transform.position, transform.rotation);
            pistola.transform.localScale = transform.localScale;
            pistola.transform.SetParent(transform);

            StartCoroutine(fireEndCoroutine(fireTime / 2));
        }

    }

    private IEnumerator fireEndCoroutine(float timeToEndAnim)
    {
        yield return new WaitForSeconds(timeToEndAnim);
        Destroy(transform.Find("pistola_unity(Clone)").gameObject);
    }

    public void backToMenu()
    {

        StartCoroutine(backCoroutine());
    }

    private IEnumerator backCoroutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.localScale = startScale;
        searchPath();
    }


}
