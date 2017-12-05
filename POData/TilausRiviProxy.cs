using POLuokat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POData
{
    //ORDER DETAILS
    public class TilausRiviProxy : TilausRivi
    {
        private TilausOtsikko _tilausOtsikko;
        private Tuote _tuote;
        private bool _tuoteHaettu = false;
        private bool _tilausOtsikkoHaettu = false;
        public TuoteRepository TuoteRepository { get; set; }
        public TilausOtsikkoRepository TilausOtsikkoRepository { get; set; }

        public override Tuote Tuote
        {
            get
            {
                if (!_tuoteHaettu)
                {
                    _tuote = TuoteRepository.Hae(base.TuoteId);
                    _tuoteHaettu = true;

                }
                return _tuote;
            }

            set
            {
                base.Tuote = value;
            }
        }

        public override TilausOtsikko TilausOtsikko //ORDER
        {
            get
            {
                if (!_tilausOtsikkoHaettu)
                {
                    _tilausOtsikko = TilausOtsikkoRepository.Hae(base.Id);
                    _tilausOtsikkoHaettu = true;
                }
                return _tilausOtsikko;
            }
            set
            {
                base.TilausOtsikko = value;
            }
        }

        public TilausRiviProxy(int id)
                            : base(id)
        { }
    }
}
