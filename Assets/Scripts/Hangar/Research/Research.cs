using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Research : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Hangar _hangar;
    [SerializeField] private Canvas _acceptMessageCanvas;
    [SerializeField] private TextMeshProUGUI _acceptTitleText;
    [SerializeField] private Canvas _messageCanvas;
    [SerializeField] private TextMeshProUGUI _messageTitleText;
    [SerializeField] private List<TankInResearch> _tanksInResearch = new List<TankInResearch>();
    private List<TankScriptableObject> _unlockedTanks = new List<TankScriptableObject>();
    private TankInResearch _selectedTankInResearch;
    private Condition _condition;
    public enum Condition 
    {
        Unlock,
        Buy
    }
    public void SetUnlockedTanksDataFromLoad(List<TankScriptableObject> unlockedTanks)
    {
        _unlockedTanks = unlockedTanks;
        UpdateData();
    }
    public void UpdateData()
    {
        _tanksInResearch.ForEach(x => x.SetBtns(CanShowUnlockBtn(x.TankScriptable), CanShowBuyBtn(x.TankScriptable)));
    }
    public bool CanShowUnlockBtn(TankScriptableObject tank)
    {
        return !_unlockedTanks.Contains(tank);
    }
    public bool CanShowBuyBtn(TankScriptableObject tank)
    {
        if (CanShowUnlockBtn(tank))
        {
            return false;
        }
        List<TankScriptableObject> boughtTanks = _hangar.GetBoughtTanks();
        return !boughtTanks.Contains(tank);
    }
    public void ShowUnlockAccept(TankInResearch tank)
    {
        if (_unlockedTanks.Contains(tank.TankScriptable.PreviousUnlock) || tank.TankScriptable.PreviousUnlock == null)
        {
            _acceptTitleText.text = "Исследовать за " + tank.TankScriptable.ExpForUnlock + " опыта";
            _selectedTankInResearch = tank;
            _condition = Condition.Unlock;
            _acceptMessageCanvas.enabled = true;
        }
        else
        {
            ShowErrorMessage("Откройте танк " + tank.TankScriptable.PreviousUnlock.TankName);
        }
    }
    public void ShowBuyAccept(TankInResearch tank)
    {
        _acceptTitleText.text = "Купить за " + tank.TankScriptable.BuyCost;
        _selectedTankInResearch = tank;
        _condition = Condition.Buy;
        _acceptMessageCanvas.enabled = true;
    }
    public void TryAcceptDialog()
    {
        switch (_condition)
        {
            case Condition.Unlock:
                if (_playerData.TrySpendExp(_selectedTankInResearch.TankScriptable.ExpForUnlock))
                {
                    _unlockedTanks.Add(_selectedTankInResearch.TankScriptable);
                    UpdateData();
                    _acceptMessageCanvas.enabled = false;
                }
                else
                {
                    ShowErrorMessage("Недостаточно опыта");
                }
                break;
            case Condition.Buy:
                if(_playerData.TrySpendMoney(_selectedTankInResearch.TankScriptable.BuyCost))
                {
                    _hangar.BuyTankFromResearch(_selectedTankInResearch.TankScriptable);
                    UpdateData();
                    _acceptMessageCanvas.enabled = false;
                }
                else
                {
                    ShowErrorMessage("Недостаточно средств");
                }
                break;
            default:break;
        }
    }
    private void ShowErrorMessage(string text)
    {
        _messageCanvas.enabled = true;
        _messageTitleText.text = text;
    }
    public TankScriptableObject[] GetUnlockedTanks()
    {
        return _unlockedTanks.ToArray();
    }
    public string[] GetUnlockedTanksId()
    {
        List<string> id = new List<string>();
        _unlockedTanks.ForEach(x => id.Add(x.itemId));
        return id.ToArray();
    }
}
