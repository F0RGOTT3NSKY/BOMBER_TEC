/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo que determina y gestiona los cromosomas.
*/

using System;
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

    /// MenuScriptOptions
    public MenuManagerScript mms;

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



    ///Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;


    //Moviemiento
    public static List<GameObject> listaEnemigos;
    public bool safe;
    private GameObject tmpEnemy;
    private AStar path;
    private int nEnemigos;
    private int frame;
    private int moveCommand;
    private int bestSol;
    private int[] bestSolPos;
    private int[] myPos;
    private int[] enemyPos;
    private int[] home;

    private void Awake()
    {
        listaEnemigos = new List<GameObject>();
    }

    void Start()
    {
        try
        {
            mms = FindObjectOfType<MenuManagerScript>();
            nEnemigos = mms.nEnemies;
            path = new AStar(10, mms.mapMatriz, mms.heightMap, mms.widthMap);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();

        tmpEnemy = null;

        safe = true;

        frame = 1;
        moveCommand = 0;
        bestSol = int.MaxValue;
        bestSolPos = new int[2];
        myPos = new int[2];
        enemyPos = new int[2];

        //Posicion Inicial "Home"
        home = new int[2];
        home[0] = Mathf.RoundToInt(myTransform.position.x); home[1] = Mathf.RoundToInt(myTransform.position.z);
    }

    
    
    private void Update()
    {        if (frame % 240 == 0)
        {
            myTransform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x),
                myTransform.transform.position.y, Mathf.RoundToInt(myTransform.position.z));

            frame = 0;
        }

        if (frame % 30 == 0)
        {
            if (safe == true)
            {
                whereToGo(1);
            }
            else
            {
                whereToGo(2);
            }
        }

        if (frame % 15 == 0)
        {
            UpdateEnemyMovement(moveCommand);
        }

        frame++;
    }
    
    public void whereToGo(int action)
    {
        myPos[0] = Mathf.RoundToInt(myTransform.position.x);
        myPos[1] = Mathf.RoundToInt(myTransform.position.z);

        //A buscar un enemigo cercano
        if (action ==1)
        {
            for (int i = 0; i < nEnemigos; i++)
            {
                try
                {
                    tmpEnemy = listaEnemigos[i];

                    if (tmpEnemy.GetComponent<Enemy>() != this)
                    {
                        enemyPos[0] = Mathf.RoundToInt(tmpEnemy.transform.position.x);
                        enemyPos[1] = Mathf.RoundToInt(tmpEnemy.transform.position.z);

                        if (path.findPath(myPos, enemyPos) == true)
                        {
                            if (path.solTamanno() < bestSol)
                            {
                                bestSol = path.solTamanno();
                                bestSolPos[0] = path.getNearNodo()[0];
                                bestSolPos[1] = path.getNearNodo()[1];
                            }
                        }
                    }
                }catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            }

            bestSol = int.MaxValue;
            movementAi(bestSolPos);
        }
        //Correr a home para salvarse
        else if(action == 2)
        {
            if (path.findPath(myPos, home) == true)
            {
                bestSolPos[0] = path.getNearNodo()[0];
                bestSolPos[1] = path.getNearNodo()[1];
                movementAi(bestSolPos);
            }
        }
    }

    private void movementAi(int[] _direccion)
    {
        if (myPos[0] > _direccion[0])
        {
            //Muevo ARRIBA
            moveCommand = 1;
        }
        else if (myPos[0] < _direccion[0])
        {
            //Muevo ABAJO
            moveCommand = 3;
        }
        else if (myPos[1] > _direccion[1])
        {
            //Muevo IZQUIERDA
            moveCommand = 2;
        }
        else if (myPos[1] < _direccion[1])
        {
            //Muevo DERECHA
            moveCommand = 4;
        }
        else
        {
            Debug.Log("ERROR NO CHANGE IN POSITION");
            moveCommand = 0;
        }
    }


    /*!
    * @brief UpdateEnemyMovement() Se encarga de acutalizar los movimientos y acciones del enemigo
    * @param mov Señala la dirección en la que se dirije el enemigo o la acción que va a ejecutar
    */
    public void UpdateEnemyMovement(int mov)
    {
        if (mov == 0)
        { //Right movement
            Debug.Log("NO me muevo");

            moveCommand = 0;
            animator.SetBool("Walking", false);

        }

        if (mov == 1)
        { //Up movement
            Debug.Log("Me muevo ARRIBA");

            moveCommand = 1;
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Walking", true);

        }

        if (mov == 2)
        { //Left movement
            Debug.Log("Me muevo IZQUIERDA");

            moveCommand = 2;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);

        }

        if (mov == 3)
        { //Down movement
            Debug.Log("Me muevo ABAJO");

            moveCommand = 3;
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);


        }

        if (mov == 4)
        { //Right movement
            Debug.Log("Me muevo DERECHA");

            moveCommand = 4;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
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

        dead = true;
        Destroy(gameObject); // 3
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

    public void setListaEnemigos(List<GameObject> lista)
    {
        listaEnemigos = lista;
    }

    public float PuntuationGenomaAux(int value, int max)
    {
        return (float)value / max;
    }

}