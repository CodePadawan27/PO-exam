using System;
using System.Collections.Generic;
using POLuokat;
using System.Data.SqlClient;
using System.Data;

namespace POData
{
    public class TuoteRepository : DataAccess//, IRepository<Tuote>
    {
        public TuoteRepository(string yhteys)
            : base(yhteys)
        {
        }

        public Tuote Hae(int id)
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "WHERE ProductID = @ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaTuote(reader);
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

        public List<Tuote> HaeKaikki()
        {
            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued " +
                         "FROM dbo.Products " +
                         "ORDER BY ProductID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeTuoteLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
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

        #region Helpers
        private List<Tuote> TeeTuoteLista(IDataReader reader)
        {
            List<Tuote> tuotteet = new List<Tuote>();
            while (reader.Read())
            {
                tuotteet.Add(TeeRivistaTuote(reader));
            }
            return tuotteet;
        }

        private Tuote TeeRivistaTuote(IDataReader reader)
        {
            Tuote paluu = new TuoteProxy(
                int.Parse(reader["ProductID"].ToString()),
                reader["ProductName"].ToString()
                );

            if (!(reader["SupplierID"] is DBNull))
            {
                paluu.ToimittajaId = int.Parse(reader["SupplierID"].ToString());
            }
            else
            {
                paluu.ToimittajaId = null;
            }

            if (!(reader["CategoryID"] is DBNull))
            {
                paluu.RyhmaId = int.Parse(reader["CategoryID"].ToString());
            }
            else
            {
                paluu.RyhmaId = null;
            }

            if (!(reader["UnitsInStock"] is DBNull))
            {
                paluu.VarastoSaldo = int.Parse(reader["UnitsInStock"].ToString());
            }
            else
            {
                paluu.VarastoSaldo = null;
            }

            if (!(reader["ReorderLevel"] is DBNull))
            {
                paluu.HalytysRaja = int.Parse(reader["ReorderLevel"].ToString());
            }
            else
            {
                paluu.HalytysRaja = null;
            }

            if (!(reader["UnitPrice"] is DBNull))
            {
                paluu.YksikkoHinta = double.Parse(reader["UnitPrice"].ToString().Replace('.', ','));
            }
            else
            {
                paluu.YksikkoHinta = null;
            }

            paluu.YksikkoKuvaus = reader["QuantityPerUnit"].ToString();
            paluu.EiKaytossa = bool.Parse(reader["Discontinued"].ToString());

            //TilausRivi-olioiden myöhempää populointia varten
            ((TuoteProxy)paluu).TilausRiviRepository = new TilausRiviRepository(ConnectionString);

            return paluu;
        }
        #endregion
    }
}

