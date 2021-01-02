using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// Objetos que guardan las imagenes de las vidas
    public Sprite life0;
    public Sprite life1;
    public Sprite life2;
    public Sprite life3;
    public Sprite life4;
    public Sprite life5;

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
        Application.targetFrameRate = 60;
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

            Debug.Log("ScreenWidth: " + Screen.width + "ScreenHeight: " + Screen.height);
            Debug.Log("ScreenWidth: " + canvas.transform.localPosition.x + "ScreenHeight: " + canvas.transform.localPosition.y);
            switch (i)
            {
                case 1:
                    player.transform.position = new Vector3(1f, 2f, 1f);
                    rt.transform.localPosition = new Vector3(-canvas.transform.localPosition.x + 100, canvas.transform.localPosition.y - (30 + 80 * (i - 1)), 0);
                    break;
                case 2:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3(-canvas.transform.localPosition.x + 100, canvas.transform.localPosition.y - (30 + 80 * (i - 1)), 0);
                    break;
                case 3:
                    player.transform.position = new Vector3(1f, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3(-canvas.transform.localPosition.x + 100, canvas.transform.localPosition.y - (30 + 80 * (i - 1)), 0);
                    break;
                case 4:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, 1f);
                    rt.transform.localPosition = new Vector3(-canvas.transform.localPosition.x + 100, canvas.transform.localPosition.y - (30 + 80 * (i - 1)), 0);
                    break;
                case 5:
                    player.transform.position = new Vector3(MFMap / 2, 2f, 1f);
                    rt.transform.localPosition = new Vector3(canvas.transform.localPosition.x - 120, canvas.transform.localPosition.y - (30 + 80 * (i - 5)), 0);
                    break;
                case 6:
                    player.transform.position = new Vector3(MFMap / 2, 2f, NCMap - 2f);
                    rt.transform.localPosition = new Vector3(canvas.transform.localPosition.x - 120, canvas.transform.localPosition.y - (30 + 80 * (i - 5)), 0);
                    break;
                case 7:
                    player.transform.position = new Vector3(MFMap - 2f, 2f, NCMap / 2);
                    rt.transform.localPosition = new Vector3(canvas.transform.localPosition.x - 120, canvas.transform.localPosition.y - (30 + 80 * (i - 5)), 0);
                    break;
                case 8:
                    player.transform.position = new Vector3(1F, 2f, NCMap / 2);
                    rt.transform.localPosition = new Vector3(canvas.transform.localPosition.x - 120, canvas.transform.localPosition.y - (30 + 80 * (i - 5)), 0);
                    break;
            }
        }
        for (int i = 0; i < TotalPlayers; i++)
        {
            ListPlayers[i].GetComponent<Players>().myObject = ListPlayers[i];
            ListPlayers[i].GetComponent<Players>().setListaEnemigos(ListPlayers);
        }
    }
    private void Update()
    {
        for(int i = 0; i < TotalPlayers; i++)
        {
            if (ListPlayers[i])
            {
                canvas.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + (i + 1);
                //Update HealthBar
                canvas.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().playerHealth;
                canvas.transform.GetChild(i).transform.GetChild(1).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().playerHealth.ToString();
                //Update PotenciaBar
                canvas.transform.GetChild(i).transform.GetChild(2).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().potenciaGenoma;
                canvas.transform.GetChild(i).transform.GetChild(2).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().potenciaGenoma.ToString();
                //Update CuracionBar
                canvas.transform.GetChild(i).transform.GetChild(3).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().curarseGenoma;
                canvas.transform.GetChild(i).transform.GetChild(3).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().curarseGenoma.ToString();
                //Update EnfermedadBar
                canvas.transform.GetChild(i).transform.GetChild(4).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().enfermedadGenoma;
                canvas.transform.GetChild(i).transform.GetChild(4).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().enfermedadGenoma.ToString();
                //Update VidasBar
                canvas.transform.GetChild(i).transform.GetChild(5).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().vidasGenoma;
                canvas.transform.GetChild(i).transform.GetChild(5).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().vidasGenoma.ToString();
                // Update VelocidadBar
                canvas.transform.GetChild(i).transform.GetChild(6).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().velocidadGenoma;
                canvas.transform.GetChild(i).transform.GetChild(6).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().velocidadGenoma.ToString();
                //Update ProtectionBar
                canvas.transform.GetChild(i).transform.GetChild(7).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().protectionGenoma;
                canvas.transform.GetChild(i).transform.GetChild(7).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().protectionGenoma.ToString();
                //Update SuerteBar
                canvas.transform.GetChild(i).transform.GetChild(8).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().suerteGenoma;
                canvas.transform.GetChild(i).transform.GetChild(8).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().suerteGenoma.ToString();
                //Update BombasBar
                canvas.transform.GetChild(i).transform.GetChild(9).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().bombGenoma;
                canvas.transform.GetChild(i).transform.GetChild(9).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().bombGenoma.ToString();
                //Update DistanciaBar
                canvas.transform.GetChild(i).transform.GetChild(10).gameObject.GetComponent<Slider>().value = ListPlayers[i].GetComponent<Players>().distanceGenoma;
                canvas.transform.GetChild(i).transform.GetChild(10).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ListPlayers[i].GetComponent<Players>().distanceGenoma.ToString();
                //Update CanDromBomb
                canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(0).gameObject.GetComponent<Image>().enabled = ListPlayers[i].GetComponent<Players>().canDropBombs == true ? true : false;
                //Update Healing
                canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(1).gameObject.GetComponent<Image>().enabled = ListPlayers[i].GetComponent<Players>().canHeal == true ? true : false;
                //Update Sickness
                canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(3).gameObject.GetComponent<Image>().enabled = ListPlayers[i].GetComponent<Players>().canGetSick == true ? true : false;
                //Update CurrentLifes
                switch (ListPlayers[i].GetComponent<Players>().usedLifes)
                {
                    case 0:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life0;
                        break;
                    case 1:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life1;
                        break;
                    case 2:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life2;
                        break;
                    case 3:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life3;
                        break;
                    case 4:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life4;
                        break;
                    case 5:
                        canvas.transform.GetChild(i).transform.GetChild(11).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = life5;
                        break;
                }
            }
            else
            {
                //Update HealthBar
                canvas.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(1).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update PotenciaBar
                canvas.transform.GetChild(i).transform.GetChild(2).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(2).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update CuracionBar
                canvas.transform.GetChild(i).transform.GetChild(3).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(3).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update EnfermedadBar
                canvas.transform.GetChild(i).transform.GetChild(4).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(4).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update VidasBar
                canvas.transform.GetChild(i).transform.GetChild(5).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(5).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                // Update VelocidadBar
                canvas.transform.GetChild(i).transform.GetChild(6).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(6).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update ProtectionBar
                canvas.transform.GetChild(i).transform.GetChild(7).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(7).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update SuerteBar
                canvas.transform.GetChild(i).transform.GetChild(8).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(8).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update BombasBar
                canvas.transform.GetChild(i).transform.GetChild(9).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(9).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                //Update DistanciaBar
                canvas.transform.GetChild(i).transform.GetChild(10).gameObject.GetComponent<Slider>().value = 0;
                canvas.transform.GetChild(i).transform.GetChild(10).transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
            }
            
        }
        
    }
}