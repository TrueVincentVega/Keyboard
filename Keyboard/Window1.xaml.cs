using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Data.Common;
using System.Data.SQLite;

namespace Keyboard
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string _name = "";
        private string _DBPathFile; // path to SQLite database
        private MainWindow _mw;
        private string _constr;
        private SQLiteConnection _connection;
        //private System.Data.SQLite.SqliteConnection _connection;
        
        
public Window1(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
            string DBPath = Assembly.GetExecutingAssembly().Location;
            DBPath = System.IO.Path.GetDirectoryName(DBPath) + "\\mydatabase.sqlite";

            if (File.Exists(DBPath))
            {
                _constr = $"DataSource={DBPath}; Version=3";
                _connection = new SQLiteConnection(_constr);
            }

            _connection.Open();
            string query = "SELECT * from resulttbl";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            SQLiteDataReader reader = command.ExecuteReader();
            int n = 1;
            TextBlock tb;
            foreach (DbDataRecord onerecord in reader)
            {
                RowDefinition rd = new RowDefinition();
                resultGrid.RowDefinitions.Add(rd);
                tb = new TextBlock();
                tb.Text = onerecord.GetValue(1).ToString();
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, n);

                tb = new TextBlock();
                tb.Text = onerecord.GetValue(2).ToString();
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 1);
                Grid.SetRow(tb, n);

                tb = new TextBlock();
                tb.Text = onerecord.GetValue(3).ToString();
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 2);
                Grid.SetRow(tb, n);
                n++;
            }
            _connection.Close();

        }

        private void addButtonClick(object sender, RoutedEventArgs e)
        {
            if (addName.Text.Length > 0)
            {
                _connection.Open();
                string query = $"INSERT INTO resulttbl (name, speed, fails) VALUES ('{addName.Text}', '{_mw.SpeedCounter}/{_mw.ChronCounter}s', '{_mw.FailCounter}');";
                SQLiteCommand command = new SQLiteCommand(query, _connection);

                command.ExecuteNonQuery();
                _connection.Close();
               

                //{addName.Text}', '{_mw.SpeedCounter}/{_mw.ChronCounter}s', '{_mw.FailCounter}');";

                RowDefinition rd = new RowDefinition();
                resultGrid.RowDefinitions.Add(rd);
                TextBlock tb = new TextBlock();
                tb.Text = addName.Text;
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, resultGrid.RowDefinitions.Count - 2);

                tb = new TextBlock();
                tb.Text = $"{ _mw.SpeedCounter}/ {_mw.ChronCounter} s";
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 1);
                Grid.SetRow(tb, resultGrid.RowDefinitions.Count - 2);

                tb = new TextBlock();
                tb.Text = _mw.FailCounter.ToString();
                resultGrid.Children.Add(tb);
                Grid.SetColumn(tb, 2);
                Grid.SetRow(tb, resultGrid.RowDefinitions.Count - 2);

                addName.Text = "";
            }
            else
            {
                MessageBox.Show("Add username!");
            }
            
            
        }

        private void calcelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
