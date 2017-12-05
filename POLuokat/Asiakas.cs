using System.Collections.Generic;

namespace POLuokat
{
    public class Asiakas : IAsiakasId
    {
        public string AsiakasId { get; set; }
        public string Nimi { get; set; }
        public string YhteysHenkilo { get; set; }
        public string YhteysTitteli { get; set; }
        public string Katuosoite { get; set; }
        public string Kaupunki { get; set; }
        public string Alue { get; set; }
        public string PostiKoodi { get; set; }
        public string Maa { get; set; }
        public string Puhelin { get; set; }
        public string Fax { get; set; }
        public virtual List<TilausOtsikko> Tilaukset { get; set; }
        public Asiakas()
        {
            Tilaukset = new List<TilausOtsikko>();
        }
        public Asiakas(string id, string nimi)
            : this()
        {
            AsiakasId = id;
            Nimi = nimi;
        }
        public override string ToString() => $"{AsiakasId} {Nimi} {Tilaukset.Count}";
    }
}
