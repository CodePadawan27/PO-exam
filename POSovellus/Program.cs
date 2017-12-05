using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POLuokat;
using POData;
using System.Configuration;
using System.Threading;

namespace POSovellus
{
    public class Program
    {
        static ConnectionStringSettings yhteysasetukset = ConfigurationManager.ConnectionStrings["DB"];
        static void AsetaDataDirectory()
        {
            // Asetetaan muuttuja DataDirectory, jota käytetään yhteysmerkkijonossa tiedostossa App.config
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        static void Main(string[] args)
        {
            AsetaDataDirectory();
            //TuoteRepository tr = new TuoteRepository(yhteysasetukset.ConnectionString);
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);

            //TilausOtsikkoRepository tlr = new TilausOtsikkoRepository(yhteysasetukset.ConnectionString);
            //TilausRiviRepository ttr = new TilausRiviRepository(yhteysasetukset.ConnectionString);

            Alkuvalikko();

            Console.ReadLine();
        }

        public static void Alkuvalikko()
        {

            Console.Clear();
            TekstinKeskitysVarilla("Northwind-asiakkaat", ConsoleColor.Yellow);
            Console.WriteLine();
            TekstinVarinVaihto("1. Hae\n2. Lisää\n3. Muuta\n4. Poista\n5. Lopeta\nValitse: ", ConsoleColor.Yellow);

            bool onTosi;
            do
            {
                onTosi = true;

                var valinta = Console.ReadKey(true);

                switch (valinta.Key)
                {
                    case ConsoleKey.D1:
                        onTosi = true;
                        Hae();
                        break;
                    case ConsoleKey.D2:
                        onTosi = true;
                        Lisaa();
                        break;
                    case ConsoleKey.D3:
                        onTosi = true;
                        Muuta();
                        break;
                    case ConsoleKey.D4:
                        onTosi = true;
                        Poista();
                        break;
                    case ConsoleKey.D5:
                        Environment.Exit(0);
                        break;

                    default:
                        TekstinVarinVaihto("Valinta on virheellinen", ConsoleColor.Red);
                        Thread.Sleep(1000);
                        Console.Clear();
                        Alkuvalikko();
                        onTosi = false;
                        break;

                }

            } while (!onTosi);
        }

        /*----------- POISTO ------------*/

        public static void Poista()
        {
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);

            Console.ForegroundColor = ConsoleColor.Yellow;

            List<Asiakas> asiakkaat;

            asiakkaat = ar.HaeKaikki();

            var asiakkaatJoillaEiTilauksia = asiakkaat.Where(x => x.Tilaukset.Count == 0);

            Console.WriteLine("Asiakkaan poistaminen");
            Console.WriteLine("Seuraavilla asiakkailla ei ole tilauksia: ");

            foreach (var asiakas in asiakkaatJoillaEiTilauksia)
            {
                Console.WriteLine($"{asiakas.AsiakasId} {asiakas.Nimi} {asiakas.Kaupunki}, {asiakas.Maa}");
            }

            string tunnus;
            bool tunnusOnTosi;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                tunnusOnTosi = true;
                Console.Write("Anna poistettava tunnus: ");
                tunnus = Console.ReadLine();

                if (!asiakkaatJoillaEiTilauksia.Any(x => x.AsiakasId == tunnus))
                {
                    TekstinVarinVaihto("Tunnusta ei löydy listalta.", ConsoleColor.Red);
                    tunnusOnTosi = false;
                }
                else
                {
                    tunnusOnTosi = true;
                }
            } while (!tunnusOnTosi);


            bool poistoOnTosi;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Haluatko varmasti poistaa asiakkaan {tunnus} (k/e)?");
                poistoOnTosi = true;

                var valinta = Console.ReadKey(true);

                switch (valinta.Key)
                {
                    case ConsoleKey.K:
                        poistoOnTosi = true;
                        try
                        {
                            ar.Poista(tunnus);
                            Console.WriteLine("Asiakas poistettu.\nPaina Enter.");
                            Console.ReadLine();
                            Alkuvalikko();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Asiakkaan poistaminen ei onnistunut.\nPaina Enter.");
                            Console.ReadLine();
                            Alkuvalikko();
                        }
                        break;
                    case ConsoleKey.E:
                        poistoOnTosi = true;
                        Console.WriteLine("Poistoa ei tehty\nPaina Enter.");
                        Console.ReadLine();
                        Alkuvalikko();
                        break;
                    default:
                        TekstinVarinVaihto("Valinta on virheellinen", ConsoleColor.Red);
                        poistoOnTosi = false;
                        break;
                }

            } while (!poistoOnTosi);
        }

        /*----------- MUUTOS ------------*/
        public static void Muuta()
        {
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Asiakkaan muuttaminen");
            Console.Write("Anna muutettavan asiakkaan tunnus: ");
            string annettuTunnus = Console.ReadLine();
            Asiakas haettuAsiakas;
            string nimi;
            string kaupunki;
            string maa;

            try
            {
                haettuAsiakas = ar.Hae(annettuTunnus);
                Console.Write($"Asiakkaan tiedot: {haettuAsiakas.AsiakasId} {haettuAsiakas.Nimi}, {haettuAsiakas.Kaupunki} {haettuAsiakas.Maa}\n");

                Console.Write("Anna uusi nimi tai tyhjä: ");
                nimi = Console.ReadLine();
                if (nimi != "")
                {
                    haettuAsiakas.Nimi = nimi;
                }

                Console.Write("Anna uusi kaupunki tai tyhjä: ");
                kaupunki = Console.ReadLine();
                if (kaupunki != "")
                {
                    haettuAsiakas.Kaupunki = kaupunki;
                }

                Console.Write("Anna uusi maa tai tyhjä: ");
                maa = Console.ReadLine();
                if (maa != "")
                {
                    haettuAsiakas.Maa = maa;
                }

                ar.Muuta(haettuAsiakas);

            }
            catch (Exception virhe)
            {
                if (virhe is ApplicationException)
                {
                    Console.WriteLine("Asiakasta ei löynyt.\nPaina Enter");
                    Console.ReadLine();
                    Alkuvalikko();
                }
                else
                {
                    Console.WriteLine("Muutos ei onnistunut.\nPaina Enter");
                    Console.ReadLine();
                    Alkuvalikko();
                }

            }

            Console.WriteLine("Asiakas muutettu.\nPaina Enter.");
            Console.ReadLine();
            Alkuvalikko();
        }

        /*----------- LISAYS ------------*/
        public static void Lisaa()
        {
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Asiakas uusiAsiakas = new Asiakas();
            Console.WriteLine("Uusi asiakas");
            string tunnus;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Anna tunnus: ");
                tunnus = Console.ReadLine();

                if (!tunnus.Length.Equals(5))
                {
                    TekstinVarinVaihto("Sallittu merkkikoko on 5", ConsoleColor.Red);
                }
                else
                {
                    uusiAsiakas.AsiakasId = tunnus;
                    break;
                }
            } while (true);
            Console.Write("Anna nimi: ");
            uusiAsiakas.Nimi = Console.ReadLine();
            Console.Write("Anna maa: ");
            uusiAsiakas.Maa = Console.ReadLine();
            Console.Write("Anna kaupunki: ");
            uusiAsiakas.Kaupunki = Console.ReadLine();

            try
            {
                ar.Lisaa(uusiAsiakas);
                Console.WriteLine("Asiakas lisätty\nPaina Enter");

            }
            catch (Exception e)
            {

                Console.WriteLine($"Lisäys ei onnistunut {e.Message}");
                Console.WriteLine("Asiakasta ei lisätty\nPaina Enter");
            }


            Console.ReadLine();
            Alkuvalikko();
        }

        /*----------- HAKU --------------*/
        public static void Hae()
        {
            Console.Clear();
            TekstinKeskitysVarilla("Northwind-asiakkaat", ConsoleColor.Yellow);
            Console.WriteLine();

            TekstinVarinVaihto("Asiakastietojen haku\nValitse hakukriteeri\n1. Nimi\n2. Kaupunki\n3. Maa\n4. Palaa takaisin\nValitse: ", ConsoleColor.Yellow);

            bool onTosi;
            do
            {
                onTosi = true;

                var valinta = Console.ReadKey(true);

                switch (valinta.Key)
                {
                    case ConsoleKey.D1:
                        onTosi = true;
                        HaeAsiakkaatNimella();
                        break;
                    case ConsoleKey.D2:
                        onTosi = true;
                        HaeAsiakkaatKaupungilla();
                        break;
                    case ConsoleKey.D3:
                        HaeAsiakkaatMaalla();
                        onTosi = true;
                        break;
                    case ConsoleKey.D4:
                        Alkuvalikko();
                        break;

                    default:

                        TekstinVarinVaihto("Valinta on virheellinen", ConsoleColor.Red);
                        Thread.Sleep(1000);
                        Console.Clear();
                        Hae();
                        onTosi = false;
                        break;

                }

            } while (!onTosi);
        }

        public static void HaeAsiakkaatNimella()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);
            TilausRiviRepository tr = new TilausRiviRepository(yhteysasetukset.ConnectionString);

            Console.Write("Anna haettavien alku: ");
            string syotto = Console.ReadLine();
            List<Asiakas> asiakkaat = ar.HaeNimi(syotto);

            if (asiakkaat.Count == 0)
            {
                Console.Write("Tällä haulla ei valitettavasti löytynyt yhtään asiakasta. Haluatko hakea uudelleen k/e?");
                var hakuUudestaan = Console.ReadKey(true);

                switch (hakuUudestaan.Key)
                {
                    case ConsoleKey.K:
                        Console.WriteLine();
                        HaeAsiakkaatNimella();
                        break;
                    case ConsoleKey.E:
                        Alkuvalikko();
                        break;
                    default:
                        Console.WriteLine("Valinta ei ole mahdollinen");
                        break;
                }
            }
            else
            {

                Console.WriteLine($"Löydetyt asiakkaat {asiakkaat.Count} kpl");
                int indexNumber = 1;

                foreach (var asiakas in asiakkaat)
                {
                    Console.WriteLine($"{indexNumber}. asiakas: {asiakas.Nimi}, {asiakas.Kaupunki} {asiakas.Maa}");

                    foreach (var tilaus in asiakas.Tilaukset)
                    {
                        Console.WriteLine($"  Tilaus: {tilaus.Id}, tuotteita: {tilaus.TilausRivit.Count}, arvo yhteensä {tilaus.TilausRivit.Sum(x => (x.Hinta * x.Maara)).ToString("f2")}");

                    }
                    while (indexNumber < asiakkaat.Count)
                    {
                        Console.Write("Seuraava painamalla Enter");
                        Console.ReadLine();
                        break;
                    }
                    indexNumber++;
                }
                Console.WriteLine("Paina Enter.");
                Console.ReadLine();
                Alkuvalikko();
            }
        }

        public static void HaeAsiakkaatKaupungilla()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);
            TilausRiviRepository tr = new TilausRiviRepository(yhteysasetukset.ConnectionString);

            Console.Write("Anna haettavien alku: ");
            string syotto = Console.ReadLine();
            List<Asiakas> asiakkaat = ar.HaeKaupunki(syotto);

            if (asiakkaat.Count == 0)
            {
                Console.Write("Tällä haulla ei valitettavasti löytynyt yhtään asiakasta. Haluatko hakea uudelleen k/e?");
                var hakuUudestaan = Console.ReadKey(true);

                switch (hakuUudestaan.Key)
                {
                    case ConsoleKey.K:
                        Console.WriteLine();
                        HaeAsiakkaatKaupungilla();
                        break;
                    case ConsoleKey.E:
                        Alkuvalikko();
                        break;
                    default:
                        Console.WriteLine("Valinta ei ole mahdollinen");
                        break;
                }
            }
            else
            {

                Console.WriteLine($"Löydetyt asiakkaat {asiakkaat.Count} kpl");
                int indexNumber = 1;

                foreach (var asiakas in asiakkaat)
                {
                    Console.WriteLine($"{indexNumber}. asiakas: {asiakas.Nimi}, {asiakas.Kaupunki} {asiakas.Maa}");

                    foreach (var tilaus in asiakas.Tilaukset)
                    {
                        Console.WriteLine($"  Tilaus: {tilaus.Id}, tuotteita: {tilaus.TilausRivit.Count}, arvo yhteensä {tilaus.TilausRivit.Sum(x => (x.Hinta * x.Maara)).ToString("f2")}");

                    }
                    while (indexNumber < asiakkaat.Count)
                    {
                        Console.Write("Seuraava painamalla Enter");
                        Console.ReadLine();
                        break;
                    }
                    indexNumber++;
                }
                Console.WriteLine("Paina Enter.");
                Console.ReadLine();
                Alkuvalikko();
            }
        }


        public static void HaeAsiakkaatMaalla()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsiakasRepository ar = new AsiakasRepository(yhteysasetukset.ConnectionString);
            TilausRiviRepository tr = new TilausRiviRepository(yhteysasetukset.ConnectionString);

            Console.Write("Anna haettavien alku: ");
            string syotto = Console.ReadLine();
            List<Asiakas> asiakkaat = ar.HaeMaa(syotto);

            if (asiakkaat.Count == 0)
            {
                Console.Write("Tällä haulla ei valitettavasti löytynyt yhtään asiakasta. Haluatko hakea uudelleen k/e?");
                var hakuUudestaan = Console.ReadKey(true);

                switch (hakuUudestaan.Key)
                {
                    case ConsoleKey.K:
                        Console.WriteLine();
                        HaeAsiakkaatMaalla();
                        break;
                    case ConsoleKey.E:
                        Alkuvalikko();
                        break;
                    default:
                        Console.WriteLine("Valinta ei ole mahdollinen");
                        break;
                }
            }
            else
            {

                Console.WriteLine($"Löydetyt asiakkaat {asiakkaat.Count} kpl");
                int indexNumber = 1;

                foreach (var asiakas in asiakkaat)
                {
                    Console.WriteLine($"{indexNumber}. asiakas: {asiakas.Nimi}, {asiakas.Kaupunki} {asiakas.Maa}");

                    foreach (var tilaus in asiakas.Tilaukset)
                    {
                        Console.WriteLine($"  Tilaus: {tilaus.Id}, tuotteita: {tilaus.TilausRivit.Count}, arvo yhteensä {tilaus.TilausRivit.Sum(x => (x.Hinta * x.Maara)).ToString("f2")}");

                    }
                    while (indexNumber < asiakkaat.Count)
                    {
                        Console.Write("Seuraava painamalla Enter");
                        Console.ReadLine();
                        break;

                    }
                    indexNumber++;

                }
                Console.WriteLine("Paina Enter.");
                Console.ReadLine();
                Alkuvalikko();

            }
        }
        #region
        /****** APUMETODIT ************/
        public static void TekstinVarinVaihto(string teksti, ConsoleColor vari)
        {
            Console.ForegroundColor = vari;
            Console.WriteLine(teksti);
            Console.ResetColor();
        }

        public static void TekstinKeskitysVarilla(string teksti, ConsoleColor vari)
        {
            Console.WriteLine();
            Console.Write(new string(' ', (Console.WindowWidth - teksti.Length) / 2));
            Console.ForegroundColor = vari;
            Console.WriteLine(teksti);
            Console.ResetColor();


        }
        #endregion
    }
}
