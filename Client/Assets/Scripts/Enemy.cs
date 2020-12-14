using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{

    // GlobalStateManager
    public GlobalStateManager globalManager;

    // Genoma del enemigo 
    private nodeGenoma enemyGenoma;





    //___________________________________________

    //Player parameters
    [Range(1, 2)] //Enables a nifty slider in the editor
    public int playerNumber;
    public float moveSpeed;
    private int bombs;


    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    public bool dead = false;



    //Prefabs
    public GameObject bombPrefab;
    public GameObject enemymodel;

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    // Método Constructor

    public Enemy()
    {

    }

    public Enemy(nodeGenoma aux, Material _EnemySkin)
    {
        enemyGenoma = aux;
        this.gameObject.GetComponent<MeshRenderer>().material = _EnemySkin;

        

        AsignarGenoma();
    }

  


    // Updates Enemy movement and facing rotation using the WASD keys and drops bombs using Space
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

    // Drops a bomb
    
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

        //playerNumber = enemyGenoma.ID;
    }


}