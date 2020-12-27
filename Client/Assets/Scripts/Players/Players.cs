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
    /// Indica que si puede moverse
    public bool canMove = true;
    /// Indica que si el jugador ha sido destruido
    public bool dead = false;
    /// Indica si el jugador se puede curar.
    public bool canHeal = true;
    /// Indica el numero de frames.
    public int frames = 0;
    

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
        //Verifica si el personaje puede dropear bombas segun el gen
        CheckBombDrop();
        UpdatePlayerGenoma();
        frames++;
    }
    private void CheckBombDrop()
    {
        int conversor = 100 / maxBombs;
        Debug.Log(playerGenoma.genomaList[playerNumber - 1].gen_bombas_numero / conversor);
        bombGenoma = playerGenoma.genomaList[playerNumber - 1].gen_bombas_numero;
        if (frames % 1000 == 0)
        {
            Debug.Log("Bombs Reset");
            droppedBombs = 0;
        }
        if (!(droppedBombs < (playerGenoma.genomaList[playerNumber - 1].gen_bombas_numero / conversor)))
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
            globalStateManager.PlayerDied(playerNumber); // 2
            Destroy(gameObject); // 3  

        }
    }
}
