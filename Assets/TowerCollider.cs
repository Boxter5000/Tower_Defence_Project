using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    public bool isColliding = false;
    DragDrop dragDrop;
    private Vector3 castPoint;

    private void Awake()
    {
        dragDrop = transform.parent.GetComponent<DragDrop>();
    }

    private void Update()
    {
        Vector3 mouse = Input.mousePosition;
        castPoint = Camera.main.ScreenToWorldPoint(mouse);

        transform.position = castPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(isColliding);
        isColliding = true;
        dragDrop.canNotPlace(isColliding);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(isColliding);
        isColliding = false;
        dragDrop.canNotPlace(isColliding);
    }
}
