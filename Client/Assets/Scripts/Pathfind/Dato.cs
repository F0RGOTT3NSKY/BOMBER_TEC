using System;

public class Dato
{
    private int[] id;

    private int H;

    private int G;

    private int F;

    private Dato padre;

    /*!
    *@brief Constructor de clase Dato
    */
    public Dato(int _i, int _j)
    {
        id = new int[2];
        id[0] = _i;
        id[1] = _j;

        padre = null;

        F = G = int.MaxValue;
        H = 0;
    }

    public int[] getId()
    {
        return this.id;
    }

    public void setId(int[] _value)
    {
        this.id = _value;
    }

    public void setPadre(Dato _padre)
    {
        this.padre = _padre;
    }

    public Dato getPadre()
    {
        return this.padre;
    }

    /*!
    *@brief Calcula el valor de H correspondiente hasta un nodo indicado
    *@return void
    */
    public void setH(Dato _fin)
    {
        this.H = Math.Abs(_fin.getId()[0] - id[0]) + Math.Abs(_fin.getId()[1] - id[1]);
    }
    public int getH()
    {
        return this.H;
    }

    public void setG(int _num)
    {
        this.G = _num;
    }

    public int getG()
    {
        return this.G;
    }

    /*!
    *@brief Calcula el valor de F Con los valores de H y G.
    *@return void
    */
    public void setF()
    {
        this.F = this.H + this.G;
    }

    public int getF()
    {
        return this.F;
    }
}
