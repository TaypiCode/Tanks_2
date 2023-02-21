using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TrailRenderer _trail;
    private BulletScriptableObject _scriptable;
    private TankCannon _tankCannon;
    private float _bulletLifetime = 5;
    private float _time;
    private Coroutine _bulletLifeTimer;
    private IEnumerator BulletLifeTimer()
    {
        while(_time > 0)
        {
            _time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        DeactivateBullet();
    }
    public void ShotBullet(TankCannon cannon, BulletScriptableObject scriptable)
    {
        _scriptable = scriptable;
        _trail.Clear();
        if (_bulletLifeTimer != null)
        {
            StopCoroutine(_bulletLifeTimer);
        }
        _time = _bulletLifetime;
        _bulletLifeTimer = StartCoroutine(BulletLifeTimer());
        _tankCannon = cannon;
        _rb.velocity = Vector3.zero;
        //_rb.AddForce(_rb.transform.forward * _scriptable.BulletSpeed, ForceMode.Impulse);
        _rb.AddForce(_rb.transform.forward * _scriptable.BulletSpeed, ForceMode.VelocityChange);
    }
    private void DeactivateBullet()
    {
        StopCoroutine(_bulletLifeTimer);
        gameObject.SetActive(false);

    }
    private bool NeedCritModule()
    {
        int r = Random.Range(0, 101);
        if(r <= _tankCannon.CritChance)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        TankModule tankModule = collision.GetComponent<TankModule>();
        if (tankModule != null)
        {
            Tank myTank = _tankCannon.GetTank();
            Tank hitTank = tankModule.GetTank();
            if (hitTank != myTank && hitTank.IsAlive) //hit any tank
            {
                if (NeedCritModule())
                {
                    tankModule.CritModule();
                }
                float damage= _tankCannon.Damage;
                hitTank.GetDamage(damage, myTank);
                myTank.AddDoneDamage(damage);
                DeactivateBullet();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() == false)
        {
            DeactivateBullet();
        }
    }
}
