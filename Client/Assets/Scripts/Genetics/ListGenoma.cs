/*!
* @file ListGenoma.cs 
* @authors Adrian Gomez Garro
* @authors Kevin Masis Leandro
* @date 10/12/2020
* @brief  Codigo que determina y gestiona los cromosomas.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
* @struct nodeGenoma
* @brief nodeGenoma determina como se estructura cada individuo.
* @details En donde cada cromosoma se divide en genes y tiene un puntaje total que se usa para determinar el mejor individuo.
* @public
*/
public struct nodeGenoma
{
                /// Gen de velocidad del individuo, su rango va de [0,100].
    public int gen_velocidad,
                /// Gen de vidas del individuo, su rango va de [0,5].
                gen_vidas,
                /// Gen de esconderse del individuo, su rango va de [0,100].
                gen_esconderse,
                /// Gen de bomba cruz del individuo, su rango va de [0,100].
                gen_bomba_cruz,
                /// Gen de curarse del individuo, su rango va de [0,100].
                gen_curarse,
                /// Gen de protection del individuo, su rango va de [0,100].
                gen_protection,
                /// Gen de lanzamiento del individuo, su rango va de [0,100].
                gen_lanzamiento,
                /// Gen del numero de bombas del individuo, su rango va de [0,20].
                gen_bombas_numero,
                /// Gen de suerte del individuo, su rango va de [0,4].
                gen_suerte,
                /// Gen de enfermedad del individuo, su rango va de [0,10].
                gen_enfermedad,
                /// Gen de potencia de bomba del individuo, su rango va de [0,4].
                gen_bomba_potencia
                ;
    /// Variable que indica el puntaje total del individuo.
    public float Puntaje;
    /*!
    * @brief setPuntaje() se usa para setear el valor del puntaje a cada individuo.
    * @param pPuntaje El puntaje que se va a asignar al puntaje del individuo.
    */
    public void setPuntaje(float pPuntaje)
    {
        this.Puntaje = pPuntaje;
    }
}
/*!
 * @class ListGenoma
 * @brief La clase ListGenoma se encarga de gestionar los cromosomas de cada individuo.
 * @details Con esta clase se puede gestionar todo lo referente a los genes de los cromosomas de cada individio. 
 * @public
 */
public class ListGenoma : MonoBehaviour
{
    /// Lista que guarda a todos los individuos con su estructura de genes.
    public List<nodeGenoma> genomaList;
    /// Variable para contar los frames
    public int frames = 0;
    
    /*!
     * @brief ListGenoma() es el constructor de la clase para instanciar la lista de individuos
     */
    public ListGenoma()
    {
        genomaList = new List<nodeGenoma>();
    }
    /*!
     * @brief Start() is called before the first frame update.
     * @details Se usa para iniciar la creacion de los individuos por primera vez y asi empezar con el algoritmo genetico.
     */
    /*public void Start()
    {
        AddNodeGenoma(20);
    }*/
    /*!
     * @brief Update() is called once per frame.
     * @details Se usa para actualizar la poblacion de individuos cada n numero de frames 
     * y asi aplicar el algoritmo genetico al combinar y mutar a los individuos.
     */
    /*public void Update()
    {
        frames++;
        if(frames % 200 == 0)
        {
            for (int i= 1; i<=10;i++)
            {
                genomaList.Add(Combine(genomaList[RandomValue(0, genomaList.Count/2)], genomaList[RandomValue(0, genomaList.Count/2)]));
                genomaList[genomaList.Count - 1] = AddGenomaValues(genomaList[genomaList.Count - 1]);
                genomaList[genomaList.Count - 1].setPuntaje(PuntuationGenoma(genomaList[genomaList.Count - 1]));
            }
            Organizar();
            int size = genomaList.Count / 2;
            for (int i = 0; i < size; i++)
            {
                genomaList.RemoveAt(genomaList.Count - 1);
            }
            
            // Asignar a los jugadores
            Debug.Log("Every 200th frame");
            for (int i = 0; i < genomaList.Count; i++)
            {
                Debug.Log(genomaList[i].Puntaje);

            }
        }
    }*/
    /*!
     * @brief AddNodeGenoma se usa para anadir nuevos individuos a la lista de  la poblacion.
     * @details Se anaden nuevos individuos, se organizan de mayor puntaje a menor, y luego se descarta la mitad de peor puntaje.
     * @param town Indica la cantidad de nuevos individuos a la poblacion.
     */
    public void AddNodeGenoma(int town)
    {
        for(int t = 0; t < town; t++)
        {
            // Se crea el nuevo Genoma y se añade los valores de los genes
            nodeGenoma aux = new nodeGenoma();
            aux = AddGenomaValues(aux);
            // Se añade el genoma la lista de genomas
            genomaList.Add(aux);
        }
        Organizar();
        // Se remueve la mitad de peor puntaje
        int size = genomaList.Count / 2;
        for (int i = 0; i < size; i++)
        {
            genomaList.RemoveAt(genomaList.Count - 1);
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

    /*!
     * @brief PuntuationGenoma() Genera la puntuacion en base a la suma de los genes de los genomas.
     * @details La puntuacion de los individuos se calcula apartir de una suma de cada gen que tiene un valor de 1 como maximo.
     * @param aux Se pasa a cada individuo para obtener cada gen de este.
     * @return float puntaje de cada individuo.
     */
    public float PuntuationGenoma(nodeGenoma aux)
    {
        float result = 0;

        result += PuntuationGenomaAux(aux.gen_velocidad,100);
        result += PuntuationGenomaAux(aux.gen_esconderse, 100);
        result += PuntuationGenomaAux(aux.gen_bomba_cruz, 100);
        result += PuntuationGenomaAux(aux.gen_curarse, 100);
        result += PuntuationGenomaAux(aux.gen_protection, 100);
        result += PuntuationGenomaAux(aux.gen_lanzamiento, 100);
        result +=PuntuationGenomaAux(aux.gen_vidas, 100);

        result += PuntuationGenomaAux(aux.gen_bombas_numero, 100);
        result += PuntuationGenomaAux(aux.gen_suerte, 100);
        result += PuntuationGenomaAux(aux.gen_enfermedad, 100);
        result += PuntuationGenomaAux(aux.gen_bomba_potencia, 100);

        return result;
    }
    /*!
     * @brief PuntuationGenomaAux() se usa para calcular que tanto se tiene cada gen.
     * @param value Indica el valor del gen.
     * @param max Indica el valor maximo del gen.
     * @return float value/max
     */
    public float PuntuationGenomaAux(int value, int max)
    {
        return (float) value / max;
    }
    /*!
     * @brief AddGenomaValues() se usa para agregar valores random a los genes.
     * @param aux  Se pasa a cada individuo para obtener cada gen de este.
     * @return El mismo individuo aux.
     */
    public nodeGenoma AddGenomaValues(nodeGenoma aux)
    {
        aux.gen_velocidad = RandomValue(0, 101);
        aux.gen_esconderse = RandomValue(0, 101);
        aux.gen_bomba_cruz = RandomValue(0, 101);
        aux.gen_curarse = RandomValue(0, 101);
        aux.gen_protection = RandomValue(0, 101);
        aux.gen_lanzamiento = RandomValue(0, 101);
        aux.gen_vidas = RandomValue(0, 101);

        aux.gen_bombas_numero = RandomValue(0, 101);
        aux.gen_suerte = RandomValue(0, 101);
        aux.gen_enfermedad = RandomValue(0, 101);
        aux.gen_bomba_potencia = RandomValue(0, 101);


        // Asignar el puntaje
        aux.Puntaje = PuntuationGenoma(aux);

        return aux;
    }

    /*!
     * @brief Organizar() se usa para organizar la lista de individuos de mayor puntaje a menor.
     * @details Para organizar la lista se usa un bublesort, esta lista de individuos es la lista global donde estan los individuos, es decir, la poblacion del algoritmo genetico.
     */
    public void Organizar()
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
    /*!
     * @brief Validacion() se usa para evitar los casos en los genes se pasen del valor maximo.
     * @details Se revisa cada gen del individuo y en el caso de que este sea mayor al maximo se hace reset de este al valor maximo.
     * @param c Se pasa a cada individuo para obtener cada gen de este.
     * @return c se retorna el individuo ya validado.
     */
    public nodeGenoma Validacion(nodeGenoma c)
    {
        if (c.gen_bombas_numero > 100)
        {
            c.gen_bombas_numero = 100;
        }
        if (c.gen_bomba_cruz > 100) 
        {
            c.gen_bomba_cruz = 100;
        }
        if (c.gen_bomba_potencia > 100)
        {
            c.gen_bomba_potencia = 100;
        }
        if (c.gen_curarse > 100)
        {
            c.gen_curarse = 100;
        }
        if(c.gen_enfermedad > 100)
        {
            c.gen_enfermedad = 100;
        }
        if (c.gen_esconderse > 100)
        {
            c.gen_esconderse =100;
        }
        if(c.gen_lanzamiento > 100)
        {
            c.gen_lanzamiento = 100;
        }
        if (c.gen_protection > 100)
        {
            c.gen_protection = 100;
        }
        if(c.gen_suerte > 100)
        {
            c.gen_suerte = 100;
        }
        if(c.gen_velocidad > 100)
        {
            c.gen_velocidad = 100;
        }
        if(c.gen_vidas > 100)
        {
            c.gen_vidas = 100;
        }
        return c;
    }
    /*!
     * @brief Combine() se usa para combinar dos cromosomas.
     * @details En esta funcion se combinan todos los genes de ambos cromosomas usando bitwise operators.
     * @param a Para obtener todos los genes del cromosoma padre.
     * @param b Para obtener todos los genes del cromosoma madre.
     * @return c Se retorna el hijo para su validacion, existe un 5% en que se retorna un hijo mutado para su validacion.
     */
    public nodeGenoma Combine(nodeGenoma a, nodeGenoma b)
    {
        nodeGenoma c = new nodeGenoma();

        a.gen_bombas_numero = a.gen_bombas_numero >> 4;
        a.gen_bombas_numero = a.gen_bombas_numero << 4;

        b.gen_bombas_numero = b.gen_bombas_numero << 4;
        b.gen_bombas_numero = b.gen_bombas_numero & 127;
        b.gen_bombas_numero = b.gen_bombas_numero >> 4;

        c.gen_bombas_numero = a.gen_bombas_numero | b.gen_bombas_numero;


        a.gen_bomba_potencia = a.gen_bomba_potencia >> 4;
        a.gen_bomba_potencia = a.gen_bomba_potencia << 4;

        b.gen_bomba_potencia = b.gen_bomba_potencia << 4;
        b.gen_bomba_potencia = b.gen_bomba_potencia & 127;
        b.gen_bomba_potencia = b.gen_bomba_potencia >> 4;

        c.gen_bomba_potencia = a.gen_bomba_potencia | b.gen_bomba_potencia;

        a.gen_vidas = a.gen_vidas >> 4;
        a.gen_vidas = a.gen_vidas << 4;

        b.gen_vidas = b.gen_vidas << 4;
        b.gen_vidas = b.gen_vidas & 127;
        b.gen_vidas = b.gen_vidas >> 4;

        c.gen_vidas = a.gen_vidas | b.gen_vidas;

        a.gen_velocidad = a.gen_velocidad >> 4;
        a.gen_velocidad = a.gen_velocidad << 4;

        b.gen_velocidad = b.gen_velocidad << 4;
        b.gen_velocidad = b.gen_velocidad & 127;
        b.gen_velocidad = b.gen_velocidad >> 4;

        c.gen_velocidad = a.gen_velocidad | b.gen_velocidad;

        a.gen_suerte = a.gen_suerte >> 4;
        a.gen_suerte = a.gen_suerte << 4;

        b.gen_suerte = b.gen_suerte << 4;
        b.gen_suerte = b.gen_suerte & 127;
        b.gen_suerte = b.gen_suerte >> 4;

        c.gen_suerte = a.gen_suerte | b.gen_suerte;

        a.gen_protection = a.gen_protection >> 4;
        a.gen_protection = a.gen_protection << 4;

        b.gen_protection = b.gen_protection << 4;
        b.gen_protection = b.gen_protection & 127;
        b.gen_protection = b.gen_protection >> 4;

        c.gen_protection = a.gen_protection | b.gen_protection;

        a.gen_bomba_cruz = a.gen_bomba_cruz >> 4;
        a.gen_bomba_cruz = a.gen_bomba_cruz << 4;

        b.gen_bomba_cruz = b.gen_bomba_cruz << 4;
        b.gen_bomba_cruz = b.gen_bomba_cruz & 127;
        b.gen_bomba_cruz = b.gen_bomba_cruz >> 4;

        c.gen_bomba_cruz = a.gen_bomba_cruz | b.gen_bomba_cruz;

        a.gen_curarse = a.gen_curarse >> 4;
        a.gen_curarse = a.gen_curarse << 4;

        b.gen_curarse = b.gen_curarse << 4;
        b.gen_curarse = b.gen_curarse & 127;
        b.gen_curarse = b.gen_curarse >> 4;

        c.gen_curarse = a.gen_curarse | b.gen_curarse;

        a.gen_esconderse = a.gen_esconderse >> 4;
        a.gen_esconderse = a.gen_esconderse << 4;

        b.gen_esconderse = b.gen_esconderse << 4;
        b.gen_esconderse = b.gen_esconderse & 127;
        b.gen_esconderse = b.gen_esconderse >> 4;

        c.gen_esconderse = a.gen_esconderse | b.gen_esconderse;

        a.gen_enfermedad = a.gen_enfermedad >> 4;
        a.gen_enfermedad = a.gen_enfermedad << 4;

        b.gen_enfermedad = b.gen_enfermedad << 4;
        b.gen_enfermedad = b.gen_enfermedad & 127;
        b.gen_enfermedad = b.gen_enfermedad >> 4;

        c.gen_enfermedad = a.gen_enfermedad | b.gen_enfermedad;

        a.gen_lanzamiento = a.gen_lanzamiento >> 4;
        a.gen_lanzamiento = a.gen_lanzamiento <<4;

        b.gen_lanzamiento = b.gen_lanzamiento << 4;
        b.gen_lanzamiento = b.gen_lanzamiento & 127;
        b.gen_lanzamiento = b.gen_lanzamiento >> 4;

        c.gen_lanzamiento = a.gen_lanzamiento | b.gen_lanzamiento;
        if(RandomValue(0, 101) < 5)
        {
            return Validacion(Mutacion(c));
        }
        return Validacion(c);
    }
    /*!
     * @brief Mutacion() se usa para mutar un bit aleatorio de un gen de un individuo random.
     * @details Se usan potencias de 2 para crear un numero binario con un 1 random, 
     * y se obtiene un random para indicar el gen al cual se usa xor para apagar o encender un bit random.
     * @param c Se usa para obtener el gen del individuo.
     * @return c Retorna el individuo con el gen ya mutado.
     */
    public nodeGenoma Mutacion(nodeGenoma c)
    {
        int value = (int)(Mathf.Pow(2, RandomValue(0, 8)));
        int gen = RandomValue(0, 12);
        switch (gen)
        {
            case 1:
                c.gen_bombas_numero ^= value; 
                break;
            case 2:
                c.gen_bomba_cruz ^= value;
                break;
            case 3:
                c.gen_bomba_potencia ^= value;
                break;
            case 4:
                c.gen_curarse ^= value;
                break;
            case 5:
                c.gen_enfermedad ^= value;
                break;
            case 6:
                c.gen_esconderse ^= value;
                break;
            case 7:
                c.gen_lanzamiento ^= value;
                break;
            case 8:
                c.gen_protection ^= value;
                break;
            case 9:
                c.gen_suerte ^= value;
                break;
            case 10:
                c.gen_velocidad ^= value;
                break;
            default:
                c.gen_vidas ^= value;
                break;
        }
        return c;
    }
}


