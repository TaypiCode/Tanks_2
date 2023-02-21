using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private Hangar _hangar;
    [SerializeField] private Research _research;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private AllTanksScript _allTanksScript;
    private Save _save;
    private void Awake()
    {
        _save = new Save();
        if (PlayerPrefs.HasKey("SV"))
        {
            _save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            _playerData.SetMoneyFromLoad(_save.playerMoney);
            _playerData.SetExpFromLoad(_save.playerExp);
            _playerData.Rating = _save.playerRating;
            _hangar.SetBoughtTanksDataFromLoad(GetTanksFromLoad(_save.boughtTanks), _save.boughtTanksBodyLevel, _save.boughtTanksCannonLevel, _save.boughtTanksTowerLevel, _save.boughtTanksTruckLevel);
            _research.SetUnlockedTanksDataFromLoad(GetTanksFromLoad(_save.unlockedTanks));
        }
        else//no save
        {
            //_playerData.SetMoneyFromLoad(10000000);
            //_playerData.SetExpFromLoad(1000000);
        }
        
    }
    
    private List<TankScriptableObject> GetTanksFromLoad(string[] tankId)
    {
        List<TankScriptableObject> tanks = new List<TankScriptableObject>();
        List<TankScriptableObject> allTanks = _allTanksScript.AllTanks;
        for(int i = 0; i < tankId.Length; i++)
        {
            for(int j = 0; j < allTanks.Count; j++)
            {
                if (tankId[i] == allTanks[j].itemId)
                {
                    tanks.Add(allTanks[j]);
                    break;
                }
            }
        }
        return tanks;
    }
}
