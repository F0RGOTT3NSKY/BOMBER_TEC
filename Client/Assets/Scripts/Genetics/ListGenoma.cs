using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void setPuntaje(float pPuntaje)
    {
        this.Puntaje = pPuntaje;
    }

}


public class ListGenoma : MonoBehaviour
{
  
    // Atributos _________________________________________________________

    // Lista de Genomas
    private int listLength;
    private List<nodeGenoma> genomaList;
    private int frames = 0;
    


    // Contructor
    public ListGenoma()
    {
        listLength = 0;
        genomaList = new List<nodeGenoma>();

    }

    void Start()
    {
        AddNodeGenoma(20);
    }
    void Update()
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
        Organizar();
        int size = genomaList.Count / 2;
        for (int i = 0; i < size; i++)
        {
            genomaList.RemoveAt(genomaList.Count - 1);
        }
    }

    // Instantiate random number generator.  
    private readonly System.Random _random = new System.Random();

    // Generates a random number within a range.      
    public int RandomValue(int min, int max)
    {
        return _random.Next(min, max);
    }


    // Generar la puntuacion en base a la suma de los genes de los genomas
    private float PuntuationGenoma(nodeGenoma aux)
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

    private float PuntuationGenomaAux(int value, int max)
    {
        return (float) value / max;
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

    // Algoritmo genetico
    private nodeGenoma Validacion(nodeGenoma c)
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
    private nodeGenoma Combine(nodeGenoma a, nodeGenoma b)
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
    // Mutar
    private nodeGenoma Mutacion(nodeGenoma c)
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


