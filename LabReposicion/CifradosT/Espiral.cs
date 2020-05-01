using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace LabReposicion.CifradosT
{
    public class Espiral
    {
        public bool CifradoCorrecto(string ubicacion, string[] nombreArchivo, string ubicacionAAlmacenar, int filas)
        {
            using (var stream = new FileStream(ubicacion, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writeStream = new FileStream(ubicacionAAlmacenar + nombreArchivo[0] + ".espiral", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            var caracteresTotales = Convert.ToDouble(reader.BaseStream.Length);
                            var columnas = Math.Ceiling(caracteresTotales / Convert.ToDouble(filas));

                            var matrizCifrado = new byte[filas, Convert.ToInt32(columnas)];

                            var caracteresLeidos = 0;

                            for (int i = 0; i < columnas; i++)
                            {
                                for (int j = 0; j < filas; j++)
                                {
                                    if (caracteresLeidos < caracteresTotales)
                                    {
                                        matrizCifrado[j, i] = reader.ReadByte();
                                        caracteresLeidos++;
                                    }
                                    else
                                    {
                                        matrizCifrado[j, i] = Convert.ToByte('$');
                                    }
                                }
                            }

                            var limiteMaximoColumnas = columnas;
                            var limiteMaximoFilas = filas;
                            var limiteMinimo = 0;
                            var cantBytesLeidos = 0;
                            var indiceMovilFilas = 0;
                            var indiceMovilColumnas = 0;
                            var espacioMatriz = filas * columnas;

                            while (cantBytesLeidos < espacioMatriz)
                            {
                                while (indiceMovilColumnas < limiteMaximoColumnas)
                                {
                                    writer.Write(matrizCifrado[indiceMovilFilas, indiceMovilColumnas]);
                                    indiceMovilColumnas++;
                                    cantBytesLeidos++;
                                }

                                indiceMovilColumnas--; indiceMovilFilas++;

                                while (indiceMovilFilas < limiteMaximoFilas)
                                {
                                    writer.Write(matrizCifrado[indiceMovilFilas, indiceMovilColumnas]);
                                    indiceMovilFilas++;
                                    cantBytesLeidos++;
                                }

                                indiceMovilFilas--; indiceMovilColumnas--;
                                limiteMaximoColumnas--; limiteMaximoFilas--;

                                while (indiceMovilColumnas >= limiteMinimo)
                                {
                                    writer.Write(Convert.ToByte(Convert.ToChar(matrizCifrado[indiceMovilFilas, indiceMovilColumnas])));
                                    indiceMovilColumnas--;
                                    cantBytesLeidos++;
                                }

                                indiceMovilColumnas++; indiceMovilFilas--;

                                while (indiceMovilFilas > limiteMinimo)
                                {
                                    writer.Write(Convert.ToByte(Convert.ToChar(matrizCifrado[indiceMovilFilas, indiceMovilColumnas])));
                                    indiceMovilFilas--;
                                    cantBytesLeidos++;
                                }
                                indiceMovilFilas++; indiceMovilColumnas++;
                                limiteMinimo++;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool DescifradoCorrecto(string ubicacion, string[] nombreArchivo, string ubicacionAAlmacenar, int columnas)
        {
            using (var stream = new FileStream(ubicacion, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writeStream = new FileStream($"{ubicacionAAlmacenar}/{nombreArchivo[0]}.espiral", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            var cantBytesTotal = Convert.ToDouble(reader.BaseStream.Length);
                            var filas = Math.Floor(cantBytesTotal / Convert.ToDouble(columnas));

                            var matrizCifrado = new byte[Convert.ToInt32(filas), columnas];


                            var filasMaximo = filas;
                            var columnasMaximo = columnas;
                            var columnasMin = 0;
                            var filasMin = 0;
                            var indiceMovilFilas = 0;
                            var indiceMovilColumnas = 0;

                            var cantBytesLeidos = 0;
                            var tamañoMatriz = columnas * filas;

                            while (cantBytesLeidos < tamañoMatriz)
                            {
                                while (indiceMovilFilas < filasMaximo)
                                {
                                    if (cantBytesLeidos < cantBytesTotal)
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = reader.ReadByte();
                                    }
                                    else
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = Convert.ToByte('$');
                                    }
                                    indiceMovilFilas++;
                                    cantBytesLeidos++;
                                }

                                indiceMovilColumnas++; indiceMovilFilas--;

                                while (indiceMovilColumnas < columnasMaximo)
                                {
                                    if (cantBytesLeidos < cantBytesTotal)
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = reader.ReadByte();
                                    }
                                    else
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = Convert.ToByte('$');
                                    }
                                    indiceMovilColumnas++;
                                    cantBytesLeidos++;
                                }

                                indiceMovilFilas--; indiceMovilColumnas--;


                                while (indiceMovilFilas >= filasMin)
                                {
                                    if (cantBytesLeidos < cantBytesTotal)
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = reader.ReadByte();
                                    }
                                    else
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = Convert.ToByte('$');
                                    }
                                    indiceMovilFilas--;
                                    cantBytesLeidos++;
                                }

                                columnasMin++; filasMin++;
                                indiceMovilColumnas--; indiceMovilFilas++;

                                while (indiceMovilColumnas >= columnasMin)
                                {
                                    if (cantBytesLeidos < cantBytesTotal)
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = reader.ReadByte();
                                    }
                                    else
                                    {
                                        matrizCifrado[indiceMovilFilas, indiceMovilColumnas] = Convert.ToByte('$');
                                    }
                                    indiceMovilColumnas--;
                                    cantBytesLeidos++;
                                }

                                indiceMovilFilas++; indiceMovilColumnas++;
                                filasMaximo--; columnasMaximo--;
                            }

                            var simboloRelleno = string.Empty;

                            for (int i = 0; i < filas; i++)
                            {
                                for (int j = 0; j < columnas; j++)
                                {
                                    if (matrizCifrado[i, j] != Convert.ToByte('$'))
                                    {
                                        writer.Write(Convert.ToByte(Convert.ToChar(matrizCifrado[i, j])));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
