using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class TankCannon : MonoBehaviour
{
    [SerializeField] private AudioSource _shotAudio;
    [Header("Effects")]
    [SerializeField] private GameObject _shotEffect;
    [SerializeField] private GameObject _hitEffect;
    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefub;
    [SerializeField] private Transform _bulletSpawnTransform;

    private Transform _cannonTransform;
    private List<GameObject> _bullets = new List<GameObject>();
    private List<GameObject> _shotEffects = new List<GameObject>();
    private List<GameObject> _hitEffects = new List<GameObject>();
    private Tank _tank;
    private TankModule _module;
    private float _reloadTimer = 10f;
    private BulletScriptableObject _selectedBullet;
    private float _critChance;
    private float _damage;
    public Transform CannonTransform { get => _cannonTransform; }
    public BulletScriptableObject SelectedBullet { get => _selectedBullet; }
    public Transform BulletSpawnTransform { get => _bulletSpawnTransform;  }
    public float CritChance { get => _critChance; }
    public float Damage { get => _damage; }

    private void Start()
    {
        _tank = GetComponentInParent<Tank>();
        _module = GetComponent<TankModule>();
        _cannonTransform = transform;

    }
    private void Update()
    {
        if(_reloadTimer >= 0)
        {
            _reloadTimer -= Time.deltaTime;
        }
    }
    public void SetData(float damage, float critChance)
    {
        _damage = damage;
        _critChance = critChance;
    }
    public void TryShot(Vector3 endPos)
    {
        if (_module.GetModuleCondition != TankModule.ModuleCondition.Broken && _reloadTimer < 0)
        {
            /* if use bullets
            GameObject bullet = null;
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i].activeSelf == false)
                {
                    bullet = _bullets[i];
                    Transform bulletTransform = bullet.transform;
                    bulletTransform.position = _bulletSpawnTransform.position;
                    bulletTransform.rotation = _bulletSpawnTransform.rotation;
                    bullet.SetActive(true);
                    break;
                }
            }
            if (bullet == null)
            {
                bullet = Instantiate(_bulletPrefub, _bulletSpawnTransform.position, _bulletSpawnTransform.rotation);
                _bullets.Add(bullet);
            }
            bullet.GetComponent<Bullet>().ShotBullet(this, _selectedBullet);
            */

            ShowShotEffect();
            _shotAudio.Play();
            //not use bullets as objects
            RaycastHit hit;
            Vector3 fromPosition = _bulletSpawnTransform.position;
            Vector3 direction = endPos - fromPosition;

            if (endPos!= Vector3.zero) {
                if (Physics.Raycast(_bulletSpawnTransform.position, direction, out hit))
                {
                    TankModule module;
                    if (module = hit.collider.GetComponent<TankModule>())
                    {
                        Tank myTank = GetTank();
                        Tank hitTank = module.GetTank();
                        if (hitTank != myTank && hitTank.IsAlive) //hit any tank
                        {
                            int r = Random.Range(0, 101);
                            if (r <= CritChance)
                            {
                                module.CritModule();
                            }
                            float damage = Damage;
                            hitTank.GetDamage(damage, myTank);
                            myTank.AddDoneDamage(damage);
                        }
                    }
                    ShowHitEffect(hit.point);


                }
            }
            ReloadCannon();
        }
    }
    private bool ReloadCannon() 
    {
        if (_tank != null)
        {
            if (_tank.TankScriptable != null)
            {
                _reloadTimer = _tank.TankScriptable.CannonReloadTimeByLevel[_tank.CannonLevel - 1];
                return true;
            }
        }
        return false;
    }
    private void ShowShotEffect()
    {
        GameObject obj = null;
        for(int i = 0; i < _shotEffects.Count; i++)
        {
            if (_shotEffects[i].activeSelf == false)
            {
                obj = _shotEffects[i];
                break;
            }
        }
        if(obj == null)
        {
            obj = Instantiate(_shotEffect, _bulletSpawnTransform.position, Quaternion.identity);
            _shotEffects.Add(obj);
        }
        obj.transform.position = _bulletSpawnTransform.position;
        obj.SetActive(true);
    }
    private void ShowHitEffect(Vector3 pos)
    {
        GameObject obj = null;
        for (int i = 0; i < _hitEffects.Count; i++)
        {
            if (_hitEffects[i].activeSelf == false)
            {
                obj = _hitEffects[i];
                break;
            }
        }
        if (obj == null)
        {
            obj = Instantiate(_hitEffect, pos, Quaternion.identity);
            _hitEffects.Add(obj);
        }
        obj.transform.position = pos;
        obj.SetActive(true);
    }
    public bool SelectBullet(BulletScriptableObject bullet)
    {
        bool r = ReloadCannon();
        _selectedBullet = bullet;
        return r;
    }
    public void RotateCannon(Vector3 target)
    {
        _cannonTransform.rotation = Quaternion.RotateTowards(_cannonTransform.rotation, Quaternion.LookRotation(target), Time.time * _tank.TankScriptable.TowerRotationSpeedByLevel[_tank.TowerLevel-1]);
    }
    public void RotateCannon(float y)
    {
        _cannonTransform.Rotate(Vector3.right, y);
    }
    public Tank GetTank()
    {
        return _tank;
    }
    public float GetReloadTimer()
    {
        return _reloadTimer;
    }
    public TankModule GetModule()
    {
        if(_module == null)
        {
            _module = GetComponent<TankModule>();
        }
        return _module;
    }
}
