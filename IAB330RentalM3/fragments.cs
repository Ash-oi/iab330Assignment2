using Android.OS;
using Android.Views;
using Android.App;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using IAB330RentalM3.Database;
using SQLite;
using SQLite.Net.Interop;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using static IAB330RentalM3.MainActivity;

namespace IAB330RentalM3
{
    public class noticesFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
       
            View view = inflater.Inflate(Resource.Layout.notices, null);

            ///set up the recycler view
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.noticesRV);

            //create & assign layout manager to the recycler view
            LinearLayoutManager NlinearLayoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(NlinearLayoutManager);

            //get database connection

            string sqliteFilename = "database.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            string path = Path.Combine(documentsPath, sqliteFilename);


            ISQLitePlatform platform = new SQLitePlatformAndroid();
            SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(platform, path);

            //pull all notices
            var contextPref = Application.Context.GetSharedPreferences("preferences", Android.Content.FileCreationMode.Private);
            string id = contextPref.GetString("userid","");

            Log.Info("THE DETECTED USERID", id);

            
            //note: SQLite relaly didnt like this being in one query
            //get the users houseID
            List<user_table> user = conn.Query<user_table>("SELECT * FROM user_table WHERE Id = ?;", id);
            var usersHouse = user[0].houseID.ToString();

            Log.Info("NUMBER OF USERS RETURNED: ", user.Count.ToString());

            //get all that house's users
            List<user_table> users = conn.Query<user_table>("SELECT * FROM user_table WHERE houseID = ?;", usersHouse);

            Log.Info("NUMBER OF House's user's RETURNED: ", users.Count.ToString());

            //get all the notices from those users
            List<event_table> notices = new List<event_table>();

            //for each user in the house
            foreach(user_table person in users)
            {
                //get all their events
                List<event_table> temp = conn.Query<event_table>("SELECT * FROM event_table WHERE type = 'notice' AND postedby = ?", person.Id.ToString());
                //add every event to notice list
                foreach (event_table events in temp)
                {
                    notices.Add(events);
                }
                
            }
    
            Log.Info("NUMBER OF NOTICES RETURNED: ", notices.Count.ToString());
            

            //specify an adapter
            var adapter = new NoticeAdapter(notices);
            recyclerView.SetAdapter(adapter);
            
            return view;
        }
    }

    public class tasksFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.tasks, null);

            ///set up the recycler view
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.tasksRV);

            //create & assign layout manager to the recycler view
            LinearLayoutManager TlinearLayoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(TlinearLayoutManager);

            //get database connection
            string sqliteFilename = "database.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            string path = Path.Combine(documentsPath, sqliteFilename);


            ISQLitePlatform platform = new SQLitePlatformAndroid();
            SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(platform, path);

            //pull all tasks
            var contextPref = Application.Context.GetSharedPreferences("preferences", Android.Content.FileCreationMode.Private);
            string id = contextPref.GetString("userid", "");

            Log.Info("THE DETECTED USERID", id);

            
            //note: SQLite relaly didnt like this being in one query
            //get the users houseID
            List<user_table> user = conn.Query<user_table>("SELECT * FROM user_table WHERE Id = ?;", id);
            var usersHouse = user[0].houseID.ToString();

            Log.Info("NUMBER OF USERS RETURNED: ", user.Count.ToString());

            //get all that house's users
            List<user_table> users = conn.Query<user_table>("SELECT * FROM user_table WHERE houseID = ?;", usersHouse);

            Log.Info("NUMBER OF House's user's RETURNED: ", users.Count.ToString());

            //get all the notices from those users
            List<event_table> notices = new List<event_table>();

            //for each user in the house
            foreach(user_table person in users)
            {
                //get all their events
                List<event_table> temp = conn.Query<event_table>("SELECT * FROM event_table WHERE type = 'task' AND postedby = ?", person.Id.ToString());
                //add every event to notice list
                foreach(event_table events in temp)
                {
                    notices.Add(events);
                }

            }

            Log.Info("NUMBER OF NOTICES RETURNED: ", notices.Count.ToString());
            

            //specify an adapter
            var adapter = new NoticeAdapter(notices);
            recyclerView.SetAdapter(adapter);
            
            return view;
        }
    }
}
