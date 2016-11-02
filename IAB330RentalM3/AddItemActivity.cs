using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.IO;

using Android.Support.V7.App;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content;
using IAB330RentalM3.Database;

namespace IAB330RentalM3
{
    [Activity(Label = "Add a Notice")]
    public class AddItemActivity : AppCompatActivity
    {
        SQLiteObj db = new SQLiteObj();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //set the theme
            SetTheme(Resource.Style.DesignTheme);
            SetContentView(Resource.Layout.additem);

            //load the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.tool2thebar);
            SetSupportActionBar(toolbar);


            //get things on page
           
            var button = FindViewById<Button>(Resource.Id.submitbutton);
            RadioButton radiotasks = FindViewById<RadioButton>(Resource.Id.radio_task);
            RadioButton radionotices = FindViewById<RadioButton>(Resource.Id.radio_notice);
            radionotices.Checked = true;

            radiotasks.Click += RadioButtonClick;
            radionotices.Click += RadioButtonClick;

            //on submit
            button.Click += delegate
            {

                var contextPref = Application.Context.GetSharedPreferences("preferences", Android.Content.FileCreationMode.Private);
                string id = contextPref.GetString("userid", "");

                string sqliteFilename = "database.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);

                var text = FindViewById<EditText>(Resource.Id.editText);

                var typeroony = FindViewById<EditText>(Resource.Id.editText);
                //submit to database
                event_table toSubmit = new event_table
                {
                    content = text.Text,
                    dateposted = DateTime.Now,
                    postedby = Convert.ToInt32(id),
                    type = taskOrNotice
                };

                db.eventInsertUpdateData(toSubmit, path);

                //navigate Home
                var intent = new Intent(this, typeof(IAB330RentalM3.MainActivity));
                StartActivity(intent);
            };
        }
        public string taskOrNotice = "";

        /// <summary>
        /// eventhandler for radiobuttons
        /// </summary>
        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            taskOrNotice = (rb.Text).ToLower();


        }
    }
    
}