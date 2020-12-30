using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaSimple
{
    private NodoLista head;
    private int size;

    /*!
    *@brief Constructor de ListaSimple.
    */
    public ListaSimple()
    {
        head = null;
        size = 0;
    }

    /*!
    *@brief Funcion que annade un nodo a la lista con el dato especificado
    *@param _nodo tipo Dato, contiene la posicion y los valores G,F,H.
    *@return void
    */
    public void enqueue(Dato _nodo)
    {
        //Console.WriteLine("Annadiendo nodo " + _nodo.getId()[0] + ", " + _nodo.getId()[1]);

        NodoLista tmp = new NodoLista();
        tmp.setData(_nodo);

        if (head == null)// Si esta vacia, inserta en el Head
        {
            head = tmp;

        }
        else           //De no ser el caso busca el ultimo nodo y lo enlaza
        {
            NodoLista aux = head;

            while (aux.getSig() != null)
            {
                aux = aux.getSig();
            }
            aux.setSig(tmp);
        }
        size++;
    }

    /*!
    *@brief Elimina en primer nodo de la lista enlazada.
    *@return void
    */
    public void dequeue()
    {
        //Console.WriteLine("Ya habia revisado  " + head.getData().getId()[0] + ", " + head.getData().getId()[1]);

        if (head != null)
        {
            head = head.getSig();

        }
        size--;
    }
    /*!
    *@brief Busca si ya existe un nodo con los mismos valores del dato.
    *@param _nodo, tipo Dato;
    *@return bool, true si existe coincidencia, false si no
    */
    public bool find(Dato _nodo)
    {
        //Console.WriteLine("Voy a buscar " + _nodo.getId()[0] + ", " + _nodo.getId()[1]);

        NodoLista tmp = head;

        while (tmp != null)
        {
            if ((tmp.getData().getId()[0] == _nodo.getId()[0]) && (tmp.getData().getId()[1] == _nodo.getId()[1]))
            {
                return true;
            }
            tmp = tmp.getSig();
        }
        return false;
    }

    /*!
    *@brief Ordena la lista de menor a mayor valor de F.
    *       Comportamiento de Bubble Sort.
    *@param _nodo, tipo Dato;
    *@return void
    */
    public void sort()
    {
        //Console.WriteLine("Entre el sort");
        printLista();

        int tamanno = this.tamanno();
        //Console.WriteLine("Tamanno: " + tamanno);
        //Console.WriteLine("Tamanno real: " + size);

        bool flag;

        if (tamanno > 1)
        {
            NodoLista current;
            //Console.WriteLine("Ordenando");

            for (int i = 0; i < tamanno - 1; i++)
            {
                current = head;

                flag = false;

                for (int j = 0; j < tamanno - 1 - i; j++)
                {
                    if (current.getData().getF() > current.getSig().getData().getF())
                    {
                        flag = true;
                        swap(current, current.getSig());
                    }
                    current = current.getSig();

                }
                if (!flag) break;
            }
        }
        //printSort();
        //printLista();
        //Console.WriteLine("Termine el sort\n");

    }

    /*!
    *@brief Intercambia valores de datos entre los dos nodos seleccionados
    *       Funcion auxiliar de sort().
    *@see sort()
    *@param _nodo1, tipo NodoLista.
    *@param _nodo2, tipo NodoLista.
    *@return void
    */
    private void swap(NodoLista _nodo1, NodoLista _nodo2)
    {
        //Console.WriteLine("Entre en el swap");
        //Console.Write(_nodo1.getData().getId()[0] + ", " + _nodo1.getData().getId()[1] + " <->");
        //Console.WriteLine(_nodo2.getData().getId()[0] + ", " + _nodo2.getData().getId()[1]);

        //Intercambio con ayuda de un temporal
        Dato tmp = _nodo1.getData();
        _nodo1.setData(_nodo2.getData());
        _nodo2.setData(tmp);

        //Console.WriteLine("Termine el swap");

    }

    /*!
    *@brief Determina la cantidad de elementos en la lista
    *       Funcion auxiliar de sort().
    *@see sort()
    *@return int
    */
    private int tamanno()
    {
        int count = 0;
        NodoLista tmp = head;

        if (tmp == null)
        {
            return count;

        }
        else
        {
            while (tmp != null)
            {
                count++;
                tmp = tmp.getSig();
            }
            return count;
        }
    }

    /*!
    *@brief Imprime en consola los valores de posicion de cada nodo.
    *@return void
    */
    public void printLista()
    {
        NodoLista temp = head;
        while (temp != null)
        {
            //Console.Write(temp.getData().getId()[0] + "," + temp.getData().getId()[1] + " -> ");
            temp = temp.getSig();
        }
        //Console.Write("// \n");

    }

    /*!
    *@brief Imprime en consola los valores de F de cada nodo.
    *@return void
    */
    public void printSort()
    {
        NodoLista temp = head;

        while (temp != null)
        {
            //Console.Write(temp.getData().getF() + " -> ");
            temp = temp.getSig();
        }
        //Console.Write("// \n");

    }

    /*!
    *@brief Libera todos los nodos guardados en la lista.
    *@return void
    */
    public void cleanLista()
    {
        //Console.WriteLine("Purgando lista");

        head = null;
    }

    /*!
    *@brief Retorna en nodo head de la lista.
    *@return NodoLista
    */
    public NodoLista getHead()
    {
        return this.head;
    }

    public int getSize()
    {
        return this.size;
    }
}
