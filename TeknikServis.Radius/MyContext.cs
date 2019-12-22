using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Entity.Core.EntityClient;


namespace TeknikServis.Radius
{
    public static class MyContext
    {
        public static radiusEntities Context(string firma)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection sec = (ConnectionStringsSection)config.GetSection("connectionStrings");
           
            if (sec.ConnectionStrings[firma]!=null)
            {
                string s = sec.ConnectionStrings[firma].ConnectionString;
                EntityConnection connection = new EntityConnection(s);
                return new radiusEntities(connection);
            }
            else
            {
                string bilgitap = sec.ConnectionStrings["BILGITAP"].ConnectionString;
                EntityConnection connection2 = new EntityConnection(bilgitap);
                return new radiusEntities(connection2);
            }
        

        }
        public static void conStringKaydet(string firma, uint port, string server, string userID,
      string password, bool persis, string database, bool zeroDate, bool allowZeroDate,string provider)
        {
            MySqlConnectionStringBuilder myCSB = new MySqlConnectionStringBuilder();

            myCSB.Port = port;
            myCSB.Server = server;
            myCSB.UserID = userID;
            myCSB.Password = password;
            myCSB.PersistSecurityInfo = persis;
            myCSB.Database = database;
            myCSB.ConvertZeroDateTime = zeroDate;
            myCSB.AllowZeroDateTime = allowZeroDate;
            myCSB.CharacterSet = "utf8";
           

            //Initialize the EntityConnectionStringBuilder
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = provider;// "MySql.Data.MySqlClient";
            
            entityBuilder.ProviderConnectionString = myCSB.ToString();

            entityBuilder.Metadata = @"res://*/radiusModel.csdl|res://*/radiusModel.ssdl|res://*/radiusModel.msl";

            string sonString = entityBuilder.ConnectionString;

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection sec = (ConnectionStringsSection)config.GetSection("connectionStrings");

            ConnectionStringSettings csAddNewConString = new ConnectionStringSettings(firma, sonString);
            sec.ConnectionStrings.Add(csAddNewConString);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");

        }
        private static radiusEntities Context2(string firma)
        {
            if (firma == "TOL")
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                ConnectionStringsSection sec = (ConnectionStringsSection)config.GetSection("connectionStrings");
                string s = sec.ConnectionStrings[firma].ConnectionString;


                EntityConnection connection = new EntityConnection(s);

                return new radiusEntities(connection);
            }
            else if (firma == "DATASURF")
            {
              

                MySqlConnectionStringBuilder myCSB = new MySqlConnectionStringBuilder();

                myCSB.Port = 3306;
                myCSB.Server = "localhost";
                myCSB.UserID = "root";
                myCSB.Password = "t0m122";
                myCSB.PersistSecurityInfo = true;
                myCSB.Database = "radius";
                myCSB.ConvertZeroDateTime = true;
                myCSB.AllowZeroDateTime = true;

                //Initialize the EntityConnectionStringBuilder
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.Provider = "MySql.Data.MySqlClient";
                entityBuilder.ProviderConnectionString = myCSB.ToString();

                ////Set the Metadata location.//res://*/DataAccess.EncounterModel.EncounterModel.csdl|res://*/DataAccess.EncounterModel.EncounterModel.ssdl|res://*/DataAccess.EncounterModel.EncounterModel.msl";
                entityBuilder.Metadata = @"res://*/radiusModel.csdl|res://*/radiusModel.ssdl|res://*/radiusModel.msl";

                ////Create entity connection
                EntityConnection connection = new EntityConnection(entityBuilder.ConnectionString);

                return new radiusEntities(connection);
            }
            else
            {
                //burada bizim veritabanını döndürebiliriz.
                return null;
            }

        }
    }
}
