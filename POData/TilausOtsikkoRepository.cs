using POLuokat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace POData
{
    //ORDERS
    public class TilausOtsikkoRepository : DataAccess//, IRepository<Tilaus>
    {
        public TilausOtsikkoRepository(string yhteys)
            : base(yhteys)
        { }

        public TilausOtsikko Hae(int id)
        {
            string sql = "SELECT OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress,ShipCity, ShipRegion, ShipPostalCode, ShipCountry" +
                         "FROM dbo.Orders " +
                         "WHERE OrderID = @OrderID";
            SqlConnection cn = null;
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", id));
                cn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                reader.Read();
                return TeeRivistaTilausOtsikko(reader);
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

        public List<TilausOtsikko> HaeAsiakkaanKaikki(string id)
        {
            string sql = "SELECT * " +
                         "FROM dbo.Orders " +
                         "WHERE CustomerID = @CustomerID " +
                         "ORDER BY CustomerID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                cn.Open();
                return TeeTilausOtsikkoLista(cmd.ExecuteReader(CommandBehavior.Default));
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

        public List<TilausOtsikko> HaeKaikki()
        {
            string sql = "SELECT *" +
                         "FROM dbo.[Orders] " +
                         "ORDER BY CustomerID";
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cn.Open();
                return TeeTilausOtsikkoLista(new SqlCommand(sql, cn).ExecuteReader(CommandBehavior.Default));
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

        private TilausOtsikko TeeRivistaTilausOtsikko(IDataReader reader)
        {
            TilausOtsikko paluu = new TilausOtsikkoProxy(
                int.Parse(reader["OrderID"].ToString()),
                reader["CustomerID"].ToString()
                );

            if (!(reader["EmployeeID"] is DBNull))
            {
                paluu.AlainenId = int.Parse(reader["EmployeeID"].ToString());
            }
            else
            {
                paluu.AlainenId = null;
            }

            if (!(reader["OrderDate"] is DBNull))
            {
                paluu.Tilauspvm = DateTime.Parse(reader["OrderDate"].ToString());
            }
            else
            {
                paluu.Tilauspvm = null;
            }

            if (!(reader["RequiredDate"] is DBNull))
            {
                paluu.Vaadittupvm = DateTime.Parse(reader["OrderDate"].ToString());
            }
            else
            {
                paluu.Vaadittupvm = null;
            }

            if(!(reader["ShippedDate"] is DBNull))
            {
                paluu.Toimitettupvm = DateTime.Parse(reader["ShippedDate"].ToString());
            }
            else
            {
                paluu.Toimitettupvm= null;
            }

            if (!(reader["ShipVia"] is DBNull))
            {
                paluu.Toimitustapa = int.Parse(reader["ShipVia"].ToString());
            }
            else
            {
                paluu.Toimitustapa = null;
            }

            if (!(reader["ShipVia"] is DBNull))
            {
                paluu.Toimitustapa = int.Parse(reader["ShipVia"].ToString());
            }
            else
            {
                paluu.Toimitustapa = null;
            }

            if (!(reader["Freight"] is DBNull))
            {
                paluu.Rahti = decimal.Parse(reader["Freight"].ToString());
            }
            else
            {
                paluu.Rahti = null;
            }

            if (!(reader["ShipName"] is DBNull))
            {
                paluu.ToimitusNimi = reader["ShipName"].ToString();
            }
            else
            {
                paluu.ToimitusNimi = null;
            }

            if (!(reader["ShipAddress"] is DBNull))
            {
                paluu.ToimitusOsoite = reader["ShipName"].ToString();
            }
            else
            {
                paluu.ToimitusOsoite = null;
            }

            if (!(reader["ShipCity"] is DBNull))
            {
                paluu.ToimitusKaupunki = reader["ShipCity"].ToString();
            }
            else
            {
                paluu.ToimitusKaupunki = null;
            }

            if (!(reader["ShipRegion"] is DBNull))
            {
                paluu.ToimitusAlue = reader["ShipRegion"].ToString();
            }
            else
            {
                paluu.ToimitusAlue = null;
            }

            if (!(reader["ShipPostalCode"] is DBNull))
            {
                paluu.ToimitusPostikoodi = reader["ShipPostalCode"].ToString();
            }
            else
            {
                paluu.ToimitusPostikoodi = null;
            }

            if (!(reader["ShipCountry"] is DBNull))
            {
                paluu.ToimitusKaupunki = reader["ShipCountry"].ToString();
            }
            else
            {
                paluu.ToimitusKaupunki = null;
            }

            //TilausRivi-olioiden myöhempää populointia varten
            ((TilausOtsikkoProxy)paluu).TilausRiviRepository = new TilausRiviRepository(ConnectionString);

            return paluu;
        }

        private List<TilausOtsikko> TeeTilausOtsikkoLista(IDataReader reader)
        {
            List<TilausOtsikko> tilaukset = new List<TilausOtsikko>();
            while (reader.Read())
            {
                tilaukset.Add(TeeRivistaTilausOtsikko(reader));
            }
            return tilaukset;
        }
    }
}
   