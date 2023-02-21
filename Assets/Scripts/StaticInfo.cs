using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticInfo
{
    private static List<TankScriptableObject> redTeamTanks = new List<TankScriptableObject>();
    private static List<TankScriptableObject> blueTeamTanks = new List<TankScriptableObject>();
    private static bool playerIsRed;
    private static int _playerTankBodyLevel;
    private static int _playerTankTowerLevel;
    private static int _playerTankCannonLevel;
    private static int _playerTankTruckLevel;
    private static float _moneyForBattle;
    private static float _expForBattle;
    private static float _ratingForBattle;
    private static int _selectedTankIndexInHangar = -1;
    public static List<TankScriptableObject> RedTeamTanks { get => redTeamTanks; set => redTeamTanks = value; }
    public static List<TankScriptableObject> BlueTeamTanks { get => blueTeamTanks; set => blueTeamTanks = value; }
    public static bool PlayerIsRed { get => playerIsRed; set => playerIsRed = value; }
    public static int PlayerTankBodyLevel { get => _playerTankBodyLevel; set => _playerTankBodyLevel = value; }
    public static int PlayerTankTowerLevel { get => _playerTankTowerLevel; set => _playerTankTowerLevel = value; }
    public static int PlayerTankCannonLevel { get => _playerTankCannonLevel; set => _playerTankCannonLevel = value; }
    public static int PlayerTankTruckLevel { get => _playerTankTruckLevel; set => _playerTankTruckLevel = value; }
    public static float MoneyForBattle { get => _moneyForBattle; set => _moneyForBattle = value; }
    public static float ExpForBattle { get => _expForBattle; set => _expForBattle = value; }
    public static float RatingForBattle { get => _ratingForBattle; set => _ratingForBattle = value; }
    public static int SelectedTankIndexInHangar { get => _selectedTankIndexInHangar; set => _selectedTankIndexInHangar = value; }
}
