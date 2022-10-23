using UnityEngine;

namespace MainGame {
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

        [SerializeField]
        float _currentCameraDistance = 15f;


        Vector3 _mousePosThisFrame;
        Vector3 _mousePosLastFrame;
        Transform _currentTarget;
        #endregion

        private void Awake()
        {
            SubcribeToEvents();
        }

        private void Update()
        {
            ToMoveCamera(_keyToMove);

            if(_currentTarget == null)
                ToRotateAroundTarget(_keyToRotate, Vector3.zero, _currentCameraDistance);
            else
                ToRotateAroundTarget(_keyToRotate, _currentTarget.position, _currentCameraDistance);

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
                    _cameraToControl.transform.position = Vector3.Slerp(_cameraToControl.transform.position, _cameraToControl.transform.position - camMoveDir, _cameraMoveSpeed * Time.deltaTime * (int)_controlType);
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
                    currentEulers = Vector3.Slerp(currentEulers, currentEulers - toRotate, _cameraPanSpeed * Time.deltaTime * (int)_controlType);
                    _cameraToControl.transform.eulerAngles = currentEulers;
                }
                _mousePosLastFrame = Input.mousePosition;
            }
        }

        void ZoomCamera()
        {
            _cameraToControl.transform.position += Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime * _cameraToControl.transform.forward;
        }

        void ToRotateAroundTarget(KeyCode movekey, Vector3 targetPos, float distanceToTarget)
        {
            if (Input.GetKeyDown(movekey))
            {
                ResetMousePosition();
            }

            if (Input.GetKey(movekey))
            {
                _mousePosThisFrame = Input.mousePosition;

                Vector3 delta = _mousePosThisFrame - _mousePosLastFrame;
                if (delta.magnitude > _minDiff)
                {
                    Vector3 currentDir = (_cameraToControl.transform.position - targetPos).normalized;
                    Vector3 currentDirXZ = currentDir;
                    currentDirXZ.y = 0;
                    currentDirXZ = currentDirXZ.normalized;
                    Vector3 targetDirXZ = Quaternion.AngleAxis((Vector3.SignedAngle(Vector3.forward, currentDirXZ, Vector3.up) + delta.x), Vector3.up) * Vector3.forward;
                   
                    Vector3 axisXZ = Vector3.Cross(Vector3.up, targetDirXZ);
                    Vector3 targetDir = Quaternion.AngleAxis(Vector3.SignedAngle(currentDirXZ, currentDir, axisXZ) + delta.y, axisXZ) * targetDirXZ;

                    Vector3 finalPosition = targetDir * distanceToTarget + targetPos;

                    if(finalPosition.y < 5)
                    {
                        finalPosition.y = 5;
                        targetDir = (finalPosition - targetPos).normalized;
                    }

                    Vector3 targetDirUp = Vector3.Cross(targetDir, axisXZ);

                    _cameraToControl.transform.position = Vector3.Slerp(_cameraToControl.transform.position, finalPosition, _cameraPanSpeed * Time.deltaTime * (int)_controlType);
                    _cameraToControl.transform.rotation = Quaternion.Slerp(_cameraToControl.transform.rotation, Quaternion.LookRotation(-targetDir, targetDirUp), _cameraPanSpeed * Time.deltaTime * (int)_controlType);

                }
                _mousePosLastFrame = Input.mousePosition;
            }
        }

        void ResetMousePosition()
        {
            _mousePosThisFrame = _mousePosLastFrame = Input.mousePosition;
        }

        void SetCameraPosition(object sender, params object[] args)
        {
            if (sender is PFTUnit)
            {
                PFTUnit unit = (PFTUnit)sender;
                unit.SetCamera(transform);
            }
        }

        void OnPlayerSpawn(object sender, params object[] args)
        {
            if(sender is PFTPlayerCardController)
            {
                PFTPlayerCardController player = (PFTPlayerCardController)sender;
                player.SetMainCam(_cameraToControl);
            }
        }

        void SetCameraTarget(object sender, params object[] args)
        {
            _currentTarget = (Transform)sender;
        }

        void SubcribeToEvents()
        {
            Core.SubscribeEvent(EventType.OnUnitSpawn, SetCameraPosition);
            Core.SubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
            Core.SubscribeEvent(EventType.OnUnitSelected, SetCameraTarget);
        }

        void UnsubcribeToEvents()
        {
            Core.UnsubscribeEvent(EventType.OnUnitSpawn, SetCameraPosition);
            Core.UnsubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
            Core.SubscribeEvent(EventType.OnUnitSelected, SetCameraTarget);

        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }
    }
}