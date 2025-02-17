using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickObject : MonoBehaviour
{
    private Camera cam;
    private PlayerController playerController;
    [SerializeField] float clickRange = 10f;
    [SerializeField] Transform objectTarget;
    [SerializeField] float moveDuration = 1f;

    public bool holdingObject = false;

    private Vector3 objectOriginalPosition;
    private Vector3 objectOriginalRotation;

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

            if (Physics.Raycast(ray, out hit, clickRange))
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
                        StartCoroutine(PlaceObject(hit.transform));
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
        objectOriginalPosition = clickedObj.position;
        objectOriginalRotation = clickedObj.rotation.eulerAngles;

        playerController.canMove = false;
        clickedObj.DOMove(objectTarget.position, moveDuration);
        clickedObj.DOLookAt(cam.gameObject.transform.position, moveDuration);
        //clickedObj.SetParent(objectTarget);

        yield return new WaitForSeconds(moveDuration);

        holdingObject = true;

        yield return null;
    }

    IEnumerator PlaceObject(Transform clickedObj)
    {
        clickedObj.DOMove(objectOriginalPosition, moveDuration);
        clickedObj.DORotate(objectOriginalRotation, moveDuration);

        yield return new WaitForSeconds(moveDuration);

        holdingObject = false;
        playerController.canMove = true;

        yield return null;
    }

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
