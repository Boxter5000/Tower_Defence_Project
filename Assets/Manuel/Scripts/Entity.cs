using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	public static LinkedList<Entity> allEntities = new LinkedList<Entity>();

	[SerializeField]
	private float movementSpeed;        //how fast this entity moves throught the level
	[SerializeField]
	private AnimationCurve moveSpeedOverLocalProgress;

	private float progress;             //specifies how far this entity progressed in this level
	private int nextCheckpoint = -1;    //specifies the next checkpoint this entity goes to
	private Vector2 nextPosition;       //specifies the next position this entity goes to

	private float lerpSpeedMultiplier;
	private float lastCheckpointReachedTime;
	private Vector2 lastCheckpointPosition;

	private Path currentPath;

	protected void Awake() {
		allEntities.AddLast(this);
	}

	protected virtual void Start() {
		InitalizeNextCheckpoint();
	}

	// Update is called once per frame
	void Update() {

		float localProgress = (Time.time - lastCheckpointReachedTime) / lerpSpeedMultiplier;
		transform.position = moveSpeedOverLocalProgress.Evaluate(localProgress) * (nextPosition - lastCheckpointPosition) + lastCheckpointPosition;


		progress = (nextCheckpoint + localProgress) / currentPath.GetWaypointAmount();
		if (localProgress >= 1f) {
			InitalizeNextCheckpoint();
		}
	}

	private void InitalizeNextCheckpoint() {
		nextCheckpoint++;

		if (nextCheckpoint >= currentPath.GetWaypointAmount()) {
			//nextCheckpoint = -1;
			CompletedPath();
			return;
		}

		nextPosition = currentPath.GetPoint(nextCheckpoint);

		lerpSpeedMultiplier = ((Vector2)transform.position - nextPosition).magnitude / movementSpeed;
		lastCheckpointReachedTime = Time.time;
		lastCheckpointPosition = transform.position;
	}

	public void Kill() {
		KillImmediate();
	}

	public void KillImmediate() {
		Destroy(gameObject);
	}

	public void CompletedPath() {
        Debug.Log("-1 live");
		Kill();
	}
	public float GetProgress() {
		return progress;
	}

	public void OnDestroy() {
		allEntities.Remove(this);
	}

	public void SetPath(Path path) {
		currentPath = path;

	}
}
