using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoLista
{
    private Dato data;
    
    private NodoLista siguiente;

    /*!
    *@brief Constructor clase NodoLista
    */
    public NodoLista()
    {
        siguiente = null;
        data = null;
    }

    public Dato getData()
    {
        return this.data;
    }

    public void setData(Dato _dato)
    {
        this.data = _dato;
    }

    public NodoLista getSig()
    {
        return this.siguiente;
    }

    public void setSig(NodoLista _nodo)
    {
        this.siguiente = _nodo;
    }
}
