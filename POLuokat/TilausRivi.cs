using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLuokat
{
    //ORDER DETAILS
    public class TilausRivi : IId
    {
        public int Id { get; }
        public int TuoteId { get; set; }
        public decimal Hinta { get; set; }
        public int Maara { get; set; }
        public decimal Alennus { get; set; }
        public virtual TilausOtsikko TilausOtsikko { get; set; }
        public virtual Tuote Tuote { get; set; }

        public TilausRivi()
        {
            TilausOtsikko = null;
            Tuote = null;
        }

        public TilausRivi(int id)
        {
            Id = id;
        }

        public override string ToString() => $"{Id}";



    }

}
