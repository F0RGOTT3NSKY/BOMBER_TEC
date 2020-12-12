using UnityEngine;
using System.Collections;
using System;



public class EnemiesScript : MonoBehaviour
{

    List<nodeEnemy> ListEnemies;


    // Start is called before the first frame update
    void Start()
    {
        ListEnemies = new List<nodeEnemy>();

        for(int i = 0; i < 8; i++)
        {
            // Se creal el nuevo Genoma y se añade los valores de los genes
            nodeEnemy aux = new nodeGenoma();
            

            // Se añade el genoma la lista de genomas
            ListEnemies.Add(aux);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }





}


public class nodeEnemy : MonoBehaviour
{

    // GlobalStateManager
    public GlobalStateManager globalManager;
    
    // Genoma del enemigo 
    private nodeGenoma enemyGenoma;



    //___________________________________________
   
    //Player parameters
    [Range(1, 2)] //Enables a nifty slider in the editor
    public int playerNumber;
    public float moveSpeed = 5f;
    private int bombs = 5;


    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    public bool dead = false;



    //Prefabs
    public GameObject bombPrefab;

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    // Método Constructor

    public nodeEnemy()
    {

    }

    public nodeEnemy(nodeGenoma aux)
    {
        enemyGenoma = aux;
        AsignarGenoma;
    }

    // Use this for initialization________________________________________________________________________________________
    void Start()
    {
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walking", false);

        if (!canMove)
        { //Return if player can't move
            return;
        }

        UpdateEnemyMovement(6);
    }

  
    // Updates Enemy movement and facing rotation using the WASD keys and drops bombs using Space
    private void UpdateEnemyMovement(int movkey)
    {
        if (movkey == 1)
        { //Up movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walking", true);
        }

        if (movkey == 2)
        { //Left movement
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Walking", true);
        }

        if (movkey == 3)
        { //Down movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (movkey == 4)
        { //Right movement
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (canDropBombs && movkey == 5)
        { //Drop bomb
            DropBomb();
        }
    }



    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
    private void DropBomb()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
                bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);

        }
    }

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

    // Asignar valores del genoma al enemigo 

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


        playerNumber = enemyGenoma.ID;


    }


}