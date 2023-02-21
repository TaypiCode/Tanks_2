using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tankNameText;
    [SerializeField] private TextMeshProUGUI _tankHPText;
    [SerializeField] private TextMeshProUGUI _tankIncomeDamageText;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Image _hpSliderFillImg;
    [SerializeField] private Color _playerTeamColor;
    [SerializeField] private Color _enemyTeamColor;
    [SerializeField] private float _showDamageTime = 1;
    private Camera _playerCamera;
    private Transform _transform;
    private float _incomeDamage;
    private Coroutine GetDamageCoroutine;
    private void Start()
    {
        _transform = transform;
        _playerCamera = Camera.main;
    }
    private void Update()
    {
        _transform.LookAt(_playerCamera.transform);
    }
    public void SetUIValues(string tankName, float maxHP, bool isPlayerTeam)
    {
        gameObject.SetActive(true);
        _tankNameText.text = tankName;
        
        if (isPlayerTeam)
        {
            _tankNameText.color = _playerTeamColor;
            _hpSliderFillImg.color = _playerTeamColor;
        }
        else
        {
            _tankNameText.color = _enemyTeamColor;
            _hpSliderFillImg.color = _enemyTeamColor;
        }
        _hpSlider.maxValue = maxHP;
        UpdateHPUI(maxHP);
    }
    public void UpdateHPUI(float hpValue)
    {
        _hpSlider.value = hpValue;
        _tankHPText.text = hpValue.ToString();
    }
    public void StartGetDamageTimer(float incomeDamage)
    {
        _incomeDamage += incomeDamage;
        if (GetDamageCoroutine != null)
        {
            StopCoroutine(GetDamageCoroutine);
        }
        if (_incomeDamage > 0)
        {
            GetDamageCoroutine = StartCoroutine(GetDamageTimer());
        }
    }
    private IEnumerator GetDamageTimer()
    {
        _tankIncomeDamageText.text = "-" + _incomeDamage;
        yield return new WaitForSeconds(_showDamageTime);
        _incomeDamage = 0;
        _tankIncomeDamageText.text = "";
        StopCoroutine(GetDamageCoroutine);
    }
}
