using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLuokat
{
    //ORDERS
    public class TilausOtsikko : IId
    {
        
        public int Id { get; }
        public string AsiakasId { get; set; }
        public int? AlainenId { get; set; }
        public DateTime? Tilauspvm { get; set; }
        public DateTime? Vaadittupvm { get; set; }
        public DateTime? Toimitettupvm { get; set; }
        public int? Toimitustapa { get; set; }
        public decimal? Rahti { get; set; }
        public string ToimitusNimi { get; set; }
        public string ToimitusOsoite { get; set; }
        public string ToimitusKaupunki { get; set; }
        public string ToimitusAlue { get; set; }
        public string ToimitusPostikoodi { get; set; }
        public string ToimitusMaa { get; set; }
        public virtual List<TilausRivi> TilausRivit { get; set; }
        public virtual List<Asiakas> Asiakkaat { get; set; }
        public TilausOtsikko()
        {
            TilausRivit = new List<TilausRivi>();
        }
        public TilausOtsikko(int id, string asiakasID)
            : this()
        {
            Id = id;
            AsiakasId = asiakasID;
        }
        public override string ToString() => $"{Id} ({TilausRivit.Count})";
    }
}
