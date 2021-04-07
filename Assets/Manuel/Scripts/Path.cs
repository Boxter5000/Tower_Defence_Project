using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
	[SerializeField] private Vector2[] wayPoints;
	[SerializeField] private float fieldSize = 1f;
    [SerializeField] private GameObject PathStart;

    private void Awake()
    {
        Instantiate(PathStart, wayPoints[0] + (Vector2)transform.position, Quaternion.identity, transform);
    }

    public void OnDrawGizmosSelected() {
		for (int i = 0; i < wayPoints.Length; i++) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(wayPoints[i] + (Vector2)transform.position, fieldSize / 2f);
			if (i < wayPoints.Length - 1) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine(wayPoints[i] + (Vector2)transform.position, wayPoints[i + 1] + (Vector2)transform.position);
			}
		}
	}

	public Vector2 GetPoint(int index) {
		if (index >= wayPoints.Length) throw new System.Exception("Out of Array Index!");
		Circle c = new Circle(wayPoints[index] + (Vector2)transform.position, fieldSize);
		return c.GetRandomCoordinate();
	}

	public int GetWaypointAmount() {
		return wayPoints.Length;
	}
}
