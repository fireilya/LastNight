using System;
using System.Linq;
using Assets.scripts.Enum;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private readonly float boost = 5f;

    [SerializeField]
    private float currentSpeed;


    [SerializeField]
    private Animator playerAnimator;

    private float backForwardRotation = 0f;
    private float leftRotation = -90f;
    private float rightRotation = 90f;
    [SerializeField]
    private Transform playerTransform;

    private float rotationSpeed = 500f;
    private float mouseXSensivity = 200f;
    private float mouseYSensivity = 200f;

    private float xCameraRotation;
    private float yCameraRotation;

    private float deltaYPlayerRotation;
    private float yPlayerRotation;

    private readonly MoveMentData temp = new(
        (Keys.Forward, KeyCode.W),
        (Keys.Back, KeyCode.S),
        (Keys.Right, KeyCode.D),
        (Keys.Left, KeyCode.A)
    );

    [SerializeField]
    private readonly float walkSpeed = 1.5f;
    [SerializeField]
    private readonly float runSpeed = 5f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        GetRotation();
        var isMove = temp.MoveKeys.Any(Input.GetKey);
        if (isMove)
        {
            GetMoveInput();
        }
        else
        {
            if (Math.Abs(currentSpeed) > 0)
            {
                if (Math.Abs(currentSpeed) < boost * Time.deltaTime) currentSpeed = 0;
                else currentSpeed -= boost * (currentSpeed > 0 ? 1 : -1) * Time.deltaTime;
            }
        }

        playerAnimator.SetFloat("PlayerProcentSpeed", Math.Abs(currentSpeed) / runSpeed);
    }

    private void GetMoveInput()
    {
        var direction = Input.GetKey(temp.Movement[Keys.Back]) ? -1 : 1;
        var rotationDirection = Input.GetKey(temp.Movement[Keys.Left])
            ? 1
            : Input.GetKey(temp.Movement[Keys.Right])
                ? -1
                : 0;

        RotatePlayerIndependent(rotationDirection, 
                (Input.GetKey(temp.Movement[Keys.Forward]) || Input.GetKey(temp.Movement[Keys.Back]))
                && (Input.GetKey(temp.Movement[Keys.Left]) || Input.GetKey(temp.Movement[Keys.Right])));

        playerAnimator.SetFloat("direction", direction == 1 ? 1 : 0);
        var neededSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        currentSpeed += boost * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, neededSpeed);
        var deltaPosition = playerTransform.forward * currentSpeed * Time.deltaTime * direction;
        playerTransform.localPosition += deltaPosition;
    }

    private void RotatePlayerIndependent(int direction, bool isTwoKeys)
    {
        var highLimit = direction == 0 ? yCameraRotation > 0 ? 90f : 0f : isTwoKeys ? 45f : 90f;
        var lowLimit= direction == 0 ? yCameraRotation > 0 ? 0f : -90f : isTwoKeys ? -45f : -90f;
        direction = direction == 0 ? yCameraRotation > 0 ? -1 : 1 : direction;
        yCameraRotation += rotationSpeed * Time.deltaTime * direction;
        deltaYPlayerRotation -= rotationSpeed * Time.deltaTime * direction;
        deltaYPlayerRotation = Mathf.Clamp(deltaYPlayerRotation, -highLimit, -lowLimit);
        yCameraRotation = Mathf.Clamp(yCameraRotation, lowLimit, highLimit);
        playerTransform.localRotation = Quaternion.Euler(0f, yPlayerRotation + deltaYPlayerRotation, 0f);
        transform.localRotation = Quaternion.Euler(
            xCameraRotation,
            yCameraRotation,
            0f);
    }

    private void GetRotation()
    {
        var mouseX=Input.GetAxis("Mouse X") * mouseXSensivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseYSensivity * Time.deltaTime;
        xCameraRotation -= mouseY;
        yPlayerRotation += mouseX;
        xCameraRotation = Mathf.Clamp(xCameraRotation, -85f, 90f);
        transform.localRotation=Quaternion.Euler(xCameraRotation, yCameraRotation, 0f);
        playerTransform.localRotation=Quaternion.Euler(0f, yPlayerRotation+deltaYPlayerRotation, 0f);
    }
}