
/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo de los jugadores que son manipulados por el usuario
*/


using UnityEngine;
using System.Collections;
using System;

/*!
* @struct Player
* @brief Player Codigo que controla a los personajes
* @details Realiza el movimiento de los personajes, además, de realizar el lanzamiento de las bombas
* @public
*/

public class Player : MonoBehaviour
{ 
    /// GlobalStateManager 
    public GlobalStateManager globalManager;
    ///Player parameters
    [Range (1, 2)] 

    /// Indiga el numero de jugador qe se está manipulado
    public int playerNumber = 1;

    /// Velocidad del movimiento
    public float moveSpeed = 5f;

    /// Indiga si puede lanzar bombas
    public bool canDropBombs = true;

    /// Indica que si puede moverse 
    public bool canMove = true;

    /// Indica que si el jugador ha sido destruido
    public bool dead = false;



    ///Prefabs
    public GameObject bombPrefab;

    ///Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    /// Use this for initialization
    void Start ()
    {
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
    }

    /// Update is called once per frame
    void Update ()
    {
        UpdateMovement ();
    }


    /*!
   * @brief UpdateMovement() Actualiza el movimiento de los jugadores
   */
    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false);

        if (!canMove)
        { //Return if player can't move
            return;
        }

        //Depending on the player number, use different input for moving
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }

    /*!
   * @brief UpdatePlayer1Movement() Actualiza el movimiento del jugador 1
   */
    private void UpdatePlayer1Movement ()
    {
        if (Input.GetKey (KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
        { //Drop bomb
            DropBomb ();
        }
    }

    /*!
   * @brief UpdatePlayer2Movement() Actualiza el movimiento del jugador 2
   */
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players 
            //without a numpad will be unable to drop bombs
            DropBomb ();
        }
    }

    /*!
   * @brief DropBomb() Metodo para lanzar bombas
   */
    private void DropBomb ()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
                bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                bombPrefab.transform.rotation);

        }
    }

    /*!
   * @brief OnTriggerEnter() Ejecuta cuando el objeto ha sido colisionado 
   * @param other Objeto que colisiona
   */
    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Explosion"))
        {
            Debug.Log ("P" + playerNumber + " hit by explosion!");
            dead = true; // 1
            globalManager.PlayerDied(playerNumber); // 2
            Destroy(gameObject); // 3  

        }
    }
}
