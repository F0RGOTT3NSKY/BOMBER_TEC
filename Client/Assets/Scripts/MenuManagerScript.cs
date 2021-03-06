﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{

    // Numero de Jugadores
    public int nPlayers = 1;

    // Numero de Jugadores
    public int nEnemies = 1;

    // Dimensiones del Mapa
    public int widthMap = 25;
    public int heightMap = 25;


    // Textos de la pantalla de menu
    public Text PlayerText;
    public Text DimensionsText;
    public Text EnemiesText;

    // Generador de la matriz
    public MapGenerator mapGenerator;

    // Matriz
    public float[,] mapMatriz;

    // Awake 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = new MapGenerator();
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Sumar los datos del menu
    public void PlayersPlus()
    {
        if (nPlayers < 4)
        {
            nPlayers++;
        }
        PlayerText.text = "Players: " + nPlayers.ToString();

    }

    public void PlayersLess()
    {
        if (nPlayers > 1)
        {
            nPlayers--;
        }
        PlayerText.text = "Players: " + nPlayers.ToString();
    }

    public void EnemiesPlus()
    {
        if (nEnemies < 4)
        {
            nEnemies++;
        }
        EnemiesText.text = "Enemies: " + nEnemies.ToString();

    }

    public void EnemiesLess()
    {
        if (nEnemies > 0)
        {
            nEnemies--;
        }
        EnemiesText.text = "Enemies: " + nEnemies.ToString();
    }

    public void DimensionsPlus()
    {
        switch (widthMap)
        {
            case 15:
                widthMap = 20;
                heightMap = 20;
                break;
            case 20:
                widthMap = 25;
                heightMap = 25;
                break;
            case 25:
                widthMap = 50;
                heightMap = 50;
                break;
            case 50:
                widthMap = 50;
                heightMap = 50;
                break;
        }
        DimensionsText.text = "Dimensions: " + widthMap.ToString() + " x " + heightMap.ToString();
    }

    public void DimensionsLess()
    {
        switch (widthMap)
        {
            case 15:
                widthMap = 15;
                heightMap = 15;
                break;
            case 20:
                widthMap = 15;
                heightMap = 15;
                break;
            case 25:
                widthMap = 20;
                heightMap = 20;
                break;
            case 50:
                widthMap = 25;
                heightMap = 25;
                break;
        }
        DimensionsText.text = "Dimensions: " + widthMap.ToString() + " x " + heightMap.ToString();
    }



    // Cambiar de escena

    public void OnPlayButton()
    {
        mapMatriz = mapGenerator.randomMap(heightMap, widthMap);
        mapMatriz = mapGenerator.checkPlayersWay(mapMatriz, nPlayers + nEnemies);

        SceneManager.LoadScene("Game");
    }


}


public class MapGenerator
{

    public float[,] map;


    /// Método constructor
    public MapGenerator()
    {

    }


    /*!
    * @brief randomMap() Retorna una matriz aleatoria dependiendo del largo y ancho que se le ingresan
    * @param filas Numero de filas que desea contener la matriz
    * @param columnas Numero de columnas que desea contener la matriz
    */
    public float[,] randomMap(int filas, int columnas)
    {
        float[,] _map = new float[filas, columnas];

        System.Random randomBlock = new System.Random();

        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);

        int Filas = NFilas - 1;
        int Columnas = NColumnas - 1;

        for (int i = 0; i < NFilas; i++)
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
                    _map[i, j] = randomBlock.Next(1, 4);
                }
            }
        }
        return _map;
    }
    /*!
    * @brief Bt() Inicio de método backtracking, retorna falso o verdadero dependiendo de que si encuentra o no el camino
    * @param _map Matriz a analizar
    * @param ptinicio Punto en donde se desea iniciar la búsqueda
    * @param ptfin Punto en donde se desea finalizar la búsqueda
    */
    private bool Bt(float[,] _map, int[] ptinicio, int[] ptfin)
    {
        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);

        return rutasBT(_map, NFilas, NColumnas, ptinicio, ptfin, 5);
    }
    /*!
    * @brief rutasBt() Metodo recursivo del backtracking
    * @param _map Matriz a analizar
    * @param ptinicio Punto en donde se desea iniciar la búsqueda
    * @param ptfin Punto en donde se desea finalizar la búsqueda
    * @param Nfilas Numero de filas de la matriz
    * @param NColumnas Numero de columnas de la matriz
    * @param dir Direccion de donde proviene la busqueda de la matriz
    */
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
    /*!
    * @brief changeMapBT() Metodo para realizar los ajustes al mapa para conseguir el mapa
    * @param _map Matriz a analizar
    * @param ptinicio Punto en donde se desea iniciar la búsqueda
    * @param ptfin Punto en donde se desea finalizar la búsqueda
    */
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
    /*!
    * @brief checkPlayersWay() Analiza que haya camino entre los jugadores, retorna la matriz analizada
    * @param _map Matriz a analizar
    * @param NPlayers Numero de jugadores
    */
    public float[,] checkPlayersWay(float[,] _map, int NPlayers)
    {

        int NFilas = _map.GetLength(0);
        int NColumnas = _map.GetLength(1);

        int[] PTinicio = new int[] { 1, 1 };
        int[] PTfin;

        if (NPlayers >= 2)
        {
            PTfin = new int[] { NFilas - 2, NColumnas - 2 };
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
                        PTfin = new int[] { (NFilas - 2) / 2, 1 };
                        if (!Bt(_map, PTinicio, PTfin))
                        {
                            _map = changeMapBT(_map, PTinicio, PTfin);
                        }

                        if (NPlayers >= 6)
                        {
                            PTfin = new int[] { (NFilas - 2) / 2, NColumnas - 2 };
                            if (!Bt(_map, PTinicio, PTfin))
                            {
                                _map = changeMapBT(_map, PTinicio, PTfin);
                            }

                            if (NPlayers >= 7)
                            {
                                PTfin = new int[] { NFilas - 2, (NColumnas - 2) / 2 };
                                if (!Bt(_map, PTinicio, PTfin))
                                {
                                    _map = changeMapBT(_map, PTinicio, PTfin);
                                }

                                if (NPlayers >= 8)
                                {
                                    PTfin = new int[] { 1, (NColumnas - 2) / 2 };
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
        int Columnas = _map.GetLength(1) - 1;

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