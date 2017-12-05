using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POLuokat;
using System.Data.SqlClient;
using System.Data;

namespace POData
{
    public class AsiakasRepository : DataAccess, IRepository<Asiakas>
    {
        public AsiakasRepository(string yhteys)
            : base(yhteys)
        { } 

        public bool Lisaa(Asiakas a)
        {
            string sql = "INSERT INTO dbo.[Customers](CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) " +
             "VALUES (@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", a.AsiakasId));
                cmd.Parameters.Add(new SqlParameter("@CompanyName", a.Nimi));
                cmd.Parameters.Add(new SqlParameter("@ContactName", a.YhteysHenkilo != null ? a.YhteysHenkilo : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", a.YhteysTitteli != null ? a.YhteysTitteli : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Address", a.Katuosoite != null ? a.Katuosoite : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", a.Kaupunki != null ? a.Kaupunki : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Region", a.Alue != null ? a.Alue : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", a.PostiKoodi != null ? a.PostiKoodi : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Country", a.Maa != null ? a.Maa : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Phone", a.Puhelin != null ? a.Puhelin : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Fax", a.Fax != null ? a.Fax : (object)DBNull.Value));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public bool Poista(string id)
        {
            string sql = "DELETE FROM dbo.[Customers] " +
                         "WHERE CustomerID = @CustomerID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public bool Muuta(Asiakas a)
        {
            string sql = "UPDATE dbo.[Customers] " +
                         "SET CompanyName = @CompanyName,  ContactName = @ContactName,  ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode= @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax " +
                         "WHERE CustomerID = @CustomerID";

            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CompanyName", a.Nimi));
                cmd.Parameters.Add(new SqlParameter("@ContactName", a.YhteysHenkilo != null ? a.YhteysHenkilo : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", a.YhteysTitteli != null ? a.YhteysTitteli : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Address", a.Katuosoite != null ? a.Katuosoite : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", a.Kaupunki != null ? a.Kaupunki : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Region", a.Alue != null ? a.Alue : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", a.PostiKoodi != null ? a.PostiKoodi : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Country", a.Maa != null ? a.Maa : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Phone", a.Puhelin != null ? a.Puhelin : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Fax", a.Fax != null ? a.Fax : (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CustomerID", a.AsiakasId));
                cn.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public List<Asiakas> HaeNimi(string nimi)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Customers] " +
                         "WHERE CompanyName LIKE @CompanyName";

            SqlConnection cn = null;
            SqlCommand cmd;

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CompanyName", nimi + "%"));
                cn.Open();

                return TeeAsiakasLista(cmd.ExecuteReader(CommandBehavior.Default));

            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public List<Asiakas> HaeKaupunki(string kaupunki)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Customers] " +
                         "WHERE City LIKE @City";

            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@City", kaupunki + "%"));
                cn.Open();
                return TeeAsiakasLista(cmd.ExecuteReader(CommandBehavior.Default));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }


        public List<Asiakas> HaeMaa(string maa)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Customers] " +
                         "WHERE Country LIKE @Country";

            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@Country", maa + "%"));
                cn.Open();
                return TeeAsiakasLista(cmd.ExecuteReader(CommandBehavior.Default));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public Asiakas Hae(string id)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Customers] " +
                         "WHERE CustomerID = @CustomerID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaAsiakas(reader);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                cn?.Close();
            }
        }

        public List<Asiakas> HaeKaikki()
        {
            string sql = "SELECT *" +
                         "FROM dbo.Customers " +
                         "ORDER BY CustomerID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeAsiakasLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Tietokantakäsittelyn virhe: " + e.Message);
            }
            finally
            {
                if (cn != null)
                {
                    cn.Close();
                }
            }
        }

        #region Helpers

        private List<Asiakas> TeeAsiakasLista(IDataReader reader)
        {
            List<Asiakas> asiakkaat = new List<Asiakas>();
            while (reader.Read())
            {
                asiakkaat.Add(TeeRivistaAsiakas(reader));
            }
            return asiakkaat;
        }
        private Asiakas TeeRivistaAsiakas(IDataReader reader)
        {
            Asiakas paluu = new AsiakasProxy(
                reader["CustomerID"].ToString(),
                reader["CompanyName"].ToString()
                )
            {
                YhteysHenkilo = reader["ContactName"].ToString(),
                YhteysTitteli = reader["ContactTitle"].ToString(),
                Katuosoite = reader["Address"].ToString(),
                Kaupunki = reader["City"].ToString(),
                Alue = reader["Region"].ToString(),
                Maa = reader["Country"].ToString(),
                PostiKoodi = reader["PostalCode"].ToString()
            };


            if (!(reader["Phone"] is DBNull))
            {
                paluu.Puhelin = reader["Phone"].ToString();
            }
            else
            {
                paluu.Puhelin = null;
            }

            if (!(reader["Fax"] is DBNull))
            {
                paluu.Fax = reader["Fax"].ToString();
            }
            else
            {
                paluu.Fax = null;
            }

            //TilausOtsikko-olioiden myöhempää populointia varten
            ((AsiakasProxy)paluu).TilausOtsikkoRepository = new TilausOtsikkoRepository(ConnectionString);

            return paluu;
        }
        #endregion 
    }
}
