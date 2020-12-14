/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo de los bloques
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
* @class Brick
* @brief Brick Clase que maneja los bloques
* @details Esta clase se encarga de asignar al bloque que la función para poder destruirse
* @public
*/

public class Brick : MonoBehaviour
{
    /// Start is called before the first frame update
    void Start()
    {
        
    }

    /// Update is called once per frame
    void Update()
    {

    }
    
    /*!
     * @brief onTriggerEnter Detecta el objeto
     * @param other El objeto que desea colisionar
     */

    public void OnTriggerEnter(Collider other)
    {
      Destroy(gameObject);
    }
}
