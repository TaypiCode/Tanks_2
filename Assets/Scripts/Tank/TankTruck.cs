using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTruck : MonoBehaviour
{
    private Tank _tank;
    private TankModule _module;
    private void Start()
    {
        _tank = GetComponentInParent<Tank>();
        _module = GetComponent<TankModule>();
    }
    public TankModule GetModule()
    {
        if (_module == null)
        {
            _module = GetComponent<TankModule>();
        }
        return _module;
    }
    public Tank GetTank()
    {
        return _tank;
    }
}
