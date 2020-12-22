using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace kudybabzamowienia.Models
{
    [Table("Orders")]
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public double Kwota { get; set; }
        public bool Zrealizowane { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
