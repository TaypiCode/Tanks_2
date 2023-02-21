using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _ratingText;
    [SerializeField] private TextMeshProUGUI _moneyInResearchText;
    [SerializeField] private TextMeshProUGUI _expInResearchText;
    private float _money;
    private float _exp;
    private float _rating;

    public float Money
    {
        get => _money;
        private set
        {
            _money = value;
            if(_money < 0)
            {
                _money = 0;
            }
            UpdatePlayerMoneyText();
        }
    }
    public float Exp
    {
        get => _exp;
        private set
        {
            _exp = value; 
            if (_exp < 0)
            {
                _exp = 0;
            }
            UpdatePlayerExpText();
        }
    }
    public float Rating
    {
        get => _rating;
        set
        {
            _rating = value;
            if (_rating < 0)
            {
                _rating = 0;
            }
            UpdatePlayerRatingText();
        }
    }
    private void Start()
    {
        if (StaticInfo.MoneyForBattle != 0)
        {
            Money += StaticInfo.MoneyForBattle;
            StaticInfo.MoneyForBattle = 0;
        }
        if (StaticInfo.ExpForBattle != 0)
        {
            Exp += StaticInfo.ExpForBattle;
            StaticInfo.ExpForBattle = 0;
        }
        if (StaticInfo.RatingForBattle != 0)
        {
            Rating += StaticInfo.RatingForBattle;
            StaticInfo.RatingForBattle = 0;
        }

        //leaderboard rating
    }
    public bool TrySpendMoney(float value)
    {
        if(_money - value >= 0)
        {
            Money -= value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool TrySpendExp(float value)
    {
        if (_exp - value >= 0)
        {
            Exp -= value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetMoneyFromLoad(float val)
    {
        Money = val;
    }
    public void SetExpFromLoad(float val)
    {
        Exp = val;
    }
    public void UpdatePlayerMoneyText()
    {
        _moneyText.text = Money.ToString();
        _moneyInResearchText.text = Money.ToString();
    }
    public void UpdatePlayerExpText()
    {
        _expText.text = Exp.ToString();
        _expInResearchText.text = Exp.ToString();
    }
    public void UpdatePlayerRatingText()
    {
        _ratingText.text = Rating.ToString();
    }
}
