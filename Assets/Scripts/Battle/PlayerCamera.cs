using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _cameraRotationSpeed = 0.5f;
    [SerializeField] private float _cameraScrollSpeed = 10f;
    [SerializeField] private float _minCameraDistance = -10;
    [SerializeField] private float _maxCameraDistance = 30;
    private Camera _zoomCamera;
    private Transform _cameraDot;
    private Transform _cameraTransform;
    private float _mouseWheelValue = 1;

    private Tank _playerTank;


    private void FixedUpdate()
    {
        if (_playerTank != null)
        {
            MouseLook();
        }
    }
    public void SetCameraValues(Tank playerTank)
    {
        _playerTank = playerTank;
        _zoomCamera = _playerTank.ZoomCamera;
        _cameraDot = _playerTank.CameraDot;

        _cameraTransform = _mainCamera.transform;
        _zoomCamera.enabled = false;

        _virtualCamera.Follow = playerTank.CameraDot;
    }
    private void MouseLook()
    {
        float vertical = Input.GetAxisRaw("Mouse X") * _cameraRotationSpeed;
        float horizontal = -Input.GetAxisRaw("Mouse Y") * _cameraRotationSpeed;
        if (_cameraDot.localRotation.eulerAngles.x + horizontal < 340f && _cameraDot.localRotation.eulerAngles.x + horizontal > 20f)
        {
            horizontal = 0;
        }
        _cameraDot.Rotate(Vector3.right, horizontal, Space.Self);
        _cameraDot.Rotate(Vector3.up, vertical, Space.World);
        _playerTank.RotateTowerAndCannon(vertical, horizontal);
    }
    public void ZoomInOut()
    {
        if (_mainCamera.enabled)
        {
            _zoomCamera.enabled = true;
            _mainCamera.enabled = false;
        }
        else
        {
            _mainCamera.enabled = true;
            _zoomCamera.enabled = false;
        }
    }
    public void Scroll(float mouseWheelValue)
    {
        _mouseWheelValue -= mouseWheelValue * _cameraScrollSpeed;
        if (_mouseWheelValue > _maxCameraDistance)
        {
            _mouseWheelValue = _maxCameraDistance;
        }
        else if (_mouseWheelValue < _minCameraDistance)
        {
            _mouseWheelValue = _minCameraDistance;
        }
        _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = _mouseWheelValue;
    }
    public Vector3 GetCameraRayPos()
    {
        RaycastHit hit;
        Camera activeCamera;
        if (_zoomCamera.enabled) 
        { 
            activeCamera = _zoomCamera; 
        }
        else 
        { 
            activeCamera = _mainCamera; 
        }
        Vector3 fwd = activeCamera.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(activeCamera.transform.position, fwd, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    public bool ZoomEnabled()
    {
        return _zoomCamera.enabled;
    }
}
