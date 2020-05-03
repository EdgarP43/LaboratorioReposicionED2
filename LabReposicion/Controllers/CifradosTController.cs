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
        public string NombreArchivo { get; set; }
        public int Niveles { get; set; }
    }
    public class EspiralData
    {
        public string NombreArchivo { get; set; }
        public int Filas { get; set; }
    }
    public class CesarData
    {
        public string NombreArchivo { get; set; }
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
        [Route("cipher/Zigzag")]
        [HttpPost]
        public void CargarParaCifrarZigZag([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            ZigZagData Zigzag = JsonConvert.DeserializeObject<ZigZagData>(a);
            ZigZag modelo = new ZigZag();
            var nombreArchivo = Zigzag.NombreArchivo.Split('.');
            var nombre = nombreArchivo[0];
            var rutaArchivo = Path.GetFullPath(Zigzag.NombreArchivo);
            var rutaCif = Path.GetFullPath("Archivos Cifrados\\" + nombre + ".zig");
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

        [Route("descipher/Zigzag")]
        [HttpPost]
        public void CargarParaDescifrarZig([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            ZigZagData Zigzag = JsonConvert.DeserializeObject<ZigZagData>(a);
            var cantNiveles = Convert.ToInt32(Zigzag.Niveles);
            var nombreArchivo = Zigzag.NombreArchivo.Split('.');
            var nombre = nombreArchivo[0];
            var ubicacion = Path.GetFullPath("Archivos Cifrados\\" + Zigzag.NombreArchivo);
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\" + nombre + ".zig");

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
            var nombreDocumento = Espiral.NombreArchivo;
            var NombreArray = nombreDocumento.Split('.');
            var ubicacion = Path.GetFullPath(nombreDocumento);
            var ubicacionCifrados = Path.GetFullPath("Archivos Cifrados\\");

            Espiral espiral = new Espiral();
            espiral.CifradoCorrecto(ubicacion, NombreArray, ubicacionCifrados, cantFilas);
        }

        [Route("descipher/ruta")]
        [HttpPost]
        public void CargaParaDescifrarEspi([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            EspiralData Espiral = JsonConvert.DeserializeObject<EspiralData>(a);
            var cantFilas = Convert.ToInt32(Espiral.Filas);
            var nombreDocumento = Espiral.NombreArchivo;
            var NombreArray = nombreDocumento.Split('.');
            var ubicacion = Path.GetFullPath("Archivos Cifrados\\" + Espiral.NombreArchivo);
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\");

            Espiral espiral = new Espiral();
            espiral.DescifradoCorrecto(ubicacion, NombreArray, ubicacionCifrados, cantFilas);
        }

        [Route("cipher/caesar")]
        [HttpPost]
        public void CargaParaCifrarCesar([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            CesarData Cesar = JsonConvert.DeserializeObject<CesarData>(a);
            Cesar nuevoCesar = new Cesar();
            var ClaveArray = Cesar.Clave.ToCharArray();

            var NombreDocumento = Cesar.NombreArchivo;
            var extension = "Cesar";
            var ArrayNombre = NombreDocumento.Split('.');
            var Cifrado = true;
            var ClaveByteArray = Encoding.Default.GetBytes(ClaveArray);

            var ubicacion = Path.GetFullPath(NombreDocumento);
            var ubicacionCifrados = Path.GetFullPath("Archivos Cifrados\\");


            var AbecedarioArray = nuevoCesar.Abecedario();
            var NuevoAbecedario = nuevoCesar.AbecedarioModificado(ClaveByteArray, AbecedarioArray.Length, AbecedarioArray);
            var DiccionarioDeCaracteres = nuevoCesar.FormandoDiccionario(AbecedarioArray, NuevoAbecedario);
            nuevoCesar.CifrarMensaje(ClaveArray, ubicacion, ubicacionCifrados, ArrayNombre, DiccionarioDeCaracteres, extension, Cifrado);
        }

        [Route("descipher/caesar")]
        [HttpPost]
        public void CargaParaDescifrarCesar([FromBody] object Descifrar)
        {
            var a = JsonConvert.SerializeObject(Descifrar);
            CesarData Cesar = JsonConvert.DeserializeObject<CesarData>(a);
            Cesar nuevoCesar = new Cesar();
            var ClaveArray = Cesar.Clave.ToCharArray();

            var NombreDocumento = Cesar.NombreArchivo;
            var extension = "Cesar";
            var ArrayNombre = NombreDocumento.Split('.');
            var cifrado = false;
            var ClaveByteArray = Encoding.Default.GetBytes(ClaveArray);

            var ubicacion = Path.GetFullPath("Archivos Cifrados\\" + Cesar.NombreArchivo);
            var ubicacionCifrados = Path.GetFullPath("Archivos Descifrados\\");

            var AbecedarioArray = nuevoCesar.Abecedario();
            var NuevoAbecedario = nuevoCesar.AbecedarioModificado(ClaveByteArray, AbecedarioArray.Length, AbecedarioArray);
            var DiccionarioInvertido = nuevoCesar.FormandoDiccionario(NuevoAbecedario, AbecedarioArray);

            nuevoCesar.CifrarMensaje(ClaveArray, ubicacion, ubicacionCifrados, ArrayNombre, DiccionarioInvertido, extension, cifrado);
        }

    }
}