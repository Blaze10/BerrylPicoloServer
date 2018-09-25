using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
//using System.Web.UI.WebControls;

namespace Utility.DataAccessUtility
{
    /// <summary>
    /// Summary description for DBConnection
    /// </summary>
    public class DBConnection
    {

        public static string Connectionstring
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DemoProjectEntities"].ToString();
            }
        }


        public static SqlConnection getConnection()
        {
            return new SqlConnection(Connectionstring);
        }
        public static DataTable getDataTable(string sql)
        {

            SqlDataAdapter dap = new SqlDataAdapter(sql, Connectionstring);
            DataTable dt = new DataTable();
            dap.Fill(dt);
            return dt;
        }
        public static long ExecuteQuery_GetID(String sql, String tableName)
        {
            return ExecuteQuery_GetID(sql, tableName);
        }
        public static bool ExecuteSQL(String SQL)
        {
            bool success = true;
            try
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = SQL;
                com.CommandType = CommandType.Text;
                com.Connection = getConnection();
                com.Connection.Open();
                com.ExecuteNonQuery();
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
        }

        //public static DataTable getDataTableSp(string sp, List<SqlParameter> param)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = sp;
        //        cmd.Connection = getConnection(connectionString);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        addParamstoCommand(cmd, param);
        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);


        //        DataTable dt = new DataTable();
        //        dap.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        //Error_Msg.insertErrorMsg(msg, "Database,getDataTable()");
        //        return null;
        //    }
        //    return null;
        //}
        public static void FillDropdownList(string procName, SqlParameter[] Parameters, DropDownList ddl)
        {

            DataSet ds = ExecuteProcedure(procName, Parameters);
            DataTable Filldt = ds.Tables[0];
            if (Filldt.Rows.Count > 0)
            {
                ddl.DataSource = Filldt;
                ddl.DataValueField = "id";
                ddl.DataTextField = "Value";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--Select--", "0"));
                ddl.SelectedIndex = 0;
            }
        }

        public static void Fillradiolist(string procName, SqlParameter[] Parameters, RadioButtonList rdb)
        {
            DataSet ds = ExecuteProcedure(procName, Parameters);
            DataTable Filldt = ds.Tables[0];
            if (Filldt.Rows.Count > 0)
            {
                rdb.DataSource = Filldt;
                rdb.DataValueField = "id";
                rdb.DataTextField = "Value";
                rdb.DataBind();
            }
        }

        public static DataSet ExecuteProcedure(string procName, SqlParameter[] Parameters)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con = getConnection();
            cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Parameters != null)
            {

                foreach (SqlParameter para in Parameters)
                {

                    cmd.Parameters.AddWithValue(para.ParameterName, para.Value);

                }

            }
            cmd.Connection.Open();
            da.SelectCommand = cmd;
            da.Fill(ds);
            if (con.State == ConnectionState.Open)
            {

                con.Close();

            }
            return ds;
        }

        public static DataSet ExecuteDataSetSPDB(string ProcedureName, SqlParameter[] Parameters)
        {
            DataSet ds = new DataSet();

            if (ProcedureName != "")
            {

                SqlConnection m_Scon = getConnection();

                SqlCommand m_Scmd = new SqlCommand(ProcedureName, m_Scon);
                SqlDataAdapter da = new SqlDataAdapter();
                m_Scmd.CommandType = CommandType.StoredProcedure;



                if (Parameters != null)
                {

                    foreach (SqlParameter para in Parameters)
                    {

                        m_Scmd.Parameters.AddWithValue(para.ParameterName, para.Value);

                    }

                }

                m_Scmd.Connection.Open();
                da.SelectCommand = m_Scmd;
                da.Fill(ds);

                if (m_Scon.State == ConnectionState.Open)
                {

                    m_Scon.Close();

                }

                return ds;

            }

            else
            {

                return ds = null;

            }

        }


        public static int ExecuteNonQuery(string procedureName, SqlParameter[] parameters)
        {

            SqlConnection oConnection = getConnection();

            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);



            oCommand.CommandType = CommandType.StoredProcedure;

            int iReturnValue;

            oConnection.Open();

            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {

                try
                {

                    if (parameters != null)

                        oCommand.Parameters.AddRange(parameters);



                    oCommand.Transaction = oTransaction;

                    iReturnValue = oCommand.ExecuteNonQuery();

                    oTransaction.Commit();

                }

                catch
                {

                    oTransaction.Rollback();

                    throw;

                }

                finally
                {

                    if (oConnection.State == ConnectionState.Open)
                        oCommand.Parameters.Clear();
                    oConnection.Close();

                    oConnection.Dispose();

                    oCommand.Dispose();


                }

            }

            return iReturnValue;

        }
        public static void Show(string message)
        {
            string cleanMessage = message.Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }
    }
}