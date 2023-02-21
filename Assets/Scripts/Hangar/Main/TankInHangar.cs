using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankInHangar : MonoBehaviour
{
    [SerializeField] private Image _tankImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _battleRatingText;
    private Hangar _hangar;
    private TankScriptableObject _tankScriptable;
    public void SetData(Hangar hangar, TankScriptableObject tankScriptable)
    {
        _hangar = hangar;
        _tankScriptable = tankScriptable;
        _tankImage.sprite = _tankScriptable.TankImage;
        _nameText.text = _tankScriptable.TankName;
        _battleRatingText.text = _tankScriptable.BattleRating.ToString();
    }
    public void SelectThisTank()
    {
        _hangar.SelectTank(_tankScriptable);
    }
}
