using UnityEngine;

public class AStar
{
    private int distanciaHor, nWidth, nHeight;

    private float[,] matriz;

    public ListaSimple listaAbierta, listaCerrada, solucion;

    /*!
    *@brief Constructor de AStar
    */
    public AStar(int _pesoHoriz, float[,] _matriz, int _nHeight, int _nWidth)
    {
        this.distanciaHor = _pesoHoriz;

        this.matriz = _matriz;

        this.nHeight = _nHeight;
        this.nWidth = _nWidth;
        
        listaAbierta = new ListaSimple();

        listaCerrada = new ListaSimple();

        solucion = new ListaSimple();

        Debug.Log("Se instancio AStar");
        Debug.Log("Filas = " + nHeight);
        Debug.Log("Columnas = " + nWidth);
    }

    /*!
    *@brief Prepara la matriz y los nodos de inicio y meta para buscar la posible solucion.
    *@param Matriz _matriz, referencia al mapa
    *@param int[] _inicio, par ordenado de coordenadas donde iniciar la busqueda
    *@param int[] _fin, par ordenado de cordenadas donde esta la meta
    *@return bool true si hay camino a la meta, false si no.
    *@see isSolution(Dato _inicio, Dato _fin)
    */
    public bool findPath(int[] _inicio, int[] _fin)
    {
        //Console.WriteLine("Inicio PathFinder A*:");
        //Console.WriteLine("Inicio: " + _inicio[0] + ", " + _inicio[1]);
        //Console.WriteLine("Final: " + _fin[0] + ", " + _fin[1]);

        Dato inicio = new Dato(_inicio[0], _inicio[1]);
        Dato final = new Dato(_fin[0], _fin[1]);

        return isSolution(inicio, final);
    }

    /*!
    *@brief Contruccion de relaciones entre los nodos usando la logica de A*
    *@see findPath(Matriz _matriz, int[] _inicio, int[] _fin)
    *@param _inicio tipo Dato nodo desde donde se iniciara a buscar un camino
    *@param _fin tipo Dato, nodo meta
    *@return bool true si encuentra un camino al nodo meta, false si no existe un camino
    */
    private bool isSolution(Dato _inicio, Dato _fin)
    {
        //Console.WriteLine("\nBuscando si hay solucion...\n");

        _inicio.setH(_fin);
        _inicio.setG(0);
        _inicio.setF();

        listaAbierta.enqueue(_inicio);

        Dato current = _inicio;

        //Mientras la lista abierta no esta vacia o no sea el nodo que buscamos
        while ((listaAbierta.getHead() != null) && (isGoal(current, _fin) == false))
        {
            listaAbierta.sort();

            //Mientras la lista abierta no esta vacia y el nodo no se encuentre en la lista cerrada
            while ((listaAbierta.getHead() != null)
                            && (listaCerrada.find(listaAbierta.getHead().getData()) == true))
            {
                listaAbierta.dequeue();
            }

            //Si la lista abierta esta vacia despues de los dequeue salir del ciclo
            if (listaAbierta.getHead() == null)
            {
                break;
            }

            current = listaAbierta.getHead().getData();
            //Console.WriteLine("\nActual:" + current.getId()[0] + ", " +
            //                      current.getId()[1] + ":" + isGoal(current, _fin) + "\n");

            //Si no se sale de los limites, intentar annadir el nodo ARRIBA a listaAbierta
            if (current.getId()[0] - 1 >= 0)
            {
                //Console.WriteLine("\nSetting TOP:");
                int[] tmp = { current.getId()[0] - 1, current.getId()[1] };

                addToOpen(current, tmp, distanciaHor, _fin);
            }

            //Si no se sale de los limites, intentar annadir el nodo ABAJO a listaAbierta
            if (current.getId()[0] + 1 < nHeight)
            {
                //Console.WriteLine("\nSetting BOTTOM:");
                int[] tmp = { current.getId()[0] + 1, current.getId()[1] };
                addToOpen(current, tmp, distanciaHor, _fin);
            }

            //Si no se sale de los limites, intentar annadir el nodo IZQUIERDO a listaAbierta
            if (current.getId()[1] - 1 >= 0)
            {
                //Console.WriteLine("\nSetting LEFT:");
                int[] tmp = { current.getId()[0], current.getId()[1] - 1 };
                addToOpen(current, tmp, distanciaHor, _fin);
            }

            //Si no se sale de los limites, intentar annadir el nodo DERECHO a listaAbierta
            if (current.getId()[1] + 1 < nWidth)
            {
                //Console.WriteLine("\nSetting RIGHT:");
                int[] tmp = { current.getId()[0], current.getId()[1] + 1 };
                addToOpen(current, tmp, distanciaHor, _fin);
            }

            //Console.Write("Closed: ");
            listaCerrada.enqueue(current);

        }
        //Console.WriteLine("Sali del While; o lista Abierta esta vacia o entcontre el final.");

        //printOpen();
        //printClosed();

        //Si despues del while el nodo actual es la meta seguir los padres recursivamente para formar la solucion
        //Si no, esto quiere decir que nos quedamos sin nodos abiertod para probar, lo que significa que no hay ccamino entre el inicio y la meta
        if ((current.getId()[0] == _fin.getId()[0]) &&
                                (current.getId()[1] == _fin.getId()[1]))
        {
            while (current != null)
            {
                solucion.enqueue(current);
                current = current.getPadre();
            }
            return true;

        }
        else
        {
            return false;
        }
    }

    /*!
    *@brief Verifica que el nodo no se encuentre en la lista cerrada o que no sea un obstaculo
    *@details de ser el caso se hacen los calculos necesarios y se annade a la lista abierta
    *@param _current tipo Dato del cual se annadira el nuevo nodo.
    *@param _vecino tipo int[] de dos posiciones, contiene la posicion (x,y).
    *@param _Distancia tipo int peso que se le da al moverse del nodo padre a este.
    *@param _fin tipo Dato Corresponde al nodo meta
    *@return void
    */
    private void addToOpen(Dato _current, int[] _vecino,
                                            int _distancia, Dato _fin)
    {
        //Console.WriteLine("Entre a addToOpen");
        Dato nodoVecino = new Dato(_vecino[0], _vecino[1]);

        if ((listaCerrada.find(nodoVecino) == false) &&
                    (this.matriz[nodoVecino.getId()[0], nodoVecino.getId()[1]] != 2f))
        {
            nodoVecino.setPadre(_current);
            nodoVecino.setG(_current.getG() + _distancia);
            nodoVecino.setH(_fin);
            nodoVecino.setF();

            listaAbierta.enqueue(nodoVecino);
        }/*
        else
        {
            Console.WriteLine("Era un obstaculo o ya lo habia visitado");
        }*/
    }

    /*!
    *@brief Verifica si el nodo evaluado es igual al nodo meta.
    *@param _nodo tipo Dato nodo que se quiere verificar
    *@param _fin tipo Dato nodo meta
    *@return bool true si es equivalente, false si no lo es
    */
    private bool isGoal(Dato _nodo, Dato _fin)
    {
        if ((_nodo.getId()[0] == _fin.getId()[0])
                            && (_nodo.getId()[1] == _fin.getId()[1]))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    /*!
    *@brief Imprime en consola la lista que contendria las soluciones.
    *@return void
    */
    public void printSol()
    {
        //Console.WriteLine("Solucion: ");
        solucion.printLista();
    }

    /*!
    *@brief Imprime en consola la lista cerrada (nodos visitados).
    *@return void
    */
    public void printClosed()
    {
        //Console.WriteLine("Closed: ");
        listaCerrada.printLista();
    }

    /*!
    *@brief Imprime en consola la lista abierta (nodos no visitados 
            que tienen posibilidad de ser mejor solucion).
    *@return void
    */
    public void printOpen()
    {
        //Console.WriteLine("Open: ");
        listaAbierta.printLista();
    }

    /*!
    *@brief Devuelve el par ordenado de la solucion que se encuentre 
    *        mas cercano al inicio, si este es (-1,-1) indica que la solucion 
    *        esta vacia y no existia un camino
    *@return int[]
    */
    public int[] getNearNodo()
    {
        int[] res = new int[2];

        if (solucion.getHead() != null)
        {
            NodoLista aux = null, current = solucion.getHead();

            while (current.getSig() != null)
            {
                aux = current;
                current = current.getSig();
            }

            if (aux != null)
            {
                res[0] = aux.getData().getId()[0];
                res[1] = aux.getData().getId()[1];
                return res;

            }
            else
            {
                res[0] = current.getData().getId()[0];
                res[1] = current.getData().getId()[1];
                return res;
            }
        }
        else
        {
            res[0] = -1;
            res[1] = -1;
            return res;
        }
    }

    /*!
    *@brief Purga las listas (Abierta, Solucion, Cerrada)
    *@details Purga las listas (Abierta, Solucion, Cerrada) con el fin de 
    *       que esten limpias para la siguiente corrida del algoritmo
    *@return void
    */
    public void purge()
    {
        listaAbierta.cleanLista();
        listaCerrada.cleanLista();
        solucion.cleanLista();
    }
}
