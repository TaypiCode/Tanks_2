using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletScriptableObject", menuName = "ScriptableObjects/BulletScriptableObject", order = 1)]
public class BulletScriptableObject : ScriptableObject
{
    [ScriptableObjectId] public string itemId;
    [SerializeField] private float _bulletSpeed = 1000;
    [SerializeField] private GameObject _bulletPrefub;

    public float BulletSpeed { get => _bulletSpeed;  }
    public GameObject BulletPrefub { get => _bulletPrefub; }
}
