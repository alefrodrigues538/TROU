using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGrid : MonoBehaviour {

	public List <GameObject> chunksObjects = new List <GameObject>();

	//arrays para montar a MeshGrid
	public List <int> tipoBloco = new List <int>();
	public List <int> materialBloco = new List <int>();
	public List <int> estadoBloco = new List <int>();
	public List <int> direcaoBloco = new List <int>();

	public List <int> tipoBackgroundBloco = new List <int>();
	public List <int> materialBackgroundBloco = new List <int>();
	public List <int> estadoBackgroundBloco = new List <int>();

	//tamanho da MeshGrid;
	public int gridSizeX;
	public int gridSizeY;

	//autorizaçao para so chunks começar a se preencher de dados das listas principais
	public static bool podePreencher;

	//lista de blocos em volta
	List <int> blocosEmVolta = new List <int>();
	

	void Awake(){

	}
	void Start () {

	}
	void Update(){
		//CRIAR MESHGRID
		if(Input.GetKeyDown(KeyCode.Space) && !podePreencher){
			makeDiscreteGrid();
			print("podePreencher");
			
		}
		if(Input.GetKeyDown(KeyCode.Alpha1) && !podePreencher){
			direcaoBloco.Clear();
			atualizarDirBloco();
		}

		if(Input.GetKeyDown(KeyCode.Alpha4)){
			Vector2 p = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y));
			
			int posX =  Mathf.FloorToInt(p.x);
			int posY = Mathf.FloorToInt(p.y);
			printValueDirBloco(posX,posY);

			int indexBloco = (posY + (posX * gridSizeY)) * 4;
			print(direcaoBloco[indexBloco]);
		}
		

	}

	void printValueDirBloco(int x,int y){
		//formulas para descobrir blocos em volta
				int cimaEsquerda = (y + (x * gridSizeY) - (gridSizeY - 1))*4;
				int cima = (y + (x * gridSizeY) + 1) * 4;
				int cimaDireita = (y + (x * gridSizeY) + (gridSizeY + 1)) * 4;
				int meioEsquerda = (y + (x * gridSizeY) - gridSizeY) * 4;
				int meio = (y + (x * gridSizeY)) * 4;
				int meioDireita = (y + (x * gridSizeY) + gridSizeY) * 4;
				int baixoEsquerda = (y + (x * gridSizeY) - (gridSizeY + 1)) * 4;
				int baixo = (y + (x * gridSizeY) - 1) * 4;
				int baixoDireita = (y + (x * gridSizeY) + (gridSizeY - 1)) * 4;

				//RESETANDO OS BLOCOS QUE ESTIVEREM EM POSIÇOES NEGATIVAS E/OU SUA POSIÇAO FOR MENOR QUE O VETOR
				if(cimaEsquerda < 0){
					cimaEsquerda = 0;
					estadoBloco[cimaEsquerda] = 0;
				}
				if(cima < 0){
					cima = 0;
					estadoBloco[cima] = 0;
				}
				if(cimaDireita < 0){
					cimaDireita = 0;
					estadoBloco[cimaDireita] = 0;
				}
				if(meioEsquerda < 0){
					meioEsquerda = 0;
					estadoBloco[meioEsquerda] = 0;
				}
				if(meioDireita < 0){
					meioDireita = 0;
					estadoBloco[meioDireita] = 0;
				}
				if(baixoEsquerda < 0){
					baixoEsquerda = 0;
					estadoBloco[baixoEsquerda] = 0;
				}
				if(baixo < 0){
					baixo = 0;
					estadoBloco[baixo] = 0;
				}
				if(baixoDireita < 0){
					baixoDireita = 0;
					estadoBloco[baixoDireita] = 0;
				}

					blocosEmVolta.Clear();


				//PREENCHENDO BLOCOS EM VOLTA
					blocosEmVolta.Add(materialBloco[cimaEsquerda]);
					blocosEmVolta.Add(materialBloco[cima]);
					blocosEmVolta.Add(materialBloco[cimaDireita]);
					blocosEmVolta.Add(materialBloco[meioEsquerda]);
					blocosEmVolta.Add(materialBloco[meio]);
					blocosEmVolta.Add(materialBloco[meioDireita]);
					blocosEmVolta.Add(materialBloco[baixoEsquerda]);
					blocosEmVolta.Add(materialBloco[baixo]);
					blocosEmVolta.Add(materialBloco[baixoDireita]);

				//DEFININDO VALORES DAS DIREÇOES DA LISTA BLOCOS EM VOLTA
				int dirBloco = 0;

				if (blocosEmVolta [0] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 8;
				}

				if (blocosEmVolta [1] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 16;
				}

				if (blocosEmVolta [2] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 32;
				}

				if (blocosEmVolta [3] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 64;
				}

				if (blocosEmVolta [4] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 128;
				}

				if (blocosEmVolta [5] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 256;
				}

				if (blocosEmVolta [6] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 512;
				}

				if (blocosEmVolta [7] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 1024;
				}

				if (blocosEmVolta [8] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
					dirBloco += 2048;
				}

				print(dirBloco);

	}
	void makeDiscreteGrid () {

		//Set trackers integer 
		int v = 0;
		int t = 0;

		int material = 0;
		int estado = 1;

		//Create vertex grid
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				material = Random.Range(0,3);
				//print(material);

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

	public void atualizarDirBloco(){
		
		for (int x = 0; x < gridSizeX; x++){
			for (int y = 0; y < gridSizeY; y++){
				setValueDirBloco(x,y);
			}
		}
		print(direcaoBloco.Count);
		podePreencher = true;
	}
	
	void setValueDirBloco(int x,int y){

		//formulas para descobrir blocos em volta
		int cimaEsquerda = (y + (x * gridSizeY) - (gridSizeY - 1))*4;
		int cima = (y + (x * gridSizeY) + 1) * 4;
		int cimaDireita = (y + (x * gridSizeY) + (gridSizeY + 1)) * 4;
		int meioEsquerda = (y + (x * gridSizeY) - gridSizeY) * 4;
		int meio = (y + (x * gridSizeY)) * 4;
		int meioDireita = (y + (x * gridSizeY) + gridSizeY) * 4;
		int baixoEsquerda = (y + (x * gridSizeY) - (gridSizeY + 1)) * 4;
		int baixo = (y + (x * gridSizeY) - 1) * 4;
		int baixoDireita = (y + (x * gridSizeY) + (gridSizeY - 1)) * 4;

		//RESETANDO OS BLOCOS QUE ESTIVEREM EM POSIÇOES NEGATIVAS E/OU SUA POSIÇAO FOR MENOR QUE O VETOR
		if(cimaEsquerda < 0){
			cimaEsquerda = 0;
			estadoBloco[cimaEsquerda] = 0;
		}
		if(cima < 0){
			cima = 0;
			estadoBloco[cima] = 0;
		}
		if(cimaDireita < 0){
			cimaDireita = 0;
			estadoBloco[cimaDireita] = 0;
		}
		if(meioEsquerda < 0){
			meioEsquerda = 0;
			estadoBloco[meioEsquerda] = 0;
		}
		if(meioDireita < 0){
			meioDireita = 0;
			estadoBloco[meioDireita] = 0;
		}
		if(baixoEsquerda < 0){
			baixoEsquerda = 0;
			estadoBloco[baixoEsquerda] = 0;
		}
		if(baixo < 0){
			baixo = 0;
			estadoBloco[baixo] = 0;
		}
		if(baixoDireita < 0){
			baixoDireita = 0;
			estadoBloco[baixoDireita] = 0;
		}

		blocosEmVolta.Clear();


		//PREENCHENDO BLOCOS EM VOLTA
		blocosEmVolta.Add(materialBloco[cimaEsquerda]);
		blocosEmVolta.Add(materialBloco[cima]);
		blocosEmVolta.Add(materialBloco[cimaDireita]);
		blocosEmVolta.Add(materialBloco[meioEsquerda]);
		blocosEmVolta.Add(materialBloco[meio]);
		blocosEmVolta.Add(materialBloco[meioDireita]);
		blocosEmVolta.Add(materialBloco[baixoEsquerda]);
		blocosEmVolta.Add(materialBloco[baixo]);
		blocosEmVolta.Add(materialBloco[baixoDireita]);

		//DEFININDO VALORES DAS DIREÇOES DA LISTA BLOCOS EM VOLTA

		int dirBloco = 0;

		if (blocosEmVolta [0] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 8;
		}

		if (blocosEmVolta [1] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 16;
		}

		if (blocosEmVolta [2] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 32;
		}

		if (blocosEmVolta [3] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 64;
		}

		if (blocosEmVolta [4] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 128;
		}

		if (blocosEmVolta [5] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 256;
		}

		if (blocosEmVolta [6] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 512;
		}

		if (blocosEmVolta [7] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 1024;
		}

		if (blocosEmVolta [8] != blocosEmVolta [4] && estadoBloco[meio] != 0) {
			dirBloco += 2048;
		}

		direcaoBloco.Add(dirBloco);
		direcaoBloco.Add(dirBloco);
		direcaoBloco.Add(dirBloco);
		direcaoBloco.Add(dirBloco);
		dirBloco = 0;
	}
}
