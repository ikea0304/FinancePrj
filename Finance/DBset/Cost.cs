using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Finance.DBset
{
    public class Cost
    {

            string ConnStr = ConfigurationManager.ConnectionStrings["CostEntities"].ConnectionString;
            public int ID { get; set; }
            public DateTime? date { get; set; }
            public string classification { get; set; }
            public string name { get; set; }
            public string cost { get; set; }
            public string total { get; set; }

            public List<Cost> GetCosts()
            {
                string ConnStr = ConfigurationManager.ConnectionStrings["CostEntities"].ConnectionString;
                List<Cost> costs = new List<Cost>();
                SqlConnection sqlConnection = new SqlConnection(ConnStr);
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Cost");
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cost cost = new Cost
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            date = reader.GetDateTime(reader.GetOrdinal("date")),
                            classification = reader.GetString(reader.GetOrdinal("classification")),
                            name = reader.GetString(reader.GetOrdinal("name")),
                            cost = reader.GetString(reader.GetOrdinal("cost")),
                            //total = reader.GetString(reader.GetOrdinal("total")),
                        };
                        costs.Add(cost);
                    }
                }
                else
                {
                    Console.WriteLine("資料庫為空！");
                }
                sqlConnection.Close();
                return costs;
            }
            public runResult ACost(string name, string classification, string cost)
            {
                runResult r = new runResult();

                using (SqlConnection connect = new SqlConnection(ConnStr))
                {
                    string query = "Insert Into Cost (date, classification, name, cost) Values(@DATE, @CLASSIFICATION, @NAME, @COST)";
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connect);
                        command.Parameters.AddWithValue("DATE", DateTime.Now);
                        command.Parameters.AddWithValue("CLASSIFICATION", classification);
                        command.Parameters.AddWithValue("NAME", name);
                        command.Parameters.AddWithValue("COST", cost);
                        connect.Open();
                        r.Count = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        r.ErrorMsg = e.Message;
                    }
                }
                return r;
            }
            public runResult UpdateACost(string name, string classification, string cost, int ID)
            {
                runResult r = new runResult();

                using (SqlConnection connect = new SqlConnection(ConnStr))
                {
                    using (SqlCommand cmd = connect.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "UPDATE Cost SET classification = @CLASSIFICATION, name = @NAME, cost = @COST WHERE ID = " + ID;

                            cmd.Parameters.AddWithValue("@CLASSIFICATION", classification);
                            cmd.Parameters.AddWithValue("@NAME", name);
                            cmd.Parameters.AddWithValue("@COST", cost);
                            connect.Open();
                            r.Count = cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            r.ErrorMsg = e.Message;
                        }
                    }
                }
                return r;
            }
            public runResult DeleteACost(int ID)
            {
                runResult r = new runResult();
                using (SqlConnection connect = new SqlConnection(ConnStr))
                {
                    using (SqlCommand cmd = connect.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "DELETE FROM Cost WHERE ID='" + ID + "'";
                            connect.Open();
                            r.Count = cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            r.ErrorMsg = e.Message;
                        }
                    }
                }
                return r;
            }
        
    }
}