using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInterface : MonoBehaviour
{
    private TowerBase _towerBase;
    
    private void ShowDiferentUI()
    {
        if (_towerBase.AimType == TowerBase.Aim.Player_Target)
        {
            
        }
        if (_towerBase.AimType == TowerBase.Aim.In_Range)
        {
            
        }
        if (_towerBase.AimType == TowerBase.Aim.Auto_Target)
        {
            
        }
    }

    public void NewPlayerTarget()
    {
        _towerBase.GetPlacePos();
    }
    public void GetReferenceTower(TowerBase tower)
    {
        _towerBase = tower;
    }

    public void SellTower()
    {
        Destroy(_towerBase.gameObject);
    }
}
