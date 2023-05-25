using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Base : IBase
    {
        private string connectionString = ConfigurationManager.AppSettings["MySql"];

        public List<IBase> Buscar()
        {
            var lista = new List<IBase>();
            using(MySqlConnection con = new MySqlConnection(connectionString))
            {
                List<string> where = new List<string>();
                string chavePrimaria = string.Empty;
                foreach(PropertyInfo pi in this.GetType().GetProperties
                    (BindingFlags.Instance | BindingFlags.Public)) {
                    OpcoesBase opcoes=(OpcoesBase)pi.GetCustomAttribute
                        (typeof(OpcoesBase));
                    if (opcoes != null)
                    {
                        if (opcoes.ChavePrimaria)
                        {
                            chavePrimaria = pi.Name + "=" + pi.GetValue(this);
                        }
                        else
                        {
                            if(tipoPropriedade(pi)=="varchar(255)" ||
                                tipoPropriedade(pi) =="datetime")
                            where.Add(pi.Name+"='"+pi.GetValue(this)+"'");
                            else
                                where.Add(pi.Name + "=" + pi.GetValue(this));
                        }
                    }
                }
                string sql;
                if (Key == 0)
                {
                    sql = "select * from " + this.GetType().Name + "s ";
                    if(where.Count > 0)
                    {
                        sql += " where  " + string.Join(" or ", where.ToArray());
                    }
                }
                else
                {
                    sql = "select * from " + this.GetType().Name + "s where " 
                        + chavePrimaria;
                }
                MySqlCommand mySql = new MySqlCommand(sql, con);
                mySql.Connection.Open();
                MySqlDataReader myDataReader = mySql.ExecuteReader();
                while(myDataReader.Read())
                {
                    var obj = (IBase)Activator.CreateInstance(this.GetType());
                    foreach (PropertyInfo info in obj.GetType().GetProperties(
                        BindingFlags.Public | BindingFlags.Instance))
                    {
                        OpcoesBase opcoes=(OpcoesBase)info.GetCustomAttribute
                            (typeof(OpcoesBase));
                        if(opcoes != null)
                        {
                            info.SetValue(obj, myDataReader[info.Name]);
                        }
                        lista.Add(obj);
                    }
                }
                mySql.Connection.Close();
            }
            return lista;
        } 
        // método que retorna o tipo da propriedade
        private string tipoPropriedade(PropertyInfo pi)
        {
            switch(pi.PropertyType.Name)
            {
                case "Int32":
                    return "int";
                case "Int64":
                    return "bigint";
                case "double":
                    return "decimal(9, 2)";
                case "DateTime":
                    return "datetime";
                default:
                    return "varchar(255)";
            }
        }

        public void CriarTabela()
        {
            throw new NotImplementedException();
        }

        public void Excluir()
        {
            using(MySqlConnection connection=new MySqlConnection(connectionString))
            {
                string sql = "delete from " + this.GetType().Name + "s where id=" 
                    + this.Key+";";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public int Key
        {
            get
            {
                foreach (PropertyInfo pi in this.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance))
                {
                    OpcoesBase opcoes = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));
                    if (opcoes != null && opcoes.ChavePrimaria)
                    {
                        return Convert.ToInt32(pi.GetValue(this));
                    }
                }
                return 0;
            }

        }

        public void Salvar()
        {
            throw new NotImplementedException();
        }

        public List<IBase> Todos()
        {
            var lista = new List<IBase>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
               
                string sql;
                if (Key == 0)
                {
                    sql = "select * from " + this.GetType().Name + "s ";
                    if (where.Count > 0)
                    {
                        sql += " where  " + string.Join(" or ", where.ToArray());
                    }
                }
                else
                {
                    sql = "select * from " + this.GetType().Name + "s where "
                        + chavePrimaria;
                }

                PublicKey void Salvar()
                {
                    using (MySqlConnection con = new MySqlConnection (connectionString))
                    {
                        List<string> campos = new List<string> ();
                        List<string> valores = new List<string>();
                        foreach (PropertyInfo pi in this.GetType().GetProperties
                            (BindingFlags.Public | BindingFlags.Instance)) { 
                            OpcoesBase opcoes =(OpcoesBase)pi.GetCustomAtribute
                                (typeof(OpcoesBase));
                            if(this.Key == 0)
                            {
                                campos.Add(pi.Name);
                                valores.Add("'" + pi.GetValue(this) + "'");
                            }
                            else
                            {
                                if (!opcoes.ChavePrimaria)
                                {
                                    valores.Add(pi.Name + "='" + pi.GetValue(this) + "'");
                                }
                            }
                    }
                        string sql = "";
                        if (this.Key == 0)
                        {
                            sql = "insert into " + this.GetType().Name + "s(";
                            sql += string.Join(",  ", campos.ToArray());
                            sql += ") values (" + string.Join(", ", valores.ToArray()) + ")";

                        }
                        else 
                        {
                            sql = "update " + this.GetType().Name + "s set ";
                            sql += string.Join(", ", valores.ToArray()) + "where Id=" + this.Key;
                        }
                        MySql.Command cmd = new MySqlSqlCommand(sql, con);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Clase();

                        MySqlCommand mySql = new MySqlCommand(sql, con);
                mySql.Connection.Open();
                MySqlDataReader myDataReader = mySql.ExecuteReader();
                while (myDataReader.Read())
                {
                    var obj = (IBase)Activator.CreateInstance(this.GetType());
                    foreach (PropertyInfo info in obj.GetType().GetProperties(
                        BindingFlags.Public | BindingFlags.Instance))
                    {
                        OpcoesBase opcoes = (OpcoesBase)info.GetCustomAttribute
                            (typeof(OpcoesBase));
                        if (opcoes != null)
                        {
                            info.SetValue(obj, myDataReader[info.Name]);
                        }
                        lista.Add(obj);
                    }
                }
                mySql.Connection.Close();
            }
            return lista;
        }
    }
}
