using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using UnityEngine.PlayerLoop;

public class Hangar : MonoBehaviour
{
    [SerializeField] private List<int> _battleScenesId = new List<int>();
    [SerializeField] private SaveGame _save;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Balancer _balancer;
    [SerializeField] private Canvas _acceptMessageCanvas;
    [SerializeField] private TextMeshProUGUI _acceptTitleText;
    [SerializeField] private Canvas _messageCanvas;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Transform _boughtTankListUI;
    [SerializeField] private GameObject _tankInListPrefub;
    [SerializeField] private TankObjectsInHangar _tankObjectsInHangar;
    [SerializeField] private TextMeshProUGUI _selectedTankNameText;
    [Header("TankModules")]
    [SerializeField] private ModuleInHangar _bodyModule;
    [SerializeField] private ModuleInHangar _towerModule;
    [SerializeField] private ModuleInHangar _cannonModule;
    [SerializeField] private ModuleInHangar _truckModule;
    [Header("TankStats")]
    [SerializeField] private Canvas _tankStatsCanvas;
    [SerializeField] private TextMeshProUGUI _battleRatingStatText;
    [SerializeField] private TextMeshProUGUI _hpStatText;
    [SerializeField] private TextMeshProUGUI _damageStatText;
    [SerializeField] private TextMeshProUGUI _reloadStatText;
    [SerializeField] private TextMeshProUGUI _speedStatText;
    private int _selectedTankIndex = -1;
    private List<int> _boughtTanksBodyLevel = new List<int>();
    private List<int> _boughtTanksTowerLevel = new List<int>();
    private List<int> _boughtTanksCannonLevel = new List<int>();
    private List<int> _boughtTanksTruckLevel = new List<int>();
    private List<TankScriptableObject> _boughtTanks = new List<TankScriptableObject>();
    private ModuleForUpgrade _moduleForUpgrade;
    public enum ModuleForUpgrade
    {
        Body,
        Tower,
        Cannon,
        Truck
    }
    private void Start()
    {
        Cursor.visible = true;
        CreateBoughtTanksUI();
    }
    public void SetBoughtTanksDataFromLoad(List<TankScriptableObject> boughtTanks, int[] boughtTanksBodyLevel, int[] boughtTanksCannonLevel, int[] boughtTanksTowerLevel, int[] boughtTanksTruckLevel)
    {
        _boughtTanks = boughtTanks;
        _boughtTanksBodyLevel = boughtTanksBodyLevel.ToList();
        _boughtTanksTowerLevel = boughtTanksTowerLevel.ToList();
        _boughtTanksCannonLevel = boughtTanksCannonLevel.ToList();
        _boughtTanksTruckLevel = boughtTanksTruckLevel.ToList();
    }
    public void GoBattle()
    {
        if (_selectedTankIndex >= 0)
        {
            _save.SaveProgress();
            _balancer.Balance(_boughtTanks[_selectedTankIndex], _boughtTanksBodyLevel[_selectedTankIndex], _boughtTanksTowerLevel[_selectedTankIndex], _boughtTanksCannonLevel[_selectedTankIndex], _boughtTanksTruckLevel[_selectedTankIndex]);
            int randomScene = _battleScenesId[Random.Range(0, _battleScenesId.Count)];
            SceneManager.LoadScene(randomScene);
        }
        else
        {
            ShowMessage("Танк не выбран");
        }
    }
    public void BuyTankFromResearch(TankScriptableObject tank)
    {
        _boughtTanks.Add(tank);
        _boughtTanksBodyLevel.Add(1);
        _boughtTanksTowerLevel.Add(1);
        _boughtTanksCannonLevel.Add(1);
        _boughtTanksTruckLevel.Add(1);
        AddBoughtTankToListUI(tank);
        if (_selectedTankIndex < 0)
        {
            SelectTank(_boughtTanks[0]);
        }
    }
    private void CreateBoughtTanksUI()
    {
        Transform[] childs = _boughtTankListUI.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childs.Length; i++)
        {
            Destroy(childs[i].gameObject);
        }
        for (int i = 0; i < _boughtTanks.Count; i++)
        {
            AddBoughtTankToListUI(_boughtTanks[i]);
        }
        if (_boughtTanks.Count > 0)
        {
            int index = 0;
            if(StaticInfo.SelectedTankIndexInHangar >= 0)
            {
                index = StaticInfo.SelectedTankIndexInHangar;
            }
            SelectTank(_boughtTanks[index]);
        }
        else//hide tank UI
        {
            UpdateSelectedTankUI();
        }
    }
    private void AddBoughtTankToListUI(TankScriptableObject tank)
    {
        GameObject tankObj = Instantiate(_tankInListPrefub, _boughtTankListUI.transform);
        tankObj.GetComponent<TankInHangar>().SetData(this, tank);

    }

    public void SelectTank(TankScriptableObject tank)
    {
        _selectedTankIndex = _boughtTanks.IndexOf(tank);
        StaticInfo.SelectedTankIndexInHangar = _selectedTankIndex;
        _tankObjectsInHangar.SelectTank(tank);
        UpdateSelectedTankUI();
    }
    private void UpdateSelectedTankUI()
    {
        if (_selectedTankIndex >= 0)
        {
            TankScriptableObject selectedTank = _boughtTanks[_selectedTankIndex];
            _selectedTankNameText.text = selectedTank.TankName;
        }
        else //not selected
        {
            _selectedTankNameText.text = "";
        }
        UpdateModulesUI();
        UpdateTankStatsUI();
    }
    private void UpdateModulesUI()
    {
        if (_selectedTankIndex >= 0)
        {
            TankScriptableObject tankScriptable = _boughtTanks[_selectedTankIndex];
            bool canUpgrade;
            int currentLevel;

            currentLevel = _boughtTanksBodyLevel[_selectedTankIndex];
            canUpgrade = currentLevel < tankScriptable.MaxBodyLevel;
            _bodyModule.ShowModule(currentLevel, canUpgrade);

            currentLevel = _boughtTanksTowerLevel[_selectedTankIndex];
            canUpgrade = currentLevel < tankScriptable.MaxTowerLevel;
            _towerModule.ShowModule(currentLevel, canUpgrade);

            currentLevel = _boughtTanksCannonLevel[_selectedTankIndex];
            canUpgrade = currentLevel < tankScriptable.MaxCannonLevel;
            _cannonModule.ShowModule(currentLevel, canUpgrade);

            currentLevel = _boughtTanksTruckLevel[_selectedTankIndex];
            canUpgrade = currentLevel < tankScriptable.MaxTruckLevel;
            _truckModule.ShowModule(currentLevel, canUpgrade);
        }
        else
        {
            _bodyModule.HideModule();
            _towerModule.HideModule();
            _cannonModule.HideModule();
            _truckModule.HideModule();
        }
    }
    private void UpdateTankStatsUI()
    {
        if (_selectedTankIndex >= 0)
        {
            TankScriptableObject tankScriptable = _boughtTanks[_selectedTankIndex];
            _battleRatingStatText.text = tankScriptable.BattleRating.ToString();
            _hpStatText.text = tankScriptable.BodyHPByLevel[_boughtTanksBodyLevel[_selectedTankIndex] - 1].ToString();
            _damageStatText.text = tankScriptable.CannonDamageByLevel[_boughtTanksCannonLevel[_selectedTankIndex] - 1].ToString();
            _reloadStatText.text = tankScriptable.CannonReloadTimeByLevel[_boughtTanksCannonLevel[_selectedTankIndex] - 1].ToString();
            _speedStatText.text = tankScriptable.TruckMovementSpeedByLevel[_boughtTanksTruckLevel[_selectedTankIndex] - 1].ToString();
            _tankStatsCanvas.enabled = true;
        }
        else
        {
            _tankStatsCanvas.enabled = false;
        }
    }
    public void ShowModuleUpgradeAcceptDialog(ModuleForUpgrade module)
    {
        _moduleForUpgrade = module;
        _acceptMessageCanvas.enabled = true;
        float cost = GetNextLevelCost(_moduleForUpgrade);
        string moduleNameRus = "";
        switch (module)
        {
            case ModuleForUpgrade.Body:
                moduleNameRus = "корпус";
                break;
            case ModuleForUpgrade.Cannon:
                moduleNameRus = "пушку";
                break;
            case ModuleForUpgrade.Tower:
                moduleNameRus = "башню";
                break;
            case ModuleForUpgrade.Truck:
                moduleNameRus = "ходовую";
                break;
        }
        _acceptTitleText.text = "Улучшить " + moduleNameRus + " за " + cost;
    }
    public void TryUpgradeModule()
    {
        float cost = GetNextLevelCost(_moduleForUpgrade);
        if (_playerData.TrySpendMoney(cost))
        {
            switch (_moduleForUpgrade)
            {
                case ModuleForUpgrade.Body:
                    _boughtTanksBodyLevel[_selectedTankIndex]++;
                    break;
                case ModuleForUpgrade.Tower:
                    _boughtTanksTowerLevel[_selectedTankIndex]++;
                    break;
                case ModuleForUpgrade.Cannon:
                    _boughtTanksCannonLevel[_selectedTankIndex]++;
                    break;
                case ModuleForUpgrade.Truck:
                    _boughtTanksTruckLevel[_selectedTankIndex]++;
                    break;
            }
            UpdateSelectedTankUI();
            _acceptMessageCanvas.enabled = false;
        }
        else
        {
            ShowMessage("Недостаточно средств");
        }
    }
    private void ShowMessage(string text)
    {
        _messageCanvas.enabled = true;
        _messageText.text = text;
    }
    private float GetNextLevelCost(ModuleForUpgrade module)
    {
        float cost = -1;
        switch (module)
        {
            case ModuleForUpgrade.Body:
                cost = _boughtTanks[_selectedTankIndex].BodyUpgradeCostByLevel[_boughtTanksBodyLevel[_selectedTankIndex] - 1];
                break;
            case ModuleForUpgrade.Tower:
                cost = _boughtTanks[_selectedTankIndex].TowerUpgradeCostByLevel[_boughtTanksTowerLevel[_selectedTankIndex] - 1];
                break;
            case ModuleForUpgrade.Cannon:
                cost = _boughtTanks[_selectedTankIndex].CannonUpgradeCostByLevel[_boughtTanksCannonLevel[_selectedTankIndex] - 1];
                break;
            case ModuleForUpgrade.Truck:
                cost = _boughtTanks[_selectedTankIndex].TruckUpgradeCostByLevel[_boughtTanksTruckLevel[_selectedTankIndex] - 1];
                break;
        }
        return cost;
    }
    public List<TankScriptableObject> GetBoughtTanks()
    {
        return _boughtTanks;
    }
    public string[] GetBoughtTanksId()
    {
        List<string> id = new List<string>();
        _boughtTanks.ForEach(x => id.Add(x.itemId));
        return id.ToArray();
    }
    public int[] GetBoughtTanksBodyLevel()
    {
        return _boughtTanksBodyLevel.ToArray();
    }
    public int[] GetBoughtTanksTowerLevel()
    {
        return _boughtTanksTowerLevel.ToArray();
    }
    public int[] GetBoughtTanksCannonLevel()
    {
        return _boughtTanksCannonLevel.ToArray();
    }
    public int[] GetBoughtTanksTruckLevel()
    {
        return _boughtTanksTruckLevel.ToArray();
    }
}
