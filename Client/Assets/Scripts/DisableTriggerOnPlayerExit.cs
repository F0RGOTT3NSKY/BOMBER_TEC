/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief This script makes sure that a bomb can be laid down at the player's feet without causing buggy movement when the player walks away.
*/

using UnityEngine;
using System.Collections;


/*!
* @class DisableTriggerOnPlayerExit
* @brief DestroySelf Realiza la destrucción del objeto
* @details This script makes sure that a bomb can be laid down at the player's feet without causing buggy movement when the player walks away.
* @public
*/
public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        { // When the player exits the trigger area
            GetComponent<Collider> ().isTrigger = false; // Disable the trigger
        }
    }
}
