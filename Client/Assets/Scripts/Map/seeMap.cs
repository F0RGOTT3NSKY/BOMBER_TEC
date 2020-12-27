/*!
* @file seeMap.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo que crea y renderiza el mapa.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*!
* @class seeMap
* @brief Crea un mapa aleatoria para luego renderizardo
* @details La clase seeMap tiene el proposito de generar una matriz aleatoria con las dimensiones que le de el usuario. Luego asegurar con un algoritmo de backtracking que se puedan viajar correctamente por el mismo y por último renderiza con ayuda del motor gráfico de Unity
* @public
*/

public class seeMap : MonoBehaviour
{
    /// Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition ;
    /// Matriz que almacena el mapa del suelo
    private float[,] map;
    /// Numero de filas del mapa
    public static int NFilas_Map = 25;
    /// Numero de columnas del mapa
    public static int NColumnas_Map = 25;
    /// Cantidad de jugadores
    private int NPlayers_Map = 8;
    /// Bloques de piso del juego
    public GameObject floorPrefab;
    /// Bloques fijios del juego
    public GameObject fixBlockPrefab;
    /// Bloques destruibles del juego
    public GameObject destructibleBlockPrefab;
    /// MenuManagerScript
    public MenuManagerScript mms;


    /*!
     * @brief Start() is called before the first frame update
     * @details Se usa para iniciar la creacion del mapa aleatoria.
     */
    public void Start()
    {
        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            NPlayers_Map = mms.nPlayers;
            NFilas_Map = mms.heightMap;
            NColumnas_Map = mms.widthMap;
            map = mms.mapMatriz;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }


        createMap(map);
    }
    /*!
    * @brief createMap() Este método renderiza la matriz a los cubos en unity
    * @param _map Es la matriz a renderizar
    */
    public void createMap(float[,] _map)
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
                    /*
                    screenPosition = new Vector3(i, 2f, j);
                    GameObject a = Instantiate(destructibleBlockPrefab) as GameObject;
                    a.transform.position = screenPosition;*/

                    screenPosition = new Vector3(i, 1f, j);
                    GameObject b = Instantiate(floorPrefab) as GameObject;
                    b.transform.position = screenPosition;
                }
            }
        }
    }

    
}
