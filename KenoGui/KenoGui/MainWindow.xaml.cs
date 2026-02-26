using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KenoGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> sorsoltSzamok = new List<int>();
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            GridLetrehoz();
        }
        private void GridLetrehoz()
        {
            gridSzamok.Children.Clear();
            gridSzamok.RowDefinitions.Clear();
            gridSzamok.ColumnDefinitions.Clear();

            for (int i = 0; i < 8; i++)
                gridSzamok.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 10; i++)
                gridSzamok.ColumnDefinitions.Add(new ColumnDefinition());

            int szam = 1;
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    Border b = new Border();
                    b.BorderBrush = Brushes.Black;
                    b.BorderThickness = new Thickness(1);
                    b.Background = Brushes.LightGreen;

                    TextBlock t = new TextBlock();
                    t.Text = szam.ToString();
                    t.HorizontalAlignment = HorizontalAlignment.Center;
                    t.VerticalAlignment = VerticalAlignment.Center;

                    b.Child = t;

                    Grid.SetRow(b, r);
                    Grid.SetColumn(b, c);

                    gridSzamok.Children.Add(b);
                    szam++;
                }
            }
        }

       
        private void BtnBetoltes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                lbSzelvenyek.Items.Clear();
                foreach (var sor in ofd.FileName)
                    lbSzelvenyek.Items.Add(sor);
            }
        }


        private void BtnSorsolas_Click(object sender, RoutedEventArgs e)
        {
            sorsoltSzamok.Clear();
            lbSorsolt.Items.Clear();

            while (sorsoltSzamok.Count < 20)
            {
                int szam = rnd.Next(1, 81);
                if (!sorsoltSzamok.Contains(szam))
                    sorsoltSzamok.Add(szam);
            }

            sorsoltSzamok.Sort();

            foreach (var sz in sorsoltSzamok)
                lbSorsolt.Items.Add(sz);
        }

        private void lbSzelvenyek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSzelvenyek.SelectedItem == null)
                return;

            GridLetrehoz();

            string sor = lbSzelvenyek.SelectedItem.ToString();
            string[] darabok = sor.Split(',');

            int tet = int.Parse(darabok[0].Replace("KN", ""));
            txtSzorzó.Text = $"Szorzó: {tet}";

            List<int> szelvenySzamok = new List<int>();
            for (int i = 1; i < darabok.Length; i++)
                szelvenySzamok.Add(int.Parse(darabok[i]));

            foreach (Border b in gridSzamok.Children)
            {
                TextBlock t = b.Child as TextBlock;
                int szam = int.Parse(t.Text);

                if (szelvenySzamok.Contains(szam))
                    b.Background = Brushes.Yellow;
            }

   
            if (sorsoltSzamok.Count > 0)
            {
                int talalat = szelvenySzamok.Intersect(sorsoltSzamok).Count();
                int nyeremeny = Szorzo(tet, talalat);

                txtNyeremeny.Text = $"Nyeremény: {nyeremeny}";
            }
        }

        private int Szorzo(int tet, int talalat)
        {
            return tet * talalat * 100;
        }
    }
}