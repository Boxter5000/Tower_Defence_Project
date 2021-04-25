using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnMouseDown()
    {
        transform.parent.GetComponent<TowerBase>().SetPlayerPosition(transform.position);
        Destroy(gameObject);
    }  
    
}
