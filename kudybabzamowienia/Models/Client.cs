using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace kudybabzamowienia.Models
{
    [Table("Clients")]
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Imie { get; set; }
        public String Nazwisko { get; set; }
        public int NIP { get; set; }
        public String Nazwa_firmy { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
