using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POLuokat;

namespace POData
{
    public class TuoteProxy : Tuote
    {
        private List<TilausRivi> _tilausRivit;
        private bool _tilausRivitHaettu = false;
        public TilausRiviRepository TilausRiviRepository { get; set; }

        public override List<TilausRivi> TilausRivit
        {
            get
            {
                if (!_tilausRivitHaettu)
                {
                    _tilausRivit = TilausRiviRepository.HaeTuotteenKaikki(this.Id);
                    _tilausRivitHaettu = true;
                }
                return _tilausRivit;
            }
            set
            {
                base.TilausRivit = value;
            }
        }

        public TuoteProxy(int id, string nimi)
                            : base(id, nimi)
        { }
    }
}
