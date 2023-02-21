using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBattleItem : MonoBehaviour, IPlayerBattleItem
{
    private Tank _tank;
    public bool Use()
    {
        if(_tank == null)
        {
            _tank = FindObjectOfType<PlayerBattle>().Tank;
        }
        if (_tank.IsAlive)
        {
            _tank.GetCannonModule().FastRepair();
            _tank.GetTowerModule().FastRepair();
            _tank.GetTruckModule().ForEach(x => x.FastRepair());
            return true;
        }
        return false;
    }
}
