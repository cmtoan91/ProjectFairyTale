using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFTCameraController : MonoBehaviour
{
    #region props

    [SerializeField]
    Camera _cameraToControl;

    [SerializeField]
    float _cameraMoveSpeed = 5f;

    [SerializeField]
    float _cameraPanSpeed = 20f;

    [SerializeField]
    float _zoomSpeed = 20f;

    [SerializeField]
    PFTCameraControlType _controlType = PFTCameraControlType.Uninverted; 

    [SerializeField]
    KeyCode _keyToMove = KeyCode.Mouse1;

    [SerializeField]
    KeyCode _keyToRotate = KeyCode.Mouse0;

    [SerializeField]
    float _minDiff = 1f;


    Vector3 _mousePosThisFrame;
    Vector3 _mousePosLastFrame;
    #endregion


    private void Update()
    {
        ToMoveCamera(_keyToMove);
        ToRotateCameraAroundSelf(_keyToRotate);
        ZoomCamera();
    }


    void ToMoveCamera(KeyCode moveKey)
    {
        if (Input.GetKeyDown(moveKey))
        {
            ResetMousePosition();
        }

        if (Input.GetKey(moveKey))
        {
            _mousePosThisFrame = Input.mousePosition;
            Vector3 delta = _mousePosThisFrame - _mousePosLastFrame;
            if (delta.magnitude > _minDiff)
            {
                Vector3 camMoveDir = _cameraToControl.transform.right * delta.x + _cameraToControl.transform.up * delta.y;
                _cameraToControl.transform.position -= camMoveDir * _cameraMoveSpeed * Time.deltaTime * (int)_controlType;
            }
            _mousePosLastFrame = Input.mousePosition;
        }
    }

    void ToRotateCameraAroundSelf(KeyCode rotateKey)
    {
        if (Input.GetKeyDown(rotateKey))
        {
            ResetMousePosition();
        }

        if (Input.GetKey(rotateKey))
        {
            _mousePosThisFrame = Input.mousePosition;

            Vector3 delta = _mousePosThisFrame - _mousePosLastFrame;
            if (delta.magnitude > _minDiff)
            {
                Vector3 toRotate = new Vector3(delta.y, -delta.x, 0);
                Vector3 currentEulers = _cameraToControl.transform.eulerAngles;
                currentEulers.z = 0;
                currentEulers -= toRotate * _cameraPanSpeed * Time.deltaTime * (int)_controlType;
                _cameraToControl.transform.eulerAngles = currentEulers;
            }
            _mousePosLastFrame = Input.mousePosition;
        }
    }

    void ZoomCamera()
    {
        _cameraToControl.transform.position += Input.mouseScrollDelta.y * _zoomSpeed *Time.deltaTime * _cameraToControl.transform.forward;
    }

    void ToRotateAroundTarget(Vector3 targetPos)
    {

    }

    void ResetMousePosition()
    {
        _mousePosThisFrame = _mousePosLastFrame = Input.mousePosition;

    }
}
