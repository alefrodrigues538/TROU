using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneManager : MonoBehaviour {
	proceduralGrid pGrid;
	public float moveSpeed = 10f;
	public static Vector2 playerPosition;

	Vector3 spawnPoint;

	void Awake(){
		pGrid = (proceduralGrid)FindObjectOfType(typeof(proceduralGrid));
		spawnPoint = new Vector3(pGrid.gridSizeX/2,pGrid.gridSizeY,0f);
	}

	void Start(){
		transform.position = spawnPoint;
	}
	// Update is called once per frame
	void FixedUpdate () {
		playerPosition = transform.position;
		transform.Translate(Input.GetAxis("Horizontal")* moveSpeed*Time.fixedDeltaTime,
							Input.GetAxis("Vertical")* moveSpeed*Time.fixedDeltaTime,
							0);
		
	}
}
