using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server;
using LabReposicion.Data;
using LabReposicion.Huffman;
using LabReposicion;
using Newtonsoft.Json;

namespace LabReposicion.Controllers
{
    [Produces("application/json")]
    [Route("api/Compresion")]
    public class CompresionController : Controller
    {
        public class ArchivoData
        {
            public string RutaArchivo { get; set; }
            public string NuevoNombre { get; set; }
        }

        public static Dictionary<string, Archivo> DatosDeArchivos = new Dictionary<string, Archivo>();
        // GET api/values
        [Route("Compressions")]
        [HttpGet]
        public string Get()
        {
            foreach (var item in DatosDeArchivos)
            {
                var txt = "Nombre original: " + item.Key + "\n";
                txt += "Nombre nuevo: " + item.Value.NuevoNombre + "\n";
                txt += "Ruta nuevo archivo: " + item.Value.RutaNuevoNombre + "\n";
                txt += "Razon de compresion: " + item.Value.Razon + "\n";
                txt += "Factor de compresión: " + item.Value.Factor + "\n";
                txt += "Porcentaje de reducción: " + item.Value.Porcentaje + "\n";
                return txt;
            }
            return "agregar más archivos...";
        }


        // POST api/values
        [Route("Compress/Huffman")]
        [HttpPost]
        public void PostCompri([FromBody] object Objeto)
        {
            var a = JsonConvert.SerializeObject(Objeto);
            ArchivoData Huff = JsonConvert.DeserializeObject<ArchivoData>(a);
            double PesoOriginal = 0;
            using (var file = new FileStream(Huff.RutaArchivo, FileMode.OpenOrCreate))
            { PesoOriginal = Convert.ToDouble(file.Length); };
            var vec1 = Huff.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var ubica = Path.GetFullPath("Archivos Comprimidos\\");
            var UbicacionHuffman = "Archivos comprimidos\\" + vec2[0];
            Huffman.Huffman.Instancia.CompresiónHuffman(Huff.RutaArchivo, vec2, ubica, Huff.NuevoNombre);
            var pathArchivoCompri = Path.GetFullPath("Archivos Comprimidos\\");
            var RutaArchivoCompreso = pathArchivoCompri + Huff.NuevoNombre + ".huff";
            var ArchivoCompreso = new FileInfo(RutaArchivoCompreso);
            var PesoCompreso = Convert.ToDouble(ArchivoCompreso.Length);
            var Archivo = new Archivo();
            Archivo.NombreArchivo = vec2[0];
            Archivo.Factor = Math.Round(PesoOriginal / PesoCompreso, 3);
            Archivo.Razon = Math.Round(PesoCompreso / PesoOriginal, 3);
            Archivo.Porcentaje = Math.Round(100 * (1 - Convert.ToDouble(Archivo.Razon)), 3);
            Archivo.NuevoNombre = Huff.NuevoNombre + ".huff";
            Archivo.RutaNuevoNombre = ubica;
            DatosDeArchivos.Add(Archivo.NombreArchivo, Archivo);
        }

        [Route("Descompress/Huffman")]
        [HttpPost]
        public void PostDescompri([FromBody] object Objeto)
        {
            var a = JsonConvert.SerializeObject(Objeto);
            ArchivoData Huff = JsonConvert.DeserializeObject<ArchivoData>(a);
            var filePath = Path.GetFullPath("Archivos Comprimidos\\");
            var pathDescompress = Path.GetFullPath("Archivos Descomprimidos\\");
            var vec1 = Huff.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var model = filePath + vec1[vec1.Length - 1];
            var UbicacionDescomprimidos = pathDescompress;
            Huffman.Huffman.Instancia.Descompresion(model, vec2, UbicacionDescomprimidos);

        }

        [Route("Compress/LZW")]
        [HttpPost]
        public void CompresiónLZW([FromBody] object Objeto)
        {
            var a = JsonConvert.SerializeObject(Objeto);
            ArchivoData Huff = JsonConvert.DeserializeObject<ArchivoData>(a);
            double PesoOriginal = 0;
            using (var file = new FileStream(Huff.RutaArchivo, FileMode.OpenOrCreate))
            { PesoOriginal = Convert.ToDouble(file.Length); };
            var vec1 = Huff.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var ubica = Path.GetFullPath("Archivos Comprimidos\\");
            var UbicacionAAlmacenarLZW = "Archivos comprimidos\\" + vec2[0];
            LZW.LZW.Comprimir(Huff.RutaArchivo, vec2, ubica, Huff.NuevoNombre);
            var pathArchivoCompri = Path.GetFullPath("Archivos Comprimidos\\");
            var RutaArchivoCompreso = (pathArchivoCompri + Huff.NuevoNombre + ".lzw");
            var ArchivoCompreso = new FileInfo(RutaArchivoCompreso);
            var PesoCompreso = Convert.ToDouble(ArchivoCompreso.Length);
            var Archivo = new Archivo();
            Archivo.NombreArchivo = vec2[0];
            Archivo.Factor = Math.Round(PesoOriginal / PesoCompreso, 3);
            Archivo.Razon = Math.Round(PesoCompreso / PesoOriginal, 3);
            Archivo.Porcentaje = Math.Round(100 * (1 - Convert.ToDouble(Archivo.Razon)), 3);
            Archivo.NuevoNombre = Huff.NuevoNombre + ".lzw";
            Archivo.RutaNuevoNombre = ubica;
            DatosDeArchivos.Add(Archivo.NombreArchivo, Archivo);
        }

        [Route("Descompress/LZW")]
        [HttpPost]
        public void DescompresiónLZW([FromBody] object Objeto)
        {
            var a = JsonConvert.SerializeObject(Objeto);
            ArchivoData Huff = JsonConvert.DeserializeObject<ArchivoData>(a);
            var filePath = Path.GetFullPath("Archivos Comprimidos\\");
            var pathDescompress = Path.GetFullPath("Archivos Descomprimidos\\");
            var vec1 = Huff.RutaArchivo.Split("/");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var model = filePath + vec1[vec1.Length - 1];
            var UbicacionDescomprimidos = pathDescompress;
            LZW.LZW.Descomprimir(model, vec2, UbicacionDescomprimidos);
        }
    }
}