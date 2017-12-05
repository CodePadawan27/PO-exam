using System.Collections.Generic;

namespace POLuokat
{
    public class Tuote : IId
    {
        public int Id { get; }
        public string Nimi { get; set; }
        public int? ToimittajaId { get; set; }
        public int? RyhmaId { get; set; }
        public string YksikkoKuvaus { get; set; }
        public double? YksikkoHinta { get; set; }
        public int? VarastoSaldo { get; set; }
        public int? TilausSaldo { get; set; }
        public int? HalytysRaja { get; set; }
        public bool EiKaytossa { get; set; }

        public virtual List<TilausRivi> TilausRivit { get; set; }

        public Tuote(int id, string nimi)
        {
            Id = id;
            Nimi = nimi;
        }
        public override string ToString() => $"{Id} {Nimi}";
    }
}
