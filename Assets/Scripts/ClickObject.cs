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

    public bool canClick = true;

    void Start()
    {
        cam = Camera.main;
        playerController = GetComponent<PlayerController>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerController.canMove)
        {
            Debug.Log("clicked and can move");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, clickRange))
            {
                if (hit.transform.gameObject.CompareTag("Clickable"))
                {
                    Debug.Log("clicked a clickable");
                    StartCoroutine(MoveObject(hit.transform));

                }
            }
        }
    }

    IEnumerator MoveObject(Transform clickedObj)
    {
        playerController.canMove = false;
        clickedObj.DOMove(objectTarget.position, moveDuration);
        clickedObj.DOLookAt(cam.gameObject.transform.position, moveDuration);
        clickedObj.SetParent(objectTarget);

        yield return new WaitForSeconds(moveDuration);

        playerController.canMove = true;

        yield return null;
    }
}
