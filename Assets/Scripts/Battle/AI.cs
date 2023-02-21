using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI : MonoBehaviour
{
    private Tank _tank;
    private List<Tank> _targets = new List<Tank>();
    private Tank _target;
    private bool _needSetTargets = false;
    private Transform _bulletSpawnTransform;
    private Vector3 _stuckPosition;
    private float _stuckTime;
    private float _maxStuckTime = 2f;

    private void Start()
    {
        _tank = GetComponent<Tank>();
        _bulletSpawnTransform = _tank.GetBulletSpawnTransform();
    }
    private void Update()
    {
        if (_tank.IsAlive)
        {
            if (_needSetTargets)
            {
                FindTargets();
            }
            if (_stuckTime >= _maxStuckTime)
            {
                if (_target == null || _target.IsAlive == false)
                {
                    FindVisibleOrFarTarget(true);
                }
            }
            else
            {
                FindVisibleOrFarTarget();
            }
            if (_target != null)
            {
                if (_target.IsAlive)
                {
                    LookAtTarget();
                    if (AimedOnEnemy())
                    {
                        RaycastHit hit;
                        Vector3 shotPos = Vector3.zero;
                        if (Physics.Raycast(_bulletSpawnTransform.position, (_target.BodyTransform.position - _bulletSpawnTransform.position), out hit, Mathf.Infinity))
                        {
                            TankModule module;
                            if (module = hit.collider.GetComponent<TankModule>())
                            {
                                if (_target == module.GetTank())
                                {
                                    shotPos = hit.point;
                                }
                            }
                        }

                        _tank.TryShot(shotPos);
                        _tank.StopMoving();
                    }
                    else
                    {
                        if (_stuckPosition != _tank.BodyTransform.position)
                        {
                            _stuckPosition = _tank.BodyTransform.position;
                            _stuckTime = 0;
                            MoveToTarget();
                        }
                        else
                        {
                            if (_stuckTime < _maxStuckTime)
                            {
                                _stuckTime += Time.deltaTime;
                            }
                            else
                            {
                                _stuckTime = 0;
                                MoveBackInRandom(10);
                            }
                        }
                    }
                }
                else
                {
                    _tank.StopMoving();
                }
            }
        }
        else
        {
            _tank.StopMoving();
        }
    }
    private void MoveBackInRandom(float range)
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        Vector3 newPos = new Vector3(x, 0, z);
        _tank.MoveToTarget(newPos);
    }
    private void MoveToTarget()
    {
        if (_target != null)
        {
            if (Vector3.Distance(_target.BodyTransform.position, _tank.BodyTransform.position) > 20)
            {
                _tank.MoveToTarget(_target.BodyTransform.position);
            }
        }
        else
        {
            SetTargets();
        }
    }
    private void LookAtTarget()
    {
        if (_tank.LookAtTarget(_target.BodyTransform.position) == false)
        {
            SetTargets();
        }
    }
    private bool FindVisibleOrFarTarget(bool skipCurrentTarget = false)
    {
        if (_targets.Count > 0)
        {
            try
            {
                List<Tank> targets = _targets.Where(x => x.IsAlive).OrderBy(x => Vector3.Distance(x.BodyTransform.position, _tank.BodyTransform.position)).ToList();
                if (skipCurrentTarget)
                {
                    targets.Remove(_target);
                }
                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (IsVisible(targets[i]))
                        {
                            _target = targets[i];
                            return true;
                        }
                    }
                    //get far target is nor return
                    _target = targets.Last();
                }
                else
                {
                    SetTargets();
                }
            }
            catch
            {
                SetTargets();
            }
        }
        return false;
    }
    public void SetTargets()
    {
        _needSetTargets = true;
    }
    private void FindTargets()
    {
        _targets = new List<Tank>();
        Tank.Fraction fraction = _tank.TankFraction;
        _targets.AddRange(_tank.Battle.AliveTanks.Where(x => x.TankFraction != fraction && x.IsAlive).ToArray());
        _targets.Remove(_tank);
        _needSetTargets = false;
    }
    private bool IsVisible(Tank tank)
    {
        RaycastHit hit;

        if (Physics.Raycast(_tank.BodyTransform.position, (tank.BodyTransform.position - _tank.BodyTransform.position), out hit, Mathf.Infinity))
        {
            TankModule module;
            if (module = hit.collider.GetComponent<TankModule>())
            {
                if (tank == module.GetTank())
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool AimedOnEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(_bulletSpawnTransform.position, _bulletSpawnTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Tank tank = null;
            TankModule module;
            if (module = hit.collider.GetComponent<TankModule>())
            {
                tank = module.GetTank();
                
            }
            if (tank != null)
            {
                if (tank.IsAlive)
                {
                    if (tank.TankFraction != _tank.TankFraction)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public Tank GetTank()
    {
        if(_tank == null)
        {
            _tank = GetComponent<Tank>();
        }
        return _tank;
    }
}
