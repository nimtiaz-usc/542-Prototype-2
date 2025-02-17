using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Camera cam;
    [SerializeField] float walkSpeed = 6f;

    [SerializeField] float lookSpeed = 2f;
    [SerializeField] float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;

    public bool canMove = true;

    CharacterController characterController;

    private void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

    }

}
