using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModuleInHangar : MonoBehaviour
{
    [SerializeField] private Hangar _hangar;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _upgradeBtn;
    [SerializeField] private ModuleType _moduleType;
    public enum ModuleType
    {
        Body,
        Truck,
        Tower,
        Cannon
    }
    public void ShowModule(int level, bool canUpgrade)
    {
        _levelText.text = level.ToString();
        _upgradeBtn.SetActive(canUpgrade);
        gameObject.SetActive(true);
    }
    public void HideModule()
    {
        gameObject.SetActive(false);
    }
    public void UpgradeModule()
    {
        switch (_moduleType)
        {
            case ModuleType.Body:
                _hangar.ShowModuleUpgradeAcceptDialog(Hangar.ModuleForUpgrade.Body);
                break;
            case ModuleType.Truck:
                _hangar.ShowModuleUpgradeAcceptDialog(Hangar.ModuleForUpgrade.Truck);
                break;
            case ModuleType.Tower:
                _hangar.ShowModuleUpgradeAcceptDialog(Hangar.ModuleForUpgrade.Tower);
                break;
            case ModuleType.Cannon:
                _hangar.ShowModuleUpgradeAcceptDialog(Hangar.ModuleForUpgrade.Cannon);
                break;
        }
    }
}
