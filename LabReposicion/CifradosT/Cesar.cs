using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace LabReposicion.CifradosT
{
    public class Cesar
    {
        public bool VerificarClave(string clave, ref char[] ClaveArray)
        {
            var CharList = new List<char>();
            var VectorString = clave.Split(' ');
            if (VectorString.Length > 1)
            {
                return false;
            }
            var textoNormalizado = clave.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9]");
            var claveSinTildes = reg.Replace(textoNormalizado, "");
            ClaveArray = claveSinTildes.ToCharArray();
            for (int i = 0; i < ClaveArray.Length; i++)
            {
                if (!char.IsLetter(ClaveArray[i]))
                {
                    return false;
                }
                if (!CharList.Contains(ClaveArray[i]))
                {
                    CharList.Add(ClaveArray[i]);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public void ProcesarClave(ref char[] claveArray, string clave)
        {
            var textoNormalizado = clave.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9]");
            var claveSinTildes = reg.Replace(textoNormalizado, "");
            claveArray = claveSinTildes.ToCharArray();
        }

        public byte[] Abecedario()
        {
            string abecedario = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
            char[] ArrayOriginal = abecedario.ToCharArray();
            return Encoding.Default.GetBytes(ArrayOriginal);
        }
        public byte[] AbecedarioModificado(byte[] claveArray, int ArrayLenght, byte[] AbecedarioArray)
        {
            byte[] tmpArray = new byte[ArrayLenght];
            var contador = claveArray.Length;
            var contadorRepetidos = 0;
            for (int i = 0; i < claveArray.Length; i++)
            {
                if (!tmpArray.Contains(claveArray[i]))
                {
                    tmpArray[i - contadorRepetidos] = claveArray[i];
                }
                else
                {
                    contadorRepetidos++;
                }
            }
            contador -= contadorRepetidos;
            for (int i = 0; i < tmpArray.Length; i++)
            {
                if (!tmpArray.Contains(AbecedarioArray[i]) && contador < tmpArray.Length)
                {
                    tmpArray[contador] = AbecedarioArray[i];
                    contador++;
                }
            }
            return tmpArray;
        }
        public Dictionary<byte, byte> FormandoDiccionario(byte[] Abecedario, byte[] AbecedarioConClave)
        {
            var tmpDiccionario = new Dictionary<byte, byte>();
            for (int i = 0; i < Abecedario.Length; i++)
            {
                if (!tmpDiccionario.ContainsKey(Abecedario[i]))
                {
                    tmpDiccionario.Add(Abecedario[i], AbecedarioConClave[i]);
                }
            }
            var espacioByte = Convert.ToByte(' ');
            tmpDiccionario.Add(espacioByte, espacioByte);
            return tmpDiccionario;
        }
        const int bufferLenght = 10000;
        public void CifrarMensaje(char[] claveArray, string ubicacion, string ubicacionDestino, string[] ArrayNombre, Dictionary<byte, byte> DiccionarioCaracteres, string extension, bool bandera)
        {
            using (var stream = new FileStream(ubicacion, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writeStream = new FileStream($"{ubicacionDestino}/{ArrayNombre[0]}.{extension}", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            var byteBuffer = new byte[bufferLenght];

                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLenght);
                                var caracter = new byte();
                                var escrito = new byte();
                                for (int i = 0; i < byteBuffer.Length; i++)
                                {
                                    caracter = byteBuffer[i];
                                    if (bandera == true)
                                    {
                                        if (!DiccionarioCaracteres.ContainsKey(caracter))
                                        {
                                            writer.Write(caracter);
                                        }
                                        else
                                        {
                                            escrito = DiccionarioCaracteres[caracter];
                                            writer.Write(escrito);
                                        }
                                    }
                                    else
                                    {
                                        if (!DiccionarioCaracteres.ContainsKey(caracter))
                                        {
                                            writer.Write(caracter);
                                        }
                                        else
                                        {
                                            writer.Write(DiccionarioCaracteres[caracter]);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
