using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrtographicCameraFollowCharacter : MonoBehaviour {

	public GameObject target;
	public GameObject leftLimit;
	public GameObject rightLimit;
	public Camera camera;

	private float width;

	// Use this for initialization
	void Start () {
		float height = 2f * camera.orthographicSize;
		width = height * camera.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		float position = getCameraPositionX();
		if (position == -1) {
			camera.transform.position = new Vector3 (target.transform.position.x, camera.transform.position.y, camera.transform.position.z);
		} else {
			camera.transform.position = new Vector3 (position, camera.transform.position.y, camera.transform.position.z);
		}
	}
		

	private float getCameraPositionX(){
		BoxCollider2D box = target.GetComponent<BoxCollider2D>();
		float targetColliderHalfWidth = box != null ? box.size.x / 2.0f : 0.0f;
		float rightPointLimit = rightLimit.transform.position.x - (width / 2.0f);
		float leftPointLimit = leftLimit.transform.position.x + (width / 2.0f);
		float targetPosition = target.transform.position.x;
		if (targetPosition - targetColliderHalfWidth < leftPointLimit) {
			return leftPointLimit;
		}
		if (targetPosition + targetColliderHalfWidth > rightPointLimit) {
			return rightPointLimit;
		}
		return -1;
	}
}
