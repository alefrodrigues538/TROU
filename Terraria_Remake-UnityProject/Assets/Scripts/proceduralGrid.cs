using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGrid : MonoBehaviour {


	//chunks
	public List <GameObject> chunksObjects = new List<GameObject>();

	//arrays para montar a MeshGrid
	public List <int> tipoBloco = new List <int>();
	public List <int> materialBloco = new List <int>();
	public List <int> estadoBloco = new List <int>();
	public List <int> direcaoBloco = new List <int>();

	public List <int> tipoBackgroundBloco = new List <int>();
	public List <int> materialBackgroundBloco = new List <int>();
	public List <int> estadoBackgroundBloco = new List <int>();
	public List <int> direcaoBackgroundBloco = new List <int>();


	List<int> blocosEmVolta = new List<int>();

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
			//setDirecaoBlocos ();
			podePreencher = true;
		}

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

	void calcularDirecaoBlocos(){
		int dirBloco = 0;
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {

				//formulas para identificar todos os blocos em volta do bloco a ser analisado
				int cimaEsquerda = (y + (x * gridSizeY) - (gridSizeY -1)) * 4;
				int cima = (y + (x * gridSizeY) + 1) * 4;
				int cimaDireita = (y + (x * gridSizeY) + (gridSizeY +1)) * 4;
				int meioEsquerda = (y + (x * gridSizeY) - gridSizeY) * 4;
				int meio = (y + (x * gridSizeY)) * 4;
				int meioDireita = (y + (x * gridSizeY) + gridSizeY) * 4;
				int baixoEsquerda = (y + (x * gridSizeY) - (gridSizeY +1)) * 4;
				int baixo = (y + (x * gridSizeY) - 1) * 4;
				int baixoDireita = (y + (x * gridSizeY) + (gridSizeY -1)) * 4;



				//resetando os bloco que estiverem em posiçoes negativas e/ou sua posiçoes forem maiores menores que o vetor
				if (cimaEsquerda < 0) {
					cimaEsquerda = 0;
					estadoBloco [cimaEsquerda] = 0;
				}
				if (cima < 0) {
					cima = 0;
					estadoBloco [cima] = 0;
				}
				if (cimaDireita < 0) {
					cimaDireita = 0;
					estadoBloco [cimaDireita] = 0;
				}
				if (meioEsquerda < 0) {
					meioEsquerda = 0;
					estadoBloco [meioEsquerda] = 0;
				}
				if (meioDireita < 0) {
					meioDireita = 0;
					estadoBloco [meioDireita] = 0;
				}
				if (baixoEsquerda < 0) {
					baixoEsquerda = 0;
					estadoBloco [baixoEsquerda] = 0;
				}
				if (baixo < 0) {
					baixo = 0;
					estadoBloco [baixo] = 0;
				}
				if (baixoDireita < 0) {
					baixoDireita = 0;
					estadoBloco [baixoDireita] = 0;
				}

				blocosEmVolta.Add (materialBloco[cimaEsquerda]);
				blocosEmVolta.Add (materialBloco[cima]);
				blocosEmVolta.Add (materialBloco[cimaDireita]);
				blocosEmVolta.Add (materialBloco[meioEsquerda]);
				blocosEmVolta.Add (materialBloco[meio]);
				blocosEmVolta.Add (materialBloco[meioDireita]);
				blocosEmVolta.Add (materialBloco[baixoEsquerda]);
				blocosEmVolta.Add (materialBloco[baixo]);
				blocosEmVolta.Add (materialBloco[baixoDireita]);

				//definindo valores do dirBloco
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


				//definindo valores da lista direcaoBLoco

				//se o estado do bloco for 0
				if(estadoBloco[meio] == 0){
					direcaoBloco.Add (17);
					dirBloco = 0;
				}

				//tudo
				else if (
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
					direcaoBloco.Add (5);
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
					direcaoBloco.Add (10);
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
					direcaoBloco.Add (1);
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
					direcaoBloco.Add (2);
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
					direcaoBloco.Add (3);
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
					direcaoBloco.Add (4);
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
					direcaoBloco.Add (6);
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
					direcaoBloco.Add (7);
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
					direcaoBloco.Add (8);
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
					direcaoBloco.Add (9);
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
					direcaoBloco.Add (11);
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
					direcaoBloco.Add (12);
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
					direcaoBloco.Add (13);
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
					direcaoBloco.Add (14);
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
					direcaoBloco.Add (15);
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
					direcaoBloco.Add (16);
					dirBloco = 0;
				} 

			}
		}
	}
}
