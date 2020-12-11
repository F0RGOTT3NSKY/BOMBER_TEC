using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenomaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct nodeGenoma
{
    public int  gen_velocidad,
                gen_vidas,
                gen_esconderse,
                gen_bomba_cruz,
                gen_curarse,
                gen_protection,
                gen_lanzamiento,
                gen_bombas_numero,
                gen_suerte,
                gen_enfermedad,
                gen_bomba_potencia
                ;

    public int ID;
    public float Puntaje;

}


public class ListGenoma
{
   
    // Atributos _________________________________________________________

    // Lista de Genomas
    private int listLength;
    private List<nodeGenoma> genomaList;
    


    // Contructor
    public ListGenoma()
    {
        listLength = 0;
        genomaList = new List<nodeGenoma>();

    }



    // Métodos ______________________________________________________
    
    // Añadir Genoma
    private void AddNodeGenoma(int town)
    {

        for(int t = 0; t < town; t++)
        {
            // Se creal el nuevo Genoma y se añade los valores de los genes
            nodeGenoma aux = new nodeGenoma();
            aux = AddGenomaValues(aux);

            // Se añade el genoma la lista de genomas
            genomaList.Add(aux);


        }

    }

    // Generar valores aleatorios a los genomas
    private int RandomValue(int min, int max)
    {
        System.Random rand = new System.Random();
        return rand.Next(min, max);
    }

    // Generar la puntuacion en base a la suma de los genes de los genomas
    private float PuntuationGenoma(nodeGenoma aux)
    {
        float result = 0;

        _ =+ PuntuationGenomaAux(aux.gen_velocidad,100);
        _ =+ PuntuationGenomaAux(aux.gen_esconderse, 100);
        _ =+ PuntuationGenomaAux(aux.gen_bomba_cruz, 100);
        _ =+ PuntuationGenomaAux(aux.gen_curarse, 100);
        _ =+ PuntuationGenomaAux(aux.gen_protection, 100);
        _ =+ PuntuationGenomaAux(aux.gen_lanzamiento, 20);
        _ = +PuntuationGenomaAux(aux.gen_vidas, 5);

        _ =+ PuntuationGenomaAux(aux.gen_bombas_numero, 3);
        _ =+ PuntuationGenomaAux(aux.gen_suerte, 10);
        _ =+ PuntuationGenomaAux(aux.gen_enfermedad, 100);
        _ =+ PuntuationGenomaAux(aux.gen_bomba_potencia, 3);




        return result;
    }

    private float PuntuationGenomaAux(int value, int max)
    {
        return value / max;
    }

    // Añadir los valores de a los genes de los genomas
    private nodeGenoma AddGenomaValues(nodeGenoma aux)
    {
        listLength++;
        aux.ID = listLength;

        aux.gen_velocidad = RandomValue(0, 101);
        aux.gen_esconderse = RandomValue(0, 101);
        aux.gen_bomba_cruz = RandomValue(0, 101);
        aux.gen_curarse = RandomValue(0, 101);
        aux.gen_protection = RandomValue(0, 101);
        aux.gen_lanzamiento = RandomValue(0, 21);
        aux.gen_vidas = RandomValue(0, 6);

        aux.gen_bombas_numero = RandomValue(0, 4);
        aux.gen_suerte = RandomValue(0, 11);
        aux.gen_enfermedad = RandomValue(0, 101);
        aux.gen_bomba_potencia = RandomValue(0, 4);


        // Asignar el puntaje
        aux.Puntaje = PuntuationGenoma(aux);
        


        return aux;
    }

    // Retornar genoma en especial
    private nodeGenoma ReturnGenoma(int selectGenoma)
    {

        nodeGenoma aux = genomaList[0];
        return aux;

    }

    //Convertidores de bases
    private int DecimalToBinary(int num)
    {
        return int.Parse(System.Convert.ToString(num, 2)) ;
    }

    private int BinaryToDecimal(int num)
    {
        return int.Parse(System.Convert.ToString(num, 10));

    }

    // Organizar lista de los genomas -> Requiere que la lista de los genomas sea global
    private void Organizar()
    {
        //Metodo de burbuja

        bool sw = false;
        while (!sw)
        {
            sw = true;
            for (int i = 1; i < genomaList.Count; i++)
            {
                if (genomaList[i].Puntaje > genomaList[i - 1].Puntaje)
                {
                    nodeGenoma ind = genomaList[i];
                    genomaList[i] = genomaList[i - 1];
                    genomaList[i - 1] = ind;
                    sw = false;
                }
            }
        }
    }

    // Elimina Genoma de la lista de los genomas
    private List<nodeGenoma> DeleteGenoma(List<nodeGenoma> aux, int nodePosition)
    {
        aux.RemoveAt(nodePosition);
        return aux;
    }

 



    // Algoritmo genetico

    // Combinar
    // Mutar


    // Asignar a los personajes 



}


