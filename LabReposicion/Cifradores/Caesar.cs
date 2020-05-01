using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace LabReposicion.Cifradores
{
    public class Caesar
    {
        public string AbecedarioOriginal = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,.-{}´+'¿|<>;:_[]¨*°?¡ !\"#$%&/()=+\r" + (char)10;
        public string AbecesarioCifrado = "";
        public List<string> ListaOriginal = new List<string>();
        public List<string> ListaCif = new List<string>();

        public void ArmarNuevoDic(int clave)
        {
            for (int i = clave; i < AbecedarioOriginal.Length; i++)
            {
                AbecesarioCifrado += AbecedarioOriginal[i];
            }
            var corrido = AbecedarioOriginal.Length - AbecesarioCifrado.Length;
            for (int i = 0; i < corrido; i++)
            {
                AbecesarioCifrado += AbecedarioOriginal[i];
            }
        }

        public string CifrarCesar(string Texto)
        {
            for (int i = 0; i < AbecedarioOriginal.Length; i++)
            {
                ListaOriginal.Add(AbecedarioOriginal[i].ToString());
            }
            for (int i = 0; i < AbecesarioCifrado.Length; i++)
            {
                ListaCif.Add(AbecesarioCifrado[i].ToString());
            }
            var cif = ""; var listapos = new List<int>();
            for (int i = 0; i < Texto.Length; i++)
            {
                listapos.Add(ListaOriginal.IndexOf(Texto[i].ToString()));
            }
            foreach (var item in listapos)
            {
                cif += AbecesarioCifrado[item].ToString();
            }
            return cif;
        }

        public string DescifrarCesar(string texto)
        {
            for (int i = 0; i < AbecesarioCifrado.Length; i++)
            {
                ListaCif.Add(AbecesarioCifrado[i].ToString());
            }
            var listaposdesc = new List<int>(); var descif = "";
            for (int i = 0; i < texto.Length; i++)
            {
                listaposdesc.Add(ListaCif.IndexOf(texto[i].ToString()));
            }
            foreach (var item in listaposdesc)
            {
                descif += AbecedarioOriginal[item].ToString();
            }
            return descif;
        }

        public void EscribirTextoParaCifrar(string texto, string ruta, string Llave)
        {
            using (var file = new FileStream(ruta, FileMode.OpenOrCreate))
            {
                using (var escritor = new StreamWriter(file))
                {
                    escritor.Write(texto);
                    escritor.WriteLine("\n");
                    escritor.WriteLine("$LLaveCifrada: " + Llave);
                }
            }
        }

        public void EscribirTextoDescifrado(string texto, string ruta)
        {
            using (var file = new FileStream(ruta, FileMode.OpenOrCreate))
            {
                using (var escritor = new StreamWriter(file))
                {
                    escritor.Write(texto);
                }
            }
        }

        public string CargarArchivo(string ruta)
        {
            var txt = "";
            using (var file = new FileStream(ruta, FileMode.OpenOrCreate))
            {
                using (var lectro = new StreamReader(file))
                {
                    txt = lectro.ReadToEnd();
                }
            }
            return txt;
        }
    }
}
