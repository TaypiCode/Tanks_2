using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankInResearch : MonoBehaviour
{
    [SerializeField] private Research _research;
    [SerializeField] private TankScriptableObject _tankScriptable;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _tankName;
    [SerializeField] private GameObject _unlockBtn;
    [SerializeField] private GameObject _buyBtn;

    public TankScriptableObject TankScriptable { get => _tankScriptable;  }
    private void Start()
    {
        _image.sprite = _tankScriptable.TankImage;
        _tankName.text = _tankScriptable.TankName;
    }
    public void SetBtns(bool canUnlock, bool canBuy)
    {
        _unlockBtn.SetActive(canUnlock);
        _buyBtn.SetActive(canBuy);
    }
    public void ShowUnlockDialog()
    {
        _research.ShowUnlockAccept(this);
    }
    public void ShowBuyDialog()
    {
        _research.ShowBuyAccept(this);
    }
}
