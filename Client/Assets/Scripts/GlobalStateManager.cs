/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief Analiza si los jugadores estan muertos o no
*/


using UnityEngine;
using System.Collections;


/*!
* @class GlobalStateManager
* @brief GlobalStateManager Analiza la destruccion de los jugadores
* @details Esta clases analiza la destruccion de los objetos para declarar un ganador
* @public
*/
public class GlobalStateManager : MonoBehaviour
{
    /// Numero de jugadores
    private int deadPlayers = 0;

    /// Numero de jugadores muertos
    private int deadPlayerNumber = -1;


    /*!
    * @brief PlayerDied() Destruye el personaje
    * @param playerNumber Ingrese sel personaje que ha sido destruido
    */
    public void PlayerDied (int playerNumber)
    {
        deadPlayers++; // 1

        if (deadPlayers == 1)
        { // 2
            deadPlayerNumber = playerNumber; // 3
            Invoke("CheckPlayersDeath", .3f); // 4
        }

    }

    /*!
    * @brief CheckPlayerDeath() Termina el juego y señala el ganador
    */
    void CheckPlayersDeath()
    {
        // 1
        if (deadPlayers == 1)
        {
            // 2
            if (deadPlayerNumber == 1)
            {
                Debug.Log("Player 2 is the winner!");
                // 3
            }
            else
            {
                Debug.Log("Player 1 is the winner!");
            }
            // 4
        }
        else
        {
            Debug.Log("The game ended in a draw!");
        }
    }

}
