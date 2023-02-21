using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    private Save save = new Save();
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Research _research;
    [SerializeField] private Hangar _hangar;
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(){
        SaveProgress();
    }
#endif
    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {
        save.playerMoney = _playerData.Money;
        save.playerExp = _playerData.Exp;
        save.playerRating = _playerData.Rating;
        save.unlockedTanks = _research.GetUnlockedTanksId();
        save.boughtTanks = _hangar.GetBoughtTanksId();
        save.boughtTanksBodyLevel = _hangar.GetBoughtTanksBodyLevel();
        save.boughtTanksTowerLevel = _hangar.GetBoughtTanksTowerLevel();
        save.boughtTanksCannonLevel = _hangar.GetBoughtTanksCannonLevel();
        save.boughtTanksTruckLevel = _hangar.GetBoughtTanksTruckLevel();

        LeaderboardScript.SetLeaderboardValue(LeaderboardScript.Names.Rating, _playerData.Rating);
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(save));
        PlayerPrefs.Save();
    }
}
[Serializable]
public class Save
{
    public float playerMoney;
    public float playerExp;
    public float playerRating;
    public string[] unlockedTanks;
    public string[] boughtTanks;
    public int[] boughtTanksBodyLevel;
    public int[] boughtTanksTowerLevel;
    public int[] boughtTanksCannonLevel;
    public int[] boughtTanksTruckLevel;
}