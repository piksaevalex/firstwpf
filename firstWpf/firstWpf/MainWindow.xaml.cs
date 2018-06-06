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
            List<Chapter> Chapter0 = new List<Chapter>();
            List<Position> Position0 = new List<Position>();
            List<TzmMch> TzmMch0 = new List<TzmMch>();

            foreach (XElement Chapters in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement Chapter in Chapters.Elements("Chapter"))
                {
                    foreach (XAttribute attr in Chapter.Attributes("Caption"))
                    {
                        Console.WriteLine("CHAPTER    {0}", attr.Value);

                        Chapter0.Add(new Chapter() { Caption = attr.Value.ToString() });
                        // ВОТ тут attr это текст который мне нужен
                        // Chapter = attr attr.ToString()
                    }

                    foreach (XElement Position in Chapter.Elements("Position"))
                    {
                        foreach (XAttribute attr in Position.Attributes("Caption"))
                        {
                            XAttribute atr = Position.Attributes("Code").First();
                            XAttribute units = Position.Attributes("Units").First();
                            Console.WriteLine("Position    {0}", attr);
                            Position0.Add(new Position() {
                                Caption = attr.Value.ToString(),
                                Code = atr.Value.ToString(),
                                Units = units.Value.ToString()
                            });
                        }
                            foreach (XElement Quantity in Position.Elements("Quantity"))
                            {
                                foreach (XAttribute attr in Quantity.Attributes("Fx"))
                                {
                                    Console.WriteLine("Quantity    {0}", attr);
                                    Position0.Add( new Position() { Quantity = attr.Value.ToString() });
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
        class Chapter
        {
            public string Caption { get; set; }
            public List<Position> Positions { get; }
        }
        class Position
        {
            public int Number { get; set; }
            public string Code { get; set; }
            public string Caption { get; set; }
            public string Units { get; set; }
            public string Quantity { get; set; } //Resources
        }
        
        class TzmMch
        {
            public string Code { get; set; }
            public string Caption { get; set; }
            public string Quantity { get; set; }
        }
        

    }
}
