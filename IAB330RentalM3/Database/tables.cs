using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;



namespace IAB330RentalM3.Database
{
    /// <summary>
    /// Holds cards information
    /// "Content" is a JSON object containing the contents of the card.
    /// "Type" represents whether this is a notice or a task.
    /// </summary>
    public class event_table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Unique]
        public int Id { get; set; }

        public string content { get; set; }

        //represents a user
        [ForeignKey(typeof(user_table))]
        public int postedby { get; set; }

        public DateTime dateposted { get; set; }

        public string type { get; set; }

        public bool pinned { get; set; }

        [TextBlob("checkboxBlob"), Ignore]
        public List<customCheckbox> checkboxes { get; set; }

        public string checkboxBlob { get; set; } 

        //many to one relationship with users
        [ManyToOne, Ignore]
        public user_table users { get; set; }

        public override string ToString()
        {
            return string.Format("[event: ID={0}, content={1}, postedby={2}, dateposted={3}, type={4}, pinned={5}]", Id, content, postedby, dateposted, type, pinned);
        }
    }

    /// <summary>
    /// Holds login information as well as what house the user belongs to.
    /// </summary>
    public class user_table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Unique]
        public int Id { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        // One to many relationship with events
        [OneToMany(CascadeOperations = CascadeOperation.All), Ignore]
        public List<event_table> events { get; set; }
        
        //represents a house, e.g a user belongs to a house.
        [ForeignKey(typeof(house_table))]
        public int houseID { get; set; }

        // many to one relationship with house
        [ManyToOne, Ignore]
        public house_table house { get; set; }

        public override string ToString()
        {
            return string.Format("[user: ID={0}, email={1}, password={2}, firstname={3}, lastname={4}, houseID={5}]", Id, email, password, firstname, lastname, houseID);
        }
    }

    /// <summary>
    /// contains houses
    /// </summary>
    public class house_table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Unique]
        public int Id { get; set; }

        public string name { get; set; }

        //one to many relationship with users
        [OneToMany(CascadeOperations = CascadeOperation.All), Ignore]
        public List<user_table> users { get; set; }

        public override string ToString()
        {
            return string.Format("[house: ID={0}, name={1}]", Id, name);
        }
    }

    /// <summary>
    /// little class for holding checkboxes
    /// </summary>
    public class customCheckbox
    {
        public bool isChecked { get; set; }
        public string description { get; set; } 

        public customCheckbox(bool isChecked, string description)
        {
            this.isChecked = isChecked;
            this.description = description;
        }
    }
}

