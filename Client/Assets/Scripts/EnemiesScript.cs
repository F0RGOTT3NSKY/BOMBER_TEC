/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Script que se encarga de realizar las ejecuciones de las acciones de los jugadores enemigos
*/

using UnityEngine;
using System.Collections.Generic;


/*!
* @class EnemiesScript
* @brief EnemiesScript Clase que se encarga de realizar los movimientos de los jugadores enemigos
* @details Esta clase se encarga de asignar el genoma a cada jugador enemigo, además, realiza reparte la ejecucion de estos en los frames.
* @public
*/

public class EnemiesScript : MonoBehaviour
{
    /// MenuScriptOptions
    public MenuManagerScript mms;


    /// Lista de los enemigos
    private List<GameObject> ListEnemies;

    /// Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;

    /// Objeto que almacena el jugador enemigo
    public GameObject PlayerEnemy;


    /// Numero de Filas del mapa
    public static int MFMap = 25;

    /// Numero de Columnas del mapa
    public static int NCMap = 25;

    /// Cantidad de enemgos
    public static int NEnemies = 6;

    // Matriz del Menu
    public float[,] mapMatriz;


    private int frame;

    /// Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            NEnemies = mms.nPlayers - 2;
            MFMap = mms.heightMap;
            NCMap = mms.widthMap;
            mapMatriz = mms.mapMatriz;

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }



        ListEnemies = new List<GameObject>();

        for (int i = 0; i < NEnemies; i++)
        {

            switch (i)
            {
                case 0:
                    screenPosition = new Vector3(1f, 2f, NCMap - 2f);
                    break;
                case 1:
                    screenPosition = new Vector3(MFMap - 2f, 2f, 1f);
                    break;
                case 2:
                    screenPosition = new Vector3(MFMap / 2, 2f, 1f);
                    break;
                case 3:
                    screenPosition = new Vector3(MFMap / 2, 2f, NCMap - 2f);
                    break;
                case 4:
                    screenPosition = new Vector3(MFMap - 2f, 2f, NCMap / 2);
                    break;
                case 5:
                    screenPosition = new Vector3(1F, 2f, NCMap / 2);
                    break;
            }

            GameObject a = Instantiate(PlayerEnemy) as GameObject;
            a.transform.position = screenPosition;

            ListEnemies.Add(a);

        }

        frame = 0;
    }





}

