using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle : MonoBehaviour
{
    [SerializeField] private float _moneyForKillByBattleLevel;
    [SerializeField] private float _expForKillByBattleLevel;
    [SerializeField] private float _moneyForOneDamageByBattleLevel;
    [SerializeField] private float _expForOneDamagelByBattleLevel;
    [SerializeField] private float _moneyCostForStartBattleByBattleLevel;
    [SerializeField] private float _moneyMultiplierForWin;
    [SerializeField] private float _expMultiplierForWin;
    [SerializeField] private float _moneyMultiplierForAds;
    [SerializeField] private float _expMultiplierForAds;
    [SerializeField] private float _giveRatingByEachBattleLevel;
    [SerializeField] private PlayerBattleUI _playerBattleUI;
    [SerializeField] private List<Transform> _redTeamSpawnPositions = new List<Transform>();
    [SerializeField] private List<Transform> _blueTeamSpawnPositions = new List<Transform>();
    private List<AI> _ai = new List<AI>();
    private List<Tank> _tanks = new List<Tank>();
    private List<Tank> _aliveTanks = new List<Tank>();
    private Tank.Fraction _playerFraction;
    private List<Tank> _playerTeamTanks = new List<Tank>();
    private List<Tank> _enemyTeamTanks = new List<Tank>();
    private Tank _playerTank;
    private static int _gamesCount;
    public List<Tank> Tanks { get => _tanks; }
    public List<Tank> AliveTanks { get => _aliveTanks; }

    private void Start()
    {
        SpawnTanks();
    }
    private void SpawnTanks()
    {
        _ai = new List<AI>();
        List<Transform> redTeamAvailableSpawnPositions = new List<Transform>();
        List<Transform> blueTeamAvailableSpawnPositions = new List<Transform>();
        redTeamAvailableSpawnPositions.AddRange(_redTeamSpawnPositions);
        blueTeamAvailableSpawnPositions.AddRange(_blueTeamSpawnPositions);

        List<TankScriptableObject> redTeamTanks = new List<TankScriptableObject>();
        List<TankScriptableObject> blueTeamTanks = new List<TankScriptableObject>();
        redTeamTanks.AddRange(StaticInfo.RedTeamTanks);
        blueTeamTanks.AddRange(StaticInfo.BlueTeamTanks);

        SpawnTeam(StaticInfo.PlayerIsRed, redTeamTanks, redTeamAvailableSpawnPositions, Tank.Fraction.Red);
        SpawnTeam(!StaticInfo.PlayerIsRed, blueTeamTanks, blueTeamAvailableSpawnPositions, Tank.Fraction.Blue);

        _playerTeamTanks = _tanks.Where(x => x.TankFraction == _playerFraction).ToList();
        _enemyTeamTanks = _tanks.Where(x => x.TankFraction != _playerFraction).ToList();
        SetTeamTankInfoUI();
        SetTeamHPSliders();
        SetTanksUI();

        _ai.ForEach(x => x.SetTargets());
    }
    private Transform GetRandomSpawnPosition(List<Transform> spawnPositions)
    {
        int r = Random.Range(0, spawnPositions.Count);
        return spawnPositions[r];
    }
    private void SpawnTeam(bool havePlayer, List<TankScriptableObject> tanks, List<Transform> availableSpawnPositions, Tank.Fraction fraction)
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            Transform pos = GetRandomSpawnPosition(availableSpawnPositions);
            availableSpawnPositions.Remove(pos);
            GameObject tankObj = Instantiate(tanks[i].TankPrefub, pos.position, pos.rotation);
            Tank tank = tankObj.GetComponent<Tank>();
            int bodyLevel;
            int towerLevel;
            int cannonLevel;
            int truckLevel;
            bool isPlayer = false;
            if (havePlayer && i == 0)
            {
                tankObj.AddComponent<PlayerBattle>();
                bodyLevel = StaticInfo.PlayerTankBodyLevel;
                towerLevel = StaticInfo.PlayerTankTowerLevel;
                cannonLevel = StaticInfo.PlayerTankCannonLevel;
                truckLevel = StaticInfo.PlayerTankTruckLevel;
                _playerFraction = fraction;
                isPlayer = true;
                _playerTank = tank;
            }
            else
            {
                tankObj.AddComponent<AI>();
                bodyLevel = tanks[i].MaxBodyLevel;
                towerLevel = tanks[i].MaxTowerLevel;
                cannonLevel = tanks[i].MaxCannonLevel;
                truckLevel = tanks[i].MaxTruckLevel;
                _ai.Add(tankObj.GetComponent<AI>());
            }
            tank.SetTankProperties(bodyLevel, towerLevel, cannonLevel, truckLevel, tanks[i], fraction, isPlayer);
            _tanks.Add(tank);
            _aliveTanks.Add(tank);
        }
    }
    public void DestroyTank(Tank tank, Tank from)
    {
        if (tank.IsPlayer)
        {
            _playerBattleUI.HideUIOnDead();
        }
        _aliveTanks.Remove(tank);
        UpdateHPUI();
        UpdateTankInfoInTeam(from);
        UpdateTankInfoInTeam(tank);
        _playerBattleUI.ShowKillMessage(from.TankScriptable.TankName, tank.TankScriptable.TankName, from.TankFraction == _playerFraction);
        _ai.ForEach(x => x.SetTargets());
        CheckForEndGame();
    }
    private void SetTanksUI()
    {
        for (int i = 0; i < _ai.Count; i++)
        {
            _ai[i].GetTank().SetTankUI(_ai[i].GetTank().TankFraction == _playerFraction);
        }
    }
    private void SetTeamTankInfoUI()
    {
        _playerBattleUI.CreateTeamTanksInfo(_playerTeamTanks, _enemyTeamTanks);
    }
    private void UpdateTankInfoInTeam(Tank tank)
    {
        bool isPlayerTeam = tank.TankFraction == _playerFraction;
        _playerBattleUI.UpdateTankInfoInTeam(tank, isPlayerTeam);
    }
    private void SetTeamHPSliders()
    {
        float playerTeamMaxHP = _playerTeamTanks.Sum(x => x.MaxHP);
        float enemyTeamMaxHP = _enemyTeamTanks.Sum(x => x.MaxHP);
        _playerBattleUI.SetHPSliderValue(playerTeamMaxHP, enemyTeamMaxHP);
        UpdateHPUI();
    }
    public void UpdateHPUI()
    {
        int playerTeamAliveCount = _playerTeamTanks.Where(x => x.IsAlive == true).Count();
        int enemyTeamAliveCount = _enemyTeamTanks.Where(x => x.IsAlive == true).Count();
        float playerTeamHP = _playerTeamTanks.Sum(x => x.HP);
        float enemyTeamHP = _enemyTeamTanks.Sum(x => x.HP);
        _playerBattleUI.UpdateTeamHP(playerTeamAliveCount, enemyTeamAliveCount, playerTeamHP, enemyTeamHP);
    }
    public void UpdatePlayerHPUI(float val, bool isDamage)
    {
        _playerBattleUI.UpdatePlayerHPSlider(val);
        if (isDamage)
        {
            _playerBattleUI.ShowHitEffect();
        }
    }
    private void CheckForEndGame()
    {
        bool isBattleEnd = false;
        bool isWin = false;
        if (_aliveTanks.Count(x => x.TankFraction == Tank.Fraction.Red && x.IsAlive) == 0)
        {
            isWin = _playerFraction == Tank.Fraction.Blue;
            isBattleEnd = true;
        }
        else if (_aliveTanks.Count(x => x.TankFraction == Tank.Fraction.Blue && x.IsAlive) == 0)
        {
            isWin = _playerFraction == Tank.Fraction.Red;
            isBattleEnd = true;
        }
        if (isBattleEnd)
        {
            CalculateMoneyForGame(isWin);
            CalculateExpForGame(isWin);
            CalculateRatingForGame(isWin);
            _playerBattleUI.ShowEndGameUI(isWin, StaticInfo.MoneyForBattle, StaticInfo.ExpForBattle, StaticInfo.RatingForBattle);
            _gamesCount++;
            if(_gamesCount >= 5)
            {
                RateUsScript.ShowRateUs();
                _gamesCount = 0;
            }
            PlayerBattle.CanShoot = false;
        }

    }
    private void CalculateMoneyForGame(bool isWin)
    {
        int battleLevel = GetBattleLevel();
        float moneyForBattle = 0;
        moneyForBattle += _playerTank.DamageDone * _moneyForOneDamageByBattleLevel * battleLevel;
        moneyForBattle += _playerTank.KillCount * _moneyForKillByBattleLevel * battleLevel;
        if (isWin)
        {
            moneyForBattle *= _moneyMultiplierForWin;
        }
        StaticInfo.MoneyForBattle = moneyForBattle - GetSpendMoney();
    }
    private float GetSpendMoney()
    {
        float money = _moneyCostForStartBattleByBattleLevel * GetBattleLevel();
        return money;
    }
    private void CalculateExpForGame(bool isWin)
    {
        int battleLevel = GetBattleLevel();
        float expForBattle = 0;
        expForBattle += _playerTank.DamageDone * _expForOneDamagelByBattleLevel * battleLevel;
        expForBattle += _playerTank.KillCount * _expForKillByBattleLevel * battleLevel;
        if (isWin)
        {
            expForBattle *= _expMultiplierForWin;
        }
        StaticInfo.ExpForBattle = expForBattle;
    }
    private void CalculateRatingForGame(bool isWin)
    {
        int battleLevel = GetBattleLevel();
        float ratingForBattle = _giveRatingByEachBattleLevel * battleLevel;
        if (isWin == false)
        {
            ratingForBattle = -ratingForBattle;
        }
        StaticInfo.RatingForBattle = ratingForBattle;
    }
    private int GetBattleLevel()
    {
        return Mathf.FloorToInt(_tanks.Sum(x => x.BattleRating) / _tanks.Count);
    }
    public void GetAdsReward()
    {
        float spendMoney = GetSpendMoney();
        StaticInfo.MoneyForBattle = (StaticInfo.MoneyForBattle + spendMoney) * _moneyMultiplierForAds - spendMoney;
        StaticInfo.ExpForBattle = StaticInfo.ExpForBattle * _expMultiplierForAds;
        _playerBattleUI.UpdateEndGameUICurrencyForAds(StaticInfo.MoneyForBattle, StaticInfo.ExpForBattle);
    }
}
