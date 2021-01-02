using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public GlobalStateManager globalStateManager;
    
    // Caracteristicas

    [Range(0, 4)]
    /// Indica el numero del jugador qe se está manipulado
    public int playerNumber = 1;

    public int totalplayers = 0;
    /// Velocidad del movimiento
    public float moveSpeed = 0;
    /// Indica el numero de bombas que ha tirado el jugador.
    public int droppedBombs = 0;
    /// Indica el numero de vidas que ha perdido el jugador.
    public int usedLifes = 0;
    /// Indica la cantidad de vida del jugador.
    public float playerHealth = 100;
    /// Indica el tiempo en que se regenera la vida.
    public int healingTime = 10;
    /// Indica el tiempo en que se reduce la vida por enfermedad.
    public int sicknessTime = 15;
    /// Indica que si el jugador ha sido destruido
    public bool dead = false;

    // Genomas

    /// Indica el valor del genoma especializado en la cantidad de bombas en el inventario del jugador.
    public int bombGenoma = 0;
    /// Indica lo largo que se generan la explosiones de las bombas.
    public int distanceGenoma = 0;
    /// Indica la potencia de damage que tendra la bomba.
    public int potenciaGenoma = 0;
    /// Indica la cantidad de vidas que tendra el jugador.
    public int vidasGenoma = 0;
    /// Indica la cantidad de vida que se puede curar el jugador.
    public int curarseGenoma = 0;
    /// Indica la cantidad de vida que va a perder el jugador por enfermedad.
    public int enfermedadGenoma = 0;
    /// Indica la cantidad de proteccion que tiene el jugador.
    public int protectionGenoma = 0;
    /// Indica la cantidad de velocidad que tiene el jugador.
    public int velocidadGenoma = 0;
    /// Indica la cantidad de suerte que puede tener el jugador para esquivar un ataque.
    public int suerteGenoma = 0;
    /// Indica la cantidad de distacia a la que puede lanzar la bomba un jugador.
    public int lanzamientoGenoma = 0;
    /// Indica la habilidad de esconderse del jugador.
    public int esconderseGenoma = 0;
    
    // Maximos y Minimos

    /// Indica el numero maximo de bombas que se pueden lanzar por jugador.
    public int maxBombs = 10;
    /// Indica la distancia maxima a la que puede alcanzar las explosiones de las bombas.
    public int maxDistance = 10;
    /// Indica la potencia maxima que tendra la bomba.
    public int maxPotencia = 50;
    /// Indica el numero maximo de vidas que pueden tener los jugadores.
    public int maxVidas = 5;
    /// Indica la cantidad maxima que se puede curar el jugador.
    public int maxCuracion = 25;
    /// Indica la cantidad maxima que pierde el jugador por enfermedad.
    public int maxIllness = 25;
    /// Indica la cantidad maxima de protection que puede tener un jugador.
    public int maxProtection = 40;
    /// Indica la cantidad maxima de velocidad del jugador.
    public int maxSpeed = 12;
    /// Indica la cantidad minima de velocidad del jugador.
    public int minSpeed = 5;
    /// Indica la cantidad maxima de suerte del jugador.
    public int maxLuck = 10;

    // Limitadores

    /// Indica si puede lanzar bombas
    public bool canDropBombs = true;
    /// Indica que si puede moverse
    public bool canMove = true;
    /// Indica si el jugador se puede curar.
    public bool canHeal = true;
    /// Indica si el jugador puede enfermarse.
    public bool canGetSick = false;

    //Moviemiento
    public static List<GameObject> listaEnemigos;
    private GameObject tmpEnemy;
    public GameObject myObject;
    private AStar path;
    public bool safe;
    private int moveCommand;
    private int bestSol;
    private int[] bestSolPos;
    private int[] myPos;
    private int[] enemyPos;
    private int[] home;

    /// MenuScriptOptions
    public MenuManagerScript mms;

    //Contadores

    /// Indica el numero de frames.
    public int frames = 0;
    /// Indica la ultima vez que una bomba colisiono al jugador;
    public int lastHitExplosionFrame = 0;

    ///Prefabs
    public GameObject bombPrefab;
    ///Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    ListGenoma playerGenoma = new ListGenoma();

    private void Awake()
    {
        listaEnemigos = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.Find("PlayerModel").GetComponent<Animator>();
        //Inicia la poblacion de genomas
        playerGenoma.AddNodeGenoma(20);

        //Si es un ENEMY
        if(playerNumber == 0)
        {
            try
            {
                //Obtener los datos del menu 
                mms = FindObjectOfType<MenuManagerScript>();
                path = new AStar(10, mms.mapMatriz, mms.heightMap, mms.widthMap);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            tmpEnemy = null;

            safe = true;
            moveCommand = 0;
            bestSol = int.MaxValue;
            bestSolPos = new int[2];
            myPos = new int[2];
            enemyPos = new int[2];

            //Posicion Inicial "Home"
            home = new int[2];
            home[0] = Mathf.RoundToInt(myTransform.position.x); home[1] = Mathf.RoundToInt(myTransform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        EnemyMovement();
        UpdatePlayerGenoma();
        ResetLimits();
        //Verifica si el personaje puede dropear bombas segun el gen
        CheckBombDrop();
        UpdateGenomas();
        frames++;
    }
    private void EnemyMovement()
    {
        //Si es un ENEMY
        if (playerNumber == 0)
        {
            if (frames % 240 == 0)
            {
                myTransform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x),
                    myTransform.transform.position.y, Mathf.RoundToInt(myTransform.position.z));
            }

            if (frames % 30 == 0)
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

            if (frames % 15 == 0)
            {
                UpdateEnemyMovement(moveCommand);
            }
        }
    }
    private void ResetLimits()
    {
        //Update player movement speed 
        float conversor = 100 / (maxSpeed - minSpeed);
        moveSpeed = (velocidadGenoma / conversor) + minSpeed;

        // Hace reset a las bombas disponibles.
        if (frames % 1000 == 0)
        {
            Debug.Log("Bombs Reset");
            droppedBombs = 0;
        }
        // Hace reset de la habilidad de curacion.
        if (frames % 4500 == 0)
        {
            canHeal = true;
            Debug.Log("Heal Reset");
        }
        // Verifica si el jugador esta lo suficientemente debil para enfermarse.
        if (playerHealth <= 80)
        {
            canGetSick = true;
        }
        if (playerHealth > 80)
        {
            canGetSick = false;
        }

    }
    private void UpdatePlayerGenoma()
    {
        // Update gen_bombas_numero
        bombGenoma = playerGenoma.genomaList[totalplayers - 1].gen_bombas_numero;
        // Update gen_bomba_potencia
        potenciaGenoma = playerGenoma.genomaList[totalplayers - 1].gen_bomba_potencia;
        // Update gen_vidas
        vidasGenoma = playerGenoma.genomaList[totalplayers - 1].gen_vidas;
        // Update gen_bomba_cruz
        distanceGenoma = playerGenoma.genomaList[totalplayers - 1].gen_bomba_cruz;
        // Update  gen_curarse
        curarseGenoma = playerGenoma.genomaList[totalplayers - 1].gen_curarse;
        // Update gen_enfermedad
        enfermedadGenoma = playerGenoma.genomaList[totalplayers - 1].gen_enfermedad;
        // Update gen_protection
        protectionGenoma = playerGenoma.genomaList[totalplayers - 1].gen_protection;
        // Update gen_velocidad
        velocidadGenoma = playerGenoma.genomaList[totalplayers - 1].gen_velocidad;
        // Update gen_suerte
        suerteGenoma = playerGenoma.genomaList[totalplayers - 1].gen_suerte;
        // Update gen_lanzamiento
        lanzamientoGenoma = playerGenoma.genomaList[totalplayers - 1].gen_lanzamiento;
        // Update  gen_esconderse
        esconderseGenoma = playerGenoma.genomaList[totalplayers - 1].gen_esconderse;
    }
    private void CheckBombDrop()
    {
        float conversor = 100 / maxBombs;
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
    public void UpdateGenomas()
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
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 270, 0);   
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
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
            Bomb.GetComponent<Bomb>().explosiondistanceGenoma = distanceGenoma;
            Bomb.GetComponent<Bomb>().maxDistanceExplosion = maxDistance;
        }
    }
    private void CheckCurrentHealth(int potenciaGenomaOrigin)
    {
        float suerte_conversor = 100 / maxLuck;
        float suerte = suerteGenoma / suerte_conversor;
        Debug.Log(suerte + "% suerte");
        if(RandomValue(0,101)< suerte)
        {
            Debug.Log("Hit esquivado");
        }
        else
        {
            if (lastHitExplosionFrame == 0 || (frames - lastHitExplosionFrame) >= 500)
            {
                float potencia_conversor = 100 / maxPotencia;
                float protection_conversor = 100 / maxProtection;
                float potencia = (potenciaGenomaOrigin / potencia_conversor);
                float protection = (protectionGenoma / protection_conversor);
                Debug.Log("-" + (potencia - ((potencia * protection) / 100)) + " Vida");
                playerHealth -= potencia - ((potencia * protection) / 100);
                lastHitExplosionFrame = frames;
                if (playerHealth <= 0)
                {
                    CheckLifesLeft();
                    playerHealth = 100;
                }
                else
                {
                    if (canHeal == true)
                    {
                        // Healing for 10 seconds.
                        StartCoroutine("Healing");
                        canHeal = false;
                    }
                    if (canGetSick == true)
                    {
                        // Sickness for 15 seconds.
                        StartCoroutine("Sickness");
                    }
                }
            }
        }
        
    }
    private void CheckLifesLeft()
    {
        float conversor = 100 / maxVidas;
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
    private IEnumerator Healing()
    {
        float conversor = 100 / maxCuracion;
        float curacion = curarseGenoma / conversor;
        for(int i = 0; i < healingTime; i++)
        {
            if (playerHealth >= 100)
            {
                playerHealth = 100;
                yield break;
            }
            else
            {
                playerHealth += (curacion / healingTime);
                //Debug.Log("Healed for 1 second");
                yield return new WaitForSeconds(1f);
            }
            
        }
       //Debug.Log("+" + curacion + " over " + healingTime + " seg");    
    }
    private IEnumerator Sickness()
    {
        float conversor = 100 / maxIllness;
        float enfermedad = enfermedadGenoma / conversor;
        for(int i = 0; i < sicknessTime; i++)
        {
            if (playerHealth <= 0)
            {
                CheckLifesLeft();
                playerHealth = 100;
                yield break;
            }
            else
            {
                playerHealth -= (enfermedad / sicknessTime);
                //Debug.Log("Sickness for 1 second");
                yield return new WaitForSeconds(1f);
            }
        }
        //Debug.Log("-" + enfermedad + " over " + sicknessTime + " seg");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            GameObject playerOrigin = other.gameObject.GetComponent<DestroySelf>().spawnBombOrigin;
            CheckCurrentHealth(playerOrigin.GetComponent<Players>().potenciaGenoma);
            //animacion de golpe (cambiar la transparencia del renderer)
        }
    }
    /// Instantiate random number generator.  
    public readonly System.Random _random = new System.Random();
    /*!
     * @brief Generates a random number within a range.
     * @param min Indica el minimo del rango sin incluirlo.
     * @param max Indica el maximo del rango sin incluirlo.
     * @return int random value
     */
    public int RandomValue(int min, int max)
    {
        return _random.Next(min, max);
    }

    //==============================================================================
    //                        =>  PARA EL ENEMY  <=
    //==============================================================================

    /*!
    * @details busca un camino hacia un enemigo en el campo o hacia un lugar seguro
    * @param action, int 1 para buscar a enemigos, 2 para camino seguro
    * @return void
    */
    public void whereToGo(int action)
    {
        myPos[0] = Mathf.RoundToInt(myTransform.position.x);
        myPos[1] = Mathf.RoundToInt(myTransform.position.z);

        //A buscar un enemigo cercano
        if (action == 1)
        {
            for (int i = 0; i < totalplayers; i++)
            {
                //si el enemigo no se haya destruido
                if (listaEnemigos[i].Equals(null) != true)
                {
                    tmpEnemy = listaEnemigos[i];

                    //No sea igual a esta instancia (self, this)
                    if (tmpEnemy != myObject)
                    {
                        enemyPos[0] = Mathf.RoundToInt(tmpEnemy.transform.position.x);
                        enemyPos[1] = Mathf.RoundToInt(tmpEnemy.transform.position.z);

                        //si hay un camino
                        if (path.findPath(myPos, enemyPos) == true)
                        {
                            int auxNumber = path.solTamanno();

                            // si la cantidad de nodos entre esta posicion y la del enemigo sea menor o igual a la anterior
                            // cambiar a la mejor solucion
                            if (auxNumber <= bestSol)
                            {
                                Debug.Log("Menor camino");
                                bestSol = auxNumber;
                                bestSolPos[0] = path.getNearNodo()[0];
                                bestSolPos[1] = path.getNearNodo()[1];
                            }

                            else
                            {
                                System.Random random = new System.Random();
                                int cambio = random.Next(1, 101);
                                Debug.Log("Ahora es un 5% de perseguir a alguien mas " + cambio);

                                if (cambio <= 5)
                                {
                                    Debug.Log("Cambie de opinion y persigo a alguien mas... Jejepz");
                                    bestSolPos[0] = path.getNearNodo()[0];
                                    bestSolPos[1] = path.getNearNodo()[1];
                                }
                            }
                        }
                    }
                }
            }

            bestSol = int.MaxValue;
            movementAi(bestSolPos);
        }
        //Correr a home para salvarse
        else if (action == 2)
        {
            if (path.findPath(myPos, home) == true)
            {
                bestSolPos[0] = path.getNearNodo()[0];
                bestSolPos[1] = path.getNearNodo()[1];
                movementAi(bestSolPos);
            }
        }
    }

    /*!
    * @brief Compara la posicion del nodo cerdano con la posicion actual del personaje, para decidir en que direccion moverse
    * @param _direcction, int[] par ordenado de coordenadas del nodo mas cercano al inicio con direccion a la meta
    * @return void
    */
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

    /*
     * @brief setter para la lista de enemigos creados para este nivel
     * @return void
     */
    public void setListaEnemigos(List<GameObject> lista)
    {
        listaEnemigos = lista;
    }
}
