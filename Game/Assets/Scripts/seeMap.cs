using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seeMap : MonoBehaviour
{

    // Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition ;

    // Matriz que almacena el mapa del suelo
    private float[,] map;

    

    // Bloques del juego
    public GameObject floorPrefab;
    public GameObject fixBlockPrefab;
    public GameObject destructibleBlockPrefab;

    // Tamaño del mapa y cantidad de personajes
    public int Rows;
    public int Colums;
    public int Players;


    // Start is called before the first frame update
    void Start()
    {
        
        randomMap(Rows, Colums, Players);
        createMap(map);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Método para generar los mapas
    private void createMap(float[,] _map)
    {

        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);


        for (int i = 0; i < NFilas; i++)
        {
            for (int j = 0; j < NColumnas; j++)
            {
                if (_map[i, j] == 1f)
                {
                    screenPosition = new Vector3(i, 1f, j);
                    GameObject a = Instantiate(floorPrefab) as GameObject;
                    a.transform.position = screenPosition;
                } 
                else if (_map[i, j] == 2f)
                {
                    screenPosition = new Vector3(i, 2f, j);
                    GameObject a = Instantiate(fixBlockPrefab) as GameObject;
                    a.transform.position = screenPosition;
                }
                else if (_map[i,j] == 3f)
                {
                    screenPosition = new Vector3(i, 2f, j);
                    //GameObject a = Instantiate(destructibleBlockPrefab) as GameObject;
                    //a.transform.position = screenPosition;

                    screenPosition = new Vector3(i, 1f, j);
                    GameObject b = Instantiate(floorPrefab) as GameObject;
                    b.transform.position = screenPosition;
                }
            }
        }
    }

    // Método para generar mapas aleatorios
    private void randomMap(int filas, int columnas, int personajes)
    {
        map = new float[filas, columnas];

        System.Random randomBlock = new System.Random();

        int NFilas = map.GetLength(0);
        int NColumnas = map.GetLength(1);

        int Filas = NFilas - 1;
        int Columnas = NColumnas - 1;

        for (int i = 0; i < NFilas; i ++)
        {
            for (int j = 0; j < NColumnas; j++)
            {
                // Crear el marco derecho e iquierdo del mapa
                if (j == 0 || j == Columnas)
                {
                    map[i, j] = 2f;
                }
                // Crea el marco superior e inferior del mapa
                else if (i == 0 || i == Filas)
                {
                    map[i, j] = 2f;
                }
                // Crea el interior del mapa
                else
                {
                    map[i, j] = randomBlock.Next(1,4);
                }
            }
        }


        int Filas_2 = Filas / 2;
        int Columnas_2 = Columnas / 2;


        // Espacio para el spawn de los personajes
        if (personajes >= 1)
        {
            map[1, 1] = 1f;
            map[1, 2] = 1f;
            map[2, 1] = 1f;

            if (personajes >= 2)
            {
                map[Filas - 1, Columnas - 1] = 1f;
                map[Filas - 2, Columnas - 1] = 1f;
                map[Filas - 1, Columnas - 2] = 1f;

                if (personajes >= 3)
                {
                    map[1, Columnas - 2] = 1f;
                    map[1, Columnas - 1] = 1f;
                    map[2, Columnas - 1] = 1f;

                    if (personajes >= 4)
                    {
                        map[Filas - 2, 1] = 1f;
                        map[Filas - 1, 1] = 1f;
                        map[Filas - 1, 2] = 1f;

                        if (personajes >= 5)
                        {
                            map[Filas_2 - 1, 1] = 1f;
                            map[Filas_2, 1] = 1f;
                            map[Filas_2, 2] = 1f;
                            map[Filas_2 + 1, 1] = 1f;

                            if (personajes >= 6)
                            {
                                map[Filas_2 - 1, Columnas - 1] = 1f;
                                map[Filas_2, Columnas - 2] = 1f;
                                map[Filas_2, Columnas - 1] = 1f;
                                map[Filas_2 + 1, Columnas - 1] = 1f;

                                if (personajes >= 7)
                                {
                                    map[Filas - 1, Columnas_2 - 1] = 1f;
                                    map[Filas - 1, Columnas_2] = 1f;
                                    map[Filas - 1, Columnas_2 + 1] = 1f;
                                    map[Filas - 2, Columnas_2] = 1f;

                                    if (personajes >= 8)
                                    {
                                        map[1, Columnas_2 - 1] = 1f;
                                        map[1, Columnas_2] = 1f;
                                        map[1, Columnas_2 + 1] = 1f;
                                        map[2, Columnas_2] = 1f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        
        
        
        
        
        

    }


}
