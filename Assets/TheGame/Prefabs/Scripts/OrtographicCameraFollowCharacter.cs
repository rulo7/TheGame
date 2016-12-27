using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrtographicCameraFollowCharacter : MonoBehaviour {
	// target GO to follow
	public GameObject target;
	// the limits of the scenario
	public Renderer limits;
	// scene camera
	public Camera camera;
	// distance to move the target for change the camera direction offset
	public float targetFreeDistance = 0.1f;
	// The camera speed
	public float speed;
	private float cameraWidth;
	private float cameraHeight;
	private float lastTargetX;
	private int lastDirectionIndex = 0;

	// Use this for initialization
	void Start () {
		cameraHeight = 2f * camera.orthographicSize;
		cameraWidth = cameraHeight * camera.aspect;
		lastTargetX = target.transform.position.x;
	}
			
	int directionTargetIndex;
	private float positionX;
	private float positionY;

	void FixedUpdate(){
		directionTargetIndex = updateTargetIndex();
		positionX = getCameraPositionX(directionTargetIndex);
		positionY = getCameraPositionY();
	}

	// Update is called once per frame
	void Update () {
		float distance = Vector2.Distance(new Vector2(camera.transform.position.x, camera.transform.position.y), new Vector2(target.transform.position.x, positionY));
		float step = (Time.deltaTime * distance) * speed;
		Vector3 newPosition = new Vector3 (target.transform.position.x, positionY, camera.transform.position.z);
		if (positionX != -1){
			newPosition = new Vector3 (positionX, positionY, camera.transform.position.z);	
		}
		camera.transform.position = Vector3.MoveTowards(camera.transform.position, newPosition, step);
	}

	private int updateTargetIndex ()
	{
		if (target.transform.position.x > (lastTargetX + targetFreeDistance)) {
			lastDirectionIndex = 1;
			lastTargetX = target.transform.position.x;
		} else if (target.transform.position.x < (lastTargetX +-targetFreeDistance)) {
			lastDirectionIndex = -1;
			lastTargetX = target.transform.position.x;
		}
		return lastDirectionIndex;
	}
		

	private float getCameraPositionX(int directionTargetIndex){
		BoxCollider2D box = target.GetComponent<BoxCollider2D>();
		float targetColliderHalfWidth = box != null ? box.size.x / 2.0f : 0.0f;

		float rightLimit = limits.bounds.center.x + (limits.bounds.size.x / 2.0f);
		float leftLimit = limits.bounds.center.x - (limits.bounds.size.x / 2.0f);

		float rightPointLimit = rightLimit - (cameraWidth / 2.0f);
		float leftPointLimit = leftLimit + (cameraWidth / 2.0f);
		float targetPosition = target.transform.position.x;
		if (targetPosition - targetColliderHalfWidth < leftPointLimit) {
			return leftPointLimit;
		}
		if (targetPosition + targetColliderHalfWidth > rightPointLimit) {
			return rightPointLimit;
		}
		return -1;
	}


	private float getCameraPositionY(){
		BoxCollider2D box = target.GetComponent<BoxCollider2D>();
		float targetColliderHalfHeight = box != null ? box.size.y / 2.0f : 0.0f;

		float topLimit = limits.bounds.center.y + (limits.bounds.size.y / 2.0f);
		float bottomLimit = limits.bounds.center.y - (limits.bounds.size.y / 2.0f);

		float topPointLimit = topLimit - (cameraHeight / 2.0f);
		float bottomPointLimit = bottomLimit + (cameraHeight / 2.0f);

		float targetPosition = target.transform.position.y;

		if (targetPosition - targetColliderHalfHeight <= (bottomPointLimit - 0.01)) {
			return bottomPointLimit;
		}
		if (targetPosition + targetColliderHalfHeight >= (topPointLimit +  0.01)) {
			return topPointLimit;
		}
		return target.transform.position.y;
	}
}
