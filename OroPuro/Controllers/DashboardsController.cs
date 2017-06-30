using Newtonsoft.Json;
using OroPuro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OroPuro.Controllers
{
    public class DashboardsController : Controller
    {
        // GET: Dashboards
        public ActionResult DashVentas()
        {
            return View();
        }

        public JsonResult DatosVentas(string selV, string selP, string selA, string selM, string selS)
        {
            OroPuroEntities db = new OroPuroEntities();
            var datos = db.SBO_SP_EntregasVendedor();
            List<SBO_SP_EntregasVendedor_Result> listaDatos = new List<SBO_SP_EntregasVendedor_Result>(datos);

            if (selV != "" || selV != "null")
            {
                string[] ven = selV.Split(',');
                List<SBO_SP_EntregasVendedor_Result> lista = new List<SBO_SP_EntregasVendedor_Result>();
                foreach (string item in ven)
                {
                    List<SBO_SP_EntregasVendedor_Result> resultado = listaDatos.Where(p => p.Vendedor.Contains(item)).ToList();
                    lista.AddRange(resultado);
                }
                listaDatos = lista;
            }

            if (selP != "" || selP != "null")
            {
                string[] pro = selP.Split(',');
                List<SBO_SP_EntregasVendedor_Result> lista = new List<SBO_SP_EntregasVendedor_Result>();
                foreach (string item in pro)
                {
                    List<SBO_SP_EntregasVendedor_Result> resultado = listaDatos.Where(p => p.Descripcion.Contains(item)).ToList();
                    lista.AddRange(resultado);
                }
                listaDatos = lista;
            }

            if (selA != "")
            {
                string[] año = selA.Split(',');
                List<SBO_SP_EntregasVendedor_Result> lista = new List<SBO_SP_EntregasVendedor_Result>();
                foreach (string item in año)
                {
                    List<SBO_SP_EntregasVendedor_Result> resultado = listaDatos.Where(p => p.Fecha.Year.ToString() == item).ToList();
                    lista.AddRange(resultado);
                }
                listaDatos = lista;
            }

            if (selM != "")
            {
                string[] mes = selM.Split(',');
                List<SBO_SP_EntregasVendedor_Result> lista = new List<SBO_SP_EntregasVendedor_Result>();
                foreach (string item in mes)
                {
                    List<SBO_SP_EntregasVendedor_Result> resultado = listaDatos.Where(p => p.Fecha.Month == Int32.Parse(item)).ToList();
                    lista.AddRange(resultado);
                }
                listaDatos = lista;
            }

            if (selS != "" || selS != "null")
            {
                string[] sem = selS.Split(',');
                List<SBO_SP_EntregasVendedor_Result> lista = new List<SBO_SP_EntregasVendedor_Result>();
                foreach (string item in sem)
                {
                    List<SBO_SP_EntregasVendedor_Result> resultado = listaDatos.Where(p => p.Sem.Contains(item)).ToList();
                    lista.AddRange(resultado);
                }
                listaDatos = lista;
            }

            var vendedores = listaDatos.Select(x => x.Vendedor).Distinct().ToList();
            var productos = listaDatos.Select(x => x.Descripcion).Distinct().ToList();
            var años = listaDatos.Select(x => x.Fecha.Year).Distinct().ToList();
            var meses = listaDatos.Select(x => x.Fecha.Month).Distinct().ToList();
            var semanas = listaDatos.Select(x => x.Sem).Distinct().ToList();
            vendedores.Remove("Direccion");
            List<decimal> TotVend = new List<decimal>();
            List<decimal> TotProd = new List<decimal>();
            List<decimal> CanProd = new List<decimal>();

            foreach (string item in vendedores)
            {
                var ven = listaDatos.Where(x => x.Vendedor.Contains(item));
                TotVend.Add(ven.Select(x => x.Total).Sum().Value);
            }

            foreach (string item in productos)
            {
                var ven = listaDatos.Where(x => x.Descripcion.Contains(item));
                TotProd.Add(ven.Select(x => x.Total).Sum().Value);
            }

            foreach (string item in productos)
            {
                var ven = listaDatos.Where(x => x.Descripcion.Contains(item));
                CanProd.Add(ven.Select(x => x.Cant).Sum().Value);
            }

            DashVentasModel Datos = new DashVentasModel();
            Datos.Vendedor = vendedores;
            Datos.Producto = productos;
            Datos.Año = años;
            Datos.Mes = meses;
            Datos.Sem = semanas;
            Datos.TotVentas = TotVend;
            Datos.TotProducto = TotProd;
            Datos.TotCanProd = CanProd;

            var r = JsonConvert.SerializeObject(Datos);
            var res = Json(r, JsonRequestBehavior.AllowGet);
            res.MaxJsonLength = int.MaxValue;
            db.Dispose();
            return res;
        }
    }
}