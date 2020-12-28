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

    public AStar path;

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
            path = new AStar(10, mapMatriz, MFMap, NCMap);

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }



        ListEnemies = new List<GameObject>();

        for(int i = 0; i < NEnemies; i++)
        {
            
            switch (i){
                case 0:
                    screenPosition = new Vector3(1f, 2f, NCMap - 2f);
                    break;
                case 1:
                    screenPosition = new Vector3(MFMap - 2f, 2f, 1f);
                    break;
                case 2:
                    screenPosition = new Vector3(MFMap/2 , 2f, 1f);
                    break;
                case 3:
                    screenPosition = new Vector3(MFMap/2 , 2f, NCMap -2f);
                    break;
                case 4:
                    screenPosition = new Vector3(MFMap - 2f, 2f, NCMap/2 );
                    break;
                case 5:
                    screenPosition = new Vector3(1F, 2f, NCMap/2);
                    break;
            }

            
            GameObject a = Instantiate(PlayerEnemy) as GameObject;
            a.transform.position = screenPosition;

            
            ListEnemies.Add(a);
          
        }

        
    }

    /// Update is called once per frame
    void Update()
    {
        // Actualizar posición del enemigo
        //ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(3);
        
        for (int i = 0; i < NEnemies; i++)
        {
            int[] Pos1 = { (int)ListEnemies[0].transform.position.x, (int)ListEnemies[0].transform.position.z };
            int[] Pos2 = { (int)ListEnemies[1].transform.position.x, (int)ListEnemies[1].transform.position.z };



            if (path.findPath(Pos1, Pos2))
            {
                int[] tmp = path.getNearNodo();

                int xPos = (int)ListEnemies[0].transform.position.x;
                int yPos = (int)ListEnemies[0].transform.position.y;

                if (xPos > tmp[0])
                {
                    //Muevo ARRIBA
                    ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(1);
                }
                else if (xPos < tmp[0])
                {
                    //Muevo ABAJO
                    ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(3);
                }
                else if (yPos > tmp[1])
                {
                    //Muevo IZQUIERDA
                    ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(2);
                }
                else if (yPos < tmp[1])
                {
                    //Muevo DERECHA
                    ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(4);
                }
                else
                {
                    Debug.Log("ERROR NO CHANGE IN POSITION");
                    ListEnemies[0].GetComponent<Enemy>().UpdateEnemyMovement(6);
                }
            }
            else
            {
                Debug.Log("No hay camino");
            }


        }

    }





}

