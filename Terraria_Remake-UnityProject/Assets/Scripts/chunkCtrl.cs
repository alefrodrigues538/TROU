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
		if(proceduralGrid.podePreencher && chunkAtualizado == false){
			makeChunkMeshGrid();

			chunkAtualizado = true;

			mesh.Clear();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.RecalculateNormals();
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

				//total de blocos na HORIZONTAL
				float totalBlocosHorizontal = 17;

				//total de blocos na VERTICAL
				float totalBlocosVertical = 3;

				//posiçoes da UV;
				float posX = (1 / totalBlocosHorizontal);
				float posY = (1 / totalBlocosVertical);

				//comparando o MATERIAL do bloco para definir qual TEXTURA usar
				if(pGrid.materialBloco[indexMaterialBloco] == 0){
					posY = 0;
				}
				else if(pGrid.materialBloco[indexMaterialBloco] == 1){
					posY = (1 / totalBlocosVertical);
				}
				else if(pGrid.materialBloco[indexMaterialBloco] == 2){
					posY = (1 / totalBlocosVertical)*2;
				}

				int dirBloco = 0;

				//se o estado do bloco for 0
				if(pGrid.estadoBloco[indexMaterialBloco] == 0){
					dirBloco = 17;
					uvs.Add(new Vector2(posX*16,posY));
					uvs.Add(new Vector2(posX*16,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*17,posY));
					uvs.Add(new Vector2(posX*17,posY+(1 / totalBlocosVertical)));
				}

				//tudo
				else if (
					pGrid.direcaoBloco[indexBloco] == 0 ||
					pGrid.direcaoBloco[indexBloco] == 2600 ||
					pGrid.direcaoBloco[indexBloco] == 2088 ||
					pGrid.direcaoBloco[indexBloco] == 552 ||
					pGrid.direcaoBloco[indexBloco] == 40 ||
					pGrid.direcaoBloco[indexBloco] == 2560 ||
					pGrid.direcaoBloco[indexBloco] == 8 ||
					pGrid.direcaoBloco[indexBloco] == 32 ||
					pGrid.direcaoBloco[indexBloco] == 512 ||
					pGrid.direcaoBloco[indexBloco] == 520 ||
					pGrid.direcaoBloco[indexBloco] == 2048 ||
					pGrid.direcaoBloco[indexBloco] == 544 ||
					pGrid.direcaoBloco[indexBloco] == 4085 ||
					pGrid.direcaoBloco[indexBloco] == 690 ||
					pGrid.direcaoBloco[indexBloco] == 2080 ||
					pGrid.direcaoBloco[indexBloco] == 2592 ||
					pGrid.direcaoBloco[indexBloco] == 2056) 
				{
					dirBloco = 5;
					uvs.Add(new Vector2(posX+(posX*3),posY));
					uvs.Add(new Vector2(posX+(posX*3),posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX+(posX*4),posY));
					uvs.Add(new Vector2(posX+(posX*4),posY+(1 / totalBlocosVertical)));
					
				}

				//nada
				else if(
					pGrid.direcaoBloco[indexBloco] == 3960 ||
					pGrid.direcaoBloco[indexBloco] == 3952 ||
					pGrid.direcaoBloco[indexBloco] == 3928 ||
					pGrid.direcaoBloco[indexBloco] == 3448 ||
					pGrid.direcaoBloco[indexBloco] == 1912 ||
					pGrid.direcaoBloco[indexBloco] == 1880 ||
					pGrid.direcaoBloco[indexBloco] == 1360 ||
					pGrid.direcaoBloco[indexBloco] == 3920 || 
					pGrid.direcaoBloco[indexBloco] == 1400 ||
					pGrid.direcaoBloco[indexBloco] == 1904 ||
					pGrid.direcaoBloco[indexBloco] == 3416 ||
					pGrid.direcaoBloco[indexBloco] == 3440)
				{
					dirBloco = 10;
					uvs.Add(new Vector2(posX*9,posY));
					uvs.Add(new Vector2(posX*9,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*10,posY));
					uvs.Add(new Vector2(posX*10,posY+(1 / totalBlocosVertical)));
					
				}

				//cima esquerda
				else if (
					pGrid.direcaoBloco[indexBloco] == 88 ||
					pGrid.direcaoBloco[indexBloco] == 600 ||
					pGrid.direcaoBloco[indexBloco] == 120 ||
					pGrid.direcaoBloco[indexBloco] == 632 ||
					pGrid.direcaoBloco[indexBloco] == 624 ||
					pGrid.direcaoBloco[indexBloco] == 112 ||
					pGrid.direcaoBloco[indexBloco] == 592 ||
					pGrid.direcaoBloco[indexBloco] == 80 ||
					pGrid.direcaoBloco[indexBloco] == 2136 ||
					pGrid.direcaoBloco[indexBloco] == 2648 ||
					pGrid.direcaoBloco[indexBloco] == 2168 ||
					pGrid.direcaoBloco[indexBloco] == 2680 ||
					pGrid.direcaoBloco[indexBloco] == 2672 ||
					pGrid.direcaoBloco[indexBloco] == 2160 ||
					pGrid.direcaoBloco[indexBloco] == 2640 ||
					pGrid.direcaoBloco[indexBloco] == 2128) 
				{
					dirBloco = 1;
					uvs.Add(new Vector2(posX-posX,posY));
					uvs.Add(new Vector2(posX-posX,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX,posY));
					uvs.Add(new Vector2(posX,posY+(1 / totalBlocosVertical)));
					
				}

				//cima
				else if ( 
					pGrid.direcaoBloco[indexBloco] == 16 ||
					pGrid.direcaoBloco[indexBloco] == 24 ||
					pGrid.direcaoBloco[indexBloco] == 48 ||
					pGrid.direcaoBloco[indexBloco] == 56 ||
					pGrid.direcaoBloco[indexBloco] == 528 ||
					pGrid.direcaoBloco[indexBloco] == 536 ||
					pGrid.direcaoBloco[indexBloco] == 560 ||
					pGrid.direcaoBloco[indexBloco] == 568 ||
					pGrid.direcaoBloco[indexBloco] == 2064 ||
					pGrid.direcaoBloco[indexBloco] == 2072 ||
					pGrid.direcaoBloco[indexBloco] == 2096 ||
					pGrid.direcaoBloco[indexBloco] == 2104 ||
					pGrid.direcaoBloco[indexBloco] == 2576 ||
					pGrid.direcaoBloco[indexBloco] == 2584 ||
					pGrid.direcaoBloco[indexBloco] == 2608 ||
					pGrid.direcaoBloco[indexBloco] == 2616) 
				{
					dirBloco = 2;
					uvs.Add(new Vector2(posX,posY));
					uvs.Add(new Vector2(posX,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*2,posY));
					uvs.Add(new Vector2(posX*2,posY+(1 / totalBlocosVertical)));
					
				}

				//cima direita
				else if (
					pGrid.direcaoBloco[indexBloco] == 304 ||
					pGrid.direcaoBloco[indexBloco] == 2352 ||
					pGrid.direcaoBloco[indexBloco] == 312 ||
					pGrid.direcaoBloco[indexBloco] == 2360 ||
					pGrid.direcaoBloco[indexBloco] == 272 ||
					pGrid.direcaoBloco[indexBloco] == 2320 ||
					pGrid.direcaoBloco[indexBloco] == 280 ||
					pGrid.direcaoBloco[indexBloco] == 2328 ||
					pGrid.direcaoBloco[indexBloco] == 816 ||
					pGrid.direcaoBloco[indexBloco] == 2864 ||
					pGrid.direcaoBloco[indexBloco] == 824 ||
					pGrid.direcaoBloco[indexBloco] == 2872 ||
					pGrid.direcaoBloco[indexBloco] == 784 ||
					pGrid.direcaoBloco[indexBloco] == 2832 ||
					pGrid.direcaoBloco[indexBloco] == 792 ||
					pGrid.direcaoBloco[indexBloco] == 2840) 
				{
					dirBloco = 3;
					uvs.Add(new Vector2(posX*2,posY));
					uvs.Add(new Vector2(posX*2,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*3,posY));
					uvs.Add(new Vector2(posX*3,posY+(1 / totalBlocosVertical)));
					
				}

				//indexBloco esquerda
				else if (
					pGrid.direcaoBloco[indexBloco] == 64 ||
					pGrid.direcaoBloco[indexBloco] == 576 ||
					pGrid.direcaoBloco[indexBloco] == 72 ||
					pGrid.direcaoBloco[indexBloco] == 584 ||
					pGrid.direcaoBloco[indexBloco] == 2112 ||
					pGrid.direcaoBloco[indexBloco] == 2624 ||
					pGrid.direcaoBloco[indexBloco] == 2120 ||
					pGrid.direcaoBloco[indexBloco] == 2632 ||
					pGrid.direcaoBloco[indexBloco] == 96 ||
					pGrid.direcaoBloco[indexBloco] == 608 ||
					pGrid.direcaoBloco[indexBloco] == 104 ||
					pGrid.direcaoBloco[indexBloco] == 616 ||
					pGrid.direcaoBloco[indexBloco] == 2144 ||
					pGrid.direcaoBloco[indexBloco] == 2656 ||
					pGrid.direcaoBloco[indexBloco] == 2152 ||
					pGrid.direcaoBloco[indexBloco] == 2664) 
				{
					dirBloco = 4;
					uvs.Add(new Vector2(posX*3,posY));
					uvs.Add(new Vector2(posX*3,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*4,posY));
					uvs.Add(new Vector2(posX*4,posY+(1 / totalBlocosVertical)));
					
				}

				//indexBloco direita
				else if (
					pGrid.direcaoBloco[indexBloco] == 256 ||
					pGrid.direcaoBloco[indexBloco] == 2304 ||
					pGrid.direcaoBloco[indexBloco] == 288 ||
					pGrid.direcaoBloco[indexBloco] == 2336 ||
					pGrid.direcaoBloco[indexBloco] == 768 ||
					pGrid.direcaoBloco[indexBloco] == 2816 ||
					pGrid.direcaoBloco[indexBloco] == 800 ||
					pGrid.direcaoBloco[indexBloco] == 2848 ||
					pGrid.direcaoBloco[indexBloco] == 264 ||
					pGrid.direcaoBloco[indexBloco] == 2312 ||
					pGrid.direcaoBloco[indexBloco] == 296 ||
					pGrid.direcaoBloco[indexBloco] == 2344 ||
					pGrid.direcaoBloco[indexBloco] == 776 ||
					pGrid.direcaoBloco[indexBloco] == 2824 ||
					pGrid.direcaoBloco[indexBloco] == 808 ||
					pGrid.direcaoBloco[indexBloco] == 2856) 
				{
					dirBloco = 6;
					uvs.Add(new Vector2(posX*5,posY));
					uvs.Add(new Vector2(posX*5,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*6,posY));
					uvs.Add(new Vector2(posX*6,posY+(1 / totalBlocosVertical)));
					
				}

				//baixo esquerda
				else if (
					pGrid.direcaoBloco[indexBloco] == 1600 ||
					pGrid.direcaoBloco[indexBloco] == 1608 ||
					pGrid.direcaoBloco[indexBloco] == 3648 ||
					pGrid.direcaoBloco[indexBloco] == 3656 ||
					pGrid.direcaoBloco[indexBloco] == 1632 ||
					pGrid.direcaoBloco[indexBloco] == 1640 ||
					pGrid.direcaoBloco[indexBloco] == 3680 ||
					pGrid.direcaoBloco[indexBloco] == 3688 ||
					pGrid.direcaoBloco[indexBloco] == 1088 ||
					pGrid.direcaoBloco[indexBloco] == 1096 ||
					pGrid.direcaoBloco[indexBloco] == 3136 ||
					pGrid.direcaoBloco[indexBloco] == 3144 ||
					pGrid.direcaoBloco[indexBloco] == 1120 ||
					pGrid.direcaoBloco[indexBloco] == 1128 ||
					pGrid.direcaoBloco[indexBloco] == 3168 ||
					pGrid.direcaoBloco[indexBloco] == 3176) 
				{
					dirBloco = 7;
					uvs.Add(new Vector2(posX*6,posY));
					uvs.Add(new Vector2(posX*6,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*7,posY));
					uvs.Add(new Vector2(posX*7,posY+(1 / totalBlocosVertical)));
					
				}

				//baixo
				else if ( 
					pGrid.direcaoBloco[indexBloco] == 1024 ||
					pGrid.direcaoBloco[indexBloco] == 1536 ||
					pGrid.direcaoBloco[indexBloco] == 3072 ||
					pGrid.direcaoBloco[indexBloco] == 3584 ||
					pGrid.direcaoBloco[indexBloco] == 1032 ||
					pGrid.direcaoBloco[indexBloco] == 1544 ||
					pGrid.direcaoBloco[indexBloco] == 3080 ||
					pGrid.direcaoBloco[indexBloco] == 3592 ||
					pGrid.direcaoBloco[indexBloco] == 1056 ||
					pGrid.direcaoBloco[indexBloco] == 1568 ||
					pGrid.direcaoBloco[indexBloco] == 3104 ||
					pGrid.direcaoBloco[indexBloco] == 3616 ||
					pGrid.direcaoBloco[indexBloco] == 1064 ||
					pGrid.direcaoBloco[indexBloco] == 1576 ||
					pGrid.direcaoBloco[indexBloco] == 3112 ||
					pGrid.direcaoBloco[indexBloco] == 2576 ||
					pGrid.direcaoBloco[indexBloco] == 2096 ||
					pGrid.direcaoBloco[indexBloco] == 2608 ||
					pGrid.direcaoBloco[indexBloco] == 3624) 
				{
					dirBloco = 8;
					uvs.Add(new Vector2(posX*7,posY));
					uvs.Add(new Vector2(posX*7,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*8,posY));
					uvs.Add(new Vector2(posX*8,posY+(1 / totalBlocosVertical)));
					
				}

				//baixo direita
				else if (
					pGrid.direcaoBloco[indexBloco] == 3328 ||
					pGrid.direcaoBloco[indexBloco] == 3360 ||
					pGrid.direcaoBloco[indexBloco] == 3840 ||
					pGrid.direcaoBloco[indexBloco] == 3872 ||
					pGrid.direcaoBloco[indexBloco] == 1280 ||
					pGrid.direcaoBloco[indexBloco] == 1312 ||
					pGrid.direcaoBloco[indexBloco] == 1792 ||
					pGrid.direcaoBloco[indexBloco] == 1824 ||
					pGrid.direcaoBloco[indexBloco] == 3336 ||
					pGrid.direcaoBloco[indexBloco] == 3368 ||
					pGrid.direcaoBloco[indexBloco] == 3848 ||
					pGrid.direcaoBloco[indexBloco] == 3880 ||
					pGrid.direcaoBloco[indexBloco] == 1288 ||
					pGrid.direcaoBloco[indexBloco] == 1320 ||
					pGrid.direcaoBloco[indexBloco] == 1800 ||
					pGrid.direcaoBloco[indexBloco] == 1832) 
				{
					dirBloco = 9;
					uvs.Add(new Vector2(posX*8,posY));
					uvs.Add(new Vector2(posX*8,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*9,posY));
					uvs.Add(new Vector2(posX*9,posY+(1 / totalBlocosVertical)));
					
				}

				//ponta cima
				else if (
					pGrid.direcaoBloco[indexBloco] == 376 ||
					pGrid.direcaoBloco[indexBloco] == 368 ||
					pGrid.direcaoBloco[indexBloco] == 344 ||
					pGrid.direcaoBloco[indexBloco] == 336 ||
					pGrid.direcaoBloco[indexBloco] == 888 ||
					pGrid.direcaoBloco[indexBloco] == 880 ||
					pGrid.direcaoBloco[indexBloco] == 856 ||
					pGrid.direcaoBloco[indexBloco] == 848 ||
					pGrid.direcaoBloco[indexBloco] == 2424 ||
					pGrid.direcaoBloco[indexBloco] == 2416 ||
					pGrid.direcaoBloco[indexBloco] == 2392 ||
					pGrid.direcaoBloco[indexBloco] == 2384 ||
					pGrid.direcaoBloco[indexBloco] == 2936 ||
					pGrid.direcaoBloco[indexBloco] == 2928 ||
					pGrid.direcaoBloco[indexBloco] == 2904 ||
					pGrid.direcaoBloco[indexBloco] == 2896) 
				{
					dirBloco = 11;
					uvs.Add(new Vector2(posX*10,posY));
					uvs.Add(new Vector2(posX*10,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*11,posY));
					uvs.Add(new Vector2(posX*11,posY+(1 / totalBlocosVertical)));
					
				}

				//ponta baixo
				else if (
					pGrid.direcaoBloco[indexBloco] == 3904 ||
					pGrid.direcaoBloco[indexBloco] == 3392 ||
					pGrid.direcaoBloco[indexBloco] == 1856 ||
					pGrid.direcaoBloco[indexBloco] == 1344 ||
					pGrid.direcaoBloco[indexBloco] == 3912 ||
					pGrid.direcaoBloco[indexBloco] == 3400 ||
					pGrid.direcaoBloco[indexBloco] == 1862 ||
					pGrid.direcaoBloco[indexBloco] == 1352 ||
					pGrid.direcaoBloco[indexBloco] == 3936 ||
					pGrid.direcaoBloco[indexBloco] == 3424 ||
					pGrid.direcaoBloco[indexBloco] == 1888 ||
					pGrid.direcaoBloco[indexBloco] == 1376 ||
					pGrid.direcaoBloco[indexBloco] == 3944 ||
					pGrid.direcaoBloco[indexBloco] == 3432 ||
					pGrid.direcaoBloco[indexBloco] == 1896 ||
					pGrid.direcaoBloco[indexBloco] == 1384) 
				{
					dirBloco = 12;
					uvs.Add(new Vector2(posX*11,posY));
					uvs.Add(new Vector2(posX*11,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*12,posY));
					uvs.Add(new Vector2(posX*12,posY+(1 / totalBlocosVertical)));
					
				}

				//ponta esquerda
				else if (
					pGrid.direcaoBloco[indexBloco] == 1624 ||
					pGrid.direcaoBloco[indexBloco] == 1616 ||
					pGrid.direcaoBloco[indexBloco] == 1112 ||
					pGrid.direcaoBloco[indexBloco] == 1104 ||
					pGrid.direcaoBloco[indexBloco] == 3672 ||
					pGrid.direcaoBloco[indexBloco] == 3664 ||
					pGrid.direcaoBloco[indexBloco] == 3160 ||
					pGrid.direcaoBloco[indexBloco] == 3152 ||
					pGrid.direcaoBloco[indexBloco] == 1656 ||
					pGrid.direcaoBloco[indexBloco] == 1648 ||
					pGrid.direcaoBloco[indexBloco] == 1144 ||
					pGrid.direcaoBloco[indexBloco] == 1136 ||
					pGrid.direcaoBloco[indexBloco] == 3704 ||
					pGrid.direcaoBloco[indexBloco] == 3696 ||
					pGrid.direcaoBloco[indexBloco] == 3192 ||
					pGrid.direcaoBloco[indexBloco] == 3184) 
				{
					dirBloco = 13;
					uvs.Add(new Vector2(posX*12,posY));
					uvs.Add(new Vector2(posX*12,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*13,posY));
					uvs.Add(new Vector2(posX*13,posY+(1 / totalBlocosVertical)));
					
				}

				//ponta direita
				else if (
					pGrid.direcaoBloco[indexBloco] == 3376 ||
					pGrid.direcaoBloco[indexBloco] == 3344 ||
					pGrid.direcaoBloco[indexBloco] == 1328 ||
					pGrid.direcaoBloco[indexBloco] == 1296 ||
					pGrid.direcaoBloco[indexBloco] == 3888 ||
					pGrid.direcaoBloco[indexBloco] == 3856 ||
					pGrid.direcaoBloco[indexBloco] == 1840 ||
					pGrid.direcaoBloco[indexBloco] == 1808 ||
					pGrid.direcaoBloco[indexBloco] == 3384 ||
					pGrid.direcaoBloco[indexBloco] == 3832 ||
					pGrid.direcaoBloco[indexBloco] == 1816 ||
					pGrid.direcaoBloco[indexBloco] == 1784 ||
					pGrid.direcaoBloco[indexBloco] == 3896 ||
					pGrid.direcaoBloco[indexBloco] == 3864 ||
					pGrid.direcaoBloco[indexBloco] == 1848 ||
					pGrid.direcaoBloco[indexBloco] == 1336 ||
					pGrid.direcaoBloco[indexBloco] == 1816) 
				{
					dirBloco = 14;
					uvs.Add(new Vector2(posX*13,posY));
					uvs.Add(new Vector2(posX*13,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*14,posY));
					uvs.Add(new Vector2(posX*14,posY+(1 / totalBlocosVertical)));
					
				}

				//meio vertical
				else if (
					pGrid.direcaoBloco[indexBloco] == 2920 ||
					pGrid.direcaoBloco[indexBloco] == 2888 ||
					pGrid.direcaoBloco[indexBloco] == 872 ||
					pGrid.direcaoBloco[indexBloco] == 840 ||
					pGrid.direcaoBloco[indexBloco] == 2912 ||
					pGrid.direcaoBloco[indexBloco] == 2880 ||
					pGrid.direcaoBloco[indexBloco] == 864 ||
					pGrid.direcaoBloco[indexBloco] == 832 ||
					pGrid.direcaoBloco[indexBloco] == 2408 ||
					pGrid.direcaoBloco[indexBloco] == 2376 ||
					pGrid.direcaoBloco[indexBloco] == 360 ||
					pGrid.direcaoBloco[indexBloco] == 328 ||
					pGrid.direcaoBloco[indexBloco] == 2400 ||
					pGrid.direcaoBloco[indexBloco] == 2368 ||
					pGrid.direcaoBloco[indexBloco] == 352 ||
					pGrid.direcaoBloco[indexBloco] == 320)
				{
					dirBloco = 15;
					uvs.Add(new Vector2(posX*14,posY));
					uvs.Add(new Vector2(posX*14,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*15,posY));
					uvs.Add(new Vector2(posX*15,posY+(1 / totalBlocosVertical)));
					
				}

				//meio horizontal
				else if (
					pGrid.direcaoBloco[indexBloco] == 3640 ||
					pGrid.direcaoBloco[indexBloco] == 3128 ||
					pGrid.direcaoBloco[indexBloco] == 1592 ||
					pGrid.direcaoBloco[indexBloco] == 1080 ||
					pGrid.direcaoBloco[indexBloco] == 3632 ||
					pGrid.direcaoBloco[indexBloco] == 3120 ||
					pGrid.direcaoBloco[indexBloco] == 1584 ||
					pGrid.direcaoBloco[indexBloco] == 1072 ||
					pGrid.direcaoBloco[indexBloco] == 3608 ||
					pGrid.direcaoBloco[indexBloco] == 3096 ||
					pGrid.direcaoBloco[indexBloco] == 3088 ||
					pGrid.direcaoBloco[indexBloco] == 1560 ||
					pGrid.direcaoBloco[indexBloco] == 1048 ||
					pGrid.direcaoBloco[indexBloco] == 3600 ||
					pGrid.direcaoBloco[indexBloco] == 2088 ||
					pGrid.direcaoBloco[indexBloco] == 1532 ||
					pGrid.direcaoBloco[indexBloco] == 1040) 
				{
					dirBloco = 16;
					uvs.Add(new Vector2(posX*15,posY));
					uvs.Add(new Vector2(posX*15,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*16,posY));
					uvs.Add(new Vector2(posX*16,posY+(1 / totalBlocosVertical)));
					
				}
				else{
					uvs.Add(new Vector2(posX*9,posY));
					uvs.Add(new Vector2(posX*9,posY+(1 / totalBlocosVertical)));
					uvs.Add(new Vector2(posX*10,posY));
					uvs.Add(new Vector2(posX*10,posY+(1 / totalBlocosVertical)));
				}
				dirBloco = 0;
			}
		}

		print(uvs.Count+","+vertices.Count+","+triangles.Count);

	}

	//-----------------------------------FIM--------------------------------------
}
