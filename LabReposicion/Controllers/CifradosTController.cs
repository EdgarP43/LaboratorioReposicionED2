using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using LabReposicion.CifradosT;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Web;
using System.Text;

namespace LabReposicion.Controllers
{
    public class ZigZagData
    {
        public string RutaArchivo { get; set; }
        public int Niveles { get; set; }
    }
    public class EspiralData
    {
        public string RutaArchivo { get; set; }
        public int Filas { get; set; }
    }
    public class CesarData
    {
        public string RutaArchivo { get; set; }
        public string Clave { get; set; }
    }
    [Produces("application/json")]
    [Route("api/CifradosT")]
    public class CifradosTController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("cipher/zigzag")]
        [HttpPost]
        public void CargarParaCifrarZigZag([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            ZigZagData Zigzag = JsonConvert.DeserializeObject<ZigZagData>(a);
            ZigZag modelo = new ZigZag();
            var vec1 = Zigzag.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var nombre = vec2[0];
            var rutaArchivo = Zigzag.RutaArchivo;
            var rutaCif = Path.GetFullPath("Archivos Cifrados\\" + "Zigzag" + nombre + ".txt");
            var Archivo = new FileStream(rutaArchivo, FileMode.OpenOrCreate);
            using (var lectura = new BinaryReader(Archivo))
            {
                var textoArchivo = new byte[Archivo.Length];
                var i = 0;
                while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                {
                    textoArchivo[i] = lectura.ReadByte();
                    i++;
                }
                var TextoCifrado = modelo.EncryptionZigZag(textoArchivo, Zigzag.Niveles);
                using (var writeStream = new FileStream(rutaCif, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(writeStream))
                    {
                        writer.Write(TextoCifrado);
                    }
                }
            }
        }

        [Route("descipher/zigzag")]
        [HttpPost]
        public void CargarParaDescifrarZig([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            ZigZagData Zigzag = JsonConvert.DeserializeObject<ZigZagData>(a);
            var vec1 = Zigzag.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var cantNiveles = Convert.ToInt32(Zigzag.Niveles);
            var nombre = vec2[0];
            var ubicacion = Zigzag.RutaArchivo;
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\" + nombre + ".txt");

            ZigZag zigzag = new ZigZag();
            var Archivo = new FileStream(ubicacion, FileMode.OpenOrCreate);
            using (var lectura = new BinaryReader(Archivo))
            {

                var textoArchivo = new byte[Archivo.Length];
                var i = 0;
                while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                {
                    textoArchivo[i] = lectura.ReadByte();
                    i++;
                }
                var textoDescifrado = zigzag.DecryptZigZag(textoArchivo, Zigzag.Niveles);
                using (var writeStream = new FileStream(ubicacionCifrados, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(writeStream))
                    {
                        writer.Write(textoDescifrado);
                    }
                }
            }
            Archivo.Close();
        }

        [Route("cipher/ruta")]
        [HttpPost]
        public void CargaParaCifrarEspi([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            EspiralData Espiral = JsonConvert.DeserializeObject<EspiralData>(a);
            var cantFilas = Convert.ToInt32(Espiral.Filas);
            var vec1 = Espiral.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var ubicacion = Espiral.RutaArchivo;
            var ubicacionCifrados = Path.GetFullPath("Archivos Cifrados\\");

            Espiral espiral = new Espiral();
            espiral.CifradoCorrecto(ubicacion, vec2, ubicacionCifrados, cantFilas);
        }

        [Route("descipher/ruta")]
        [HttpPost]
        public void CargaParaDescifrarEspi([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            EspiralData Espiral = JsonConvert.DeserializeObject<EspiralData>(a);
            var cantFilas = Convert.ToInt32(Espiral.Filas);
            var vec1 = Espiral.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var ubicacion = Espiral.RutaArchivo;
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\");

            Espiral espiral = new Espiral();
            espiral.DescifradoCorrecto(ubicacion, vec2, ubicacionCifrados, cantFilas);
        }

        [Route("cipher/caesar")]
        [HttpPost]
        public void CargaParaCifrarCesar([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            CesarData Cesar = JsonConvert.DeserializeObject<CesarData>(a);
            Cesar nuevoCesar = new Cesar();
            var ClaveArray = Cesar.Clave.ToCharArray();
            var vec1 = Cesar.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var extension = "Cesar";
            var Cifrado = true;
            var ClaveByteArray = Encoding.Default.GetBytes(ClaveArray);

            var ubicacion = Cesar.RutaArchivo;
            var ubicacionCifrados = Path.GetFullPath("Archivos Cifrados\\");


            var AbecedarioArray = nuevoCesar.Abecedario();
            var NuevoAbecedario = nuevoCesar.AbecedarioModificado(ClaveByteArray, AbecedarioArray.Length, AbecedarioArray);
            var DiccionarioDeCaracteres = nuevoCesar.FormandoDiccionario(AbecedarioArray, NuevoAbecedario);
            nuevoCesar.CifrarMensaje(ClaveArray, ubicacion, ubicacionCifrados, vec2, DiccionarioDeCaracteres, extension, Cifrado);
        }

        [Route("descipher/caesar")]
        [HttpPost]
        public void CargaParaDescifrarCesar([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            CesarData Cesar = JsonConvert.DeserializeObject<CesarData>(a);
            Cesar nuevoCesar = new Cesar();
            var ClaveArray = Cesar.Clave.ToCharArray();
            var vec1 = Cesar.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var extension = "Cesar";
            var cifrado = false;
            var ClaveByteArray = Encoding.Default.GetBytes(ClaveArray);

            var ubicacion = Cesar.RutaArchivo;
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\");

            var AbecedarioArray = nuevoCesar.Abecedario();
            var NuevoAbecedario = nuevoCesar.AbecedarioModificado(ClaveByteArray, AbecedarioArray.Length, AbecedarioArray);
            var DiccionarioInvertido = nuevoCesar.FormandoDiccionario(NuevoAbecedario, AbecedarioArray);

            nuevoCesar.CifrarMensaje(ClaveArray, ubicacion, ubicacionCifrados, vec2, DiccionarioInvertido, extension, cifrado);
        }

    }
}