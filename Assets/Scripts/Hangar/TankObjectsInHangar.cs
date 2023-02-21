using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankObjectsInHangar : MonoBehaviour
{
    [SerializeField] private AllTanksScript _allTanksScript;
    [SerializeField] private float _rotationSpeed;
    private List<TankScriptableObject> _allTanksScriptable = new List<TankScriptableObject>();
    private List<GameObject> _allTanksObj = new List<GameObject>();
    private Transform _transform;
    private void Start()
    {
        _transform = transform;
        _allTanksScriptable = _allTanksScript.AllTanks;
        CreateTanks();
    }
    private void FixedUpdate()
    {
        RotateY();
    }
    private void CreateTanks()
    {
        for(int i = 0; i < _allTanksScriptable.Count; i++)
        {
            GameObject tank = Instantiate(_allTanksScriptable[i].TankPrefub, _transform);
            Destroy(tank.GetComponent<Rigidbody>());
            tank.SetActive(false);
            _allTanksObj.Add(tank);
        }
    }
    private void RotateY()
    {
        _transform.Rotate(0, _rotationSpeed, 0);
    }
    public void SelectTank(TankScriptableObject tank)
    {
        int index = _allTanksScriptable.IndexOf(tank);
        _allTanksObj.ForEach(x => x.SetActive(false));
        _allTanksObj[index].SetActive(true);
    }
}
