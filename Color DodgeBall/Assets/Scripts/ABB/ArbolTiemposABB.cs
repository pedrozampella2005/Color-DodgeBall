using System.Collections.Generic;

public interface ArbolTiemposTDA
{
    void InicializarArbol();
    bool ArbolVacio();
    void Agregar(ResultadoNivel x);
    void RecorridoInOrder(List<ResultadoNivel> resultados);
}

public class NodoTiempoABB
{
    public ResultadoNivel info;
    public NodoTiempoABB hijoIzq;
    public NodoTiempoABB hijoDer;
}

public class ArbolTiemposABB : ArbolTiemposTDA
{
    private NodoTiempoABB raiz;

    public void InicializarArbol()
    {
        raiz = null;
    }

    public bool ArbolVacio()
    {
        return raiz == null;
    }

    public void Agregar(ResultadoNivel x)
    {
        raiz = AgregarRecursivo(raiz, x);
    }

    private NodoTiempoABB AgregarRecursivo(NodoTiempoABB nodo, ResultadoNivel x)
    {
        if (nodo == null)
        {
            NodoTiempoABB nuevo = new NodoTiempoABB();
            nuevo.info = x;
            nuevo.hijoIzq = null;
            nuevo.hijoDer = null;
            return nuevo;
        }

        if (x.tiempoCompletado < nodo.info.tiempoCompletado)
        {
            nodo.hijoIzq = AgregarRecursivo(nodo.hijoIzq, x);
        }
        else
        {
            nodo.hijoDer = AgregarRecursivo(nodo.hijoDer, x);
        }

        return nodo;
    }

    public void RecorridoInOrder(List<ResultadoNivel> resultados)
    {
        InOrderRecursivo(raiz, resultados);
    }

    private void InOrderRecursivo(NodoTiempoABB nodo, List<ResultadoNivel> resultados)
    {
        if (nodo != null)
        {
            InOrderRecursivo(nodo.hijoIzq, resultados);
            resultados.Add(nodo.info);
            InOrderRecursivo(nodo.hijoDer, resultados);
        }
    }
}