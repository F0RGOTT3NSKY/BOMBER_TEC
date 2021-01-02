using System.Collections.Generic;
using UnityEngine;

public class PlayersScript : MonoBehaviour
{
   
    /// Menu Manager Script
    public MenuManagerScript mms;

    /// Global State Manager
    public GlobalStateManager globalManager;

    /// Canvas
    public Canvas canvas;

    /// Lista de los jugadores
    private List<GameObject> ListPlayers;

    /// Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;

    /// Objeto que almacena el jugador
    public GameObject PlayerPrefab;

    /// Objeto que almacena el enemigo
    public GameObject EnemyPrefab;

    /// Objeto que almacena el display de atributos
    public GameObject playerDisplay;

    /// Objeto para instanciar a los jugadores.
    private GameObject player;

    /// Objeto para instanciar el display de atributos. 
    private GameObject diplay;

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

    public int TotalPlayers = 0;

    /// Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            nPlayers = mms.nPlayers;
            nEnemies = mms.nEnemies;
            TotalPlayers = nPlayers + nEnemies;
            MFMap = mms.heightMap;
            NCMap = mms.widthMap;
            mapMatriz = mms.mapMatriz;

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        ListPlayers = new List<GameObject>();
        
        for (int i = 1; i <= TotalPlayers; i++)
        {
            if (i <= nPlayers)
            {
                player = Instantiate(PlayerPrefab) as GameObject;
                player.GetComponent<Players>().playerNumber = i;
            }
            else
            {
                player = Instantiate(EnemyPrefab) as GameObject;
                player.GetComponent<Players>().playerNumber = 0;
            }
            player.transform.SetParent(GameObject.FindGameObjectWithTag("Players").transform, false);
            player.GetComponent<Players>().totalplayers = TotalPlayers;
            player.GetComponent<Players>().globalStateManager = globalManager;
            player.transform.position = screenPosition;
            ListPlayers.Add(player);
            GameObject display = Instantiate(playerDisplay) as GameObject;
            display.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            RectTransform rt = display.GetComponent<RectTransform>();
            //b.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "Player 1";
            Debug.Log("ScreenWidth: " + Screen.width + "ScreenHeight: "+ Screen.height);
            Debug.Log("ScreenWidth: " + Screen.width/3 + "ScreenHeight: " + Screen.height/2);
            switch (i)
            {
                case 1:
                    player.transform.position = new Vector3(1f, 2f, 1f);
                    rt.transform.localPosition = new Vector3((Screen.width/3) - 684, ((Screen.height/2)-21)-(80*(i-1)), 0);
                    break;
                case 2:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) - 684, ((Screen.height / 2) - 21) - (80 * (i - 1)), 0);
                    break;
                case 3:
                    player.transform.position = new Vector3(1f, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) - 684, ((Screen.height / 2) - 21) - (80 * (i - 1)), 0);
                    break;
                case 4:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, 1f);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) - 684, ((Screen.height / 2) - 21) - (80 * (i - 1)), 0);
                    break;
                case 5:
                    player.transform.position = new Vector3(MFMap / 2, 2f, 1f);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) + 36, ((Screen.height / 2) - 21) - (80 * (i - 5)), 0);
                    break;
                case 6:
                    player.transform.position = new Vector3(MFMap / 2, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) + 36, ((Screen.height / 2) - 21) - (80 * (i - 5)), 0);
                    break;
                case 7:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, NCMap / 2);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) + 36, ((Screen.height / 2) - 21) - (80 * (i - 5)), 0);
                    break;
                case 8:
                    player.transform.position = new Vector3(1F, 2f, NCMap / 2);
                    rt.transform.localPosition = new Vector3((Screen.width / 3) + 36, ((Screen.height / 2) - 21) - (80 * (i - 5)), 0);
                    break;
            }
            
        }
    }
}