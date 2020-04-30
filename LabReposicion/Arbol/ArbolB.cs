using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabReposicion.Arbol;
using LabReposicion.Modelos;

namespace LabReposicion.Arbol
{
    public class ArbolB
    {
        private Nodo raiz;
        public List<ModeloSoda> contenidoArbol = new List<ModeloSoda>();
        public void inserta(ModeloSoda dato)
        {
            if (raiz == null)
            {
                var nuevoNodo = new Nodo();
                nuevoNodo.valorIzquierdo = dato;
                raiz = nuevoNodo;

            }
            else
            {
                var temporal = insertar(dato, raiz);
                if (temporal != null)
                {
                    raiz = temporal;
                }

            }

        }
        public Nodo insertar(ModeloSoda dato, Nodo nodo)
        {
            if (nodo.hijoIzquierdo == null && nodo.hijoMedio == null && nodo.hijoDerecho == null)
            {
                if (nodo.valorDerecho == null)
                {
                    if (nodo.valorIzquierdo.nombre.CompareTo(dato.nombre) == 1)
                    {
                        nodo.valorDerecho = nodo.valorIzquierdo;
                        nodo.valorIzquierdo = dato;
                    }
                    else
                    {
                        nodo.valorDerecho = dato;
                    }
                    return null;
                }
                else
                {

                    if (nodo.valorDerecho.nombre.CompareTo(dato.nombre) == -1)
                    {
                        var nuevoParaSubir = new Nodo();
                        var partirActual = new Nodo();
                        nuevoParaSubir.valorIzquierdo = nodo.valorDerecho;
                        partirActual.valorIzquierdo = dato;
                        nodo.valorDerecho = null;
                        nuevoParaSubir.hijoIzquierdo = nodo;
                        nuevoParaSubir.hijoMedio = partirActual;


                        return nuevoParaSubir;
                    }

                    else if (nodo.valorIzquierdo.nombre.CompareTo(dato.nombre) == 1)
                    {
                        var nuevoParaSubir = new Nodo();
                        var partirActual = new Nodo();
                        partirActual.valorIzquierdo = nodo.valorDerecho;
                        nodo.valorDerecho = null;
                        nuevoParaSubir.valorIzquierdo = nodo.valorIzquierdo;
                        nodo.valorIzquierdo = dato;
                        nuevoParaSubir.hijoIzquierdo = nodo;
                        nuevoParaSubir.hijoMedio = partirActual;


                        return nuevoParaSubir;
                    }
                    else
                    {
                        var nuevoParaSubir = new Nodo();
                        nuevoParaSubir.valorIzquierdo = dato;
                        var partirActual = new Nodo();
                        partirActual.valorIzquierdo = nodo.valorDerecho;
                        nodo.valorDerecho = null;
                        nuevoParaSubir.hijoIzquierdo = nodo;
                        nuevoParaSubir.hijoMedio = partirActual;
                        return nuevoParaSubir;
                    }
                }
            }
            else
            {
                if (nodo.valorDerecho == null)
                {
                    if (nodo.valorIzquierdo.nombre.CompareTo(dato.nombre) == -1)
                    {
                        var temp = insertar(dato, nodo.hijoMedio);
                        if (temp != null)
                        {
                            nodo.valorDerecho = temp.valorIzquierdo;
                            nodo.hijoMedio = temp.hijoIzquierdo;
                            nodo.hijoDerecho = temp.hijoMedio;
                        }
                        return null;
                    }
                    else
                    {
                        var temp = insertar(dato, nodo.hijoIzquierdo);
                        if (temp != null)
                        {
                            nodo.valorDerecho = nodo.valorIzquierdo;
                            nodo.valorIzquierdo = temp.valorIzquierdo;
                            nodo.hijoIzquierdo = temp.hijoIzquierdo;
                            nodo.hijoDerecho = nodo.hijoMedio;
                            nodo.hijoMedio = temp.hijoMedio;
                        }
                        return null;
                    }
                }
                else
                {

                    if (dato.nombre.CompareTo(nodo.valorIzquierdo.nombre) == -1)
                    {
                        var temporal = insertar(dato, nodo.hijoIzquierdo);
                        if (temporal != null)
                        {
                            var nodoParaSubir = new Nodo();
                            nodoParaSubir.valorIzquierdo = nodo.valorIzquierdo;
                            nodoParaSubir.hijoIzquierdo = temporal;
                            nodo.valorIzquierdo = nodo.valorDerecho;
                            nodo.valorDerecho = null;
                            nodo.hijoIzquierdo = nodo.hijoMedio;
                            nodo.hijoMedio = nodo.hijoDerecho;
                            nodo.hijoDerecho = null;
                            nodoParaSubir.hijoMedio = nodo;
                            return nodoParaSubir;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else if ((dato.nombre.CompareTo(nodo.valorDerecho.nombre) == -1))
                    {
                        var temporal = insertar(dato, nodo.hijoMedio);
                        if (temporal != null)
                        {
                            var nodoParaSubir = new Nodo();
                            nodoParaSubir.valorIzquierdo = temporal.valorIzquierdo;
                            var partirActual = new Nodo();
                            partirActual.valorIzquierdo = nodo.valorDerecho;
                            partirActual.hijoMedio = nodo.hijoDerecho;
                            partirActual.hijoIzquierdo = temporal.hijoMedio;
                            nodo.valorDerecho = null;
                            nodo.hijoDerecho = null;
                            nodoParaSubir.hijoIzquierdo = nodo;
                            nodo.hijoMedio = temporal.hijoIzquierdo;
                            nodoParaSubir.hijoMedio = partirActual;
                            partirActual.hijoIzquierdo = temporal.hijoMedio;
                            return nodoParaSubir;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        var temporal = insertar(dato, nodo.hijoDerecho);
                        if (temporal != null)
                        {
                            var nodoParaSubir = new Nodo();
                            nodoParaSubir.valorIzquierdo = nodo.valorDerecho;
                            nodo.hijoDerecho = null;
                            nodo.valorDerecho = null;
                            nodoParaSubir.hijoIzquierdo = nodo;
                            nodoParaSubir.hijoMedio = temporal;
                            return nodoParaSubir;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public string InOrden()
        {
            string contenido = null;
            contenidoArbol.RemoveAll(x => 0 == contenidoArbol.Count());
            var nodos = Recorrido(raiz);
            if (nodos != null)
            {
                foreach (var item in nodos)
                {
                    var mostrar = "-------------\n" + "Nombre: " + item.nombre + "\n" + "Sabor: " + item.sabor + "\n" + "Volumen: " + item.volumen + "\n" + "Precio: " + item.precio + "\n" + "Casa productora: " + item.productora + "\n" + "-----------------\n";
                    contenido += mostrar;
                }
            }
            else
            {
                contenido = "Arbol vacio";
            }

            return contenido;
        }
        public List<ModeloSoda> Recorrido(Nodo nodo)
        {

            if (nodo != null)
            {
                Recorrido(nodo.hijoIzquierdo);
                contenidoArbol.Add(nodo.valorIzquierdo);
                Recorrido(nodo.hijoMedio);
                if (nodo.valorDerecho != null)
                {
                    contenidoArbol.Add(nodo.valorDerecho);
                    Recorrido(nodo.hijoDerecho);
                }
            }
            return contenidoArbol;
        }

        public ModeloSoda buscar(string nombre)
        {
            return BuscarCompa(nombre, raiz);
        }
        public ModeloSoda BuscarCompa(string nombre, Nodo nodo)
        {
            if (nodo.hijoIzquierdo == null && nodo.hijoMedio == null && nodo.hijoDerecho == null)
            {
                if (nodo.valorDerecho == null)
                {
                    if (nodo.valorIzquierdo.nombre == nombre)
                    {
                        return nodo.valorIzquierdo;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (nodo.valorDerecho.nombre == nombre)
                    {
                        return nodo.valorDerecho;
                    }
                    else if (nodo.valorIzquierdo.nombre == nombre)
                    {
                        return nodo.valorIzquierdo;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (nodo.valorDerecho == null)
            {
                if (nodo.valorIzquierdo.nombre == nombre)
                {
                    return nodo.valorIzquierdo;
                }
                else
                {
                    if (nombre.CompareTo(nodo.valorIzquierdo.nombre) == 1)
                    {
                        return BuscarCompa(nombre, nodo.hijoMedio);
                    }
                    else
                    {
                        return BuscarCompa(nombre, nodo.hijoIzquierdo);
                    }
                }
            }
            else
            {
                if (nombre == nodo.valorIzquierdo.nombre)
                {
                    return nodo.valorIzquierdo;
                }
                else if (nombre == nodo.valorDerecho.nombre)
                {
                    return nodo.valorDerecho;
                }
                else if (nombre.CompareTo(nodo.valorIzquierdo.nombre) == -1)
                {
                    return BuscarCompa(nombre, nodo.hijoIzquierdo);
                }
                else if (nodo.valorIzquierdo.nombre.CompareTo(nombre) == -1 && nombre.CompareTo(nodo.valorDerecho.nombre) == -1)
                {
                    return BuscarCompa(nombre, nodo.hijoMedio);
                }
                else
                {
                    return BuscarCompa(nombre, nodo.hijoDerecho);
                }

            }
        }
    }
}
