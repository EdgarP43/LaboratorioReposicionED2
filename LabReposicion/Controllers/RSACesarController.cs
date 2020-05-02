using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Web;
using System.Text;
using LabReposicion.Cifradores;

namespace LabReposicion.Controllers
{
    public class RSAData
    {
        public string RutaArchivo { get; set; }
        public int claveCesar { get; set; }
        public int[] Kpriv { get; set; }
    }
    public class ArchivoData
    {
        public string NombreArchivo { get; set; }
    }
    [Produces("application/json")]
    [Route("api/RSACesar")]
    public class RSACesarController : Controller
    {

        // GET api/values/5
        [HttpGet("cipher/getPublicKey")]
        public string[] Getcesar([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            ArchivoData rsa = JsonConvert.DeserializeObject<ArchivoData>(a);
            var text = "Cipher: RSA";
            RSA modelo = new RSA();
            Random azar = new Random();
            var p = azar.Next(1, 100);
            var primo = modelo.esPrimoV2(p);
            while (!primo)
            {
                p = azar.Next(1, 100);
                primo = modelo.esPrimoV2(p);
            }
            var q = azar.Next(1, 100);
            primo = modelo.esPrimoV2(q);
            while (!primo)
            {
                q = azar.Next(1, 100);
                primo = modelo.esPrimoV2(q);
            }
            var N = modelo.N(p, q);
            var øN = modelo.øN(p, q);
            var Euler = modelo.EncontrarEuler(øN, N);
            var D = modelo.EncontrarD(Euler, øN);
            var Kpub = modelo.ReceptorKpub(Euler, N);
            var Kpriv = modelo.ReceptorKpriv(D, N);
            var vec = rsa.NombreArchivo.Split(".");
            var nombreArch = vec[0];
            var rutaLlaves = Path.GetFullPath("Llaves\\" + nombreArch + ".Keys");
            var listaAux = new List<int[]>();
            listaAux.Add(Kpub);
            listaAux.Add(Kpriv);
            modelo.EscribirLLaves(listaAux, rutaLlaves);
            var text2 = "Key: " + Kpub[0].ToString() + "," + Kpub[1].ToString();
            return new string[] { text, text2 };
        }

        // POST api/values
        [Route("cipher/caesar2")]
        [HttpPost]
        public void Cifrar([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            RSAData rsa = JsonConvert.DeserializeObject<RSAData>(a);
            var vec = rsa.RutaArchivo.Split("/");
            var vec2 = vec[vec.Length - 1].Split(".");
            var nombre = vec2[0];
            var rutaCif = Path.GetFullPath("CifradosRSA\\" + vec[vec.Length - 1]);
            var rutaLlaves = Path.GetFullPath("Llaves\\" + nombre + ".Keys");
            RSA modelo = new RSA();
            var Kpub = modelo.LecturaLLaves(rutaLlaves)[0];
            var Cifrado = modelo.FormulazoCifrado(Kpub, rsa.claveCesar);
            Caesar cesar = new Caesar();
            cesar.ArmarNuevoDic(rsa.claveCesar);
            var textoPlano = cesar.CargarArchivo(rsa.RutaArchivo);
            var textoCifrado = cesar.CifrarCesar(textoPlano);
            cesar.EscribirTextoParaCifrar(textoCifrado, rutaCif, Cifrado);
        }

        [Route("descipher/Caesar2")]
        [HttpPost]
        public void Descifrar([FromBody] object Cifrar)
        {
            var a = JsonConvert.SerializeObject(Cifrar);
            RSAData rsa = JsonConvert.DeserializeObject<RSAData>(a);
            var vec = rsa.RutaArchivo.Split("/");
            var vec2 = vec[vec.Length - 1].Split(".");
            var nombre = vec2[0];
            var rutaDescif = Path.GetFullPath("DescifradosRSA\\" + vec[vec.Length - 1]);
            var rutaLlaves = Path.GetFullPath("Llaves\\" + nombre + ".Keys");
            RSA modelo = new RSA();
            var kpriv = modelo.LecturaLLaves(rutaLlaves)[1];
            Caesar cesar = new Caesar();
            cesar.ArmarNuevoDic(rsa.claveCesar);
            var textoPlano = cesar.CargarArchivo(rsa.RutaArchivo);
            var textoCifrado = cesar.DescifrarCesar(textoPlano);
            cesar.EscribirTextoDescifrado(textoCifrado, rutaDescif);
        }

    }
}