using System;
using System.Collections.Generic;
using System.Linq;
using RememberBoxVF.Data;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Diagnostics;
using CrystalDecisions.Data;
using CrystalDecisions.CrystalReports;
using System.IO;

namespace RememberBoxVF.Controllers
{
    public class HomeController : Controller
    {
        private evento_wtcEntities10 db = new evento_wtcEntities10();
        public static int eventoGeneral = 0;
        public static int idEventoGral;
       


        //Mostrar las vistas con consultas
        public ActionResult Indice() {
            List<servicios> serv = db.servicios.SqlQuery("SELECT * from servicios").ToList();
            ViewBag.servicio = serv;

            List<jardin> jard = db.jardin.SqlQuery("SELECT * from jardin").ToList();
            ViewBag.jar = jard;
            
            List<contratos> co = db.contratos.SqlQuery("SELECT * from contratos ORDER BY FechaEm DESC").ToList();
            ViewBag.cont = co;

            List<evento> ev = db.evento.SqlQuery("SELECT * from evento").ToList();
            ViewBag.eve = ev;

            return View();
        }

        public ActionResult principal1(String search)
        {

            List<jardin> ja = db.jardin.SqlQuery("SELECT * from jardin where NomEnc=" + search).ToList();
            ViewBag.ja1 = ja;
            return View(ja);
        }

        //Retornos a vistas
        public ActionResult Login()
        {
            
            return View();
        }


        public ActionResult calendario3() {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult HeaderAdministrador()
        {
            return View();
        }

        public ActionResult Header()
        {
            return View();
        }
        public ActionResult EditarServ()
        {
            return View();
        }

        public ActionResult CatServ()
        {
            return View();
        }

        public ActionResult Calendario()
        {
            return View();
        }

        public ViewResult Calend()
        {
            return View();
        }

        public ActionResult Calendario12()
        {
            return View();
        }


        //Controladores de acciones
        public JsonResult GetEvents()
        {
            //Here MyDatabaseEntities is our entity datacontext (see Step 4)

            //var v = db.evento.OrderBy(a => a.fecha).ToList();
            //return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            var events = db.contratos.OrderBy(a => a.FechaEv).ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        //Realizar Registros
        public ActionResult altaServicio(String nombre, int precio, String prop)
        {

            servicios serv = new servicios();
            serv.Servicio = nombre;
            serv.Precio = precio;
            serv.Propiedades = prop;

            db.servicios.Add(serv);
            db.SaveChanges();
            ViewBag.result = "Record Inserted Successfully!";

            return RedirectToAction("Indice", "Home");
        }

        public ActionResult altaJardin(String nombre1, String nombre2, String dir, String tel)
        {

            jardin jar = new jardin();
            jar.NomEnc = nombre1;
            jar.NomJar = nombre2;
            jar.Direccion = dir;
            jar.Telefono = tel;

            db.jardin.Add(jar);
            db.SaveChanges();
            ViewBag.result = "Record Inserted Successfully!";

            return RedirectToAction("Indice", "Home");
        }

        public ActionResult altaContrato(String cliente, String jardin, DateTime inp, String serv, int precio, String fp)
        {
            var date = DateTime.Now;

            contratos cont = new contratos();
            cont.Nombre = cliente;
            cont.Jardin = jardin;
            cont.FechaEv = inp;
            cont.FechaEm = date;
            cont.Servicios = serv;
            cont.Precio = precio;
            cont.FormaPago = fp;

            db.contratos.Add(cont);
            db.SaveChanges();
            ViewBag.result = "Record Inserted Successfully!";

            return RedirectToAction("ExportReport", "Contrato");
        }


        //vistas obteniendo un parámetro
        public ActionResult Editar(int id)
        {
            List<servicios> serv = db.servicios.SqlQuery("SELECT * FROM servicios WHERE Id_servicio =" + id).ToList();
            ViewBag.service = serv;
            return View();

        }

        public ActionResult EditarJardin(int id)
        {
            List<jardin> serv = db.jardin.SqlQuery("SELECT * FROM jardin WHERE Id_Jardin =" + id).ToList();
            ViewBag.jdn = serv;
            return View();

        }

        public ActionResult Prueba(int id)
        {
            List<jardin> serv = db.jardin.SqlQuery("SELECT * FROM jardin WHERE Id_Jardin =" + id).ToList();
            ViewBag.servi = serv;
            return null;

        }

        public ActionResult VJardin(int id)
        {
            List<jardin> serv1 = db.jardin.SqlQuery("SELECT * FROM jardin WHERE Id_Jardin =" + id).ToList();
            ViewBag.serv2 = serv1;
            return View();
        }


      

        // Actualizar registros

        public ActionResult actualizarServicio(String nombre, int precio, String prop, int id)
        {
            //int precio1 = Convert.ToInt32(precio);
            servicios serv = db.servicios.FirstOrDefault(x => x.Id_servicio == id);
            serv.Servicio = nombre;
            serv.Precio = precio;
            serv.Propiedades = prop;
            
            db.SaveChanges();

            ViewBag.result = "Record Inserted Successfully!";
            return RedirectToAction("Indice","Home");
        }


        //Editar Registros
        public ActionResult EditJardin(String nombre1, String nombre2, String dir,String tel, int id)
        {
            jardin ja = db.jardin.FirstOrDefault(x => x.Id_Jardin == id);
            ja.NomEnc = nombre1;
            ja.NomJar = nombre2;
            ja.Direccion = dir;
            ja.Telefono = tel;

            db.SaveChanges();

            ViewBag.result = "Record Inserted Successfully!";
            return RedirectToAction("Indice", "Home");
        }


        //eliminar registros
        public ActionResult Eliminar(int id)
        {
            

            var query = (from p in db.servicios
                         where p.Id_servicio == id
                         select p).Single();

            db.servicios.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Indice","Home");

        }

       
        public ActionResult EliminarJardin(int id)
        {
            var query1 = (from j in db.jardin
                          where j.Id_Jardin == id
                          select j).Single();
            db.jardin.Remove(query1);
            db.SaveChanges();
            return RedirectToAction("Indice", "Home");
        }
       
       

    }
}