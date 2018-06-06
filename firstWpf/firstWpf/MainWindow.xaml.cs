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


        }
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            string path = "../../LS.xml";
            // XmlDocument xmlDocument = new XmlDocument();
            string text = File.ReadAllText(path, Encoding.GetEncoding("windows-1251"));
            //StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("windows-1251"));
            XDocument xdoc = XDocument.Parse(text);
            int Count = 0;
            List<Chapters> Chapter0 = new List<Chapters>();
            List<Positions> Position0 = new List<Positions>();
            List<TzmMch> TzmMch0 = new List<TzmMch>();

            foreach (XElement Chapterss in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement Chapter in Chapterss.Elements("Chapter"))
                {
                    foreach (XAttribute attr in Chapter.Attributes("Caption"))
                    {
                        Console.WriteLine("CHAPTER    {0}", attr.Value);

                        Chapter0.Add(new Chapters() { Caption = attr.Value.ToString() });
                        
                    }

                    foreach (XElement Position in Chapter.Elements("Position"))
                    {
                        foreach (XAttribute Caption in Position.Attributes("Caption"))
                        {
                            XAttribute Code = Position.Attributes("Code").First();
                            XAttribute units = Position.Attributes("Units").First();
                            Console.WriteLine("Position    {0}", Caption);
                            Count++;
                            
                            

                            foreach (XElement Quantity in Position.Elements("Quantity"))
                            {
                                foreach (XAttribute Quantity1 in Quantity.Attributes("Fx"))
                                {
                                    Console.WriteLine("Quantity    {0}", Quantity1);
                                    Chapters.Positions.Add(new Positions(Count, Code.Value.ToString(), Caption.Value.ToString(), units.Value.ToString(), Quantity1.Value.ToString()));
                                }
                            }
                            
                        }

                        foreach (XElement Resources in Position.Elements("Resources"))
                        {
                            foreach (XElement Tzm in Resources.Elements("Tzm"))
                                foreach (XAttribute attr in Tzm.Attributes("Caption"))
                                {
                                    Console.WriteLine("Tzm    {0}", attr);
                                    TzmMch0.Add(new TzmMch() { Caption = attr.Value.ToString() });
                                }
                                    
                            foreach (XElement Mch in Resources.Elements("Mch"))
                                foreach (XAttribute attr in Mch.Attributes("Caption"))
                                {
                                    Console.WriteLine("Tzm    {0}", attr);
                                    TzmMch0.Add(new TzmMch() { Caption = attr.Value.ToString() });
                                }
                        }
                    }
                }
            }

            //grid.ItemsSource = Chapter0;
            grid.ItemsSource = Position0;
            //grid.ItemsSource = TzmMch0;
            


        }
        class Chapters
        {
            public string Caption { get; set; }
            public List<Positions> Positions { get; }
        }
        class Positions
        {
            public int Number { get; set; }
            public string Code { get; set; }
            public string Caption { get; set; }
            public string Units { get; set; }
            public string Quantity { get; set; } //Resources
            public Positions(int Number, string Code, string Caption, string Units, string Quantity)
            {
                this.Number = Number;
                this.Code = Code;
                this.Caption = Caption;
                this.Units = Units;
                this.Quantity = Quantity;
            }
        }
        
        class TzmMch
        {
            public string Code { get; set; }
            public string Caption { get; set; }
            public string Quantity { get; set; }
        }
        

    }
}
