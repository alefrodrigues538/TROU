using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkCtrl : MonoBehaviour {

	//procedural grid lugar aonde fica todas as informaçoes do mundo criado
	public proceduralGrid pGrid;

	//propriedades de montagem do chunk
	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int>();
	List<Vector2> uvs = new List<Vector2>();

	//dimensoes do chunk
	public int chunkX;
	public int chunkY;

	//Blocos de fundo
	public GameObject background;

	//propriedades da MeshGrid do Chunk
	Mesh mesh;
	Mesh backgroundMesh;


	bool chunkAtualizado = false;
	// Use this for initialization
	void Awake () {
		mesh = GetComponent<MeshFilter>().mesh;
		backgroundMesh = background.GetComponent<MeshFilter>().mesh;
		pGrid = (proceduralGrid)FindObjectOfType(typeof(proceduralGrid));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha2) && proceduralGrid.podePreencher && chunkAtualizado == false){
			makeChunkMeshGrid();

			mesh.Clear();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.RecalculateNormals();

			chunkAtualizado = true;

			print("MESHGRID DO CHUNK PREENCHIDA");
		}
	}

	//COMEÇO----------------PREENCHENDO E CRIANDO A MESHGRID DO CHUNK------------

	void makeChunkMeshGrid(){
		vertices.Clear();
		triangles.Clear();
		uvs.Clear();

		//trackers
		int t = 0;

		for(int x = chunkX; x < chunkX + 100; x++){
			for(int y = chunkY; y < chunkY + 100; y++){
				//FORMULA PARA ENCONTRAR O BLOCO NAS LISTAS PRINCIPAIS
				int indexBloco = (y + (x * pGrid.gridSizeY)) * 4;
				int indexMaterialBloco = (y + (x * pGrid.gridSizeY)) * 4;

				//PREENCHENDO VERTICES
				vertices.Add(new Vector3 (x,y,0));
				vertices.Add(new Vector3 (x,y+1,0));
				vertices.Add(new Vector3 (x+1,y,0));
				vertices.Add(new Vector3 (x+1,y+1,0)); 

				//PREENCHENDO TRIANGULOS
				triangles.Add(t);
				triangles.Add(t+1);
				triangles.Add(t+2);
				triangles.Add(t+2);
				triangles.Add(t+1);
				triangles.Add(t+3);

				t += 4;

				//PREENCHENDO UV
				if(pGrid.direcaoBloco[indexBloco] == 1){
					setDirectionBloco(1,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 2){
					setDirectionBloco(2,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 3){
					setDirectionBloco(3,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 4){
					setDirectionBloco(4,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 5){
					setDirectionBloco(5,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 6){
					setDirectionBloco(6,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 7){
					setDirectionBloco(7,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 8){
					setDirectionBloco(8,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 9){
					setDirectionBloco(9,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 10){
					setDirectionBloco(10,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 11){
					setDirectionBloco(11,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 12){
					setDirectionBloco(12,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 13){
					setDirectionBloco(13,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 14){
					setDirectionBloco(14,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 15){
					setDirectionBloco(15,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 16){
					setDirectionBloco(16,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}
				else if(pGrid.direcaoBloco[indexBloco] == 17){
					setDirectionBloco(17,indexBloco,pGrid.materialBloco[indexMaterialBloco]);
				}


				
			}
		}
	}

	//-----------------------------------FIM--------------------------------------

	//DEFINIR A TEXTURA DA DIREÇAO DO BLOCO
	void setDirectionBloco (int idDirecaoBloco, int indexBloco, int materialIndex) {

		//total de blocos na HORIZONTAL
		float totalBlocosHorizontal = 17;

		//total de blocos na VERTICAL
		float totalBlocosVertical = 3;

		//posiçoes da UV;
		float posX = (1 / totalBlocosHorizontal);
		float posY = (1 / totalBlocosVertical);

		if(materialIndex == 0){
			posY = 0;
		}
		else if(materialIndex == 1){
			posY = (1 / totalBlocosVertical);
		}
		else if(materialIndex == 2){
			posY = (1 / totalBlocosVertical)*2;
		}

		switch (idDirecaoBloco)
		{


			case 1:
				//cima esquerda
			uvs.Add(new Vector2(posX-posX,posY));
			uvs.Add(new Vector2(posX-posX,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX,posY));
			uvs.Add(new Vector2(posX,posY+(1 / totalBlocosVertical)));
				break;
			case 2:
				//cima
			uvs.Add(new Vector2(posX,posY));
			uvs.Add(new Vector2(posX,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*2,posY));
			uvs.Add(new Vector2(posX*2,posY+(1 / totalBlocosVertical)));
				break;
			case 3:
				//cima direita
			uvs.Add(new Vector2(posX*2,posY));
			uvs.Add(new Vector2(posX*2,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*3,posY));
			uvs.Add(new Vector2(posX*3,posY+(1 / totalBlocosVertical)));
				break;

			case 4:
				//meio esquerda
			uvs.Add(new Vector2(posX*3,posY));
			uvs.Add(new Vector2(posX*3,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*4,posY));
			uvs.Add(new Vector2(posX*4,posY+(1 / totalBlocosVertical)));
				break;
			case 5:
				//meio
			uvs.Add(new Vector2(posX+(posX*3),posY));
			uvs.Add(new Vector2(posX+(posX*3),posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX+(posX*4),posY));
			uvs.Add(new Vector2(posX+(posX*4),posY+(1 / totalBlocosVertical)));
				break;
			case 6:
				//meio direita
			uvs.Add(new Vector2(posX*5,posY));
			uvs.Add(new Vector2(posX*5,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*6,posY));
			uvs.Add(new Vector2(posX*6,posY+(1 / totalBlocosVertical)));
				break;
			case 7:
				//baixo esquerda
			uvs.Add(new Vector2(posX*6,posY));
			uvs.Add(new Vector2(posX*6,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*7,posY));
			uvs.Add(new Vector2(posX*7,posY+(1 / totalBlocosVertical)));
				break;
			case 8:
				//baixo
			uvs.Add(new Vector2(posX*7,posY));
			uvs.Add(new Vector2(posX*7,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*8,posY));
			uvs.Add(new Vector2(posX*8,posY+(1 / totalBlocosVertical)));
				break;
			case 9:
				//baixo direita
			uvs.Add(new Vector2(posX*8,posY));
			uvs.Add(new Vector2(posX*8,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*9,posY));
			uvs.Add(new Vector2(posX*9,posY+(1 / totalBlocosVertical)));
				break;
			case 10:
				//bloco inteiro
			uvs.Add(new Vector2(posX*9,posY));
			uvs.Add(new Vector2(posX*9,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*10,posY));
			uvs.Add(new Vector2(posX*10,posY+(1 / totalBlocosVertical)));
				break;
			case 11:
			//bloco ponta cima
			uvs.Add(new Vector2(posX*10,posY));
			uvs.Add(new Vector2(posX*10,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*11,posY));
			uvs.Add(new Vector2(posX*11,posY+(1 / totalBlocosVertical)));
				break;
			case 12:
			//bloco ponta baixo
			uvs.Add(new Vector2(posX*11,posY));
			uvs.Add(new Vector2(posX*11,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*12,posY));
			uvs.Add(new Vector2(posX*12,posY+(1 / totalBlocosVertical)));
			break;
			case 13:
			//bloco ponta esquerda
			uvs.Add(new Vector2(posX*12,posY));
			uvs.Add(new Vector2(posX*12,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*13,posY));
			uvs.Add(new Vector2(posX*13,posY+(1 / totalBlocosVertical)));
			break;
			case 14:
			//bloco ponta direita
			uvs.Add(new Vector2(posX*13,posY));
			uvs.Add(new Vector2(posX*13,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*14,posY));
			uvs.Add(new Vector2(posX*14,posY+(1 / totalBlocosVertical)));
			break;
			case 15:
			//bloco meio vertical
			uvs.Add(new Vector2(posX*14,posY));
			uvs.Add(new Vector2(posX*14,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*15,posY));
			uvs.Add(new Vector2(posX*15,posY+(1 / totalBlocosVertical)));
				break;
			case 16:
			//bloco meio horizontal
			uvs.Add(new Vector2(posX*15,posY));
			uvs.Add(new Vector2(posX*15,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*16,posY));
			uvs.Add(new Vector2(posX*16,posY+(1 / totalBlocosVertical)));
				break;
			case 17:
			//bloco meio horizontal
			uvs.Add(new Vector2(posX*16,posY));
			uvs.Add(new Vector2(posX*16,posY+(1 / totalBlocosVertical)));
			uvs.Add(new Vector2(posX*17,posY));
			uvs.Add(new Vector2(posX*17,posY+(1 / totalBlocosVertical)));
				break;

		}
	}
}
