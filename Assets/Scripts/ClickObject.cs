using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ClickObject : MonoBehaviour
{
    private Camera cam;
    private PlayerController playerController;
    [SerializeField] float clickRange = 10f;
    [SerializeField] Transform objectTarget;
    [SerializeField] float moveDuration = 1f;
    public LayerMask layerMask;

    public AudioSource chime;

    public bool holdingObject = false;

    private int collectedCount = 0;

    //private Vector3 objectOriginalPosition;
    //private Vector3 objectOriginalRotation;

    [SerializeField] Image vignette;
    [SerializeField] Image[] cursor;
    [SerializeField] Image whiteScreen;
    [SerializeField] TextMeshProUGUI counter;

    [SerializeField] Image blackScreen;
    [SerializeField] AudioSource doorSFX;
    [SerializeField] AudioSource lightSFX;

    void Start()
    {
        cam = Camera.main;
        playerController = GetComponent<PlayerController>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, clickRange, layerMask))
            {
                if (hit.transform.gameObject.CompareTag("Clickable"))
                {
                    if (playerController.canMove)
                    {
                        StartCoroutine(GrabObject(hit.transform));
                    }

                    if (holdingObject)
                    {
                        Debug.Log("clicking");
                        StartCoroutine(FinishObject(hit.transform));
                    }
                    

                }

                if (hit.transform.gameObject.CompareTag("Door"))
                {
                    playerController.canMove = false;
                    StartCoroutine(ExitGame());
                }
            }
        }
    }

    IEnumerator GrabObject(Transform clickedObj)
    {
        //objectOriginalPosition = clickedObj.position;
        //objectOriginalRotation = clickedObj.rotation.eulerAngles;

        playerController.canMove = false;
        clickedObj.DOMove(objectTarget.position, moveDuration);
        clickedObj.DOLookAt(cam.gameObject.transform.position, moveDuration);

        foreach (Image ui in cursor)
        {
            ui.DOFade(0, moveDuration);
        }
        counter.DOFade(0, moveDuration);
        vignette.DOFade(1, moveDuration);


        foreach (AudioSource audio in clickedObj.GetComponent<ClickableObject>().nearAudio)
        {
            audio.DOFade(1, moveDuration);
        }

        yield return new WaitForSeconds(moveDuration);

        holdingObject = true;

        yield return null;
    }

    IEnumerator FinishObject(Transform clickedObj)
    {
        whiteScreen.DOFade(1, moveDuration);

        vignette.DOFade(0, moveDuration);

        foreach (AudioSource audio in clickedObj.GetComponent<ClickableObject>().nearAudio)
        {
            audio.DOFade(0, moveDuration);
        }
        clickedObj.GetComponent<ClickableObject>().farAudio.DOFade(0, moveDuration);

        yield return new WaitForSeconds(moveDuration);

        chime.Play();

        Destroy(clickedObj.GetComponent<ClickableObject>().sparkles);
        Destroy(clickedObj.gameObject);

        foreach (Image ui in cursor)
        {
            ui.DOFade(1, 0);
        }
        collectedCount++;
        counter.DOFade(1, 0);
        counter.text = "Collected: " + collectedCount + "/6";

        yield return new WaitForSeconds(1f);

        whiteScreen.DOFade(0, moveDuration);

        holdingObject = false;
        playerController.canMove = true;

        yield return null;
    }
    /*
    IEnumerator PlaceObject(Transform clickedObj)
    {
        clickedObj.DOMove(objectOriginalPosition, moveDuration);
        clickedObj.DORotate(objectOriginalRotation, moveDuration);

        foreach (Image ui in cursor)
        {
            ui.DOFade(1, moveDuration);
        }
        vignette.DOFade(0, moveDuration);

        foreach (AudioSource audio in clickedObj.GetComponent<ClickableObject>().nearAudio)
        {
            audio.DOFade(0, moveDuration);
        }

        yield return new WaitForSeconds(moveDuration);

        holdingObject = false;
        playerController.canMove = true;

        yield return null;
    }
    */

    IEnumerator ExitGame()
    {
        blackScreen.DOFade(1, 0);
        lightSFX.Play();

        yield return new WaitForSeconds(1f);

        doorSFX.Play();

        yield return new WaitForSeconds(4f);

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        yield return null;
    }
}
