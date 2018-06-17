using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGrid : MonoBehaviour {

	//arrays para montar a MeshGrid
	public List <int> tipoBloco = new List <int>();
	public List <int> materialBloco = new List <int>();
	public List <int> estadoBloco = new List <int>();

	public List <int> tipoBackgroundBloco = new List <int>();
	public List <int> materialBackgroundBloco = new List <int>();
	public List <int> estadoBackgroundBloco = new List <int>();

	//tamanho da MeshGrid;
	public int gridSizeX;
	public int gridSizeY;

	//mainCamera
	public Camera myCam;

	//autorizaçao para so chunks começar a se preencher de dados das listas principais
	public static bool podePreencher;
	

	void Awake(){
	}
	void Start () {

	}
	void Update(){
		//CRIAR MESHGRID
		if(Input.GetKeyDown(KeyCode.Space) && !podePreencher){
			makeDiscreteGrid();

			podePreencher = true;
		}

	}

	void makeDiscreteGrid () {
		//Set trackers integer 
		int v = 0;
		int t = 0;

		int material = 0;
		int estado = 1;

		float xOrg;
		float yOrg = 100f;


		//Create vertex grid
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				material = Random.Range(0,3);
				//tipo do bloco
				tipoBloco.Add(2);
				tipoBloco.Add(2);
				tipoBloco.Add(2);
				tipoBloco.Add(2);
				tipoBloco.Add(2);
				tipoBloco.Add(2);

				tipoBackgroundBloco.Add(2);
				tipoBackgroundBloco.Add(2);
				tipoBackgroundBloco.Add(2);
				tipoBackgroundBloco.Add(2);
				tipoBackgroundBloco.Add(2);
				tipoBackgroundBloco.Add(2);

				//material do bloco
				materialBloco.Add(material);
				materialBloco.Add(material);
				materialBloco.Add(material);
				materialBloco.Add(material);
				materialBloco.Add(material);
				materialBloco.Add(material);

				materialBackgroundBloco.Add(material);
				materialBackgroundBloco.Add(material);
				materialBackgroundBloco.Add(material);
				materialBackgroundBloco.Add(material);
				materialBackgroundBloco.Add(material);
				materialBackgroundBloco.Add(material);

				//estado do bloco
				estadoBloco.Add(estado);
				estadoBloco.Add(estado);
				estadoBloco.Add(estado);
				estadoBloco.Add(estado);
				estadoBloco.Add(estado);
				estadoBloco.Add(estado);

				estadoBackgroundBloco.Add(estado);
				estadoBackgroundBloco.Add(estado);
				estadoBackgroundBloco.Add(estado);
				estadoBackgroundBloco.Add(estado);
				estadoBackgroundBloco.Add(estado);
				estadoBackgroundBloco.Add(estado);
				
				v += 4;
				t += 6;

			}
		}
	}
}
