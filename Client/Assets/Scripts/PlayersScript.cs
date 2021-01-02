using System.Collections.Generic;
using UnityEngine;

public class PlayersScript : MonoBehaviour
{
   
    /// Menu Manager Script
    public MenuManagerScript mms;

    /// Global State Manager
    public GlobalStateManager globalManager;

    /// Lista de los jugadores
    private List<GameObject> ListPlayers;

    /// Lista de los enemigos
    private List<GameObject> ListEnemies;

    /// Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;

    /// Objeto que almacena el jugador
    public GameObject PlayerPrefab;

    /// Objeto que almacena el enemigo
    public GameObject EnemyPrefab;


    /// Numero de Filas del mapa
    public static int MFMap = 25;

    /// Numero de Columnas del mapa
    public static int NCMap = 25;

    /// Cantidad de Jugadores
    public static int nPlayers = 6;

    /// Cantidad de Enemigos
    public static int nEnemies = 0;

    // Matriz del Menu
    public float[,] mapMatriz;

    /// Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            nPlayers = mms.nPlayers;
            nEnemies = mms.nEnemies;
            MFMap = mms.heightMap;
            NCMap = mms.widthMap;
            mapMatriz = mms.mapMatriz;

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }



        ListPlayers = new List<GameObject>();
        ListEnemies = new List<GameObject>();

        for (int i = 1; i < nPlayers; i++)
        {

            switch (i)
            {
                case 1:
                    screenPosition = new Vector3(1f, 2f, 1f);
                    break;
                case 2:
                    screenPosition = new Vector3(MFMap - 2f, 2f, NCMap - 2f);
                    break;
                case 3:
                    screenPosition = new Vector3(1f, 2f, NCMap - 2f);
                    break;
                case 4:
                    screenPosition = new Vector3(MFMap - 2f, 2f, 1f);
                    break;
            }

            GameObject a = Instantiate(PlayerPrefab) as GameObject;
            a.transform.position = screenPosition;

            a.GetComponent<Players>().playerNumber = i;

            ListPlayers.Add(a);

        }


        for (int i = 0; i < nEnemies; i++)
        {
            switch (i)
            {
                case 1:
                    screenPosition = new Vector3(MFMap / 2, 2f, 1f);
                    break;
                case 2:
                    screenPosition = new Vector3(MFMap / 2, 2f, NCMap - 2f);
                    break;
                case 3:
                    screenPosition = new Vector3(MFMap - 2f, 2f, NCMap / 2);
                    break;
                case 4:
                    screenPosition = new Vector3(1F, 2f, NCMap / 2);
                    break;
            }

            GameObject a = Instantiate(EnemyPrefab) as GameObject;
            a.transform.position = screenPosition;

            a.GetComponent<Players>().playerNumber = 0;

            ListEnemies.Add(a);

        }

    }

}
