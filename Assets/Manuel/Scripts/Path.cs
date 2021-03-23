using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
	public static Path instance;

	[SerializeField] private Vector2[] wayPoints;
	[SerializeField] private float fieldSize = 1f;

	public void Awake() {
		instance = this;
	}

	public void OnDrawGizmosSelected() {
		for (int i = 0; i < wayPoints.Length; i++) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(wayPoints[i], fieldSize / 2f);
			if (i < wayPoints.Length - 1) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
			}
		}
	}

	public Vector2 GetPoint(int index) {
		if (index >= wayPoints.Length) throw new System.Exception("Out of Array Index!");
		Circle c = new Circle(wayPoints[index], fieldSize);
		return c.GetRandomCoordinate();
	}

	public int GetWaypointAmount() {
		return wayPoints.Length;
	}
}
