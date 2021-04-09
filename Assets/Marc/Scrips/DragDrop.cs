using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject Tower;
    [SerializeField] private GameObject Button;
    [SerializeField] public GameObject Collider;

    private Vector3 castPoint;
    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector3 mouse = Input.mousePosition;
        castPoint = Camera.main.ScreenToWorldPoint(mouse);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Begin Click");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        GameObject tower = Instantiate(Tower, new Vector3(castPoint.x, castPoint.y, 0.5f), Quaternion.identity);
        tower.GetComponent<TowerBase>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    public bool canNotPlace(bool Collidet)
    {
        if (Collidet)
        {
            image.GetComponent<Image>().color = new Color32(255, 0, 0, 200);
        }
        else
        {
            image.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        return Collidet;
    }
}
