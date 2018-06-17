using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneManager : MonoBehaviour {
	public float moveSpeed = 10f;
	public static Vector2 playerPosition;

	// Update is called once per frame
	void FixedUpdate () {
		playerPosition = transform.position;
		transform.Translate(Input.GetAxis("Horizontal")* moveSpeed*Time.fixedDeltaTime,
							Input.GetAxis("Vertical")* moveSpeed*Time.fixedDeltaTime,
							0);
		
	}
}
