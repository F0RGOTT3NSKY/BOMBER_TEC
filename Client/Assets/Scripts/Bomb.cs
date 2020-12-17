/*!
* @file Bomb.cs 
* @author Adrian Gomez 
* @date 15/11/2020
* @brief  Codigo para generar las explosiones de las bombas
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
* @class Bomb
* @brief La clase Bomb se encarga de instanciar las explosiones al instante en el que el jugador pone una bomba en el suelo.
* @details Para crear las explosiones se invoca el la funcion de Explode() se instancian las explosiones mediante coroutines.
* Ademas, si la explosion toca una bomba sin explotar se procede a explotarla instantaneamente.
* @public 
*/
public class Bomb : MonoBehaviour
{
    /// Objeto prefab de la explosion.
    public GameObject explosionPrefab;
    /// Layermask donde hace efecto el RaycastHit.
    public LayerMask levelMask;
    /// Indica si alguna bomba cercana ha explotado o no.
    private bool exploded = false;

    /*!
    * @brief Start() is called before the first frame update.
    * @details Se invoca la funcion de Explode para instanciar las explosiones de la bomba.
    */
    public void Start()
    {
        Invoke("Explode", 3f);
    }
    /*!
    * @brief Explode() es invocado para instanciar explosiones en el juego.
    * @details Para instanciar las explosiones se usan coroutines apuntado a todas las direcciones para crear la explosion.
    * Ademas, si la explosion toca una bomba sin explotar se procede a explotarla instantaneamente.
    */
    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity); 
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        transform.Find("Collider").gameObject.SetActive(false); 
        Destroy(gameObject, .3f);
    }
    /*!
    * @brief CreateExplosions() es un algoritmo dedicado la creacion de las explosiones.
    * @details Para crear estas explosiones se crea un rayo de colision para detectar si existe un bloque en donde no se puede crear una explosion.
    * Sino existe ningun bloque en la LayerMask entonces se instancia una la explosion en la hacia la direccion elegida.
    * @param direction Indica la direccion a la que se va a crear la explosion.
    */
    public IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);

            if (!hit.collider)
            {
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(.05f);
        }

    }
    /*!
    * @brief OnTriggerEnter() se usa para detectar si otra explosion toca la bomba.
    * @details Si una explosion toca una bomba que no ha explotado, esta bomba prodece a explotar tambien.
    * @param other Indica que objeto ha entrado en el collider de la bomba.
    */
    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        { 
            CancelInvoke("Explode");
            Explode();
        }

    }

}
