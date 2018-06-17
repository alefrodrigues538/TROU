using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkControl : MonoBehaviour {

	//procedural grid lugar aonde fica todas as informaçoes do mundo criado
	proceduralGrid pGrid;

	//Blocos de fundo
	public GameObject background;

	//propriedades da MeshGrid do Chunk
	public Mesh mesh;
	public Mesh backgroundMesh;

	//chunk grid
	public List<Vector2> chunkGrid = new List<Vector2>();

	//listas para montar o chunk
	public List<Vector2> chunkUVGrid = new List<Vector2>();
	public List<Vector3> chunkVerticesGrid = new List<Vector3>();
	public List<int> chunkTrianglesGrid = new List<int>();
	public List<int> chunkEstadoBloco = new List<int>();
	public List<int> chunkTipoBloco = new List<int>();
	public List<int> chunkMaterialBloco = new List<int>();

	//listas para montar o background do chunk
	public List<Vector2> chunkUVBackgroundGrid = new List<Vector2>();
	public List<Vector3> chunkVerticesBackgroundGrid = new List<Vector3>();
	public List<int> chunkTrianglesBackgroundGrid = new List<int>();
	public List<int> chunkEstadoBackgroundBloco = new List<int>();
	public List<int> chunkTipoBackgroundBloco = new List<int>();
	public List<int> chunkMaterialBackgroundBloco = new List<int>();

	public List<int> blocosEmVolta = new List<int>();

	public int chunkX;
	public int chunkY;

	public bool chunkAtualizado = false;
	public bool chunkTexturaAtualizado = false;

	int i = 0;

	void Start(){

	}
	void Awake(){
		mesh = GetComponent<MeshFilter>().mesh;
		backgroundMesh = background.GetComponent<MeshFilter>().mesh;
		pGrid = (proceduralGrid)FindObjectOfType(typeof(proceduralGrid));

	}

	void Update() {
		
		if(proceduralGrid.podePreencher && chunkAtualizado == false){
			chunkGrid.Clear ();
			for (int x = 0; x <= 99; x++) {
				for (int y = 0; y <= 99; y++) {
					chunkGrid.Add (new Vector2(x,y));
				}
			}
			preencherChunk();
			preencherBackgroundChunk();
			chunkAtualizado = true;

			atualizarChunk ();

			i = 0;
		}

		if(Vector2.Distance(new Vector2(chunkX,chunkY),sceneManager.playerPosition) <= 200.0f){
			mostrarMesh();
		}else if(Vector2.Distance(new Vector2(chunkX,chunkY),sceneManager.playerPosition) > 200.0f){
			esconderMesh();
		}


		if(Input.GetKey(KeyCode.Mouse0)){
			Vector2 p = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y));
			
			int posX =  Mathf.FloorToInt(p.x);
			int posY = Mathf.FloorToInt(p.y);

			if(p.y > 99){
				posX =  Mathf.FloorToInt(p.x - 1f);
			}
			int chunkBlocoAtual = (posY + (posX * 100))*4;
			int estBloco = chunkEstadoBloco[chunkBlocoAtual];

			if((posX >= chunkX && posX <= chunkX + 99) && (posY >= chunkY && posY <= chunkY + 99) && estBloco == 1){
				//deletarBloco(posX,posY);
				definirTexturaBloco(posX,posY);
			}
		}

		if(Input.GetKey(KeyCode.Mouse1)){
			Vector2 p = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y));
			
			int posX =  Mathf.FloorToInt(p.x);
			int posY = Mathf.FloorToInt(p.y);

			if(p.y > 99){
				posX =  Mathf.FloorToInt(p.x - 1f);
			}
			if((posX >= chunkX && posX <= chunkX + 99) && (posY >= chunkY && posY <= chunkY + 99)){
				deletarBloco(posX,posY);
				definirTexturaBloco(p.x,p.y);
			}
		}

		if(Input.GetKeyDown(KeyCode.B)){
			atualizarChunk ();
		}
	}

	void preencherChunk(){
		//resetando valores das listas do chunk
		chunkVerticesGrid.Clear();
		chunkTrianglesGrid.Clear();
		chunkUVGrid.Clear();
		chunkEstadoBloco.Clear();
		chunkTipoBloco.Clear();
		//tracker value
		int blocoAtual = 0;

		int t = 0;


		//total de blocos na HORIZONTAL
		float totalBlocosHorizontal = 17;

		//total de blocos na VERTICAL
		float totalBlocosVertical = 3;

		//posiçoes da UV;
		float uvPosX = (1 / totalBlocosHorizontal);
		float uvPosY = (1 / totalBlocosVertical);


		for( int x = chunkX; x < chunkX + 100; x++){
			for( int y = chunkY; y < chunkY + 100; y++){
				blocoAtual = (y + (x * pGrid.gridSizeY))*4;

				//preenchendo vertices do chunk
					chunkVerticesGrid.Add(new Vector3 (x,y,0));
					chunkVerticesGrid.Add(new Vector3 (x,y+1,0));
					chunkVerticesGrid.Add(new Vector3 (x+1,y,0));
					chunkVerticesGrid.Add(new Vector3 (x+1,y+1,0)); 

				//preenchendo estado dos blocos do chunk
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual]);
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual+1]);
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual+2]);
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual+3]);
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual+4]);
					chunkEstadoBloco.Add(pGrid.estadoBloco[blocoAtual+5]);

				//preenchendo tipo dos blocos do chunk
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual]);
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual+1]);
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual+2]);
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual+3]);
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual+4]);
					chunkTipoBloco.Add(pGrid.tipoBloco[blocoAtual+5]);

				//preenchendo material dos blocos do chunk
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual]);
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual+1]);
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual+2]);
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual+3]);
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual+4]);
					chunkMaterialBloco.Add(pGrid.materialBloco[blocoAtual+5]);

				if(pGrid.estadoBloco[blocoAtual] == 1){

					//preenchendo triangulos do chunk
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t+1);
						chunkTrianglesGrid.Add(t+2);
						chunkTrianglesGrid.Add(t+2);
						chunkTrianglesGrid.Add(t+1);
						chunkTrianglesGrid.Add(t+3);

				}else if(pGrid.estadoBloco[blocoAtual] == 0){

					//preenchendo triangulos do chunk
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t);
						chunkTrianglesGrid.Add(t);
				}

				//populando Uvs do chunk
					
					if(pGrid.materialBloco[blocoAtual] == 0){
						uvPosY = 0;
					}
					else if(pGrid.materialBloco[blocoAtual] == 1){
						uvPosY = (1 / totalBlocosVertical);
					}
					else if(pGrid.materialBloco[blocoAtual] == 2){
						uvPosY = (1 / totalBlocosVertical)*2;
					}

					chunkUVGrid.Add (new Vector2(uvPosX+(uvPosX*3),uvPosY));
					chunkUVGrid.Add (new Vector2(uvPosX+(uvPosX*3),uvPosY+(1 / totalBlocosVertical)));
					chunkUVGrid.Add (new Vector2(uvPosX+(uvPosX*4),uvPosY));
					chunkUVGrid.Add (new Vector2(uvPosX+(uvPosX*4),uvPosY+(1 / totalBlocosVertical)));
					
				t += 4;
			}
		}
	}

	void atualizarChunk(){
		for (int x = chunkX; x < chunkX + 100; x++) {
			for (int y = chunkY; y < chunkY + 100; y++) {
				definirTexturaBloco(x,y);
			}
		}
	}

	void preencherBackgroundChunk(){
		//resetando valores das listas do chunk
		chunkVerticesBackgroundGrid.Clear();
		chunkTrianglesBackgroundGrid.Clear();
		chunkUVBackgroundGrid.Clear();
		chunkEstadoBackgroundBloco.Clear();
		chunkTipoBackgroundBloco.Clear();
		//tracker value
		int blocoAtual = 0;

		int t = 0;


		//total de blocos na HORIZONTAL
		float totalBlocosHorizontal = 17;

		//total de blocos na VERTICAL
		float totalBlocosVertical = 3;

		//posiçoes da UV;
		float uvPosX = (1 / totalBlocosHorizontal);
		float uvPosY = (1 / totalBlocosVertical);


		for( int x = chunkX; x < chunkX + 100; x++){
			for( int y = chunkY; y < chunkY + 100; y++){
				blocoAtual = (y + (x * pGrid.gridSizeY))*4;

				//preenchendo vertices do chunk
					chunkVerticesBackgroundGrid.Add(new Vector3 (x,y,0));
					chunkVerticesBackgroundGrid.Add(new Vector3 (x,y+1,0));
					chunkVerticesBackgroundGrid.Add(new Vector3 (x+1,y,0));
					chunkVerticesBackgroundGrid.Add(new Vector3 (x+1,y+1,0)); 

				//preenchendo estado dos blocos do chunk
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual]);
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual+1]);
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual+2]);
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual+3]);
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual+4]);
					chunkEstadoBackgroundBloco.Add(pGrid.estadoBackgroundBloco[blocoAtual+5]);

				//preenchendo tipo dos blocos do chunk
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual]);
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual+1]);
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual+2]);
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual+3]);
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual+4]);
					chunkTipoBackgroundBloco.Add(pGrid.tipoBackgroundBloco[blocoAtual+5]);

				if(pGrid.estadoBackgroundBloco[blocoAtual] == 1){

					//preenchendo triangulos do chunk
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t+1);
						chunkTrianglesBackgroundGrid.Add(t+2);
						chunkTrianglesBackgroundGrid.Add(t+2);
						chunkTrianglesBackgroundGrid.Add(t+1);
						chunkTrianglesBackgroundGrid.Add(t+3);

				}else if(pGrid.estadoBackgroundBloco[blocoAtual] == 0){

					//preenchendo triangulos do chunk
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t);
						chunkTrianglesBackgroundGrid.Add(t);
				}

				//populando Uvs do chunk
					
					if(pGrid.materialBackgroundBloco[blocoAtual] == 0){
						uvPosY = 0;
					}
					else if(pGrid.materialBackgroundBloco[blocoAtual] == 1){
						uvPosY = (1 / totalBlocosVertical);
					}
					else if(pGrid.materialBackgroundBloco[blocoAtual] == 2){
						uvPosY = (1 / totalBlocosVertical)*2;
					}

					chunkUVBackgroundGrid.Add (new Vector2(uvPosX+(uvPosX*3),uvPosY));
					chunkUVBackgroundGrid.Add (new Vector2(uvPosX+(uvPosX*3),uvPosY+(1 / totalBlocosVertical)));
					chunkUVBackgroundGrid.Add (new Vector2(uvPosX+(uvPosX*4),uvPosY));
					chunkUVBackgroundGrid.Add (new Vector2(uvPosX+(uvPosX*4),uvPosY+(1 / totalBlocosVertical)));
					
				t += 4;
			}
		}


	}

	void deletarBloco(int posX, int posY){
		int blocoAtual = (posY + (posX * pGrid.gridSizeY))*4;

		if(posX > 99){
			posX -= chunkX;
		}

		int chunkBlocoAtual = (posY + (posX * 100))*4;

		pGrid.estadoBloco[blocoAtual] = 0;
		pGrid.estadoBloco[blocoAtual+1] = 0;
		pGrid.estadoBloco[blocoAtual+2] = 0;
		pGrid.estadoBloco[blocoAtual+3] = 0;

		chunkEstadoBloco[chunkBlocoAtual] = 0;
		chunkEstadoBloco[chunkBlocoAtual+1] = 0;
		chunkEstadoBloco[chunkBlocoAtual+2] = 0;
		chunkEstadoBloco[chunkBlocoAtual+3] = 0;

		chunkMaterialBloco [chunkBlocoAtual] = -1;
		chunkMaterialBloco [chunkBlocoAtual+1] = -1;
		chunkMaterialBloco [chunkBlocoAtual+2] = -1;
		chunkMaterialBloco [chunkBlocoAtual+3] = -1;

		chunkUVGrid[chunkBlocoAtual] = new Vector2(0.94117648f,0f);
		chunkUVGrid[chunkBlocoAtual+1] = new Vector2(0.94117648f,0.33333333f);
		chunkUVGrid[chunkBlocoAtual+2] = new Vector2(1.00000001f,0f);
		chunkUVGrid[chunkBlocoAtual+3] = new Vector2(1.00000001f,0.33333333f);

		//print(chunkEstadoBloco[chunkBlocoAtual]);

		//updateMesh();

	}

	void deletarBackgroundBloco(int posX, int posY){
		int blocoAtual = (posY + (posX * pGrid.gridSizeY))*4;

		if(posX > 99){
			posX -= chunkX;
		}

		int chunkBlocoAtual = (posY + (posX * 100))*4;

		pGrid.estadoBackgroundBloco[blocoAtual] = 0;
		pGrid.estadoBackgroundBloco[blocoAtual+1] = 0;
		pGrid.estadoBackgroundBloco[blocoAtual+2] = 0;
		pGrid.estadoBackgroundBloco[blocoAtual+3] = 0;

		chunkEstadoBackgroundBloco[chunkBlocoAtual] = 0;
		chunkEstadoBackgroundBloco[chunkBlocoAtual+1] = 0;
		chunkEstadoBackgroundBloco[chunkBlocoAtual+2] = 0;
		chunkEstadoBackgroundBloco[chunkBlocoAtual+3] = 0;

		chunkUVBackgroundGrid[chunkBlocoAtual] = new Vector2(0.94117648f,0f);
		chunkUVBackgroundGrid[chunkBlocoAtual+1] = new Vector2(0.94117648f,0.33333333f);
		chunkUVBackgroundGrid[chunkBlocoAtual+2] = new Vector2(1.00000001f,0f);
		chunkUVBackgroundGrid[chunkBlocoAtual+3] = new Vector2(1.00000001f,0.33333333f);

		//print(chunkEstadoBloco[chunkBlocoAtual]);

		//updateMesh();

	}

	void criarBloco(int posX, int posY){

		int blocoAtual = (posY + (posX * pGrid.gridSizeY))*4;

		if(posX > 99){
			posX -= chunkX;
		}

		int chunkBlocoAtual = (posY + (posX * 100))*4;

		pGrid.estadoBloco[blocoAtual] = 1;
		pGrid.estadoBloco[blocoAtual+1] = 1;
		pGrid.estadoBloco[blocoAtual+2] = 1;
		pGrid.estadoBloco[blocoAtual+3] = 1;

		chunkEstadoBloco[chunkBlocoAtual] = 1;
		chunkEstadoBloco[chunkBlocoAtual+1] = 1;
		chunkEstadoBloco[chunkBlocoAtual+2] = 1;
		chunkEstadoBloco[chunkBlocoAtual+3] = 1;

		definirTexturaMeshGrid (posX,posY);

		//print(chunkEstadoBloco[chunkBlocoAtual]);

		//updateMesh();
	}

	public void updateMesh(){
		mesh.Clear();
		mesh.vertices = chunkVerticesGrid.ToArray();
		mesh.triangles = chunkTrianglesGrid.ToArray();
		mesh.uv = chunkUVGrid.ToArray();
		mesh.RecalculateNormals();

		backgroundMesh.Clear();
		backgroundMesh.vertices = chunkVerticesBackgroundGrid.ToArray();
		backgroundMesh.triangles = chunkTrianglesBackgroundGrid.ToArray();
		backgroundMesh.uv = chunkUVBackgroundGrid.ToArray();
		backgroundMesh.RecalculateNormals();
	}

	void mostrarMesh(){
		mesh.Clear();
		mesh.vertices = chunkVerticesGrid.ToArray();
		mesh.triangles = chunkTrianglesGrid.ToArray();
		mesh.uv = chunkUVGrid.ToArray();
		mesh.RecalculateNormals();
	}

	void esconderMesh(){
		mesh.Clear();
	}

	void setUvBlock (int idDirecaoBloco, int indexBloco) {



		//total de blocos na HORIZONTAL
		float totalBlocosHorizontal = 17;

		//total de blocos na VERTICAL
		float totalBlocosVertical = 3;

		//posiçoes da UV;
		float posX = (1 / totalBlocosHorizontal);
		float posY = (1 / totalBlocosVertical);

		if(chunkMaterialBloco[indexBloco] == 0){
			posY = 0;
		}
		else if(chunkMaterialBloco[indexBloco] == 1){
			posY = (1 / totalBlocosVertical);
		}
		else if(chunkMaterialBloco[indexBloco] == 2){
			posY = (1 / totalBlocosVertical)*2;
		}

		switch (idDirecaoBloco)
		{


			case 1:
				//cima esquerda
			chunkUVGrid [indexBloco] = new Vector2(posX-posX,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX-posX,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX,posY+(1 / totalBlocosVertical));
				break;
			case 2:
				//cima
			chunkUVGrid [indexBloco] = new Vector2(posX,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*2,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*2,posY+(1 / totalBlocosVertical));
				break;
			case 3:
				//cima direita
			chunkUVGrid [indexBloco] = new Vector2(posX*2,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*2,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*3,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*3,posY+(1 / totalBlocosVertical));
				break;

			case 4:
				//meio esquerda
			chunkUVGrid [indexBloco] = new Vector2(posX*3,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*3,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*4,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*4,posY+(1 / totalBlocosVertical));
				break;
			case 5:
				//meio
			chunkUVGrid [indexBloco] = new Vector2(posX+(posX*3),posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX+(posX*3),posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX+(posX*4),posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX+(posX*4),posY+(1 / totalBlocosVertical));
				break;
			case 6:
				//meio direita
			chunkUVGrid [indexBloco] = new Vector2(posX*5,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*5,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*6,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*6,posY+(1 / totalBlocosVertical));
				break;
			case 7:
				//baixo esquerda
			chunkUVGrid [indexBloco] = new Vector2(posX*6,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*6,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*7,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*7,posY+(1 / totalBlocosVertical));
				break;
			case 8:
				//baixo
			chunkUVGrid [indexBloco] = new Vector2(posX*7,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*7,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*8,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*8,posY+(1 / totalBlocosVertical));
				break;
			case 9:
				//baixo direita
			chunkUVGrid [indexBloco] = new Vector2(posX*8,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*8,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*9,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*9,posY+(1 / totalBlocosVertical));
				break;
			case 10:
				//bloco inteiro
			chunkUVGrid [indexBloco] = new Vector2(posX*9,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*9,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*10,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*10,posY+(1 / totalBlocosVertical));
				break;
			case 11:
			//bloco ponta cima
			chunkUVGrid [indexBloco] = new Vector2(posX*10,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*10,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*11,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*11,posY+(1 / totalBlocosVertical));
				break;
			case 12:
			//bloco ponta baixo
			chunkUVGrid [indexBloco] = new Vector2(posX*11,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*11,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*12,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*12,posY+(1 / totalBlocosVertical));
			break;
			case 13:
			//bloco ponta esquerda
			chunkUVGrid [indexBloco] = new Vector2(posX*12,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*12,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*13,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*13,posY+(1 / totalBlocosVertical));
			break;
			case 14:
			//bloco ponta direita
			chunkUVGrid [indexBloco] = new Vector2(posX*13,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*13,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*14,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*14,posY+(1 / totalBlocosVertical));
			break;
			case 15:
			//bloco meio vertical
			chunkUVGrid [indexBloco] = new Vector2(posX*14,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*14,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*15,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*15,posY+(1 / totalBlocosVertical));
				break;
			case 16:
			//bloco meio horizontal
			chunkUVGrid [indexBloco] = new Vector2(posX*15,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*15,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*16,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*16,posY+(1 / totalBlocosVertical));
				break;
			case 17:
			//bloco meio horizontal
			chunkUVGrid [indexBloco] = new Vector2(posX*16,posY);
			chunkUVGrid [indexBloco+1] = new Vector2(posX*16,posY+(1 / totalBlocosVertical));
			chunkUVGrid [indexBloco+2] = new Vector2(posX*17,posY);
			chunkUVGrid [indexBloco+3] = new Vector2(posX*17,posY+(1 / totalBlocosVertical));
				break;

		}
	}

	public void definirTexturaBloco(float posX,float posY){

		if(posX > 99f){
			posX -= chunkX;
		}
		if(posX < 0f){
			posX = 0f;
		}
		if(posY > 99f){
			posY -= chunkY;
		}
		if(posY < 0f){
			posY = 0f;
		}


		Vector2 cimaE = new Vector2 (posX-1,posY+1);
		Vector2 cima = new Vector2 (posX,posY+1);
		Vector2 cimaD = new Vector2 (posX+1,posY+1);
		Vector2 mE = new Vector2 (posX-1,posY);
		Vector2 m = new Vector2 (posX,posY);
		Vector2 mD = new Vector2 (posX+1,posY);
		Vector2 baixoE = new Vector2 (posX-1,posY-1);
		Vector2 baixo = new Vector2 (posX,posY-1);
		Vector2 baixoD = new Vector2 (posX+1,posY-1);

		if(cimaE.x >= 0 && cimaE.y >= 0 && cimaE.x < 99 && cimaE.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(cimaE.x),Mathf.FloorToInt(cimaE.y));

		}
		if(cima.x >= 0 && cima.y >= 0 && cima.x < 99 && cima.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(cima.x),Mathf.FloorToInt(cima.y));

		}
		if(cimaD.x >= 0 && cimaD.y >= 0 && cimaD.x < 99 && cimaD.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(cimaD.x),Mathf.FloorToInt(cimaD.y));

		}
		if(mE.x >= 0 && mE.y >= 0 && mE.x < 99 && mE.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(mE.x),Mathf.FloorToInt(mE.y));

		}
		if(m.x >= 0 && m.y >= 0 && m.x < 99 && m.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(m.x),Mathf.FloorToInt(m.y));

		}
		if(mD.x >= 0 && mD.y >= 0 && mD.x < 99 && mD.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(mD.x),Mathf.FloorToInt(mD.y));

		}
		if(baixoE.x >= 0 && baixoE.y >= 0 && baixoE.x < 99 && baixoE.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(baixoE.x),Mathf.FloorToInt(baixoE.y));

		}
		if(baixo.x >= 0 && baixo.y >= 0 && baixo.x < 99 && baixo.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(baixo.x),Mathf.FloorToInt(baixo.y));

		}
		if(baixoD.x >= 0 && baixoD.y >= 0 && baixoD.x < 99 && baixoD.y < 99){
			definirTexturaMeshGrid (Mathf.FloorToInt(baixoD.x),Mathf.FloorToInt(baixoD.y));

		}



		//updateMesh ();
	}

	public void definirTexturaMeshGrid(int posX, int posY){
		bool encontrou = false;
		int[] sequencia;
		int[] matBloco = new int[9];
		//formulas para identificar todos os blocos em volta do bloco a ser analisado
		if (posX > 99) {
			posX -= chunkX;
		}
		if (posX < 0) {
			posX = 0;
		}
		int cimaEsquerda = (posY + (posX * 100) - 99) * 4;
		int cima = (posY + (posX * 100) + 1) * 4;
		int cimaDireita = (posY + (posX * 100) + 101) * 4;
		int meioEsquerda = (posY + (posX * 100) - 100) * 4;
		int meio = (posY + (posX * 100)) * 4;
		int meioDireita = (posY + (posX * 100) + 100) * 4;
		int baixoEsquerda = (posY + (posX * 100) - 101) * 4;
		int baixo = (posY + (posX * 100) - 1) * 4;
		int baixoDireita = (posY + (posX * 100) + 99) * 4;

		/*print(  cimaEsquerda+","+
				cima+","+
				meioEsquerda+","+
				meio+","+
				meioDireita+","+
				baixoEsquerda+","+
				baixo+","+
				baixoDireita);*/

		//resetando os bloco que estiverem em posiçoes negativas e/ou sua posiçoes forem maiores menores que o vetor
		if (cimaEsquerda < 0) {
			cimaEsquerda = 0;
			chunkEstadoBloco [cimaEsquerda] = 0;
			pGrid.estadoBloco [cimaEsquerda] = 0;
		}
		if (cima < 0) {
			cima = 0;
			chunkEstadoBloco [cima] = 0;
			pGrid.estadoBloco [cima] = 0;
		}
		if (cimaDireita < 0) {
			cimaDireita = 0;
			chunkEstadoBloco [cimaDireita] = 0;
			pGrid.estadoBloco [cimaDireita] = 0;
		}
		if (meioEsquerda < 0) {
			meioEsquerda = 0;
			chunkEstadoBloco [meioEsquerda] = 0;
			pGrid.estadoBloco [meioEsquerda] = 0;
		}
		if (meioDireita < 0) {
			meioDireita = 0;
			chunkEstadoBloco [meioDireita] = 0;
			pGrid.estadoBloco [meioDireita] = 0;
		}
		if (baixoEsquerda < 0) {
			baixoEsquerda = 0;
			chunkEstadoBloco [baixoEsquerda] = 0;
			pGrid.estadoBloco [baixoEsquerda] = 0;
		}
		if (baixo < 0) {
			baixo = 0;
			chunkEstadoBloco [baixo] = 0;
			pGrid.estadoBloco [baixo] = 0;
		}
		if (baixoDireita < 0) {
			baixoDireita = 0;
			chunkEstadoBloco [baixoDireita] = 0;
			pGrid.estadoBloco [baixoDireita] = 0;
		}

		//defindindo posiçao textura do bloco
		/*
		valores por direcao
		1 = cimaEsquerda
		2 = cima
		3 = cimaDireita
		4 = meioEsquerda
		5 =	meio
		6 = meioDireita
		7 = baixoEsquerda
		8 = baixo
		9 = baixoDireita
		10 = blocoInteiro
		11 = pontaCima
		12 = pontaBaixo
		13 = pontaEsquerda
		14 = pontaDireita
		15 = meioVertical
		16 = meioHorizontal
		*/

		//material dos blocos em volta

		blocosEmVolta.Clear ();

		blocosEmVolta.Add (chunkMaterialBloco [cimaEsquerda]);
		blocosEmVolta.Add (chunkMaterialBloco [cima]);
		blocosEmVolta.Add (chunkMaterialBloco [cimaDireita]);
		blocosEmVolta.Add (chunkMaterialBloco [meioEsquerda]);
		blocosEmVolta.Add (chunkMaterialBloco [meio]);
		blocosEmVolta.Add (chunkMaterialBloco [meioDireita]);
		blocosEmVolta.Add (chunkMaterialBloco [baixoEsquerda]);
		blocosEmVolta.Add (chunkMaterialBloco [baixo]);
		blocosEmVolta.Add (chunkMaterialBloco [baixoDireita]);

		int dirBloco = 0;

		if (blocosEmVolta [0] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 8;
		}

		if (blocosEmVolta [1] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 16;
		}

		if (blocosEmVolta [2] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 32;
		}

		if (blocosEmVolta [3] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 64;
		}

		if (blocosEmVolta [4] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 128;
		}

		if (blocosEmVolta [5] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 256;
		}

		if (blocosEmVolta [6] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 512;
		}

		if (blocosEmVolta [7] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 1024;
		}

		if (blocosEmVolta [8] != blocosEmVolta [4] && chunkEstadoBloco[meio] != 0) {
			dirBloco += 2048;
		}



		//tudo
		if (
			dirBloco == 2608 ||
			dirBloco == 2576 ||
			dirBloco == 2096 ||
			dirBloco == 2600 ||
			dirBloco == 2088 ||
			dirBloco == 552 ||
			dirBloco == 40 ||
			dirBloco == 2560 ||
			dirBloco == 8 ||
			dirBloco == 32 ||
			dirBloco == 512 ||
			dirBloco == 2048 ||
			dirBloco == 544 ||
			dirBloco == 2056) 
		{
			setUvBlock (5, meio);
			dirBloco = 0;
		}

		//nada
		else if(
			dirBloco == 3960 ||
			dirBloco == 3952 ||
			dirBloco == 3928 ||
			dirBloco == 3448 ||
			dirBloco == 1912 ||
			dirBloco == 1880 ||
			dirBloco == 1360 ||
			dirBloco == 3920 || 
			dirBloco == 1400 ||
			dirBloco == 1904 ||
			dirBloco == 3416 ||
			dirBloco == 3440)
		{
			setUvBlock (10, meio);
			dirBloco = 0;
		}

		//cima esquerda
		else if (dirBloco == 88 ||
		        dirBloco == 600 ||
		        dirBloco == 120 ||
		        dirBloco == 632 ||
		        dirBloco == 624 ||
		        dirBloco == 112 ||
		        dirBloco == 592 ||
		        dirBloco == 80 ||
		        dirBloco == 2136 ||
		        dirBloco == 2648 ||
		        dirBloco == 2168 ||
		        dirBloco == 2680 ||
		        dirBloco == 2672 ||
		        dirBloco == 2160 ||
		        dirBloco == 2640 ||
		        dirBloco == 2128) 
		{
			setUvBlock (1, meio);
			dirBloco = 0;
		}

		//cima
		else if ( 
			dirBloco == 16 ||
			dirBloco == 24 ||
			dirBloco == 48 ||
			dirBloco == 56 ||
			dirBloco == 528 ||
			dirBloco == 536 ||
			dirBloco == 560 ||
			dirBloco == 568 ||
			dirBloco == 2064 ||
			dirBloco == 2072 ||
			dirBloco == 2096 ||
			dirBloco == 2104 ||
			dirBloco == 2576 ||
			dirBloco == 2584 ||
			dirBloco == 2608 ||
			dirBloco == 2616) 
		{
			setUvBlock (2, meio);
			dirBloco = 0;
		}

		//cima direita
		else if (
			dirBloco == 304 ||
			dirBloco == 2352 ||
			dirBloco == 312 ||
			dirBloco == 2360 ||
			dirBloco == 272 ||
			dirBloco == 2320 ||
			dirBloco == 280 ||
			dirBloco == 2328 ||
			dirBloco == 816 ||
			dirBloco == 2864 ||
			dirBloco == 824 ||
			dirBloco == 2872 ||
			dirBloco == 784 ||
			dirBloco == 2832 ||
			dirBloco == 792 ||
			dirBloco == 2840) 
		{
			setUvBlock (3, meio);
			dirBloco = 0;
		}

		//meio esquerda
		else if (
			dirBloco == 64 ||
			dirBloco == 576 ||
			dirBloco == 72 ||
			dirBloco == 584 ||
			dirBloco == 2112 ||
			dirBloco == 2624 ||
			dirBloco == 2120 ||
			dirBloco == 2632 ||
			dirBloco == 96 ||
			dirBloco == 608 ||
			dirBloco == 104 ||
			dirBloco == 616 ||
			dirBloco == 2144 ||
			dirBloco == 2656 ||
			dirBloco == 2152 ||
			dirBloco == 2664) 
		{
			setUvBlock (4, meio);
			dirBloco = 0;
		}

		//meio direita
		else if (
			dirBloco == 256 ||
			dirBloco == 2304 ||
			dirBloco == 288 ||
			dirBloco == 2336 ||
			dirBloco == 768 ||
			dirBloco == 2816 ||
			dirBloco == 800 ||
			dirBloco == 2848 ||
			dirBloco == 264 ||
			dirBloco == 2312 ||
			dirBloco == 296 ||
			dirBloco == 2344 ||
			dirBloco == 776 ||
			dirBloco == 2824 ||
			dirBloco == 808 ||
			dirBloco == 2856) 
		{
			setUvBlock (6, meio);
			dirBloco = 0;
		}

		//baixo esquerda
		else if (
			dirBloco == 1600 ||
			dirBloco == 1608 ||
			dirBloco == 3648 ||
			dirBloco == 3656 ||
			dirBloco == 1632 ||
			dirBloco == 1640 ||
			dirBloco == 3680 ||
			dirBloco == 3688 ||
			dirBloco == 1088 ||
			dirBloco == 1096 ||
			dirBloco == 3136 ||
			dirBloco == 3144 ||
			dirBloco == 1120 ||
			dirBloco == 1128 ||
			dirBloco == 3168 ||
			dirBloco == 3176) 
		{
			setUvBlock (7, meio);
			dirBloco = 0;
		}

		//baixo
		else if ( 
			dirBloco == 1024 ||
			dirBloco == 1536 ||
			dirBloco == 3072 ||
			dirBloco == 3584 ||
			dirBloco == 1032 ||
			dirBloco == 1544 ||
			dirBloco == 3080 ||
			dirBloco == 3592 ||
			dirBloco == 1056 ||
			dirBloco == 1568 ||
			dirBloco == 3104 ||
			dirBloco == 3616 ||
			dirBloco == 1064 ||
			dirBloco == 1576 ||
			dirBloco == 3112 ||
			dirBloco == 3624) 
		{
			setUvBlock (8, meio);
			dirBloco = 0;
		}

		//baixo direita
		else if (
			dirBloco == 3328 ||
			dirBloco == 3360 ||
			dirBloco == 3840 ||
			dirBloco == 3872 ||
			dirBloco == 1280 ||
			dirBloco == 1312 ||
			dirBloco == 1792 ||
			dirBloco == 1824 ||
			dirBloco == 3336 ||
			dirBloco == 3368 ||
			dirBloco == 3848 ||
			dirBloco == 3880 ||
			dirBloco == 1288 ||
			dirBloco == 1320 ||
			dirBloco == 1800 ||
			dirBloco == 1832) 
		{
			setUvBlock (9, meio);
			dirBloco = 0;
		}

		//ponta cima
		else if (
			dirBloco == 376 ||
			dirBloco == 368 ||
			dirBloco == 344 ||
			dirBloco == 336 ||
			dirBloco == 888 ||
			dirBloco == 880 ||
			dirBloco == 856 ||
			dirBloco == 848 ||
			dirBloco == 2424 ||
			dirBloco == 2416 ||
			dirBloco == 2392 ||
			dirBloco == 2384 ||
			dirBloco == 2936 ||
			dirBloco == 2928 ||
			dirBloco == 2904 ||
			dirBloco == 2896) 
		{
			setUvBlock (11, meio);
			dirBloco = 0;
		}

		//ponta baixo
		else if (
			dirBloco == 3904 ||
			dirBloco == 3392 ||
			dirBloco == 1856 ||
			dirBloco == 1344 ||
			dirBloco == 3912 ||
			dirBloco == 3400 ||
			dirBloco == 1862 ||
			dirBloco == 1352 ||
			dirBloco == 3936 ||
			dirBloco == 3424 ||
			dirBloco == 1888 ||
			dirBloco == 1376 ||
			dirBloco == 3944 ||
			dirBloco == 3432 ||
			dirBloco == 1896 ||
			dirBloco == 1384) 
		{
			setUvBlock (12, meio);
			dirBloco = 0;
		}

		//ponta esquerda
		else if (
			dirBloco == 1624 ||
			dirBloco == 1616 ||
			dirBloco == 1112 ||
			dirBloco == 1104 ||
			dirBloco == 3672 ||
			dirBloco == 3664 ||
			dirBloco == 3160 ||
			dirBloco == 3152 ||
			dirBloco == 1656 ||
			dirBloco == 1648 ||
			dirBloco == 1144 ||
			dirBloco == 1136 ||
			dirBloco == 3704 ||
			dirBloco == 3696 ||
			dirBloco == 3192 ||
			dirBloco == 3184) 
		{
			setUvBlock (13, meio);
			dirBloco = 0;
		}

		//ponta direita
		else if (
			dirBloco == 3376 ||
			dirBloco == 3344 ||
			dirBloco == 1328 ||
			dirBloco == 1296 ||
			dirBloco == 3888 ||
			dirBloco == 3856 ||
			dirBloco == 1840 ||
			dirBloco == 1808 ||
			dirBloco == 3384 ||
			dirBloco == 3832 ||
			dirBloco == 1816 ||
			dirBloco == 1784 ||
			dirBloco == 3896 ||
			dirBloco == 3864 ||
			dirBloco == 1848 ||
			dirBloco == 1816) 
		{
			setUvBlock (14, meio);
			dirBloco = 0;
		}

		//meio vertical
		else if (
			dirBloco == 2920 ||
			dirBloco == 2888 ||
			dirBloco == 872 ||
			dirBloco == 840 ||
			dirBloco == 2912 ||
			dirBloco == 2880 ||
			dirBloco == 864 ||
			dirBloco == 832 ||
			dirBloco == 2408 ||
			dirBloco == 2376 ||
			dirBloco == 360 ||
			dirBloco == 328 ||
			dirBloco == 2400 ||
			dirBloco == 2368 ||
			dirBloco == 352 ||
			dirBloco == 320) 
		{
			setUvBlock (15, meio);
			dirBloco = 0;
		}

		//meio horizontal
		else if (
			dirBloco == 3640 ||
			dirBloco == 3128 ||
			dirBloco == 1592 ||
			dirBloco == 1080 ||
			dirBloco == 3632 ||
			dirBloco == 3120 ||
			dirBloco == 1584 ||
			dirBloco == 1072 ||
			dirBloco == 3608 ||
			dirBloco == 3096 ||
			dirBloco == 1560 ||
			dirBloco == 1048 ||
			dirBloco == 3600 ||
			dirBloco == 2088 ||
			dirBloco == 1532 ||
			dirBloco == 1040) 
		{
			setUvBlock (16, meio);
			dirBloco = 0;
		} 

		else if(chunkEstadoBloco[meio] == 0){
			setUvBlock (17, meio);
			dirBloco = 0;
		}


		/*
		//nada
		sequencia = new int[9]{	1,1,1,
								1,0,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,0,0,
								0,0,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,1,
								0,0,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,0,
								0,0,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);


		sequencia = new int[9]{	0,0,1,
								0,0,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,0,
								0,0,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,0,
								0,0,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,0,
								0,0,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,1,
								0,0,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,0,
								0,0,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,0,
								0,0,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,1,
								0,0,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,1,
								0,0,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,1,
								0,0,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,0,
								0,0,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	0,0,1,
								0,0,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);

		sequencia = new int[9]{	1,0,1,
								0,0,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,17,meio);


		//tudo
		sequencia = new int[9]{	1,1,1,
								1,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,5,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);


		sequencia = new int[9]{	0,0,1,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,10,meio);

		//------------------CIMA ESQUERDA-----------------
		sequencia = new int[9]{	0,0,1,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,1,meio);

		//------------FIM-------------


		//-----------------CIMA DIREITA ----------
		sequencia = new int[9]{	1,0,0,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,3,meio);

		//------------FIM-------------


		//---------------BAIXO DIREITA
		sequencia = new int[9]{	1,1,1,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,9,meio);

		//------------FIM-------------


		//--------------BAIXO ESQUERDA
		sequencia = new int[9]{	1,1,1,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,7,meio);

		//------------FIM-------------


		//-----------------BAIXO-----------

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,2,meio);

		//------------FIM-------------


		//--------------CIMA----------


		sequencia = new int[9]{	1,1,1,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,8,meio);



		//------------FIM-------------




		//-------------MEIO ESQUERDA--------
		sequencia = new int[9]{	0,1,1,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,1,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,1,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,1,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,1,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,4,meio);

		//------------FIM-------------


		//------------MEIO DIREITA-------

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,0,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,0,
								1,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	1,1,1,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);

		sequencia = new int[9]{	0,1,1,
								1,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,6,meio);


		//------------FIM-------------



		//------------PONTA CIMA----------

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,11,meio);


		//------------FIM-------------



		//--------------PONTA BAIXO------

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								0,0,1	};
		
		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,12,meio);

		//------------FIM-------------


		//-------------BLOCO PONTA ESQUERDA-------------


		sequencia = new int[9]{	0,0,1,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,1,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	0,0,0,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,1,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		sequencia = new int[9]{	1,0,0,
								0,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,13,meio);

		//-----------------FIM-------------



		//---------------PONTA DIREITA----------

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,0,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);


		sequencia = new int[9]{	0,0,0,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);


		sequencia = new int[9]{	0,0,1,
								1,1,0,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);


		sequencia = new int[9]{	0,0,0,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);


		sequencia = new int[9]{	0,0,1,
								1,1,0,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,14,meio);

		//--------------FiM-----------


		//---------------MEIO VERTICAL----------

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,1,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								0,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,0,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	1,1,0,
								0,1,0,
								1,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								1,1,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		sequencia = new int[9]{	0,1,1,
								0,1,0,
								0,1,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,15,meio);

		//--------------FIM-----------


		//--------------MEIO HORIZONTAL----------

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,0,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								0,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,0,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								0,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	1,0,1,
								1,1,1,
								1,0,0	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		sequencia = new int[9]{	0,0,1,
								1,1,1,
								1,0,1	};

		compararListas(blocosEmVolta.ToArray(),sequencia,16,meio);

		//--------------FIM-----------



		blocosEmVolta.Clear();

		//updateMesh ();

	}

	void compararListas(int[] blocosEmVolta,int[] sequencia,int idMatBloco,int indexBloco){
		compararMaterial (blocosEmVolta,sequencia,idMatBloco,indexBloco);
		if( blocosEmVolta[0] == sequencia[0] &&
			blocosEmVolta[1] == sequencia[1] &&
			blocosEmVolta[2] == sequencia[2] &&
			blocosEmVolta[3] == sequencia[3] &&
			blocosEmVolta[4] == sequencia[4] &&
			blocosEmVolta[5] == sequencia[5] &&
			blocosEmVolta[6] == sequencia[6] &&
			blocosEmVolta[7] == sequencia[7] &&
			blocosEmVolta[8] == sequencia[8])
		{
			setUvBlock(idMatBloco, indexBloco);
		}


	}

	void compararMaterial(int[] blocosEmVolta){
		int[] seq1 = new int[9];

	}
	*/
	}
}
