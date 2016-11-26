using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrtographicCameraFollowCharacter : MonoBehaviour {
	// target GO to follow
	public GameObject target;
	// left limit for the scene camera
	public GameObject leftLimit;
	// right limit for the scene camera
	public GameObject rightLimit;
	// main camera of the scene
	public Camera camera;
	// speed camera follows the target
	public float speed;
	// offset distance between center camera and target
	public float offset;
	// distance to move the target for change the camera direction offset
	public float targetFreeDistance = 0.1f;

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
			
	// Update is called once per frame
	void Update () {
		int directionTargetIndex = updateTargetIndex();
		float position = getCameraPositionX(directionTargetIndex);
		float step = speed * Time.deltaTime;
		Vector3 newPosition = new Vector3 (target.transform.position.x + (directionTargetIndex * offset), camera.transform.position.y, camera.transform.position.z);
		if (position != -1){
			newPosition = new Vector3 (position, camera.transform.position.y, camera.transform.position.z);	
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
		float rightPointLimit = rightLimit.transform.position.x - (cameraWidth / 2.0f);
		float leftPointLimit = leftLimit.transform.position.x + (cameraWidth / 2.0f);
		float targetPosition = target.transform.position.x;
		if (targetPosition - targetColliderHalfWidth < (leftPointLimit - (offset *directionTargetIndex))) {
			return leftPointLimit;
		}
		if (targetPosition + targetColliderHalfWidth > rightPointLimit - (offset * directionTargetIndex)) {
			return rightPointLimit;
		}
		return -1;
	}
}
