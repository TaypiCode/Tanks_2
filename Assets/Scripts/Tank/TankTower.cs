using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTower : MonoBehaviour
{
    [SerializeField] private TankModule _tankModule;
    private Tank _tank;
    private Transform _towerTransform;

    public Transform TowerTransform { get => _towerTransform; }

    private void Start()
    {
        _towerTransform = transform;
        _tank = GetComponentInParent<Tank>();
    }
    public void RotateTower(Vector3 target)
    {
        _towerTransform.rotation = Quaternion.RotateTowards(_towerTransform.rotation, Quaternion.LookRotation(target), Time.time * _tank.TankScriptable.TowerRotationSpeedByLevel[_tank.TowerLevel-1]);
    }
    public void RotateTower(float x)
    {
        _towerTransform.Rotate(Vector3.up * x);
    }
    public Tank GetTank()
    {
        return _tank;
    }
    public bool CanMoveTower()
    {
        return _tankModule.GetModuleCondition != TankModule.ModuleCondition.Broken;
    }
    public TankModule GetModule()
    {
        if (_tankModule == null)
        {
            _tankModule = GetComponent<TankModule>();
        }
        return _tankModule;
    }
}
