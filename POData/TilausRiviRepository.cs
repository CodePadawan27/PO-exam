using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POData
{
    //ORDER DETAILS
    public class TilausRiviRepository : DataAccess
    {
        public TilausRiviRepository(string yhteys)
            : base(yhteys)
        {
        }

        public List<TilausRivi> HaeTuotteenKaikki(int id)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Order Details] " +
                         "WHERE ProductID = @ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@ProductID", id));
                cn.Open();
                return TeeTilausRiviLista(cmd.ExecuteReader(CommandBehavior.Default));
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

        public List<TilausRivi> HaeTilauksenKaikkiTuotteet(int id)
        {
            string sql = "SELECT * " +
                         "FROM dbo.[Order Details] " +
                         "WHERE OrderID = @OrderID " +
                         "ORDER BY ProductID";
            SqlConnection cn = null;
            SqlCommand cmd;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", id));
                cn.Open();
                return TeeTilausRiviLista(cmd.ExecuteReader(CommandBehavior.Default));
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

        private List<TilausRivi> TeeTilausRiviLista(IDataReader reader)
        {
            List<TilausRivi> tilausRivit = new List<TilausRivi>();
            while (reader.Read())
            {
                tilausRivit.Add(TeeRivistaTilausRivi(reader));
            }
            return tilausRivit;
        }

        private TilausRivi TeeRivistaTilausRivi(IDataReader reader)
        {
            TilausRivi paluu = new TilausRiviProxy(
                int.Parse(reader["OrderID"].ToString())
                );

                paluu.TuoteId = int.Parse(reader["ProductID"].ToString());
                paluu.Hinta = decimal.Parse(reader["UnitPrice"].ToString().Replace('.', ','));
                paluu.Maara = int.Parse(reader["Quantity"].ToString());
                paluu.Alennus = decimal.Parse(reader["Discount"].ToString());

            //Tuote ja TilausOtsikko-olioiden myöhempää populointia varten
            ((TilausRiviProxy)paluu).TuoteRepository = new TuoteRepository(ConnectionString);
            ((TilausRiviProxy)paluu).TilausOtsikkoRepository = new TilausOtsikkoRepository(ConnectionString);

            return paluu;
        }


    }
}
