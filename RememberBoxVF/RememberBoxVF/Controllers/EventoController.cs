using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RememberBoxVF.Models;
using RememberBoxVF.Data;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RememberBoxVF.Report;
using System.Diagnostics;
using CrystalDecisions.Data;
using CrystalDecisions.CrystalReports;
using System.IO;

namespace RememberBoxVF.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private evento_wtcEntities10 db = new evento_wtcEntities10();

        public ActionResult imprimirReporte()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/Contrato.rpt")));
            rd.SetDataSource(db.servicios.Select(p => new
            {
                Id = p.Id_servicio,
                Servicios = p.Servicio,
                Precio = p.Precio,
                Detalles = p.Propiedades
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream
                (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Contrato.pdf");
        }


        public ActionResult altaServicio(String nombre, int precio, String prop)
        {

            servicios serv = new servicios();
            serv.Servicio = nombre;
            serv.Precio = precio;
            serv.Propiedades = prop;
            
            db.servicios.Add(serv);
            db.SaveChanges();
            ViewBag.result = "Record Inserted Successfully!";

            return RedirectToAction("Principal", "Home");
        }

    }
}