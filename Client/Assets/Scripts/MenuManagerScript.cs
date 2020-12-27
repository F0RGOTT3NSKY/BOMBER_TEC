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
        switch (widthMap)
        {
            case 15:
                widthMap = 20;
                break;
            case 20:
                widthMap = 25;
                break;
            case 25:
                widthMap = 50;
                break;
            case 50:
                widthMap = 50;
                break;
        }
        WidthText.text = widthMap.ToString();
    }

    public void WidthLess()
    {
        switch (widthMap)
        {
            case 15:
                widthMap = 15;
                break;
            case 20:
                widthMap = 15;
                break;
            case 25:
                widthMap = 20;
                break;
            case 50:
                widthMap = 25;
                break;
        }
        WidthText.text = widthMap.ToString();
    }

    public void HeightPlus()
    {
        switch (heightMap)
        {
            case 15:
                heightMap = 20;
                break;
            case 20:
                heightMap = 25;
                break;
            case 25:
                heightMap = 50;
                break;
            case 50:
                heightMap = 50;
                break;
        }
        HeightText.text = heightMap.ToString();
    }

    public void HeightLess()
    {
        switch (heightMap)
        {
            case 15:
                heightMap = 15;
                break;
            case 20:
                heightMap = 15;
                break;
            case 25:
                heightMap = 20;
                break;
            case 50:
                heightMap = 25;
                break;
        }
        HeightText.text = heightMap.ToString();
    }


    // Cambiar de escena

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }


}
