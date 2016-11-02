using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

using Android.Util;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace IAB330RentalM3.Database
{
    /// <summary>
    /// Required SQLite stuff
    /// </summary>
    class SQLiteObj
    {
        public string sqliteFilename = "database.db3";
        public string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder

        [assembly: Dependency(typeof(SQLite_Android))]
        // ...
        public class SQLite_Android : ISQLite
        {
            public string sqliteFilename = "database.db3";
            public string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder

            public SQLite_Android() { }

            public SQLite.SQLiteConnection GetConnection()
            {
                var path = Path.Combine(documentsPath, sqliteFilename);

                Log.Info("DATABASE PATH: ", path);
                // Create the connection
                var conn = new global::SQLite.SQLiteConnection(path);
                // Return the database connection
                return conn;
            }
        }

        public interface ISQLite
        {
            SQLiteConnection GetConnection();
        }

        /// <summary>
        /// creates database at given path
        /// </summary>
        public string createDatabase(string path)
        {
            try
            {
                var connection = new SQLiteConnection(path);
                {
                    connection.CreateTable<house_table>();
                    connection.CreateTable<user_table>();
                    connection.CreateTable<event_table>();
 
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// inserts/updates house table
        /// </summary>
        public string houseInsertUpdateData(house_table data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Insert(data);
                /*
                if (db.Insert(data) < -1)
                {
                    db.Update(data);
                }
                */
                return "Single data file inserted or updated";

            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// inserts/updates user table
        /// </summary>
        public string userInsertUpdateData(user_table data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Insert(data);
                /*
                if (db.Insert(data) < -1)
                {
                    db.Update(data);
                }*/
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// inserts/updates event table
        /// </summary>
        public string eventInsertUpdateData(event_table data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Insert(data);
                    /*
                if (db.Insert(data) < -1)
                {
                    db.Update(data);
                }
                */
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// gets a users full name give their ID
        ///
        public string getNameByID(string id)
        {
            string name;
            var path = Path.Combine(documentsPath, sqliteFilename);
            try
            {
                var db = new SQLiteConnection(path);
                List<user_table> result = db.Query<user_table>("SELECT * FROM user_table WHERE ID = ?", id);
                name = result[0].firstname + " " + result[0].lastname;
                return name;
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}

