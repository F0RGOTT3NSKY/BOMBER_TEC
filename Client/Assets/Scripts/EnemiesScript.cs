using UnityEngine;
using System.Collections;
using System;



public class EnemiesScript : MonoBehaviour
{
    // Lista de los enemigos
    System.Collections.Generic.List<Enemy> ListEnemies;


    // Vector3 para posicionar los bloques en la zona de trabajo
    private Vector3 screenPosition;
    public GameObject PlayerEnemy;


    // Start is called before the first frame update
    void Start()
    {
        
        ListEnemies = new System.Collections.Generic.List<Enemy>();

        for(int i = 0; i < 8; i++)
        {
            // Se crea el nuevo Genoma y se añade los valores de los genes
            Enemy aux = new Enemy();
         

            // Se añade el genoma la lista de genomas
            ListEnemies.Add(aux);









            screenPosition = new Vector3(1f, 1f, 1f);
            GameObject a = Instantiate(PlayerEnemy) as GameObject;
            a.transform.position = screenPosition;
        }


    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < 8; i++)
        {
            ListEnemies[i].UpdateEnemyMovement(6);
        }
       
    }





}

