using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OroPuro.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Colocar Usuario")]
        [StringLength(20)]
        [RegularExpression("^[A-Za-z0-9 ]+$", ErrorMessage = "Usuario Incorrecto")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "Colocar Password")]
        [DataType(DataType.Password)]
        [StringLength(20)]
        [RegularExpression("^[A-Za-z0-9 ]+$", ErrorMessage = "Password Incorrecto")]
        public string password { get; set; }
    }

    public class DashVentasModel
    {
        public List<int> Año { get; set; }
        public List<string> Sem { get; set; }        
        public List<string> Producto { get; set; }
        public List<int> Mes { get; set; }
        public List<decimal> TotVentas { get; set; }
        public List<string> Vendedor { get; set; }
        public List<decimal> TotProducto { get; set; }
        public List<decimal> TotCanProd { get; set; }
    }
}