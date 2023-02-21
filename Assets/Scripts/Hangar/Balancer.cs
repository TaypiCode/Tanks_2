using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Balancer : MonoBehaviour
{
    [SerializeField] private float _ratingSpread = 0.5f;
    [SerializeField] private AllTanksScript _allTanksScript;
    private List<TankScriptableObject> _allTanksScriptable = new List<TankScriptableObject>();
    
    private int _tanksCountInTeam = 5;
    private void Start()
    {
        _allTanksScriptable = _allTanksScript.AllTanks;
    }
    public void Balance(TankScriptableObject playerTank, int bodyLevel, int towerLevel, int cannonLevel, int truckLevel)
    {
        float br = playerTank.BattleRating;
        StaticInfo.PlayerTankBodyLevel = bodyLevel;
        StaticInfo.PlayerTankTowerLevel = towerLevel;
        StaticInfo.PlayerTankCannonLevel = cannonLevel;
        StaticInfo.PlayerTankTruckLevel = truckLevel;
        List<TankScriptableObject> availableTanks = _allTanksScriptable.Where(x => (x.BattleRating >= br - _ratingSpread && x.BattleRating <= br + _ratingSpread)).ToList();

        List<TankScriptableObject> redTeamTanks = new List<TankScriptableObject>();
        List<TankScriptableObject> blueTeamTanks = new List<TankScriptableObject>();

        int r = Random.Range(0, 2);
        for (int i = 0; i < _tanksCountInTeam; i++)
        {
            if(r == 0 && i == 0)
            {
                redTeamTanks.Add(playerTank);
                StaticInfo.PlayerIsRed = true;
            }
            else
            {
                redTeamTanks.Add(GetRandomTank(availableTanks));
            }
        }
        for (int i = 0; i < _tanksCountInTeam; i++)
        {
            if (r == 1 && i == 0)
            {
                blueTeamTanks.Add(playerTank);
                StaticInfo.PlayerIsRed = false;
            }
            else
            {
                blueTeamTanks.Add(GetRandomTank(availableTanks));
            }
        }

        StaticInfo.BlueTeamTanks.Clear();
        StaticInfo.RedTeamTanks.Clear();
        StaticInfo.BlueTeamTanks.AddRange(blueTeamTanks);
        StaticInfo.RedTeamTanks.AddRange(redTeamTanks);
        
    }
    private TankScriptableObject GetRandomTank(List<TankScriptableObject> tanks)
    {
        int r = Random.Range(0, tanks.Count);
        return tanks[r];
    }
}
