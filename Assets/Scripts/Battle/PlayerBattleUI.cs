using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerBattleUI : MonoBehaviour
{
    [Header("HitEffect")]
    [SerializeField] private PlayerDamageUIEffect _damageEffect;
    [Header("TopTanksInfo")]
    [SerializeField] private Slider _playerTeamHPSlider;
    [SerializeField] private Slider _enemyTeamHPSlider;
    [SerializeField] private TextMeshProUGUI _playerTeamHPText;
    [SerializeField] private TextMeshProUGUI _enemyTeamHPText;
    [SerializeField] private TextMeshProUGUI _tanksCountText;
    [Header("TankTeamInfo")]
    [SerializeField] private GameObject _playerTankInfoInTeamPrefub;
    [SerializeField] private GameObject _enemyTankInfoInTeamPrefub;
    [SerializeField] private Transform _playerTeamInfoSpawn;
    [SerializeField] private Transform _enemyTeamInfoSpawn;
    private List<TankInfoInTeamUI> _tankInfoInPlayerTeam = new List<TankInfoInTeamUI>();
    private List<TankInfoInTeamUI> _tankInfoInEnemyTeam = new List<TankInfoInTeamUI>();
    [Header("KillChat")]
    [SerializeField] private Transform _killChatMessageUISpawn;
    [SerializeField] private GameObject _killChatMessageUIObjectPrefub;
    [Header("PlayerTankPanel")]
    [SerializeField] private Slider _playerTankHPSlider;
    [SerializeField] private TextMeshProUGUI _playerTankHPText; 
    [SerializeField] private TextMeshProUGUI _panelInfoText;
    [SerializeField] private ModuleInTankPanelUI _cannonModuleUI;
    [SerializeField] private ModuleInTankPanelUI _towerModuleUI;
    [SerializeField] private ModuleInTankPanelUI _truckModuleUI;
    [SerializeField] private GameObject _itemsCanvas;
    [Header("Aim")]
    [SerializeField] private GameObject _aimCanvas;
    [SerializeField] private GameObject _aimImg;
    [SerializeField] private GameObject _zoomAimImg;
    [SerializeField] private Color _reloadColor;
    [SerializeField] private Color _noReloadColor;
    [SerializeField] private Slider _reloadSlider;
    [SerializeField] private Image _reloadImage;
    [Header("EndGame")]
    [SerializeField] private GameObject _endGameCanvas;
    [SerializeField] private TextMeshProUGUI _endGameWinLoseText;
    [SerializeField] private TextMeshProUGUI _moneyForBattleText;
    [SerializeField] private TextMeshProUGUI _expForBattleText;
    [SerializeField] private TextMeshProUGUI _ratingForBattleText;
    [SerializeField] private GameObject _adsBtn;
    [SerializeField] private AdsScript _ads;

    public ModuleInTankPanelUI CannonModuleUI { get => _cannonModuleUI; }
    public ModuleInTankPanelUI TowerModuleUI { get => _towerModuleUI;  }
    public ModuleInTankPanelUI TruckModuleUI { get => _truckModuleUI;  }
    private void Start()
    {
        HideCursor();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCursor();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _endGameCanvas.activeSelf == false)
        {
            HideCursor();
        }
    }
    public void GoToHangar()
    {
        _ads.ShowNonRewardAd();
        SceneManager.LoadScene(0);
    }
    public void SetHPSliderValue(float maxLeftTeamHP, float maxRightTeamHP)
    {
        _playerTeamHPSlider.minValue = 0;
        _playerTeamHPSlider.maxValue = maxLeftTeamHP;
        _playerTeamHPSlider.value = maxLeftTeamHP;
        _enemyTeamHPSlider.minValue = 0;
        _enemyTeamHPSlider.maxValue = maxRightTeamHP;
        _enemyTeamHPSlider.value = maxRightTeamHP;
    }
    public void UpdateTeamHP(int playerTeamAliveTanksCount, int enemyTeamAliveTanksCount, float playerTeamHP, float enemyTeamHP)
    {
        _tanksCountText.text = playerTeamAliveTanksCount + ":" + enemyTeamAliveTanksCount;
        _playerTeamHPSlider.value = playerTeamHP;
        _playerTeamHPText.text = playerTeamHP.ToString();
        _enemyTeamHPSlider.value = enemyTeamHP;
        _enemyTeamHPText.text = enemyTeamHP.ToString();
    }
    public void CreateTeamTanksInfo(List<Tank> playerTanks, List<Tank> enemyTanks)
    {
        _tankInfoInPlayerTeam = new List<TankInfoInTeamUI>();
        _tankInfoInEnemyTeam = new List<TankInfoInTeamUI>();
        for (int i = 0; i < playerTanks.Count; i++)
        {
            GameObject tankInfoObj = Instantiate(_playerTankInfoInTeamPrefub, _playerTeamInfoSpawn);
            TankInfoInTeamUI tankInfo = tankInfoObj.GetComponent<TankInfoInTeamUI>();
            _tankInfoInPlayerTeam.Add(tankInfo);

            tankInfo.SetTank(playerTanks[i]);
        }
        for (int i = 0; i < enemyTanks.Count; i++)
        {
            GameObject tankInfoObj = Instantiate(_enemyTankInfoInTeamPrefub, _enemyTeamInfoSpawn);
            TankInfoInTeamUI tankInfo = tankInfoObj.GetComponent<TankInfoInTeamUI>();
            _tankInfoInEnemyTeam.Add(tankInfo);
            tankInfo.SetTank(enemyTanks[i]);
        }
    }
    public void UpdateTankInfoInTeam(Tank tank, bool isPlayerTeam)
    {
        List<TankInfoInTeamUI> tankInfo;
        if (isPlayerTeam)
        {
            tankInfo = _tankInfoInPlayerTeam;
        }
        else
        {
            tankInfo = _tankInfoInEnemyTeam;
        }
        tankInfo.First(x => x.Tank == tank).SetTank(tank);
    }
    public void UpdateReloadSlider(float timeToReload, float maxReloadTimer)
    {
        _reloadSlider.maxValue = maxReloadTimer;
        float sliderVal = maxReloadTimer - timeToReload;
        _reloadSlider.value = sliderVal;
        if(sliderVal >= _reloadSlider.maxValue)
        {
            _reloadImage.color = _noReloadColor;
        }
        else
        {
            _reloadImage.color = _reloadColor;
        }
    }
    public void ShowKillMessage(string killerName, string killedName, bool isPlayerTeam)
    {
        KillChatMessageUI message = Instantiate(_killChatMessageUIObjectPrefub, _killChatMessageUISpawn).GetComponent<KillChatMessageUI>();
        message.ShowMessage(killerName, killedName, isPlayerTeam);
    }
    public void ShowEndGameUI(bool isPlayerWin, float money, float exp, float rating)
    {
        ShowCursor();
        if (isPlayerWin)
        {
            _endGameWinLoseText.text = "Вы победили";
        }
        else
        {
            _endGameWinLoseText.text = "Вы проиграли";
        }
        _moneyForBattleText.text = money.ToString();
        _expForBattleText.text = exp.ToString();
        _ratingForBattleText.text = rating.ToString();
        _endGameCanvas.SetActive(true);
    }
    public void UpdateEndGameUICurrencyForAds(float money, float exp)
    {
        _moneyForBattleText.text = money.ToString();
        _expForBattleText.text = exp.ToString();
        _adsBtn.SetActive(false);
    }
    public void SetPlayerHPSlider(float maxVal)
    {
        _playerTankHPSlider.maxValue = maxVal;
        UpdatePlayerHPSlider(maxVal);
    }
    public void UpdatePlayerHPSlider(float val)
    {
        _playerTankHPText.text = val.ToString();
        _playerTankHPSlider.value = val;
    }
    public void SetPlayerInfoPanelText(string val)
    {
        _panelInfoText.text = val;
    }
    public void HideUIOnDead()
    {
        _aimCanvas.SetActive(false);
        _itemsCanvas.SetActive(false);
        _zoomAimImg.SetActive(false);
    }
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ShowHitEffect()
    {
        _damageEffect.GetDamage();
    }
    public void ShowZoomImg()
    {
        _zoomAimImg.SetActive(true);
        _aimImg.SetActive(false);
    }
    public void HideZoomImg()
    {
        _zoomAimImg.SetActive(false);
        _aimImg.SetActive(true);
    }
}
