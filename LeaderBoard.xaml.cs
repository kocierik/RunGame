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
using SQLite;
using System.IO;
namespace sprinter
{
    /// <summary>
    /// Logica di interazione per LeaderBoard.xaml
    /// </summary>


  //  public delegate void AddItemHandler(object sender, ListObject itemToAdd);
    public partial class LeaderBoard : Window
    {
     //   public event AddItemHandler AddItem;
        public LeaderBoard(string name, string time)
        {
            InitializeComponent();
            playername.Text = name;
            playertime.Text = time;
        }

        List<Record> mylist = new List<Record>();
        Record punteggio = new Record();
        public static string databasePath = System.IO.Path.Combine
            (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "RunRecord.db");

        public static SQLiteConnection db = new SQLiteConnection(databasePath);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddScore();
            Refresh();
        }

        public void AddScore()
        {
            if(playertime.Text != "") { 
            mylist.Add(punteggio);
            Record d = new Record { Name = playername.Text, Score = playertime.Text };
            DGPoint.Items.Add(d);
            db.Insert(d);
            }
        }

        public void Refresh()
        {
            var result = db.Query<Record>("select * from Record ORDER BY Score");
            DGPoint.Items.Clear();
            foreach (var item in result)
                DGPoint.Items.Add(item);          
        }

        public class Record
        {
            [PrimaryKey, AutoIncrement]
            public int IdPlayer { get; set; }
            public string Name { get; set; }
            public string Score { get; set; }
        }
    }
}
