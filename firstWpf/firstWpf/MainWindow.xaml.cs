using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
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
using System.Xml.Linq;
using System.Xml;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace firstWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ChaptersViewModel();
        }
        //private void grid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    string path = "../../LS.xml";
        //    // XmlDocument xmlDocument = new XmlDocument();
        //    string text = File.ReadAllText(path, Encoding.GetEncoding("windows-1251"));
        //    //StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("windows-1251"));
        //    XDocument xdoc = XDocument.Parse(text);
        //    int Count = 0;
        //    List<Chapters> Chapter0 = new List<Chapters>();
        //    List<Positions> Positions = new List<Positions>();
        //    List<Positions> Position0 = new List<Positions>();
        //    List<TzmMch> TzmMch0 = new List<TzmMch>();
        //    List<Row> Row0 = new List<Row>();

        //    foreach (XElement Chapterss in xdoc.Root.Elements("Chapters"))
        //    {
        //        foreach (XElement Chapter in Chapterss.Elements("Chapter"))
        //        {
        //                XAttribute Chapter1 = Chapter.Attributes("Caption").First();                                           
        //                Chapters newChapter;
        //                Chapter0.Add(newChapter = new Chapters() { Caption = Chapter1.Value.ToString() });

        //                foreach (XElement Position in Chapter.Elements("Position"))
        //                {
        //                        XAttribute Caption = Position.Attributes("Caption").First();                            
        //                        XAttribute Code = Position.Attributes("Code").First();
        //                        XAttribute units = Position.Attributes("Units").First();                               
        //                        Count++;
        //                        XElement Quantity = Position.Elements("Quantity").First();
        //                        XAttribute Quantity1 = Quantity.Attributes("Fx").First();                                       
        //                        Position0.Add(new Positions(Count, Code.Value.ToString(), Caption.Value.ToString(), units.Value.ToString(), Quantity1.Value.ToString()));




        //                    foreach (XElement Resources in Position.Elements("Resources"))
        //                    {
        //                        foreach (XElement Tzm in Resources.Elements("Tzm"))
        //                        {
        //                                XAttribute Tzm1 = Tzm.Attributes("Caption").First();
        //                                TzmMch0.Add(new TzmMch() { Caption = Tzm1.Value.ToString() });
        //                                Row0.Add(new Row()
        //                                {
        //                                    Chaptername = Chapter1.Value.ToString(),
        //                                    PositionNumber = Count,
        //                                    PositionCode = Code.Value.ToString(),
        //                                    Positionname = Caption.Value.ToString(),
        //                                    PositionUnits = units.Value.ToString(),
        //                                    PositionQuantity = Quantity1.Value.ToString(),

        //                                    TzmMchname = Tzm1.Value.ToString()
        //                                });
        //                        }   
        //                        foreach (XElement Mch in Resources.Elements("Mch"))
        //                        {
        //                                XAttribute Mch1 = Mch.Attributes("Caption").First();
        //                                TzmMch0.Add(new TzmMch() { Caption = Mch1.Value.ToString() });
        //                                Row0.Add(new Row()
        //                                {
        //                                    Chaptername = Chapter1.Value.ToString(),
        //                                    PositionNumber = Count,
        //                                    PositionCode = Code.Value.ToString(),
        //                                    Positionname = Caption.Value.ToString(),
        //                                    PositionUnits = units.Value.ToString(),
        //                                    PositionQuantity = Quantity1.Value.ToString(),

        //                                    TzmMchname = Mch1.Value.ToString()
        //                                });
        //                        }

        //                }

        //                }


        //        }
        //    }

        //    //grid.ItemsSource = Chapter0;
        //    //dataGrid1.ItemsSource = Position0;
        //    //grid.ItemsSource = TzmMch0;
        //    dataGrid1.ItemsSource = Row0;


        //}
        //public class Chapters
        //{
        //    public string Caption { get; set; }
        //    //public List<Positions> Positions { get; set; }
        //    public List<Positions> Positions { get; set; } = new List<Positions>();
        //}
        //public class Positions
        //{
        //    public int Number { get; set; }
        //    public string Code { get; set; }
        //    public string Caption { get; set; }
        //    public string Units { get; set; }
        //    public string Quantity { get; set; } //Resources
        //    public Positions(int Number, string Code, string Caption, string Units, string Quantity)
        //    {
        //        this.Number = Number;
        //        this.Code = Code;
        //        this.Caption = Caption;
        //        this.Units = Units;
        //        this.Quantity = Quantity;
        //    }
        //}

        //public class TzmMch
        //{
        //    public string Code { get; set; }
        //    public string Caption { get; set; }
        //    public string Quantity { get; set; }
        //}

        //public class Row
        //{
        //    public string Chaptername { get; set; }

        //    public int PositionNumber { get; set; }
        //    public string PositionCode { get; set; }
        //    public string Positionname { get; set; }
        //    public string PositionUnits { get; set; }
        //    public string PositionQuantity { get; set; }

        //    public string TzmMchCode { get; set; }
        //    public string TzmMchname { get; set; }
        //    public string TzmMchQuantity { get; set; }
        //}
        //public class Rows : ObservableCollection<Row>
        //{
        //    // Creating the Tasks collection in this way enables data binding from XAML.
        //}

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Chaptername")
            {
                e.Cancel = true;
            }
        }
    }
}
