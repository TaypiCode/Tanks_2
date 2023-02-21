using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBattleItem : MonoBehaviour
{
    [SerializeField] private Image _cooldownImg;
    [SerializeField] private TextMeshProUGUI _cooldownText;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private KeyCode _key;
    [SerializeField] private float _reloadTime;
    private IPlayerBattleItem _item;
    private float _reloadTimer = 0;
    private Coroutine _timer;
    private void Start()
    {
        _item = GetComponent<IPlayerBattleItem>();
        _keyText.text = _key.ToString();
        HideCooldown();
    }
    private void Update()
    {
        if (Input.GetKey(_key) && _reloadTimer <= 0)
        {
            if (_item.Use() == true)
            {
                StartCooldown();
            }
        }
    }
    private void HideCooldown()
    {
        _cooldownText.text = "";
        _cooldownImg.enabled = false;
        if(_timer != null)
        {
            StopCoroutine(_timer);
        }
    }
    private void StartCooldown()
    {
        if (_timer != null)
        {
            StopCoroutine(_timer);
        }
        _reloadTimer = _reloadTime;
        _timer = StartCoroutine(Timer());
        SetCooldownText(Mathf.CeilToInt(_reloadTimer).ToString());
        _cooldownImg.enabled = true;
    }
    private void SetCooldownText(string val)
    {
        _cooldownText.text = val;
    }
    private IEnumerator Timer()
    {
        while(_reloadTimer > 0)
        {
            _reloadTimer -= Time.deltaTime;
            SetCooldownText(Mathf.CeilToInt(_reloadTimer).ToString());
            yield return new WaitForEndOfFrame();
        }
        HideCooldown();
    }
}
