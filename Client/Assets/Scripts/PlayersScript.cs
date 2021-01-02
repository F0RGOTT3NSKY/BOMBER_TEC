using System.Collections.Generic;
using UnityEngine;

public class PlayersScript : MonoBehaviour
{

    /// MenuScriptOptions
    public MenuManagerScript mms;


    /// Lista de los enemigos
    private List<Players> ListPlayers;

    /// Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;

    /// Objeto que almacena el jugador enemigo
    public GameObject PlayerPrefab;


    /// Numero de Filas del mapa
    public static int MFMap = 25;

    /// Numero de Columnas del mapa
    public static int NCMap = 25;

    /// Cantidad de enemgos
    public static int nPlayers = 6;

    // Matriz del Menu
    public float[,] mapMatriz;

    /// Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            nPlayers = mms.nPlayers - 2;
            MFMap = mms.heightMap;
            NCMap = mms.widthMap;
            mapMatriz = mms.mapMatriz;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }



        ListPlayers = new List<Players>();

        for (int i = 0; i < nPlayers; i++)
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


            // Se crea el nuevo Genoma y se añade los valores de los genes
            Enemy aux = new Enemy();


            // Se añade el genoma la lista de genomas
            Players.Add(aux);

            GameObject a = Instantiate(PlayerEnemy) as GameObject;
            a.transform.position = screenPosition;
        }


    }

    /// Update is called once per frame
    void Update()
    {

        for (int i = 0; i < NEnemies; i++)
        {
            ListEnemies[i].UpdateEnemyMovement(6);
        }

    }

}
