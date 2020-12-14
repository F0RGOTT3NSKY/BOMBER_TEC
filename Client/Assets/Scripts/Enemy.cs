/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo que determina y gestiona los cromosomas.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*!
* @class EnemiesScript
* @brief EnemiesScript Clase que se encarga de realizar los movimientos de los jugadores enemigos
* @details Esta clase se encarga de asignar el genoma a cada jugador enemigo, además, realiza reparte la ejecucion de estos en los frames.
* @public
*/


public class Enemy : MonoBehaviour
{

    /// GlobalStateManager
    public GlobalStateManager globalManager;

    /// Genoma del enemigo 
    private nodeGenoma enemyGenoma;


    //Player parameters
    [Range(1, 2)] //Enables a nifty slider in the editor

    /// Numero de jugador
    public int playerNumber;
    /// Velocidad a la que avanza el jugador
    public float moveSpeed;
    /// Cantidad de bombas que dispone el jugador
    private int bombs;

    /// El jugador está habilitado para lanzar combas
    public bool canDropBombs = true;
    /// El jugador puede moverse
    public bool canMove = true;
    /// El jugador se encuentra destruido
    public bool dead = false;



    /// Objeto prefabricado de las bombas
    public GameObject bombPrefab;
    /// Objeto prefabricado del enemigo
    public GameObject enemymodel;

    ///Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    /// Método Constructor

    public Enemy()
    {

    }

    public Enemy(nodeGenoma aux, Material _EnemySkin)
    {
        enemyGenoma = aux;
        this.gameObject.GetComponent<MeshRenderer>().material = _EnemySkin;

        

        AsignarGenoma();
    }



    /*!
    * @brief UpdateEnemyMovement() Se encarga de acutalizar los movimientos y acciones del enemigo
    * @param mov Señala la dirección en la que se dirije el enemigo o la acción que va a ejecutar
    */
    public void UpdateEnemyMovement(int mov)
    {
        if (mov == 1)
        { //Up movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walking", true);
        }

        if (mov == 2)
        { //Left movement
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Walking", true);
        }

        if (mov == 3)
        { //Down movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (mov == 4)
        { //Right movement
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (canDropBombs && mov == 5)
        { 
            DropBomb();
        }
    }

    /*!
    * @brief DropBomb() Metodo que se encarga de ejecutar la salida de bombas
    */
    private void DropBomb()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
                bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);

        }
    }

    /*!
    * @brief OnTriggerEnter() Metodo que se encarga de señalar la colicion del objeto
    * @param other Objeto que esta colicionando 
    */
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            Debug.Log("P" + playerNumber + " hit by explosion!");
            dead = true; // 1
            globalManager.PlayerDied(playerNumber); // 2
            Destroy(gameObject); // 3  

        }
    }

    /*!
    * @brief AsignarGenoma() Metodo que se encarga de asignar los genes del genoma a los atributos del objeto enemigo
    */
    private void AsignarGenoma()
    {
        bombs = enemyGenoma.gen_bombas_numero;
        //enemyGenoma.gen_bomba_cruz;
        //enemyGenoma.gen_bomba_potencia;
        //enemyGenoma.gen_curarse;
        //enemyGenoma.gen_enfermedad;
        //enemyGenoma.gen_esconderse;
        //enemyGenoma.gen_lanzamiento;
        //enemyGenoma.gen_protection;
        //enemyGenoma.gen_suerte;
        moveSpeed = (float)enemyGenoma.gen_velocidad;
        //enemyGenoma.gen_vidas;

        //playerNumber = enemyGenoma.ID;
    }


}