using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Numerics;

namespace LabReposicion.Cifradores
{
    public class RSA
    {
        public int N(int p, int q)
        {
            int N = p * q;
            return N;
        }
        //ø(N) = (p-1)(q-1) siempre da un numero par
        public int øN(int p, int q)
        {
            int øN = (p - 1) * (q - 1);
            return øN;
        }
        //e = 1<e<ø(N) y debe ser coprimo entre ø(N) y N

        public List<int> MCD(int Num)
        {
            List<int> ListaCocientes = new List<int>();
            int Cociente = 0, Divisor = 1, Dividendo = Num;
            while (Divisor != Dividendo)
            {
                if (Dividendo % Divisor == 0)
                {
                    Cociente = Dividendo / Divisor;
                    ListaCocientes.Add(Cociente);
                    Divisor++;
                }
                else
                {
                    Divisor++;
                }
            }
            return ListaCocientes;
        }

        public bool EncontrarCoprimo(List<int> lista1, List<int> lista2)
        {
            bool ListaMayor = false;
            bool Coprimo = false;
            if (lista1.Count > lista2.Count)
            {
                ListaMayor = false;
            }
            else
            {
                ListaMayor = true;
            }
            if (ListaMayor == false)
            {
                foreach (var item in lista1)
                {
                    if (lista2.Contains(item))
                    {
                        Coprimo = false;
                    }
                    else
                    {
                        Coprimo = true;
                    }
                }
            }
            else
            {
                foreach (var item in lista2)
                {
                    if (lista1.Contains(item))
                    {
                        Coprimo = false;
                    }
                    else
                    {
                        Coprimo = true;
                    }
                }
            }
            return Coprimo;
        }


        public int EncontrarEuler(int p, int q)
        {
            List<int> ListaCocientes1 = MCD(p);
            List<int> listaCocientes2 = MCD(q);
            int E = 0;
            Random numero = new Random();
            E = numero.Next(2, p);
            bool Coprimo = EncontrarCoprimo(ListaCocientes1, listaCocientes2);
            if (Coprimo == true)
            {
                if (ListaCocientes1.Count > listaCocientes2.Count)
                {
                    foreach (var item in listaCocientes2)
                    {
                        if (item < p && item > 1)
                        {
                            return item;
                        }
                    }
                }
                else
                {
                    foreach (var item in ListaCocientes1)
                    {
                        if (item < p && item > 1)
                        {
                            return item;
                        }
                    }
                }
                return E;
            }
            else
            {
                return E;
            }
        }

        public int EncontrarD(int Euler, int phi)
        {
            int AnteriorIzq = phi, AnteriorDer = phi, SiguienteIzq = Euler, SiguienteDer = 1;
            int Division = Convert.ToInt16(Math.Floor(Convert.ToDouble(AnteriorIzq) / Convert.ToDouble(SiguienteIzq)));
            int Multi1 = Division * SiguienteIzq, Multi2 = Division * SiguienteDer, Resta1 = SiguienteIzq, Resta2 = SiguienteDer;
            int tmp1 = 0, tmp2 = 0;

            while (SiguienteIzq != 1)
            {
                if (Multi1 < AnteriorIzq && Multi2 < AnteriorDer)//(Resta1 > 0 && Resta2 > 0)
                {

                    tmp1 = SiguienteIzq;
                    tmp2 = SiguienteDer;
                    SiguienteIzq = AnteriorIzq - Multi1;
                    SiguienteDer = AnteriorDer - Multi2;
                    AnteriorIzq = tmp1;
                    AnteriorDer = tmp2;
                    Division = Convert.ToInt16(Math.Floor(Convert.ToDouble(AnteriorIzq) / Convert.ToDouble(SiguienteIzq)));
                    Multi1 = Division * SiguienteIzq;
                    Multi2 = Division * SiguienteDer;
                }
                else //(Resta1 < 0 || Resta2 < 0 || (Resta1 < 0 && Resta2 < 0)) 
                {
                    while (Multi1 > AnteriorIzq || Multi2 > AnteriorDer || (Multi1 > AnteriorIzq && Multi2 > AnteriorDer))
                    {
                        if (Multi2 > AnteriorDer)
                        {
                            Multi2 = Multi2 - phi;
                        }
                        else if (Multi1 > AnteriorIzq)
                        {
                            Multi1 = Multi1 - phi;
                        }
                        else // (Multi2 > AnteriorDer && Multi1 > AnteriorIzq)
                        {
                            Multi1 = Multi1 - phi;
                            Multi2 = Multi2 - phi;
                        }
                    }
                }
            }
            return SiguienteDer;
        }

        public string FormulazoCifrado(int[] Kpub, int Caracter)
        {
            int TextoANum = 0;
            int Cifrado = 0;
            BigInteger POTENCIA = new BigInteger();
            TextoANum = Convert.ToInt32(Caracter);
            POTENCIA = BigInteger.Pow(TextoANum, Kpub[0]);
            Cifrado = (int)(POTENCIA % Kpub[1]);
            var AuxCif = Convert.ToString(Cifrado);
            return AuxCif;
        }

        public List<string> FormulazoDesCifrado(int[] Kpriv, string Cifrados)
        {
            List<string> DesCifrados = new List<string>();
            BigInteger POTENCIA = new BigInteger();
            int Descifrado = 0;
            POTENCIA = BigInteger.Pow(Convert.ToInt64(Cifrados), Kpriv[0]);
            Descifrado = (int)(POTENCIA % Kpriv[1]);
            DesCifrados.Add(Convert.ToString(Convert.ToChar(Descifrado)));
            return DesCifrados;
        }

        //Kpub = (e, N)

        public int[] ReceptorKpub(int Euler, int N)
        {
            int[] VectorKpub = { Euler, N };
            return VectorKpub;
        }

        //kpiv = (d, N)

        public int[] ReceptorKpriv(int D, int N)
        {
            int[] VectorKpriv = { D, N };
            return VectorKpriv;
        }

        public bool esPrimoV2(int numero)
        {
            int divisor = 2;
            int resto = 0;
            while (divisor < numero)
            {
                resto = numero % divisor;
                if (resto == 0)
                {
                    return false;
                }
                divisor = divisor + 1;
            }
            return true;
        }
        public List<int[]> LecturaLLaves(string ruta)
        {
            var aux = new List<int[]>();
            var aux1 = new int[2];
            var aux2 = new int[2];
            using (var file = new FileStream(ruta, FileMode.OpenOrCreate))
            {
                using (var escritor = new StreamReader(file))
                {
                    var lecpub = escritor.ReadLine();
                    var ve = lecpub.Split(",");
                    aux1[0] = Convert.ToInt16(ve[0]); aux1[1] = Convert.ToInt16(ve[1]);
                    aux.Add(aux1);
                    lecpub = escritor.ReadLine();
                    ve = lecpub.Split(",");
                    aux2[0] = Convert.ToInt16(ve[0]); aux2[1] = Convert.ToInt16(ve[1]);
                    aux.Add(aux2);
                }
            }
            return aux;
        }

        public void EscribirLLaves(List<int[]> texto, string ruta)
        {
            using (var file = new FileStream(ruta, FileMode.OpenOrCreate))
            {
                using (var escritor = new StreamWriter(file))
                {
                    escritor.WriteLine(texto[0][0] + "," + texto[0][1]);
                    escritor.WriteLine(texto[1][0] + "," + texto[1][1]);
                }
            }
        }
    }
}
