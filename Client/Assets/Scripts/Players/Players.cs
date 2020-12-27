using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public GlobalStateManager globalStateManager;
    [Range(1, 4)]
    /// Indica el numero del jugador qe se está manipulado
    public int playerNumber = 1;
    /// Velocidad del movimiento
    public float moveSpeed = 3f;
    /// Indica si puede lanzar bombas
    public bool canDropBombs = true;
    /// Indica el valor del genoma especializado en la cantidad de bombas en el inventario del jugador.
    public int bombGenoma = 0;
    /// Indica el numero maximo de bombas que se pueden lanzar por jugador.
    public int maxBombs = 10;
    /// Indica el numero de bombas que ha tirado el jugador.
    public int droppedBombs = 0;
    /// Indica la potencia de damage que tendra la bomba.
    public int potenciaGenoma = 0;
    /// Indica la potencia maxima que tendra la bomba.
    public int maxPotencia = 50;
    /// Indica la cantidad de vidas que tendra el jugador.
    public int vidasGenoma = 0;
    /// Indica el numero maximo de vidas que pueden tener los jugadores.
    public int maxVidas = 5;
    /// Indica el numero de vidas que ha perdido el jugador.
    public int usedLifes = 0;
    /// Indica la cantidad de vida del jugador.
    public float playerHealth = 100;
    /// Indica que si puede moverse
    public bool canMove = true;
    /// Indica que si el jugador ha sido destruido
    public bool dead = false;
    /// Indica si el jugador se puede curar.
    public bool canHeal = true;
    
    /// Indica el numero de frames.
    public int frames = 0;
 
    public int lastHitExplosionFrame = 0;
    

    ///Prefabs
    public GameObject bombPrefab;
    ///Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    ListGenoma playerGenoma = new ListGenoma();

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();

        //Inicia la poblacion de genomas
        playerGenoma.AddNodeGenoma(20);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateGenomas();
        //Verifica si el personaje puede dropear bombas segun el gen
        CheckBombDrop();
        UpdatePlayerGenoma();
        frames++;
    }
    private void UpdateGenomas()
    {
        // Update gen_bombas_numero
        bombGenoma = playerGenoma.genomaList[playerNumber - 1].gen_bombas_numero;
        // Update gen_bomba_potencia
        potenciaGenoma = playerGenoma.genomaList[playerNumber - 1].gen_bomba_potencia;
        // Update gen_vidas
        vidasGenoma = playerGenoma.genomaList[playerNumber - 1].gen_vidas;
    }
    private void CheckBombDrop()
    {
        // Hace reset a las bombas disponibles
        if (frames % 1000 == 0)
        {
            Debug.Log("Bombs Reset");
            droppedBombs = 0;
        }

        int conversor = 100 / maxBombs;
        // Verifica si tiene bombas disponibles.
        //Debug.Log(playerGenoma.genomaList[playerNumber - 1].gen_bombas_numero / conversor);
        if (!(droppedBombs < (bombGenoma / conversor)))
        {
            canDropBombs = false;
        }
        else
        {
            canDropBombs = true;
        }
    }
    public void UpdatePlayerGenoma()
    {
        if (frames % 200 == 0)
        {
            for (int i = 1; i <= 10; i++)
            {
                playerGenoma.genomaList.Add(playerGenoma.Combine(playerGenoma.genomaList[playerGenoma.RandomValue(0, playerGenoma.genomaList.Count / 2)], playerGenoma.genomaList[playerGenoma.RandomValue(0, playerGenoma.genomaList.Count / 2)]));
                playerGenoma.genomaList[playerGenoma.genomaList.Count - 1] = playerGenoma.AddGenomaValues(playerGenoma.genomaList[playerGenoma.genomaList.Count - 1]);
                playerGenoma.genomaList[playerGenoma.genomaList.Count - 1].setPuntaje(playerGenoma.PuntuationGenoma(playerGenoma.genomaList[playerGenoma.genomaList.Count - 1]));
            }
            playerGenoma.Organizar();
            int size = playerGenoma.genomaList.Count / 2;
            for (int i = 0; i < size; i++)
            {
                playerGenoma.genomaList.RemoveAt(playerGenoma.genomaList.Count - 1);
            }
            /*
            //Mostrar Puntajes
            Debug.Log("Every 200th frame");
            for (int i = 0; i < playerGenoma.genomaList.Count; i++)
            {
                Debug.Log(playerGenoma.genomaList[i].Puntaje);
            }*/
        }
    }
    private void UpdateMovement()
    {
        animator.SetBool("Walking", false);

        if (!canMove)
        { //Return if player can't move
            return;
        }

        //Depending on the player number, use different input for moving
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement();
        }
    }
    private void UpdatePlayer1Movement()
    {
        if (Input.GetKey(KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        { //Drop bomb
            if (canDropBombs)
            {
                DropBomb();
                droppedBombs++;
            }      
        }
    }
    private void DropBomb()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            GameObject Bomb = Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);
            Bomb.GetComponent<Bomb>().spawnOrigin = gameObject;

            //bombPrefab.transform.parent = player.transform;
        }
    }
    private void CheckCurrentHealth(int potenciaGenomaOrigin)
    {
        int conversor = 100 / maxPotencia;
        Debug.Log("-" + (potenciaGenomaOrigin / conversor) + " Vida");
        if (lastHitExplosionFrame == 0)
        {
            playerHealth = playerHealth - (potenciaGenomaOrigin / conversor);
            lastHitExplosionFrame = frames;
        }
        if ((frames - lastHitExplosionFrame) >= 500)
        {
            playerHealth = playerHealth - (potenciaGenomaOrigin / conversor);
            lastHitExplosionFrame = frames;
        }
        if (playerHealth <= 0)
        {
            CheckLifesLeft();
            playerHealth = 100;
        }
    }
    private void CheckLifesLeft()
    {
        int conversor = 100 / maxVidas;
        //Debug.Log(vidasGenoma / conversor);
        if (usedLifes > (vidasGenoma / conversor))
        {
            dead = true;
            globalStateManager.PlayerDied(playerNumber);
            Destroy(gameObject);
        }
        else
        {
            usedLifes++;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            GameObject playerOrigin = other.gameObject.GetComponent<DestroySelf>().spawnBombOrigin;
            CheckCurrentHealth(playerOrigin.GetComponent<Players>().potenciaGenoma);
            
            /*
            
            Debug.Log("Potencia:"+playerOrigin.GetComponent<Players>().potenciaGenoma);
            //Debug.Log("works" + other.gameObject.GetComponent<>.potenciaGenoma);
            Debug.Log("P" + playerNumber + " hit by explosion!");
            dead = true; // 1
            globalStateManager.PlayerDied(playerNumber); // 2
            Destroy(gameObject); // 3  
            */
        }
    }
}
