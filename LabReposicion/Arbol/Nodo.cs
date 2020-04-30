using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabReposicion.Modelos;

namespace LabReposicion.Arbol
{
    public class Nodo
    {
        public ModeloSoda valorDerecho { get; set; }
        public ModeloSoda valorIzquierdo { get; set; }
        public Nodo hijoIzquierdo { get; set; }
        public Nodo hijoMedio { get; set; }
        public Nodo hijoDerecho { get; set; }
    }
}
