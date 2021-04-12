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
    [SerializeField] private float TowerSize;
    [SerializeField] private LayerMask NotPlacable;

    private bool obstrakted;
    private GameObject TowerOnMouse;
    private Vector3 castPoint;
    private RectTransform rectTransform;
    private TowerBase towerBase;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector2 mouse = Input.mousePosition;
        castPoint = Camera.main.ScreenToWorldPoint(mouse);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TowerOnMouse = Instantiate(Tower, new Vector3(castPoint.x, castPoint.y, 0.5f), Quaternion.identity);
        towerBase = TowerOnMouse.GetComponent<TowerBase>();

        towerBase.ManageShooting(false);
        towerBase.ManageRangeVisuals(true);

        TowerOnMouse.GetComponent<Collider2D>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        TowerOnMouse.transform.position = castPoint - Vector3.forward * castPoint.z;

        Collider2D towerCollision = Physics2D.OverlapCircle(TowerOnMouse.transform.position, TowerSize, NotPlacable);

        if(towerCollision != null)
        {
            Debug.Log("Is Colliding");
            TowerOnMouse.transform.Find("sprite").GetComponent<SpriteRenderer>().color = Color.red;
            obstrakted = true;
        }
        else
        {
            Debug.Log("Placeble");
            TowerOnMouse.transform.Find("sprite").GetComponent<SpriteRenderer>().color = Color.white;
            obstrakted = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (obstrakted)
        {
            Destroy(towerBase.gameObject);
        }
        else
        {
            TowerOnMouse.GetComponent<Collider2D>().enabled = true;
            TowerBase towerBase = TowerOnMouse.GetComponent<TowerBase>();
            towerBase.ManageShooting(true);
            towerBase.ManageRangeVisuals(false);
        }
    }
}
