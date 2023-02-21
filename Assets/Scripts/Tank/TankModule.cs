using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankModule : MonoBehaviour
{
    [SerializeField] private float _repairTime;
    private ModuleCondition _moduleCondition = ModuleCondition.Good;
    [SerializeField] private Tank _tank;
    private float _repairTimer;
    private Coroutine _repairTimerCoroutine;
    private ModuleInTankPanelUI _moduleUI;
    public ModuleCondition GetModuleCondition { get => _moduleCondition;  }

    public enum ModuleCondition
    {
        Good,
        Bad,
        Broken
    }
    private IEnumerator RepairTimer()
    { 
        while(_repairTimer > 0)
        {
            _repairTimer -= Time.deltaTime;
            if (_moduleUI != null)
            {
                _moduleUI.SetRepairTime(_repairTimer);
            }
            yield return new WaitForEndOfFrame();
        }
        _moduleCondition = ModuleCondition.Bad;
        if (_moduleUI != null)
        {
            _moduleUI.SetOrangeColor();
        }
    }
    public void SetModuleUI(ModuleInTankPanelUI moduleUI)
    {
        _moduleUI = moduleUI;
    }
    public float CalculateWorkingSpeedMultiplierByModules()
    {
        float speedMultiplier;
        switch (_moduleCondition)
        {
            case ModuleCondition.Good:
                speedMultiplier = 1;
                break;
            case ModuleCondition.Bad:
                speedMultiplier = 0.5f;
                break;
            case ModuleCondition.Broken:
                speedMultiplier = 0;
                break;
            default:
                speedMultiplier = 0;
                break;
        }
        return speedMultiplier;
    }
    public void CritModule()
    {
        _moduleCondition = ModuleCondition.Broken;
        if(_repairTimerCoroutine != null)
        {
            StopCoroutine(_repairTimerCoroutine);
        }
        _repairTimer = _repairTime;
        _repairTimerCoroutine = StartCoroutine(RepairTimer());
        if (_moduleUI!=null)
        {
            _moduleUI.SetRedColor();
        }
    }
    public void FastRepair()
    {
        if (_repairTimerCoroutine != null)
        {
            StopCoroutine(_repairTimerCoroutine);
        }
        _moduleCondition = ModuleCondition.Good;
        _repairTimer = 0;
        if (_moduleUI != null)
        {
            _moduleUI.SetRepairTime(_repairTimer);
            _moduleUI.SetWhiteColor();
        }
    }
    public Tank GetTank()
    {
        return _tank;
    }
}
