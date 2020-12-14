using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{

    // Numero de Jugadores
    public int nPlayers = 2;

    // Dimensiones del Mapa
    public int widthMap = 15;
    public int heightMap = 15;


    // Textos de la pantalla de menu
    public Text PlayerText;
    public Text WidthText;
    public Text HeightText;


    // Awake 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Sumar los datos del menu
    public void PlayersPlus()
    {
        if(nPlayers < 8)
        {
            nPlayers++;
        }
        PlayerText.text = nPlayers.ToString();

    }

    public void PlayersLess()
    {
        if(nPlayers > 2)
        {
            nPlayers--;
        }
        PlayerText.text = nPlayers.ToString();
    }

    public void WidthPlus()
    {
        if(widthMap < 50)
        {
            widthMap++;
        }
        WidthText.text = widthMap.ToString();
    }

    public void WidthLess()
    {
        if (widthMap > 15)
        {
            widthMap--;
        }
        WidthText.text = widthMap.ToString();
    }

    public void HeightPlus()
    {
        if(heightMap < 50)
        {
            heightMap++;
        }
        HeightText.text = heightMap.ToString();
    }

    public void HeightLess()
    {
        if (heightMap > 15)
        {
            heightMap--;
        }
        HeightText.text = heightMap.ToString();
    }


    // Cambiar de escena

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }


}
