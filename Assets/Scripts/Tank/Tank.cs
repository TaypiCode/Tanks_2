using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class Tank : MonoBehaviour
{
    [SerializeField] private AudioSource _movementAudio;
    [SerializeField] private AudioSource _destroyAudio;
    [SerializeField] private NavMeshAgent _agent;
    [Header("Modules")]
    [SerializeField] private TankTower _tankTower;
    [SerializeField] private TankCannon _tankCannon;
    [SerializeField] private List<TankTruck> _tankTruck = new List<TankTruck>();

    [Header("Camera")]
    [SerializeField] private Camera _zoomCamera;
    [SerializeField] private Transform _cameraDot;

    [Header("Tank")]
    [SerializeField] Transform _bodyTransform;

    [SerializeField] private TankUI _tankUI;

    [SerializeField] private GameObject _destroyEffect;

    private int _killCount = 0;
    private float _damageDone = 0;
    private float _battleRating;
    private float _hp;
    private float _maxHP;

    private Battle _battle;

    private TankScriptableObject _tankScriptable;
    private Fraction _tankFraction;
    private int _bodyLevel;
    private int _towerLevel;
    private int _cannonLevel;
    private int _truckLevel;

    private bool _needActionAfterSetProperties = false;

    private Vector2 _targetDirection;
    private Vector2 _targetCameraDotDirection;

    private bool _isAlive = true;
    private bool _isPlayer = false;

    public enum Fraction
    {
        Red,
        Blue
    }

    public Transform BodyTransform { get => _bodyTransform; }
    public Transform CameraDot { get => _cameraDot; }
    public Camera ZoomCamera { get => _zoomCamera; }
    public Quaternion TargetOrientation { get => Quaternion.Euler(_targetDirection); }
    public Quaternion TargetCameraDotOrientation { get => Quaternion.Euler(_targetCameraDotDirection);  }
    public Fraction TankFraction { get => _tankFraction;}
    public float BattleRating { get => _battleRating;  }
    public TankScriptableObject TankScriptable { get => _tankScriptable; }
    public float HP { get => _hp;  }
    public int BodyLevel { get => _bodyLevel;}
    public int TowerLevel { get => _towerLevel; }
    public int CannonLevel { get => _cannonLevel;  }
    public int TruckLevel { get => _truckLevel; }
    public bool IsAlive { get => _isAlive; }
    public Battle Battle { get => _battle;  }
    public float MaxHP { get => _maxHP;  }
    public int KillCount { get => _killCount;  }
    public bool IsPlayer { get => _isPlayer; }
    public float DamageDone { get => _damageDone; }

    private void Start()
    {

        _targetDirection = _bodyTransform.localRotation.eulerAngles;
        _targetCameraDotDirection = _cameraDot.localRotation.eulerAngles;

        _battle = FindObjectOfType<Battle>();
    }
    private void Update()
    {
        if (_needActionAfterSetProperties)
        {
            bool result = _tankCannon.SelectBullet(_tankScriptable.AvailableAmmo);
            _needActionAfterSetProperties = !result;
        }
    }
    public void SetTankProperties(int bodyLevel, int towerLevel, int cannonLevel, int truckLevel, TankScriptableObject tankScriptable, Fraction fraction, bool isPlayer)
    {
        _bodyLevel = bodyLevel;
        _towerLevel = towerLevel;
        _cannonLevel = cannonLevel;
        _truckLevel = truckLevel;
        _tankScriptable = tankScriptable;
        _tankFraction = fraction;
        _maxHP = tankScriptable.BodyHPByLevel[bodyLevel - 1];
        _hp = _maxHP;
        _battleRating = tankScriptable.BattleRating;
        _needActionAfterSetProperties = true;
        _isPlayer = isPlayer;
        float damage = _tankScriptable.CannonDamageByLevel[cannonLevel - 1];
        float critChance = _tankScriptable.CannonCritChanceByLevel[cannonLevel - 1];
        _tankCannon.SetData(damage, critChance);
    }
    public void SetTankUI(bool isPlayerTeam)
    {
        _tankUI.SetUIValues(_tankScriptable.TankName, _maxHP, isPlayerTeam);
    }
    public void Move(float zMove)
    {
        if (_isAlive)
        {
            PlayMoveSound();
            _agent.isStopped = false;
            float speed = CalculateSpeedByModules(_tankScriptable.TruckMovementSpeedByLevel[_truckLevel - 1]);
            _agent.speed = speed;
            _agent.Move(_bodyTransform.forward * zMove *speed* Time.deltaTime);
        }
    }
    public void MoveToTarget(Vector3 target)
    {
        PlayMoveSound();
        _agent.isStopped = false;
        _agent.speed = CalculateSpeedByModules(_tankScriptable.TruckMovementSpeedByLevel[_truckLevel - 1]);
        _agent.SetDestination(target);
    }
    public void StopMoving()
    {
        StopMoveSound();
        _agent.isStopped = true;
    }
    private void PlayMoveSound()
    {
        if (_movementAudio.isPlaying == false)
        {
            _movementAudio.Play();
        }
    }
    private void StopMoveSound()
    {
        _movementAudio.Stop();
    }
    public void Rotate(float yRotation)
    {
        if (_isAlive)
        {
            float speed = CalculateSpeedByModules(_tankScriptable.TruckRotationSpeedByLevel[_truckLevel - 1]);
            Vector3 m_EulerAngleVelocity = new Vector3(0, yRotation * speed, 0);

            _bodyTransform.Rotate(m_EulerAngleVelocity);
        }
    }
    private float CalculateSpeedByModules(in float speed)
    {
        List<float> trucksSpeed = new List<float>();
        for (int i = 0; i < _tankTruck.Count; i++)
        {
            trucksSpeed.Add(_tankTruck[i].GetModule().CalculateWorkingSpeedMultiplierByModules());
        }
        float speedMultiplier = trucksSpeed.Min();
        return speed * speedMultiplier;
    }
    public void AddKill()
    {
        _killCount++;
    }
    public void AddDoneDamage(float value)
    {
        _damageDone += value;
    }
    private void DestroyTank(Tank killer)
    {
        StopMoving();
        _destroyAudio.Play();
        Instantiate(_destroyEffect, _bodyTransform.position, Quaternion.identity);
        _isAlive = false;
        killer.AddKill();
        _battle.DestroyTank(this, killer);
    }
    public bool LookAtTarget(Vector3 targetPosition)
    {
        try
        {
            if (_isAlive && _tankTower.CanMoveTower())
            {
                Vector3 target;
                target = targetPosition - _tankTower.TowerTransform.position;
                target = new Vector3(target.x, 0, target.z);
                _tankTower.RotateTower(target);
                //_tankTower.TowerTransform.LookAt(target);
                target = targetPosition - _tankCannon.CannonTransform.position;
                target = new Vector3(target.x, target.y, target.z);
                _tankCannon.RotateCannon(target);
                //_tankCannon.CannonTransform.LookAt(target);
            }
            return true;
        }
        catch { return false; }
    }
    public void RotateTowerAndCannon(float x, float y)
    {
        if (_isAlive)
        {
            _tankTower.RotateTower(x);
            _tankCannon.RotateCannon(y);
        }
    }
    public void TryShot(Vector3 endPos)
    {
        if (_isAlive)
        {
            _tankCannon.TryShot(endPos);
        }
    }
    public void GetDamage(float value, Tank from)
    {
        _hp -= value;
        if (_hp <= 0)
        {
            _hp = 0;
            DestroyTank(from);
        }
        _battle.UpdateHPUI();
        if (_isPlayer == false)
        {
            _tankUI.UpdateHPUI(_hp);
            _tankUI.StartGetDamageTimer(value);
        }
        else
        {
            _battle.UpdatePlayerHPUI(_hp, true);
        }
    }
    public Transform GetBulletSpawnTransform()
    {
        return _tankCannon.BulletSpawnTransform;
    }
    public float GetMaxReloadTime()
    {
        return _tankScriptable.CannonReloadTimeByLevel[_cannonLevel - 1];
    }
    public float GetNeedTimeToReload()
    {
        return _tankCannon.GetReloadTimer();
    }
    public TankModule GetCannonModule()
    {
        return _tankCannon.GetModule();
    }
    public TankModule GetTowerModule()
    {
        return _tankTower.GetModule();
    }
    public List<TankModule> GetTruckModule()
    {
        List<TankModule> modules = new List<TankModule>();
        _tankTruck.ForEach(x => modules.Add(x.GetModule()));
        return modules;
    }
}
