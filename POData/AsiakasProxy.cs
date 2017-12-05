using POLuokat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POData
{
    public class AsiakasProxy : Asiakas
    {
        private List<TilausOtsikko> _tilaukset;
        private bool _tilauksetHaettu = false;
        public TilausOtsikkoRepository TilausOtsikkoRepository { get; set; } //ORDERS

        public override List<TilausOtsikko> Tilaukset
        {
            get
            {
                if (!_tilauksetHaettu)
                {
                    _tilaukset = TilausOtsikkoRepository.HaeAsiakkaanKaikki(this.AsiakasId);
                    _tilauksetHaettu = true;
                }
                return _tilaukset;
            }
            set
            {
                base.Tilaukset = value;
            }
        }

        public AsiakasProxy(string id, string nimi)
            : base(id, nimi)
        { }
    }
}
