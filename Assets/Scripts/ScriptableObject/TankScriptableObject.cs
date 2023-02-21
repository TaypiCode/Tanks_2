using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TankScriptableObject", menuName = "ScriptableObjects/TankScriptableObjtct", order = 1)]
public class TankScriptableObject : ScriptableObject
{
    [ScriptableObjectId] public string itemId;
    [SerializeField] private GameObject _tankPrefub;
    [Header("Tank info")]
    [SerializeField] private TankScriptableObject _previousUnlock;
    [SerializeField] private string _tankName;
    [SerializeField] private float _expForUnlock = 1000;
    [SerializeField] private float _buyCost = 10000;
    [SerializeField] private float _battleRating = 1;
    [SerializeField] private Sprite _tankImage;
    [Header("Ammo")]
    [SerializeField] private BulletScriptableObject _availableAmmo;
    [Header("Modules Level")]
    [SerializeField] private int _maxBodyLevel = 4;
    [SerializeField] private int _maxTowerLevel = 4;
    [SerializeField] private int _maxCannonLevel = 4;
    [SerializeField] private int _maxTruckLevel = 4;
    [Header("Modules Stat Level")]
    [SerializeField] private List<float> _bodyHPByLevel = new List<float>();
    [SerializeField] private List<float> _towerRotationSpeedByLevel = new List<float>();
    [SerializeField] private List<float> _cannonReloadTimeByLevel = new List<float>();
    [SerializeField] private List<float> _cannonDamageByLevel = new List<float>();
    [SerializeField] private List<float> _cannonCritChanceByLevel = new List<float>();
    [SerializeField] private List<float> _truckMovementSpeedByLevel = new List<float>(); 
    [SerializeField] private List<float> _truckRotationSpeedByLevel = new List<float>();
    [Header("Modules Cost By Level")]
    [SerializeField] private List<float> _bodyUpgradeCostByLevel = new List<float>();//1 less then max level
    [SerializeField] private List<float> _towerUpgradeCostByLevel = new List<float>(); //1 less then max level
    [SerializeField] private List<float> _cannonUpgradeCostByLevel = new List<float>(); //1 less then max level
    [SerializeField] private List<float> _truckUpgradeCostByLevel = new List<float>(); //1 less then max level
    public GameObject TankPrefub { get => _tankPrefub; }
    public string TankName { get => _tankName; }
    public float ExpForUnlock { get => _expForUnlock; }
    public float BattleRating { get => _battleRating;  }
    public BulletScriptableObject AvailableAmmo { get => _availableAmmo; }
    public int MaxBodyLevel { get => _maxBodyLevel; }
    public int MaxTowerLevel { get => _maxTowerLevel;  }
    public int MaxCannonLevel { get => _maxCannonLevel;  }
    public int MaxTruckLevel { get => _maxTruckLevel;  }
    public List<float> BodyHPByLevel { get => _bodyHPByLevel; }
    public List<float> TowerRotationSpeedByLevel { get => _towerRotationSpeedByLevel; }
    public List<float> CannonReloadTimeByLevel { get => _cannonReloadTimeByLevel;  }
    public List<float> TruckMovementSpeedByLevel { get => _truckMovementSpeedByLevel;  }
    public List<float> TruckRotationSpeedByLevel { get => _truckRotationSpeedByLevel;  }
    public List<float> BodyUpgradeCostByLevel { get => _bodyUpgradeCostByLevel;}
    public List<float> TowerUpgradeCostByLevel { get => _towerUpgradeCostByLevel;  }
    public List<float> CannonUpgradeCostByLevel { get => _cannonUpgradeCostByLevel;  }
    public List<float> TruckUpgradeCostByLevel { get => _truckUpgradeCostByLevel;  }
    public float BuyCost { get => _buyCost;}
    public TankScriptableObject PreviousUnlock { get => _previousUnlock;}
    public Sprite TankImage { get => _tankImage;  }
    public List<float> CannonDamageByLevel { get => _cannonDamageByLevel; }
    public List<float> CannonCritChanceByLevel { get => _cannonCritChanceByLevel; }
}