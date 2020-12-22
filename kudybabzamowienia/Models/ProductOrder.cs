using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace kudybabzamowienia.Models
{
    [Table("ProductOrders")]
    public class ProductOrder
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ilosc_sztuk { get; set; }
        public String szt
        {
            get
            {
                return ilosc_sztuk.ToString()+"szt.";
            }
        }
    }
}
