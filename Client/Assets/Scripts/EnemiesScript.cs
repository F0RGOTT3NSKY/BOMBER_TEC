using UnityEngine;
using System.Collections;
using System;



public class EnemiesScript : MonoBehaviour
{

    // MenuScriptOptions
    public MenuManagerScript MenuOptions;


    // Lista de los enemigos
    private System.Collections.Generic.List<Enemy> ListEnemies;


    // Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;
    public GameObject PlayerEnemy;


    // Dimensiones del mapa;
    public static int MFMap = 25;
    public static int NCMap = 25;

    // Cantidad de enemigos
    public static int NEnemies = 6;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Entro");

        //MenuOptions = GetComponent<MenuManagerScript>();
        
        //MFMap = MenuOptions.heightMap;
        //NCMap = MenuOptions.widthMap;
        //NEnemies = MenuOptions.nPlayers;


        
        ListEnemies = new System.Collections.Generic.List<Enemy>();

        for(int i = 0; i < NEnemies; i++)
        {

            switch (i){
                case 1:
                    screenPosition = new Vector3(1f, 2f, 1f);
                    break;
                case 2:
                    screenPosition = new Vector3(MFMap - 2, 2f, NCMap - 2);
                    break;
                case 3:
                    screenPosition = new Vector3(1, 2f, NCMap - 2);
                    break;
                case 4:
                    screenPosition = new Vector3(MFMap - 2, 2f, 1f);
                    break;
                case 5:
                    screenPosition = new Vector3(MFMap/2, 2f, 1f);
                    break;
                case 6:
                    screenPosition = new Vector3(MFMap/2, 2f, NCMap-2);
                    break;
                case 7:
                    screenPosition = new Vector3(MFMap-2, 2f, NCMap/2);
                    break;
                case 8:
                    screenPosition = new Vector3(1F, 2f, NCMap/2);
                    break;
            }


            // Se crea el nuevo Genoma y se añade los valores de los genes
            Enemy aux = new Enemy();
         

            // Se añade el genoma la lista de genomas
            ListEnemies.Add(aux);
            
            //GameObject a = Instantiate(PlayerEnemy) as GameObject;
            //a.transform.position = screenPosition;
        }

        
    }

    // Update is called once per frame
    void Update()
    {/*

        for(int i = 0; i < NEnemies; i++)
        {
            ListEnemies[i].UpdateEnemyMovement(6);
        }
       */
    }





}

