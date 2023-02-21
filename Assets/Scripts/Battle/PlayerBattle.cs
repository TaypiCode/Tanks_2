using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBattle : MonoBehaviour
{
    private Tank _tank;
    private PlayerCamera _playerCamera;
    private PlayerBattleUI _playerBattleUI;
    public Tank Tank { get => _tank;  }
    public static bool CanShoot;
    private void Start()
    {
        CanShoot = true;
        _tank = GetComponent<Tank>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _playerBattleUI = FindObjectOfType<PlayerBattleUI>();
        _playerCamera.SetCameraValues(_tank);
        _playerBattleUI.SetPlayerHPSlider(_tank.MaxHP);
        _playerBattleUI.SetPlayerInfoPanelText(_tank.TankScriptable.TankName);
        _tank.GetCannonModule().SetModuleUI(_playerBattleUI.CannonModuleUI);
        _tank.GetTowerModule().SetModuleUI(_playerBattleUI.TowerModuleUI);
        _tank.GetTruckModule().ForEach(x => x.SetModuleUI(_playerBattleUI.TruckModuleUI));
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && CanShoot)
        {
            Vector3 lookPos = _playerCamera.GetCameraRayPos();
            
            _tank.TryShot(lookPos);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerCamera.ZoomInOut();
            if (_tank.IsAlive)
            {
                if (_playerCamera.ZoomEnabled())
                {
                    _playerBattleUI.ShowZoomImg();
                }
                else
                {
                    _playerBattleUI.HideZoomImg();
                }
            }
        }
        float mouseWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelValue != 0)
        {
            _playerCamera.Scroll(mouseWheelValue);
        }
        _playerBattleUI.UpdateReloadSlider(_tank.GetNeedTimeToReload(), _tank.GetMaxReloadTime());
    }
    private void FixedUpdate()
    {
        float yRotation = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        if(zMove != 0)
        {
            _tank.Move(zMove);
        }
        else
        {
            _tank.StopMoving();
        }
        if(yRotation != 0)
        {
            if(zMove < 0)
            {
                yRotation *= -1;
            }
            _tank.Rotate(yRotation);
        }
    }    
}
