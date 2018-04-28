using RememberBoxVF.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace RememberBoxVF.Controllers
{
    public class ContratoController : Controller
    {
        private evento_wtcEntities10 db = new evento_wtcEntities10();


        // GET: Contrato
        
        public ActionResult Reporte()
        {
            List<contratos> datos = db.contratos.SqlQuery("SELECT * FROM contratos").ToList();
            return View(datos);
        }

       

//+++++++++++++++++++++++++++++++++++++++++++++  EXPORTAR REPORTE  +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public ActionResult ExportReport()
        {
            contratos cont = new contratos();
            cont = db.contratos.SqlQuery("SELECT * FROM contratos ORDER by Id_Contrato DESC LIMIT 1").FirstOrDefault();
            

            //consulta jardin;
            List<jardin> jard = db.jardin.SqlQuery("SELECT * FROM jardin").ToList();
            List<contratos> c = db.contratos.SqlQuery("SELECT * FROM contratos ORDER by Id_Contrato DESC LIMIT 1").ToList();

            var innerJoinQuery =
           from jardin in jard
           join contratos in c on jardin.NomJar equals contratos.Jardin
           select new { contratos = contratos.Jardin, jardin = jardin.Direccion};

            var result = "";
            foreach (var item in innerJoinQuery)
            {
                 result = item.contratos+ ", " + item.jardin;
            }
            
            var varServ = cont.Servicios;
          
            var res = servicios();
            var prec = enletras(cont.Precio);

            var Detalles = "";
            //CARGA DE TEXTO AL REPORTE
            /* var reFinal = "Servicio de"+" "+varServ + ", en"+" "+ result +". " ;
             var Detalles = "Incluye el siguiente servicio: " + "\n" + res;
             var ejemplo= "Servicio de" + " " + varServ + ", en" + " " + result + ". "+"\n"+"\n"+ "Incluye el siguiente servicio: " + "\n" + res;*/
            //DATOS AL DATASET EN LA PARTE DEL ENCABEZADO
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            var Enc = "Contrato de Prestación de Servicios Profesionales de escenario para " + " " + varServ + ", que celebran, por una parte MARÍA TIMARÁN LÓPEZ representante de Remember Box en lo sucesivo el Profesional, y por la otra " + " "
                      + cont.Nombre + " " + "en lo sucesivo el Cliente, Instrumento que se celebra de conformidad con las siguientes Declaraciones y Cláusulas:";
            var fech = cont.FechaEv; 
           var day=  String.Format("{0:dd}", fech);
            var month = String.Format("{0:MMMM}", fech);
            var year = String.Format("{0:yyyy}", fech);

            var Foot = "Por la prestación de los servicios de renta" + " " + cont.Servicios + " " + " el Cliente pagará a el Profesional la cantidad de " + "$" + cont.Precio + " " +"(" +
                        prec +" "+ "PESOS 00/100 MN)" +" "+ cont.FormaPago + ".";
            // serv = db.servicios.SqlQuery("SELECT * FROM servicios").FirstOrDefault();
           
            var footFecha = "El presente contrato entrará en vigor el día de su firma, y concluirá el día " + "" + day + " "+"de"+" "+month+" "+"del"+" "+year+" " + " al concluir su evento.";
           //TERMINA CARGA DE TEXTO AL REPORTE

            /*for (int i = 0; i < var2.Length-1; i++)
            {
            
                // var rsd = var2[i];

                 var prba = from servicios in serv
                          join contratos in var2[i] on servicios.Servicio equals var2[i]
                            select new { contratos = var2[i], servicios = servicios.Propiedades };

                foreach (var item12 in prba)
                {
                     result2 = item12.servicios + ", " +item12.contratos;
                    
                }

            }*/
            

            //dataset
            DataTable dt = new DataTable("Contratos");
            dt.Columns.Add("IdContrato");
            dt.Columns.Add("NombreCliente");
            dt.Columns.Add("NombreJardin");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Servicios");
            dt.Columns.Add("Precio");
            dt.Columns.Add("FormaPago");
            dt.Columns.Add("Detalles");
            dt.Columns.Add("Encabezado");
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport1.rpt"));

            
                          
            dt.Rows.Add(cont.Id_Contrato, cont.Nombre, result, footFecha, res, prec, Foot, Detalles, Enc);
            
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            rd.SetDataSource(ds);
           
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            
            String rutaGuardado = Server.MapPath("~/Contrato.pdf");
            rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaGuardado);

            //mostrar
            ProcessStartInfo loPSI = new ProcessStartInfo();
            Process loProceso = new Process();
            loPSI.FileName = "";
            try
            {
                loProceso = Process.Start(rutaGuardado);
            }
            catch (Exception x)
            {
                //MessageBox.Show(Exp.Message, "XXXX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return RedirectToAction("Indice", "Home");
        }


        


//**********************************************Consulta Servicios 1******************************************************************************************************
        private String servicios()
        {
            //consulta jardin;
            List<jardin> jard = db.jardin.SqlQuery("SELECT * FROM jardin").ToList();
            List<contratos> c = db.contratos.SqlQuery("SELECT * FROM contratos ORDER by Id_Contrato DESC LIMIT 1").ToList();

            var innerJoinQuery =
           from jardin in jard
           join contratos in c on jardin.NomJar equals contratos.Jardin
           select new { contratos = contratos.Jardin, jardin = jardin.Direccion };

            var result = "";
            foreach (var item in innerJoinQuery)
            {
                result = item.contratos + ", " + item.jardin;
            }


            //Consulta Servicios
            contratos cont = new contratos();
            cont = db.contratos.SqlQuery("SELECT * FROM contratos ORDER by Id_Contrato DESC LIMIT 1").FirstOrDefault();

            List<servicios> serv = db.servicios.SqlQuery("SELECT * FROM servicios").ToList();

            var var1 = cont.Servicios;


            String[] var2 = var1.Split(',');

           
            
            int i=0;

            var val1 = "";
            var val2 = "";
            var val3 = "";
            var val4 = "";
            var val5 = "";
            var val6 = "";
            var val7 = "";
            var val8 = "";
            var val9 = "";
            var val10 = "";
            var val11 = "";
            var val12 = "";
            var val13 = "";
            var val14 = "";
            var val15 = "";
            var val16 = "";
            var val17 = "";
            var val18 = "";
            var val19 = "";
            var val20 = "";
            var cont1 = "";
            var cont2 = "";
            var cont3 = "";
            var cont4 = "";
            var cont5 = "";
            var cont6 = "";
            var cont7 = "";
            var cont8 = "";
            var cont9 = "";
            var cont10 = "";
            var cont11 = "";
            var cont12 = "";
            var cont13 = "";
            var cont14 = "";
            var cont15 = "";
            var cont16 = "";
            var cont17 = "";
            var cont18 = "";
            var cont19 = "";
            var cont20 = "";
            //serv
            var serv1 = "";
            var serv2 = "";
            var serv3 = "";
            var serv4 = "";
            var serv5 = "";
            var serv6 = "";
            var serv7 = "";
            var serv8 = "";
            var serv9 = "";
            var serv10 = "";
            var serv11 = "";
            var serv12 = "";
            var serv13 = "";
            var serv14 = "";
            var serv15 = "";
            var serv16 = "";
            var serv17 = "";
            var serv18 = "";
            var serv19 = "";
            var serv20 = "";

            for (i=0;i<var2.Length;i++)
            { 

               var prba = from servicios in serv
               join contratos in var2[i] on servicios.Servicio equals var2[i]
               select new { contratos= var2[i], servicios = servicios.Propiedades } ;

               

                foreach (var item in prba)
                {
                    if(i==0)
                    {
                        cont1 = item.contratos;
                        val1 = item.servicios;
                        serv1 ="***"+" "+ "Servicio de" + " " + cont1 + ", en" + " " + result + ". " + "\n" + "\n" + "Incluye el siguiente servicio: " + "\n" + val1;
                    }
                    else if (i == 1)
                    {
                        cont2 = item.contratos;
                        val2 = item.servicios;
                        serv2 = "***" +" "+ "Servicio de" + " " + cont2 + ", en" + " " + result + ". " + "\n" + "\n" + "Incluye el siguiente servicio: " + "\n" + val2;
                    }
                    else if (i == 2)
                    {
                        cont3 = item.contratos;
                        val3 = item.servicios;
                        serv3 = "***" + " " + "Servicio de" + " " + cont3 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val3;
                    }
                    else if (i == 3)
                    {
                        cont4 = item.contratos;
                        val4 = item.servicios;
                        serv4 = "***" + " " + "Servicio de" + " " + cont4 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val4;
                    }
                    else if (i == 4)
                    {
                        cont5 = item.contratos;
                        val5 = item.servicios;
                        serv5 = "***" + " " + "Servicio de" + " " + cont5 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val5;
                    }
                    else if (i == 5)
                    {
                        cont6 = item.contratos;
                        val6 = item.servicios;
                        serv6 = "***" + " " + "Servicio de" + " " + cont6 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val6;
                    }
                    else if (i == 6)
                    {
                        cont7 = item.contratos;
                        val7 = item.servicios;
                        serv7 = "***" + " " + "Servicio de" + " " + cont7 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val7;
                    }
                    else if (i == 7)
                    {
                        cont8 = item.contratos;
                        val8 = item.servicios;
                        serv8 = "***" + " " + "Servicio de" + " " + cont8 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val8;
                    }
                    else if (i == 8)
                    {
                        cont9 = item.contratos;
                        val9 = item.servicios;
                        serv9 = "***" + " " + "Servicio de" + " " + cont9 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val9;
                    }
                    else if (i == 9)
                    {
                        cont10 = item.contratos;
                        val10 = item.servicios;
                        serv10 = "***" + " " + "Servicio de" + " " + cont10 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val10;
                    }
                    else if (i == 10)
                    {
                        cont11 = item.contratos;
                        val11 = item.servicios;
                        serv11 = "***" + " " + "Servicio de" + " " + cont11 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val11;
                    }
                    else if (i == 11)
                    {
                        cont12 = item.contratos;
                        val12 = item.servicios;
                        serv12 = "***" + " " + "Servicio de" + " " + cont12 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val12;
                    }
                    else if (i == 12)
                    {
                        cont13 = item.contratos;
                        val13 = item.servicios;
                        serv13 = "***" + " " + "Servicio de" + " " + cont13 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val13;
                    }
                    else if (i == 13)
                    {
                        cont14 = item.contratos;
                        val14 = item.servicios;
                        serv14 = "***" + " " + "Servicio de" + " " + cont14 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val14;
                    }
                    else if (i == 14)
                    {
                        cont15 = item.contratos;
                        val15 = item.servicios;
                        serv15 = "***" + " " + "Servicio de" + " " + cont15 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val15;
                    }
                    else if (i == 15)
                    {
                        cont16 = item.contratos;
                        val16 = item.servicios;
                        serv16 = "***" + " " + "Servicio de" + " " + cont16 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val16;
                    }
                    else if (i == 16)
                    {
                        cont17 = item.contratos;
                        val17 = item.servicios;
                        serv17 = "***" + " " + "Servicio de" + " " + cont17 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val17;
                    }
                    else if (i == 17)
                    {
                        cont18 = item.contratos;
                        val18 = item.servicios;
                        serv18 = "***" + " " + "Servicio de" + " " + cont18 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val18;
                    }
                    else if (i == 18)
                    {
                        cont19 = item.contratos;
                        val19 = item.servicios;
                        serv19 = "***" + " " + "Servicio de" + " " + cont19 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val19;
                    }
                    else if (i == 19)
                    {
                        cont20 = item.contratos;
                        val20 = item.servicios;
                        serv20 = "***" + " " + "Servicio de" + " " + cont20 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val20;
                    }

                }

            }

            /*var result2 = val1 + "\n" + "\n" + val2 + "\n" + "\n" + val3 + "\n" + "\n" + val4 + "\n" + "\n" + val5 + "\n" + "\n" + val6 + "\n" + "\n" +
                          val7 + "\n" + "\n" + val8 + "\n" + "\n" + val9 + "\n" + "\n" + val10 + "\n" + "\n" + val11 + "\n" + "\n" + val12 + "\n" + "\n" +
                          val13 + "\n" + "\n" + val14 + "\n" + "\n" + val15 + "\n" + "\n" + val16 + "\n" + "\n" + val17 + "\n" + "\n" + val18 + "\n" + "\n" +
                          val19 + "\n" + "\n" + val20;*/

            var servic = serv1.ToUpper() + "\n" + "\n" + serv2.ToUpper() + "\n" + "\n" + serv3.ToUpper() + "\n" + "\n" + serv4.ToUpper() + "\n" + "\n" + serv5.ToUpper() + "\n" + "\n" + serv6.ToUpper() + "\n" + "\n" +
                         serv7.ToUpper() + "\n" + "\n" + serv8.ToUpper() + "\n" + "\n" + serv9.ToUpper() + "\n" + "\n" + serv10.ToUpper() + "\n" + "\n" + serv11.ToUpper() + "\n" + "\n" + serv12.ToUpper() + "\n" + "\n" +
                         serv13.ToUpper() + "\n" + "\n" + serv14.ToUpper() + "\n" + "\n" + serv15.ToUpper() + "\n" + "\n" + serv16.ToUpper() + "\n" + "\n" + serv17.ToUpper() + "\n" + "\n" + serv18.ToUpper() + "\n" + "\n" +
                         serv19.ToUpper() + "\n" + "\n" + serv20.ToUpper();
           
          
            

            /* 
             var prba = from servicios in serv
                    join contratos in var2[i] on servicios.Servicio equals var2[i]
                    select new { servicios = servicios.Propiedades };
             foreach (var item12 in prba)
             {

                 result2 = item12.servicios;

             }
             i++;*/



            return servic;
        }

 //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++   REIMPRIMIR   REPORTE   ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public ActionResult ReimprimirReport(int id)
        {
            contratos cont = new contratos();
            cont = db.contratos.SqlQuery("SELECT * FROM contratos WHERE Id_Contrato=" + id).FirstOrDefault();


            //****************************************************************************************************************************************************
            //consulta jardin;
            List<jardin> jard = db.jardin.SqlQuery("SELECT * FROM jardin").ToList();
            List<contratos> c = db.contratos.SqlQuery("SELECT * FROM contratos where Id_Contrato="+ id).ToList();

            var innerJoinQuery =
           from jardin in jard
           join contratos in c on jardin.NomJar equals contratos.Jardin
           select new { contratos = contratos.Jardin, jardin = jardin.Direccion };

            var result = "";
            foreach (var item in innerJoinQuery)
            {
                result = item.contratos + ", " + item.jardin;
            }


            //Consulta Servicios
            

            List<servicios> serv = db.servicios.SqlQuery("SELECT * FROM servicios").ToList();

            var var1 = cont.Servicios;


            String[] var2 = var1.Split(',');



            int i = 0;

            var val1 = "";
            var val2 = "";
            var val3 = "";
            var val4 = "";
            var val5 = "";
            var val6 = "";
            var val7 = "";
            var val8 = "";
            var val9 = "";
            var val10 = "";
            var val11 = "";
            var val12 = "";
            var val13 = "";
            var val14 = "";
            var val15 = "";
            var val16 = "";
            var val17 = "";
            var val18 = "";
            var val19 = "";
            var val20 = "";
            var cont1 = "";
            var cont2 = "";
            var cont3 = "";
            var cont4 = "";
            var cont5 = "";
            var cont6 = "";
            var cont7 = "";
            var cont8 = "";
            var cont9 = "";
            var cont10 = "";
            var cont11 = "";
            var cont12 = "";
            var cont13 = "";
            var cont14 = "";
            var cont15 = "";
            var cont16 = "";
            var cont17 = "";
            var cont18 = "";
            var cont19 = "";
            var cont20 = "";
            //serv
            var serv1 = "";
            var serv2 = "";
            var serv3 = "";
            var serv4 = "";
            var serv5 = "";
            var serv6 = "";
            var serv7 = "";
            var serv8 = "";
            var serv9 = "";
            var serv10 = "";
            var serv11 = "";
            var serv12 = "";
            var serv13 = "";
            var serv14 = "";
            var serv15 = "";
            var serv16 = "";
            var serv17 = "";
            var serv18 = "";
            var serv19 = "";
            var serv20 = "";

            for (i = 0; i < var2.Length; i++)
            {

                var prba = from servicios in serv
                           join contratos in var2[i] on servicios.Servicio equals var2[i]
                           select new { contratos = var2[i], servicios = servicios.Propiedades };



                foreach (var item in prba)
                {
                    if (i == 0)
                    {
                        cont1 = item.contratos;
                        val1 = item.servicios;
                        serv1 = "***" + " " + "Servicio de" + " " + cont1 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val1;
                    }
                    else if (i == 1)
                    {
                        cont2 = item.contratos;
                        val2 = item.servicios;
                        serv2 = "***" + " " + "Servicio de" + " " + cont2 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val2;
                    }
                    else if (i == 2)
                    {
                        cont3 = item.contratos;
                        val3 = item.servicios;
                        serv3 = "***" + " " + "Servicio de" + " " + cont3 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val3;
                    }
                    else if (i == 3)
                    {
                        cont4 = item.contratos;
                        val4 = item.servicios;
                        serv4 = "***" + " " + "Servicio de" + " " + cont4 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val4;
                    }
                    else if (i == 4)
                    {
                        cont5 = item.contratos;
                        val5 = item.servicios;
                        serv5 = "***" + " " + "Servicio de" + " " + cont5 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val5;
                    }
                    else if (i == 5)
                    {
                        cont6 = item.contratos;
                        val6 = item.servicios;
                        serv6 = "***" + " " + "Servicio de" + " " + cont6 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val6;
                    }
                    else if (i == 6)
                    {
                        cont7 = item.contratos;
                        val7 = item.servicios;
                        serv7 = "***" + " " + "Servicio de" + " " + cont7 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val7;
                    }
                    else if (i == 7)
                    {
                        cont8 = item.contratos;
                        val8 = item.servicios;
                        serv8 = "***" + " " + "Servicio de" + " " + cont8 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val8;
                    }
                    else if (i == 8)
                    {
                        cont9 = item.contratos;
                        val9 = item.servicios;
                        serv9 = "***" + " " + "Servicio de" + " " + cont9 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val9;
                    }
                    else if (i == 9)
                    {
                        cont10 = item.contratos;
                        val10 = item.servicios;
                        serv10 = "***" + " " + "Servicio de" + " " + cont10 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val10;
                    }
                    else if (i == 10)
                    {
                        cont11 = item.contratos;
                        val11 = item.servicios;
                        serv11 = "***" + " " + "Servicio de" + " " + cont11 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val11;
                    }
                    else if (i == 11)
                    {
                        cont12 = item.contratos;
                        val12 = item.servicios;
                        serv12 = "***" + " " + "Servicio de" + " " + cont12 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val12;
                    }
                    else if (i == 12)
                    {
                        cont13 = item.contratos;
                        val13 = item.servicios;
                        serv13 = "***" + " " + "Servicio de" + " " + cont13 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val13;
                    }
                    else if (i == 13)
                    {
                        cont14 = item.contratos;
                        val14 = item.servicios;
                        serv14 = "***" + " " + "Servicio de" + " " + cont14 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val14;
                    }
                    else if (i == 14)
                    {
                        cont15 = item.contratos;
                        val15 = item.servicios;
                        serv15 = "***" + " " + "Servicio de" + " " + cont15 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val15;
                    }
                    else if (i == 15)
                    {
                        cont16 = item.contratos;
                        val16 = item.servicios;
                        serv16 = "***" + " " + "Servicio de" + " " + cont16 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val16;
                    }
                    else if (i == 16)
                    {
                        cont17 = item.contratos;
                        val17 = item.servicios;
                        serv17 = "***" + " " + "Servicio de" + " " + cont17 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val17;
                    }
                    else if (i == 17)
                    {
                        cont18 = item.contratos;
                        val18 = item.servicios;
                        serv18 = "***" + " " + "Servicio de" + " " + cont18 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val18;
                    }
                    else if (i == 18)
                    {
                        cont19 = item.contratos;
                        val19 = item.servicios;
                        serv19 = "***" + " " + "Servicio de" + " " + cont19 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val19;
                    }
                    else if (i == 19)
                    {
                        cont20 = item.contratos;
                        val20 = item.servicios;
                        serv20 = "***" + " " + "Servicio de" + " " + cont20 + ", en" + " " + result + ". " + "\n" + "Incluye el siguiente servicio: " + "\n" + val20;
                    }

                }

            }

            /*var result2 = val1 + "\n" + "\n" + val2 + "\n" + "\n" + val3 + "\n" + "\n" + val4 + "\n" + "\n" + val5 + "\n" + "\n" + val6 + "\n" + "\n" +
                          val7 + "\n" + "\n" + val8 + "\n" + "\n" + val9 + "\n" + "\n" + val10 + "\n" + "\n" + val11 + "\n" + "\n" + val12 + "\n" + "\n" +
                          val13 + "\n" + "\n" + val14 + "\n" + "\n" + val15 + "\n" + "\n" + val16 + "\n" + "\n" + val17 + "\n" + "\n" + val18 + "\n" + "\n" +
                          val19 + "\n" + "\n" + val20;*/

            var servic = serv1.ToUpper() + "\n" + "\n" + serv2.ToUpper() + "\n" + "\n" + serv3.ToUpper() + "\n" + "\n" + serv4.ToUpper() + "\n" + "\n" + serv5.ToUpper() + "\n" + "\n" + serv6.ToUpper() + "\n" + "\n" +
                         serv7.ToUpper() + "\n" + "\n" + serv8.ToUpper() + "\n" + "\n" + serv9.ToUpper() + "\n" + "\n" + serv10.ToUpper() + "\n" + "\n" + serv11.ToUpper() + "\n" + "\n" + serv12.ToUpper() + "\n" + "\n" +
                         serv13.ToUpper() + "\n" + "\n" + serv14.ToUpper() + "\n" + "\n" + serv15.ToUpper() + "\n" + "\n" + serv16.ToUpper() + "\n" + "\n" + serv17.ToUpper() + "\n" + "\n" + serv18.ToUpper() + "\n" + "\n" +
                         serv19.ToUpper() + "\n" + "\n" + serv20.ToUpper();
       
       //********************************************************************************************************************************************************************

            DataTable dt = new DataTable("Contratos");
            dt.Columns.Add("IdContrato");
            dt.Columns.Add("NombreCliente");
            dt.Columns.Add("NombreJardin");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Servicios");
            dt.Columns.Add("Precio");
            dt.Columns.Add("FormaPago");
            dt.Columns.Add("Detalles");
            dt.Columns.Add("Encabezado");
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Report/CrystalReport1.rpt"));

            servicios servv = new servicios();
            var varServ = cont.Servicios;
            var prec = enletras(cont.Precio);

            var Detalles = "";
            //CARGA DE TEXTO AL REPORTE
            /* var reFinal = "Servicio de"+" "+varServ + ", en"+" "+ result +". " ;
             var Detalles = "Incluye el siguiente servicio: " + "\n" + res;
             var ejemplo= "Servicio de" + " " + varServ + ", en" + " " + result + ". "+"\n"+"\n"+ "Incluye el siguiente servicio: " + "\n" + res;*/
            //DATOS AL DATASET EN LA PARTE DEL ENCABEZADO

            var Enc = "Contrato de Prestación de Servicios Profesionales de escenario para " + " " + varServ + ", que celebran, por una parte MARÍA TIMARÁN LÓPEZ representante de Remember Box en lo sucesivo el Profesional, y por la otra " + " "
                      + cont.Nombre + " " + "en lo sucesivo el Cliente, Instrumento que se celebra de conformidad con las siguientes Declaraciones y Cláusulas:";


            var Foot = "Por la prestación de los servicios de renta" + " " + cont.Servicios + " " + " el Cliente pagará a el Profesional la cantidad de " + "$" + cont.Precio + " " + "(" +
                        prec + " " + "PESOS 00/100 MN)" +" "+ cont.FormaPago + ".";
            // serv = db.servicios.SqlQuery("SELECT * FROM servicios").FirstOrDefault();
            var fech = cont.FechaEv;
            var day = String.Format("{0:dd}", fech);
            var month = String.Format("{0:MMMM}", fech);
            var year = String.Format("{0:yyyy}", fech);

            var footFecha = "El presente contrato entrará en vigor el día de su firma, y concluirá el día " + " " + day + " " + "de" + " " + month + " " + "del" + " " + year + " " + " al concluir su evento.";
            //TERMINA CARGA DE TEXTO AL REPORTE



            /*for (int i = 0; i <= var2.Length-1; i++)
            {
                serv = db.servicios.SqlQuery("SELECT Propiedades FROM servicios where Servicio=" + var2[0].ToString()).FirstOrDefault();

            }*/

            //dt.Rows.Add(cont.Id_Contrato, cont.Nombre, cont.Jardin, cont.Fecha, cont.Servicios, cont.Precio, cont.FormaPago, servv.Propiedades);
            //dt.Rows.Add(cont.Id_Contrato, cont.Nombre, result, footFecha, servic, cont.Precio, Foot, Detalles, Enc);



            //dt.Rows.Add(cont.Nombre, cont.Jardin,cont.Fecha,cont.Servicios,cont.Precio,cont.FormaPago);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            rd.SetDataSource(ds);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
             try
             {
                 Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                 stream.Seek(0, SeekOrigin.Begin);
                 return File(stream, "application/pdf", "Contrato.pdf");
             }
             catch (Exception ex)
             {
                 throw;
             }

/*String rutaGuardado = Server.MapPath("~/Contrato.pdf");
rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaGuardado);

//mostrar
ProcessStartInfo loPSI = new ProcessStartInfo();
Process loProceso = new Process();
loPSI.FileName = "";
try
{
   loProceso = Process.Start(rutaGuardado);
}
catch (Exception x)
{
   //MessageBox.Show(Exp.Message, "XXXX", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
return RedirectToAction("Indice", "Home");*/
}

//****************************************Conversion precio a texto*****************************************************************

public string enletras(int num)
{
string res, dec = "";
Int64 entero;
int decimales;
double nro;

try
{
   nro = Convert.ToDouble(num);
}
catch
{
   return "";
}

entero = Convert.ToInt64(Math.Truncate(nro));
decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
if (decimales > 0)
{
   dec = " CON " + decimales.ToString() + "/100";
}

res = toText(Convert.ToDouble(entero)) + dec;
return res;
}

private string toText(double value)
{
string Num2Text = "";
value = Math.Truncate(value);
if (value == 0) Num2Text = "CERO";
else if (value == 1) Num2Text = "UNO";
else if (value == 2) Num2Text = "DOS";
else if (value == 3) Num2Text = "TRES";
else if (value == 4) Num2Text = "CUATRO";
else if (value == 5) Num2Text = "CINCO";
else if (value == 6) Num2Text = "SEIS";
else if (value == 7) Num2Text = "SIETE";
else if (value == 8) Num2Text = "OCHO";
else if (value == 9) Num2Text = "NUEVE";
else if (value == 10) Num2Text = "DIEZ";
else if (value == 11) Num2Text = "ONCE";
else if (value == 12) Num2Text = "DOCE";
else if (value == 13) Num2Text = "TRECE";
else if (value == 14) Num2Text = "CATORCE";
else if (value == 15) Num2Text = "QUINCE";
else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
else if (value == 20) Num2Text = "VEINTE";
else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
else if (value == 30) Num2Text = "TREINTA";
else if (value == 40) Num2Text = "CUARENTA";
else if (value == 50) Num2Text = "CINCUENTA";
else if (value == 60) Num2Text = "SESENTA";
else if (value == 70) Num2Text = "SETENTA";
else if (value == 80) Num2Text = "OCHENTA";
else if (value == 90) Num2Text = "NOVENTA";
else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
else if (value == 100) Num2Text = "CIEN";
else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
else if (value == 500) Num2Text = "QUINIENTOS";
else if (value == 700) Num2Text = "SETECIENTOS";
else if (value == 900) Num2Text = "NOVECIENTOS";
else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
else if (value == 1000) Num2Text = "MIL";
else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
else if (value < 1000000)
{
   Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
   if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
}

else if (value == 1000000) Num2Text = "UN MILLON";
else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
else if (value < 1000000000000)
{
   Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
   if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
}

else if (value == 1000000000000) Num2Text = "UN BILLON";
else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

else
{
   Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
   if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
}
return Num2Text;

}

}
}
 