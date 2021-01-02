/*!
* @file CameraConfig.cs 
* @author Adrian Gomez Garro
* @date 10/12/2020
* @brief  Codigo para gestionar la posicion de la camara.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*!
 * @brief La clase CameraConfig se utiliza para gestionar la posicion de la camara.
 * @details Con base a los distintos tamanos preseleccionados se pone la camara y el area de A* en la posicion correcta.
 * @public
 */
public class CameraConfig : MonoBehaviour
{
    /// Indica el objeto de la camara en Unity
    public GameObject Maincamera;
    /// Indica el objeto del area de A* en Unity
    public GameObject PathfingArea;
    /*!
     * @brief Start() is called before the first frame update
     * @details Dependiendo del tamano del mapa se coloca la camara y el area A* en el lugar correcto.
     */
    public void Start()
    {
        if (seeMap.NColumnas_Map == 50 && seeMap.NFilas_Map == 50)
        {
            Maincamera.transform.position = new Vector3(24.5f, 59f, 24.5f);
            //PathfingArea.transform.position = new Vector3(24.5f, 2f, 24.5f);
        }
        else if (seeMap.NColumnas_Map == 25 && seeMap.NFilas_Map == 25)
        {
            Maincamera.transform.position = new Vector3(12f, 31f, 12f);
            //PathfingArea.transform.position = new Vector3(12f, 2f, 12f);
        }
        else if (seeMap.NColumnas_Map == 20 && seeMap.NFilas_Map == 20)
        {
            Maincamera.transform.position = new Vector3(9.5f, 25f, 9.5f);
            //PathfingArea.transform.position = new Vector3(9.5f, 2f, 9.5f);
        }
        else if (seeMap.NColumnas_Map == 15 && seeMap.NFilas_Map == 15)
        {
            Maincamera.transform.position = new Vector3(7f, 20f, 7f);
            // PathfingArea.transform.position = new Vector3(7f, 2f, 7f);
        }

        Maincamera.transform.rotation = Quaternion.Euler(90f, 0f, 90f);
    }
}
