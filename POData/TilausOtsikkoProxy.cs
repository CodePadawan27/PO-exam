using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POLuokat;

namespace POData
{
    //ORDERS
    public class TilausOtsikkoProxy : TilausOtsikko
    {
        List<TilausRivi> _tilausRivit;
        bool TilausRivitHaettu = false;

        public TilausRiviRepository TilausRiviRepository { get; set; }

        public override List<TilausRivi> TilausRivit
        {
            get
            {
                if (!TilausRivitHaettu)
                {
                    _tilausRivit = TilausRiviRepository.HaeTilauksenKaikkiTuotteet(Id);
                    TilausRivitHaettu = true;
                }
                return (_tilausRivit);
            }
            set
            {
                base.TilausRivit = value;
            }
        }

        public TilausOtsikkoProxy(int id, string asiakasID)
            : base(id, asiakasID)
        {

        }
    }
}
