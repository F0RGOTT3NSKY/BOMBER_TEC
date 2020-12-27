/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief Este código destruye los objetos 
*/

using UnityEngine;
using System.Collections;

/*!
* @class DestroySelf
* @brief DestroySelf Realiza la destrucción del objeto
* @details Realiza la destrucción de objeto 
* @public
*/
public class DestroySelf : MonoBehaviour
{
    public GameObject spawnBombOrigin;
    /// Tiempo de espera mientras destruye el objeto
    public float Delay = 3f;

    void Start ()
    {
        Destroy (gameObject, Delay);
    }
}
