using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Android.Support.V7.Widget;
using Android.Views;
using IAB330RentalM3.Database;
using Android.Util;
using Android.Support.V7.App;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Runtime;
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SQLiteNetExtensions.Extensions;

using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;



using Android.Content;


namespace IAB330RentalM3
{
    /// <summary>
    /// main page that containing two fragments
    /// </summary>
    [Activity(Label = "Rental-M", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TabLayout tabLayout;
        FloatingActionButton fabN;
        FloatingActionButton fabT;
        SQLiteObj db = new SQLiteObj();
        public string email = "ash22334@hotmail.com";



        protected override void OnCreate(Bundle bundle)
        {
            //debugging
            string tag = "DATABASE CREATION STATUS: ";

            //create the database
            string sqliteFilename = "database.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            Log.Info("REQUESTED PATH: ", path);

            string success = db.createDatabase(path);
            Log.Info(tag, success);

            //put dummy data into DB --------------------->>>>>
            // none of this has relationships

            ISQLitePlatform platform = new SQLitePlatformAndroid();
            SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(platform, path);

            List<event_table> dummyListE = RandomTestingMethods.initEvents();
            List<user_table> dummyListU = RandomTestingMethods.initUsers();
            List<house_table> dummyListH = RandomTestingMethods.initHouse();

            foreach (house_table dumb in dummyListH)
            {
                db.houseInsertUpdateData(dumb, path);
            }

            foreach (user_table dumb in dummyListU)
            {

                db.userInsertUpdateData(dumb, path);
            }

            foreach (event_table dumb in dummyListE)
            {
                db.eventInsertUpdateData(dumb, path);
                //WriteOperations.UpdateWithChildren(conn, dumb);
            }

            //--------------------->>>>>

            //get currently logged in user's id
            List<user_table> users = conn.Query<user_table>("SELECT * FROM user_table");
            List<event_table> events = conn.Query<event_table>("SELECT * FROM event_table");
            List<house_table> houses = conn.Query<house_table>("SELECT * FROM house_table");

            Log.Info("LENGTH OF users", users.Count.ToString());
            Log.Info("LENGTH OF events", events.Count.ToString());
            Log.Info("LENGTH OF houses", houses.Count.ToString());

            var userid = users[0].Id.ToString();

            //save session info
            var contextPref = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);
            var contextEdit = contextPref.Edit();
            contextEdit.PutString("userid", userid).Commit();

            //doublecheck set the theme
            SetTheme(Resource.Style.DesignTheme);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            //load the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            //get rid of back button and set title
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.Title = "Rental-M";

            //load the tabbed layout
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            FnInitTabLayout();

            //load the FAB
            fabN = FindViewById<FloatingActionButton>(Resource.Id.fabN);
            fabT = FindViewById<FloatingActionButton>(Resource.Id.fabT);

            
            fabN.Click += delegate
            {
                var intent = new Intent(this, typeof(IAB330RentalM3.AddItemActivity));
                StartActivity(intent);
            };            //fabT.Click += (sender, args) => ;
            
        }

        public void animatefab(int pos)
        {
            switch (pos)
            {
                case 0:
                    fabN.Show();
                    fabT.Hide();
                    break;
                case 1:
                    fabT.Show();
                    fabN.Hide();
                    break;
                default:
                    fabN.Show();
                    fabT.Hide();
                    break;
            }
        }

        public void FnInitTabLayout()
        {
            tabLayout.SetTabTextColors(Android.Graphics.Color.Aqua, Android.Graphics.Color.AntiqueWhite);
            //Fragment array
            var fragments = new Android.Support.V4.App.Fragment[]
            {
                new noticesFragment(),
                new tasksFragment(),
            };
            //Tab title array
            var titles = CharSequence.ArrayFromStringArray(new[] { "Notices", "Tasks", });

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
            tabLayout.SetupWithViewPager(viewPager);
        }

            
        /// <summary>
        /// EVENT LISTENERS
        /// </summary>
        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }


        /// <summary>
        /// creates an Adapter for the notice recyclerview
        /// </summary>
        public class NoticeAdapter : RecyclerView.Adapter
        {

            List<event_table> items;
            SQLiteObj db = new SQLiteObj();

            public NoticeAdapter(List<event_table> data)
            {
                items = data;
            }

            public override int ItemCount
            {
                get
                {
                    return items.Count;
                }
            }
            // Create new views (invoked by the layout manager)
            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {

                // inflate from axml
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.noticeCard, parent, false);
                return new noticeViewHolder(itemView);
            }

            // binds data to the view (invoked by the layout manager)
            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var item = items[position];

                noticeViewHolder h = holder as noticeViewHolder;

                var nameString = db.getNameByID((items[position]).postedby.ToString());

                h.cNoticeContent.Text = (items[position]).content;
                h.cPostedBy.Text = nameString;
                h.cDateTimePosted.Text = (items[position]).dateposted.ToShortTimeString() + " " + (items[position]).dateposted.ToShortDateString();
            }
        }



        /// <summary>
        /// creates an Adapter for the task recyclerview
        /// </summary>
        public class TaskAdapter : RecyclerView.Adapter
        {

            List<event_table> items;
            SQLiteObj db = new SQLiteObj();


            public TaskAdapter(List<event_table> data)
            {
                items = data;
            }

            public override int ItemCount
            {
                get
                {
                    return items.Count;
                }
            }
            // Create new views (invoked by the layout manager)
            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {

                // inflate from axml
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.taskCard, parent, false);

                return new taskViewHolder(itemView);
            }

            // binds data to the view (invoked by the layout manager)
            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var item = items[position];

                taskViewHolder h = holder as taskViewHolder;
                var nameString = db.getNameByID((items[position]).postedby.ToString());

                h.cTaskContent.Text = (items[position]).content;
                h.cPostedBy.Text = nameString;
                h.cDateTimePosted.Text = (items[position]).dateposted.ToShortTimeString() + " " + (items[position]).dateposted.ToShortDateString();
            }
        }

        /// <summary>
        /// holds references to controls on the notice page
        /// </summary>
        public class noticeViewHolder : RecyclerView.ViewHolder
        {
            public TextView cNoticeContent { get; set; }
            public TextView cPostedBy { get; set; }
            public TextView cDateTimePosted { get; set; }

            public noticeViewHolder(View c) : base(c)
            {
                cNoticeContent = (TextView)c.FindViewById(Resource.Id.content);
                cPostedBy = (TextView)c.FindViewById(Resource.Id.postedby);
                cDateTimePosted = (TextView)c.FindViewById(Resource.Id.date);
            }
        }

        /// <summary>
        /// holds references to controls on the task page
        /// </summary>
        public class taskViewHolder : RecyclerView.ViewHolder
        {
            public TextView cTaskContent { get; set; }
            public TextView cPostedBy { get; set; }
            public TextView cDateTimePosted { get; set; }

            public taskViewHolder(View c) : base(c)
            {
                cTaskContent = (TextView)c.FindViewById(Resource.Id.content);
                cPostedBy = (TextView)c.FindViewById(Resource.Id.postedby);
                cDateTimePosted = (TextView)c.FindViewById(Resource.Id.date);
            }
        }

        //------------ DEBUGGING ----------------- >>> 

        /// <summary>
        /// this creates dummy data
        /// </summary>
        public static class RandomTestingMethods
        {


            public static List<event_table> initEvents()
            {
                //spoof checkboxes
                List<customCheckbox> box = new List<customCheckbox>();
                box.Add(new customCheckbox(true, "how you doin?"));
                box.Add(new customCheckbox(true, "how am I doin?"));
                box.Add(new customCheckbox(true, "hows Test doin?"));

                List<event_table> lel = new List<event_table>();

                event_table notice1 = new event_table()
                {
                    content = "lol this is a test",
                    postedby = 0,
                    dateposted = DateTime.Now,
                    type = "notice",
                    pinned = false
                };

                event_table notice2 = new event_table()
                {
                    content = "lol this is also test",
                    postedby = 1,
                    dateposted = DateTime.Now,
                    type = "notice",
                    pinned = false
                };

                event_table notice3 = new event_table()
                {
                    content = "lol this is another test",
                    postedby = 2,
                    dateposted = DateTime.Now,
                    type = "notice",
                    pinned = false
                };

                event_table task1 = new event_table()
                {
                    content = "lol this is a test",
                    postedby = 3,
                    dateposted = DateTime.Now,
                    type = "task",
                    pinned = false,
                    checkboxes = box
                };

                event_table task2 = new event_table()
                {
                    content = "lol this is also test",
                    postedby = 4,
                    dateposted = DateTime.Now,
                    type = "task",
                    pinned = false,
                    checkboxes = box
                };

                event_table task3 = new event_table()
                {
                    content = "lol this is another test",
                    postedby = 5,
                    dateposted = DateTime.Now,
                    type = "task",
                    pinned = false
                };

                lel.Add(notice1);
                lel.Add(notice2);
                lel.Add(notice3);
                lel.Add(task1);
                lel.Add(task2);
                lel.Add(task3);

                return lel;

            }

            //"[event: ID={0}, email={1}, password={2}, firstname={3}, lastname={4}, houseID={5}]"
            public static List<user_table> initUsers()
            {

                List<user_table> lel = new List<user_table>();

                user_table noticesdummydata = new user_table()
                {
                    email = "ash22334@hotmail.com",
                    password = "examplepassword",
                    firstname = "ashley",
                    lastname = "mcveigh",
                    houseID = 0
                };

                user_table noticesdummydata1 = new user_table()
                {
                    email = "bob@hotmail.com",
                    password = "examplepassword",
                    firstname = "frank",
                    lastname = "burns",
                    houseID = 0
                };

                user_table noticesdummydata2 = new user_table()
                {
                    email = "smith@hotmail.com",
                    password = "examplepassword",
                    firstname = "george",
                    lastname = "smith",
                    houseID = 0
                };

                user_table noticesdummydata3 = new user_table()
                {
                    email = "ash22334@hotmail.com",
                    password = "examplepassword",
                    firstname = "ashley",
                    lastname = "mcveigh",
                    houseID = 1
                };

                user_table noticesdummydata4 = new user_table()
                {
                    email = "bob@hotmail.com",
                    password = "examplepassword",
                    firstname = "frank",
                    lastname = "burns",
                    houseID = 1
                };

                user_table noticesdummydata5 = new user_table()
                {
                    email = "smith@hotmail.com",
                    password = "examplepassword",
                    firstname = "george",
                    lastname = "smith",
                    houseID = 1
                };

                lel.Add(noticesdummydata);
                lel.Add(noticesdummydata1);
                lel.Add(noticesdummydata2);
                lel.Add(noticesdummydata3);
                lel.Add(noticesdummydata4);
                lel.Add(noticesdummydata5);

                return lel;

            }

            public static List<house_table> initHouse()
            {

                List<house_table> lel = new List<house_table>();

                house_table noticesdummydata = new house_table()
                {
                    name = "Example House"
                };

                house_table noticesdummydata2 = new house_table()
                {
                    name = "other House"
                };

                lel.Add(noticesdummydata);
                lel.Add(noticesdummydata2);


                return lel;

            }

        }
    }

    /// <summary>
    /// this is supposed to implement the interfaces for the animation stuff, but i dont know how to reference the already running MainActivity object.
    /// </summary>
    public class animator : TabLayout.IOnTabSelectedListener, ViewPager.IOnPageChangeListener
    {

        MainActivity lol = new MainActivity();

        public IntPtr Handle
        {
            get
            {
                IntPtr lel = new IntPtr();
                return lel; 
            }
        }

        public void Dispose()
        {
            
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            
        }

        public void OnPageScrollStateChanged(int state)
        {
            
        }

        public void OnPageSelected(int position)
        {
            lol.animatefab(position);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
            
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            lol.animatefab(tab.Position);
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            
        }
    }
}

   