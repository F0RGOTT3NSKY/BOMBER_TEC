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

    private int frame;
    private int elegido;

    /// Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            NEnemies = mms.nEnemies;
            MFMap = mms.heightMap;
            NCMap = mms.widthMap;

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

        for (int i = 0; i < NEnemies; i++)
        {
            ListEnemies[i].GetComponent<Enemy>().setListaEnemigos(ListEnemies);
        }
        
        frame = 1;
        elegido = -1;
    }

    /// Update is called once per frame
    void Update()
    {
        if (frame % 1000 == 0)
        {
            System.Random random = new System.Random();
            int elegido = random.Next(0, NEnemies);
            try
            {
                //ListEnemies[elegido].GetComponent<Enemy>().UpdateEnemyMovement(5);
                ListEnemies[elegido].GetComponent<Enemy>().safe = false;
            }
            catch (System.Exception e) { }
            frame = 0;
        }

        if (frame % 100 == 0)
        {
            if(elegido >= 0)
            {
                try
                {
                    ListEnemies[elegido].GetComponent<Enemy>().safe = true;
                }
                catch (System.Exception e) { }
            }
        }
        
        frame++;
    }
}

