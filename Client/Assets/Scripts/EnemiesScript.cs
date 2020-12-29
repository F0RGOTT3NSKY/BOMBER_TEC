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

        frame = 0;
    }

    /// Update is called once per frame
    void Update()
    {
        int[] Pos1 = new int[2];
        int[] Pos2 = new int[2];

        if (frame % 250 == 0)
        {
            for (int i = 0; i < NEnemies - 1; i++)
            {
                Pos1[0] = (int)ListEnemies[i].transform.position.x;
                Pos1[1] = (int)ListEnemies[i].transform.position.z;

                Pos2[0] = (int)ListEnemies[i + 1].transform.position.x;
                Pos2[1] = (int)ListEnemies[i + 1].transform.position.z;

                if (path.findPath(Pos1, Pos2))
                {
                    int[] tmp = path.getNearNodo();
                    movementAi(ListEnemies[i], tmp);

                }
                else
                {
                    Debug.Log("NO HAY CAMINO");
                }
            }
            // el ultimo enemigo busca al primero

            Pos1[0] = (int)ListEnemies[NEnemies - 1].transform.position.x;
            Pos1[1] = (int)ListEnemies[NEnemies - 1].transform.position.z;

            Pos2[0] = (int)ListEnemies[0].transform.position.x;
            Pos2[1] = (int)ListEnemies[0].transform.position.z;

            if (path.findPath(Pos1, Pos2))
            {
                int[] tmp = path.getNearNodo();
                movementAi(ListEnemies[NEnemies - 1], tmp);

            }
            else
            {
                Debug.Log("NO HAY CAMINO");
            }

            frame = 0;

        }

        frame++;
    }

    private void movementAi(GameObject _enemy, int[] _direccion)
    {
        int xPos = (int)_enemy.transform.position.x;
        int yPos = (int)_enemy.transform.position.z;

        if (xPos > _direccion[0])
        {
            //Muevo ARRIBA
            _enemy.GetComponent<Enemy>().UpdateEnemyMovement(1);

        }
        else if (xPos < _direccion[0])
        {
            //Muevo ABAJO
            _enemy.GetComponent<Enemy>().UpdateEnemyMovement(3);

        }
        else if (yPos > _direccion[1])
        {
            //Muevo IZQUIERDA
            _enemy.GetComponent<Enemy>().UpdateEnemyMovement(2);

        }
        else if (yPos < _direccion[1])
        {
            //Muevo DERECHA
            _enemy.GetComponent<Enemy>().UpdateEnemyMovement(4);

        }
        else
        {
            Debug.Log("ERROR NO CHANGE IN POSITION");
            _enemy.GetComponent<Enemy>().UpdateEnemyMovement(5);
        }
    }
}

