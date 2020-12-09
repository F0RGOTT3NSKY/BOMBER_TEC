using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seeMap : MonoBehaviour
{

    // Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition ;

    // Matriz que almacena el mapa del suelo
    private float[,] map;


    // Dimensiones del pama;
    private int NFilas_Map = 14;
    private int NColumnas_Map = 14;

    // Cantidad de jugadores
    private int NPlayers_Map = 8;





    // Bloques del juego
    public GameObject floorPrefab;
    public GameObject fixBlockPrefab;
    public GameObject destructibleBlockPrefab;



    // Start is called before the first frame update
    void Start()
    {
        
        map = randomMap(NFilas_Map, NColumnas_Map);

        map = checkPlayersWay(map, NPlayers_Map) ;



        createMap(map);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /*
     
     --------------------------- MAPAS ALEATORIOS
     
     
     */



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
                    GameObject a = Instantiate(destructibleBlockPrefab) as GameObject;
                    a.transform.position = screenPosition;

                    screenPosition = new Vector3(i, 1f, j);
                    GameObject b = Instantiate(floorPrefab) as GameObject;
                    b.transform.position = screenPosition;
                }
            }
        }
    }

    // Método para generar mapas aleatorios
    private float[,] randomMap(int filas, int columnas)
    {
        float[,] _map = new float[filas, columnas];

        System.Random randomBlock = new System.Random();

        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);

        int Filas = NFilas - 1;
        int Columnas = NColumnas - 1;

        for (int i = 0; i < NFilas; i ++)
        {
            for (int j = 0; j < NColumnas; j++)
            {
                // Crear el marco derecho e iquierdo del mapa
                if (j == 0 || j == Columnas)
                {
                    _map[i, j] = 2f;
                }
                // Crea el marco superior e inferior del mapa
                else if (i == 0 || i == Filas)
                {
                   _map[i, j] = 2f;
                }
                // Crea el interior del mapa
                else
                {
                    _map[i, j] = randomBlock.Next(1,4);
                }
            }
        }


        
        
        return _map;
        
    }



    /*
     
     ------------------------ BACKTRAKING -------------------------------
     
     */



    // Métodos para realizar el backtracking del laberinto, el objetivo es buscar si hay camino disponible entre dos puntos

    // Método para inicializar los valores
    private bool Bt(float[,] _map, int[] ptinicio, int[] ptfin)
    {


        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);


        return rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 5);


    }

    // Método recursivo de backtraking
    private bool rutasBT(float[,] _map, int NFilas, int NColumnas, int[] ptinicio, int[] ptfin, int dir)
    {

        _map[ptinicio[0], ptinicio[1]] = 4f;

        // Condición de si haber encontrado un camino

        if (ptinicio[0] == ptfin[0] && ptinicio[1] == ptfin[1])
        {
            return true;
        }

        // Condicion para evitar salir de mapa

        //Norte, Se evalua también que el índice no provenga del sur
        if (ptinicio[0] - 1 >= 0 && dir != 2)
        {
            if (_map[ptinicio[0] - 1, ptinicio[1]] != 2f && _map[ptinicio[0] - 1, ptinicio[1]] != 4f)
            {
                ptinicio[0]--;
                if (rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 8))
                {
                    return true;
                }
            }
        }

        //Sur, Se evalua también que el índice no provenga del norte
        if (ptinicio[0] + 1 < NFilas && dir != 8)
        {
            if (_map[ptinicio[0] + 1, ptinicio[1]] != 2f && _map[ptinicio[0] + 1, ptinicio[1]] != 4f)
            {
                ptinicio[0]++;
                if (rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 2))
                {
                    return true;
                }
            }
        }

        //Este, Se evalua tambien que el índice no provenga del oeste
        if (ptinicio[1] + 1 < NColumnas && dir != 4)
        {
            if (_map[ptinicio[0], ptinicio[1] + 1] != 2f && _map[ptinicio[0], ptinicio[1] + 1] != 4f)
            {
                ptinicio[1]++;
                if (rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 6))
                {
                    return true;
                }
            }
        }

        //Oeste, Se evalua también que el índice no provenga del este
        if (ptinicio[1] - 1 >= 0 && dir != 6)
        {
            if (_map[ptinicio[0], ptinicio[1] - 1] != 2f && _map[ptinicio[0], ptinicio[1] - 1] != 4f)
            {
                ptinicio[1]--;
                if (rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 4))
                {
                    return true;
                }
            }

        }




        switch (dir)
        {
            case 8:
                ptinicio[0]++;
                break;
            case 2:
                ptinicio[0]--;
                break;
            case 6:
                ptinicio[1]--;
                break;
            case 4:
                ptinicio[1]++;
                break;
        }


        _map[ptinicio[0], ptinicio[1]] = 1f;

        return false;


    }

    private float[,] changeMapBT(float[,] _map, int[] ptinicio, int[] ptfin)
    {

        int F_Inicial = ptinicio[0];
        int C_Inicial = ptinicio[1];

        int F_Final = ptfin[0];
        int C_Final = ptfin[1];

        for (int f = F_Inicial; f < F_Final; f++)
        {

            if (_map[f, C_Inicial] == 2)
            {
                _map[f, C_Inicial] = 3f;
            }
        }

        for (int c = C_Inicial; c < C_Final; c++)
        {

            if (_map[F_Final, c] == 2)
            {
                _map[F_Final, c] = 3f;
            }
        }

      



        return _map;
    }




    // Metodo para checkear personajes
    private float[,] checkPlayersWay(float[,] _map, int NPlayers)
    {

        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);

        int[] PTinicio = new int[] { 1, 1 };
        int[] PTfin;

        if (NPlayers >= 2)
        {
            PTfin = new int[] { NFilas - 2, NColumnas - 2};
            if (!Bt(_map, PTinicio, PTfin))
            {
                _map = changeMapBT(_map, PTinicio, PTfin);
            }

            if (NPlayers >= 3)
            {
                PTfin = new int[] { 1, NColumnas - 2 };
                if (!Bt(_map, PTinicio, PTfin))
                {
                    _map = changeMapBT(_map, PTinicio, PTfin);
                }

                if (NPlayers >= 4)
                {
                    PTfin = new int[] { NFilas - 2, 1 };
                    if (!Bt(_map, PTinicio, PTfin))
                    {
                        _map = changeMapBT(_map, PTinicio, PTfin);
                    }

                    if (NPlayers >= 5)
                    {
                        PTfin = new int[] { (NFilas-2)/2, 1 };
                        if (!Bt(_map, PTinicio, PTfin))
                        {
                            _map = changeMapBT(_map, PTinicio, PTfin);
                        }

                        if (NPlayers >= 6)
                        {
                            PTfin = new int[] { (NFilas-2)/2, NColumnas - 2 };
                            if (!Bt(_map, PTinicio, PTfin))
                            {
                                _map = changeMapBT(_map, PTinicio, PTfin);
                            }

                            if (NPlayers >= 7)
                            {
                                PTfin = new int[] { NFilas - 2, (NColumnas-2)/2 };
                                if (!Bt(_map, PTinicio, PTfin))
                                {
                                    _map = changeMapBT(_map, PTinicio, PTfin);
                                }

                                if (NPlayers >= 8)
                                {
                                    PTfin = new int[] { 1, (NColumnas-2)/2 };
                                    if (!Bt(_map, PTinicio, PTfin))
                                    {
                                        _map = changeMapBT(_map, PTinicio, PTfin);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }




        int personajes = NPlayers;
        int Filas = _map.GetLength(0) - 1;
        int Columnas = _map.GetLength(1) -1 ;

        int Filas_2 = Filas / 2;
        int Columnas_2 = Columnas / 2;

        // Espacio para el spawn de los personajes
        if (personajes >= 1)
        {
            _map[1, 1] = 1f;
            _map[1, 2] = 1f;
            _map[2, 1] = 1f;

            if (personajes >= 2)
            {
                _map[Filas - 1, Columnas - 1] = 1f;
                _map[Filas - 2, Columnas - 1] = 1f;
                _map[Filas - 1, Columnas - 2] = 1f;

                if (personajes >= 3)
                {
                    _map[1, Columnas - 2] = 1f;
                    _map[1, Columnas - 1] = 1f;
                    _map[2, Columnas - 1] = 1f;

                    if (personajes >= 4)
                    {
                        _map[Filas - 2, 1] = 1f;
                        _map[Filas - 1, 1] = 1f;
                        _map[Filas - 1, 2] = 1f;

                        if (personajes >= 5)
                        {
                            _map[Filas_2 - 1, 1] = 1f;
                            _map[Filas_2, 1] = 1f;
                            _map[Filas_2, 2] = 1f;
                            _map[Filas_2 + 1, 1] = 1f;

                            if (personajes >= 6)
                            {
                                _map[Filas_2 - 1, Columnas - 1] = 1f;
                                _map[Filas_2, Columnas - 2] = 1f;
                                _map[Filas_2, Columnas - 1] = 1f;
                                _map[Filas_2 + 1, Columnas - 1] = 1f;

                                if (personajes >= 7)
                                {
                                    _map[Filas - 1, Columnas_2 - 1] = 1f;
                                    _map[Filas - 1, Columnas_2] = 1f;
                                    _map[Filas - 1, Columnas_2 + 1] = 1f;
                                    _map[Filas - 2, Columnas_2] = 1f;

                                    if (personajes >= 8)
                                    {
                                        _map[1, Columnas_2 - 1] = 1f;
                                        _map[1, Columnas_2] = 1f;
                                        _map[1, Columnas_2 + 1] = 1f;
                                        _map[2, Columnas_2] = 1f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        // Corregir desperfectos del mapa
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(0); j++)
            {
                if (_map[i, j] == 4f)
                {
                    _map[i, j] = 3f;
                }
            }

        }

        return _map;
    }





}
