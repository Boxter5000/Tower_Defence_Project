using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInterface : MonoBehaviour
{
    private TowerBase _towerBase;
    
    private void ShowDiferentUI()
    {
        if (_towerBase.aimType == TowerBase.Aim.PlayerTarget)
        {
            
        }
        if (_towerBase.aimType == TowerBase.Aim.InRange)
        {
            
        }
        if (_towerBase.aimType == TowerBase.Aim.AutoTarget)
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
        gameObject.SetActive(false);
        Destroy(_towerBase.gameObject);
        _towerBase = null;
    }

    public void DisableInterface()
    {
        gameObject.SetActive(false);
    }
}
