using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Text.RegularExpressions;

namespace Database_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            populateStateMenu();// add states to drop menu
            populateCityMenu();
            populateBusinessCategoryMenu();
            populateDays();
            populateZip();
        }

        public class Business
        {
            public string name { get; set; }
            public string address { get; set; }
            public string numTips { get; set; }
            public string totalCheckins { get; set; }
            public string id { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }
        public class Friends
        {
            public string user_id { get; set; }
            public string friend_id { get; set; }
            public string business_id { get; set; }
            public string name { get; set; }
            public string average_stars { get; set; }
            public string yelping_since { get; set; }
        }
        public class Tips 
        {
            public string userNameTip { get; set; }
            public string businessTip { get; set; }
            public string cityTip { get; set; }
            public string tipText { get; set; }
        }

        /* To establish connection with PostgreSQL Database */
        private string buildConnString()
        {
            return "Host=127.0.0.1; Port=5432; Username=postgres; Password=12345; Database=milestone3";
        }

        #region First tab: User Information 
        /// <summary>
        /// User Information:
        /// Use Cases: 
        /// 1. The user enters his/her own user id and retrieves his/her user profile information including:
        /// name, average stars, date he/she joined yelp, number of fans, average stars, and count of votes.
        /// 2. The list of the user’s friends and the latest tip each friend posted are displayed. 
        /// 3. User may rate one of his/her friends or remove a friend.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setUserbutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT name,average_stars,yelping_since FROM users WHERE user_id='" + userIDtextBox.Text + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nameTextBox.Text = reader.GetString(0);
                            starsTextBox.Text = reader.GetValue(1).ToString();
                            yelpingSinceTextBox.Text = reader.GetString(2);
                        }
                    }

                    cmd.CommandText = "SELECT type,count FROM votes WHERE user_id = '" + userIDtextBox.Text + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) == "funny")
                            {
                                funnyTextBox.Text = reader.GetValue(1).ToString();
                            }
                            else if (reader.GetString(0) == "cool")
                            {
                                coolTextBox.Text = reader.GetValue(1).ToString();
                            }
                            else if (reader.GetString(0) == "useful")
                            {
                                usefulTextBox.Text = reader.GetValue(1).ToString();
                            }
                        }
                    }

                    // TODO: add fans to fans textbox

                    // Add friends to text box
                    friendsDataGrid.Items.Clear();
                    cmd.CommandText = "SELECT name,average_stars,yelping_since,users.user_id,friend_id FROM users INNER JOIN friends ON friends.friend_id=users.user_id WHERE friends.user_id='" + userIDtextBox.Text + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            friendsDataGrid.Items.Add(new Friends() { name = reader.GetValue(0).ToString(), average_stars = reader.GetValue(1).ToString(), yelping_since = reader.GetValue(2).ToString(), user_id = reader.GetString(3), friend_id = reader.GetString(4) });
                        }
                    }

                    // Add to tips by friends data grid
                    tipsByFriendsDataGrid.Items.Clear();
                    cmd.CommandText = "SELECT users.name,business.name,business.city,tip.txt FROM users INNER JOIN friends ON friends.friend_id = users.user_id INNER JOIN tip ON tip.user_id = friends.friend_id INNER JOIN business ON business.business_id=tip.business_id WHERE friends.user_id='" + userIDtextBox.Text + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipsByFriendsDataGrid.Items.Add(new Tips() { userNameTip = reader.GetValue(0).ToString(), businessTip = reader.GetValue(1).ToString(), cityTip = reader.GetValue(2).ToString(), tipText = reader.GetValue(3).ToString() });
                        }
                    }
                }
            }
            return;
        }

        private void removeFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string[] tables = { "friends", "votes", "tip", "users" };
            Friends f = (Friends)friendsDataGrid.SelectedItem;

            foreach (string s in tables)
            {
                using (var connection = new NpgsqlConnection(buildConnString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "DELETE FROM " + s + " WHERE user_id='" + f.friend_id + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            }
                        }
                    }
                }
            }
        }

        private void rateFrriendButton_Click(object sender, RoutedEventArgs e)
        {
            Friends f = (Friends)friendsDataGrid.SelectedItem;
            string txt = rateFriendScoreTextBox.Text;
            int rating = Int32.Parse(txt);
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE users SET average_stars = ((average_stars + " + rating.ToString() + ")/2) WHERE user_id='" + f.friend_id + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
            }
        }

        #endregion First Tab

        #region Second Tab: Business Search
        private void searchBusinessButton_Click(object sender, RoutedEventArgs e)
        {
            searchResultsDataGrid.Items.Clear();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    var where = buildWhereSearchString();
                    cmd.CommandText = "SELECT DISTINCT name,full_address,review_count,numcheckins,business_id FROM business " + where + ";";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            searchResultsDataGrid.Items.Add(new Business() { name = reader.GetString(0), address = reader.GetString(1), numTips = reader.GetValue(2).ToString(), totalCheckins = reader.GetValue(3).ToString(), id = reader.GetString(4) });

                        }
                    }
                }
            }
            Business b = (Business)searchResultsDataGrid.Items[0];
            enterTipTextBox.Text = b.id;
        }

        private string buildWhereSearchString()
        {
            Boolean needed = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");

            if (statesComboBox.SelectedIndex > -1)
            {
                needed = true;
                sb.Append("state='" + statesComboBox.SelectedItem + "' ");
            }

            if (citiesListBox.SelectedIndex > -1)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("city='" + citiesListBox.SelectedItem + "' ");
            }

            if (selectedCategoriesListBox.Items.Count > 0)
            {
                //category = true;
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM categories WHERE ");

                for (int i = 0; i < selectedCategoriesListBox.Items.Count; i++)
                {
                    if (i != 0) { sb.Append("AND "); }
                    sb.Append("category = '" + selectedCategoriesListBox.Items[i] + "' ");
                }

                sb.Append(")");
            }

            if (zipCodeListBox.SelectedIndex > 0)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("RIGHT(full_address,5)='" + zipCodeListBox.SelectedItem + "' ");
            }

            if (fromComboBox.SelectedIndex > 0 && toComboBox.SelectedIndex > 0 && dayOfTheWeekComboBox.SelectedIndex > 0)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM hours WHERE open <= CAST('" + fromComboBox.SelectedItem + "' AS TIME) AND close >= CAST('" + toComboBox.SelectedItem + "' AS TIME) AND day='" + dayOfTheWeekComboBox.SelectedItem + "')");
                sb.Append("AND business_id IN (SELECT business_id FROM hours WHERE open=close AND day='" + dayOfTheWeekComboBox.SelectedItem + "')");
            }

            if (needed)
            {
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }
        private void populateStateMenu()
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statesComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return;
        }

        private void populateCityMenu()
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT city FROM business ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            citiesListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return;
        }
        private void populateBusinessCategoryMenu()
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT category FROM categories ORDER BY category;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            businessCategoryListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return;
        }

        private void populateDays()
        {
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            foreach (string s in days)
            {
                dayOfTheWeekComboBox.Items.Add(s);
            }

            for (int i = 0; i < 24; i++)
            {
                var s = "";
                if (i < 10)
                {
                    s = "0" + i.ToString() + ":00:00";
                }
                else
                {
                    s = i.ToString() + ":00:00";
                }
                fromComboBox.Items.Add(s);
                toComboBox.Items.Add(s);
            }
        }

        private void populateZip()
        {
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT RIGHT(full_address,5) FROM business;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zipCodeListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return;
        }

        private void statesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateCities();
            updateZipCode();
            updateCategories();
        }

        public void updateCities()
        {
            citiesListBox.Items.Clear(); // clear previous results
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT city FROM business " + buildCitiesWhere() + " ORDER BY city";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            citiesListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
        }

        private string buildCitiesWhere()
        {
            Boolean needed = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");

            if (statesComboBox.SelectedIndex > -1)
            {
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE state ='" + statesComboBox.SelectedItem + "') ");
            }
            if (zipCodeListBox.SelectedIndex > -1)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE RIGHT(full_address,5)='" + zipCodeListBox.SelectedItem + "') ");
            }
            if (needed)
            {
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        private void updateZipCode()
        {
            zipCodeListBox.Items.Clear();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT RIGHT(full_address,5) FROM business " + buildZipWhere() + "ORDER BY RIGHT(full_address,5);";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zipCodeListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return;
        }

        private string buildZipWhere()
        {
            Boolean needed = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");
            if (statesComboBox.SelectedIndex > -1)
            {
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE state ='" + statesComboBox.SelectedItem + "') ");
            }
            if (citiesListBox.SelectedIndex > -1)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE city ='" + citiesListBox.SelectedItem + "') ");
            }

            if (needed)
            {
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        private void updateCategories()
        {
            businessCategoryListBox.Items.Clear();
            StringBuilder sb = new StringBuilder();
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT DISTINCT category FROM categories " + buildCategoriesWhereString() + " ORDER BY category;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            businessCategoryListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            selectedBusinessesTextBox.Text = (businessCategoryListBox.Items.Count.ToString());

        }

        private string buildCategoriesWhereString()
        {
            Boolean needed = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");
            if (statesComboBox.SelectedIndex > -1)
            {
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE state ='" + statesComboBox.SelectedItem + "') ");
            }
            if (citiesListBox.SelectedIndex > -1)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE city ='" + citiesListBox.SelectedItem + "') ");
            }
            if (zipCodeListBox.SelectedIndex > -1)
            {
                if (needed)
                {
                    sb.Append("AND ");
                }
                needed = true;
                sb.Append("business_id IN (SELECT business_id FROM business WHERE RIGHT(full_address,5)='" + zipCodeListBox.SelectedItem + "') ");
            }
            if (needed)
            {
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            selectedCategoriesListBox.Items.Remove(selectedCategoriesListBox.SelectedItem);
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var v in selectedCategoriesListBox.Items)
            {
                if (businessCategoryListBox.SelectedItem == v)
                {
                    return;
                }
            }
            selectedCategoriesListBox.Items.Add(businessCategoryListBox.SelectedItem);
        }

        private void citiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateCategories();
            if (zipCodeListBox.SelectedIndex < 0) //only update if none selected
            { 
                updateZipCode();
            }
        }

        private void zipCodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateCategories();
            if (citiesListBox.SelectedIndex < 0)
            {
                updateCities();
            }
        }

        private void showCheckinsButton_Click(object sender, RoutedEventArgs e)
        {
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            if (searchResultsDataGrid.SelectedIndex < 0)
            {
                return;
            }

            Business b = (Business)searchResultsDataGrid.SelectedItem;
            var bId = b.id;
            List<int> dayData = new List<int>(); List<int> count = new List<int>();

            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT day, SUM(COUNT) FROM checkin WHERE business_id = '" + b.id + "' GROUP BY day;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dayData.Add(reader.GetInt32(0));
                            count.Add(reader.GetInt32(1));
                            //data.Add(new KeyValuePair<int,int>(reader.GetInt32(0), reader.GetInt32(1)));
                        }
                    }
                }
            }


            List<KeyValuePair<string, int>> myData = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < 7; i++)
            {
                if (dayData.Contains(i))
                {
                    int c = count[dayData.IndexOf(i)];
                    myData.Add(new KeyValuePair<string, int>(days[i], c));
                }
                else
                {
                    myData.Add(new KeyValuePair<string, int>(days[i], 0));
                }
            }
            ShowCheckinsWindow w2 = new ShowCheckinsWindow();
            w2.checkinChart.DataContext = myData;
            w2.Show();
        }

        private void numBusinessPerCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> myData = new Dictionary<string, int>();
            
            for (int i = 0; i < searchResultsDataGrid.Items.Count; i++)
            {
                Business b = (Business)searchResultsDataGrid.Items[i];

                using (var connection = new NpgsqlConnection(buildConnString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT category FROM Categories WHERE business_id='" + b.id + "' ";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (myData.ContainsKey(reader.GetString(0)))
                                {
                                    myData[reader.GetString(0)] += 1;
                                }
                                else
                                {
                                    myData[reader.GetString(0)] = 1;
                                }
                            }
                        }
                    }
                }
            }
            CategoriesChart w2 = new CategoriesChart();
            w2.categoryChart.DataContext = myData;
            w2.Show();
        }

        private void avgStarsPerCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> categoryCount = new Dictionary<string, int>();
            Dictionary<string, int> categoryStarSum = new Dictionary<string, int>();
            List<string> cat = new List<string>();

            for (int i = 0; i < searchResultsDataGrid.Items.Count; i++)
            {
                Business b = (Business)searchResultsDataGrid.Items[i];
                cat.Clear();

                using (var connection = new NpgsqlConnection(buildConnString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT category FROM Categories WHERE business_id='" + b.id + "' ";
                        using (var reader = cmd.ExecuteReader())
                        { //gets all categories for business into list
                            while (reader.Read())
                            {
                                cat.Add(reader.GetString(0));
                            }
                        }
                        foreach (string s in cat)
                        {
                            cmd.CommandText = "SELECT stars FROM business WHERE business_id='" + b.id + "' ";//adds stars for each category to a list for count and sum FIND AVG (sum/count)
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (categoryCount.ContainsKey(s))
                                    {
                                        categoryCount[s] += 1;
                                        categoryStarSum[s] += reader.GetInt32(0);
                                    }
                                }
                            }
                        }
                    }
                }
            }    

            CategoriesChart w2 = new CategoriesChart();
            foreach (KeyValuePair<string, int> kvp in categoryStarSum)
            {
                categoryStarSum[kvp.Key] = kvp.Value / categoryCount[kvp.Key];
            }
            w2.categoryChart.DataContext = categoryStarSum;
            w2.Show();
        }

        private void showTipsButton_Click(object sender, RoutedEventArgs e)
        {
            TipsTable w2 = new TipsTable();
            Business b = (Business)searchResultsDataGrid.SelectedItem;
            using (var connection = new NpgsqlConnection(buildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name,thedate,likes,txt FROM Tip INNER JOIN users ON tip.business_id = '" + b.id + "' AND tip.user_id = users.user_id ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipsTable.Data d = new TipsTable.Data { user = reader.GetString(0), date = reader.GetValue(1).ToString(), numLikes = reader.GetValue(2).ToString(), text = reader.GetString(3) };
                            w2.tipsGrid.Items.Add(d);
                        }
                    }
                }
            }
            w2.Show();
        }
        #endregion Second Tab

    }
}
