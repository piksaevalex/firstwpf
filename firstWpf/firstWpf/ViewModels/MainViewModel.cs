using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using firstWpf.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace firstWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            IsExport = false;
            Chapters = new ObservableCollection<Chapter>();
            LoadCommand = new RelayCommand(Load);
            ExportCommand = new RelayCommand(Export);

        }

        public ICommand LoadCommand { get; }

        public ICommand ExportCommand { get; }

        public ObservableCollection<Chapter> Chapters { get; }

        private async void Load(object param)
        {
            string path = "ЛС 02-01-01-01.xml";
            string text;

            using (var reader = File.OpenText(path))
            {
                IsBusy = true;
                text = await reader.ReadToEndAsync();
                await Task.Delay(1000);
                IsBusy = false;
            }

            IsExport = true;
            var windows1251 = Encoding.GetEncoding("windows-1251");
            var utf8 = Encoding.UTF8;

            var originalBytes = windows1251.GetBytes(text.ToString());
            var correctXmlString = utf8.GetString(originalBytes);
            XDocument xdoc = XDocument.Parse(correctXmlString);
            //XDocument xdoc = XDocument.Parse(text);

            foreach (XElement chapterss in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement Chapter in chapterss.Elements("Chapter"))
                {
                    int count = 0;
                    XAttribute chapter1 = Chapter.Attributes("Caption").First();
                    //Chapters newChapter;
                    //Chapter0.Add(newChapter = new Chapters() { Caption = Chapter1.Value.ToString() });
                    var chapter = new Chapter(chapter1.Value);

                    foreach (XElement position in Chapter.Elements("Position"))
                    {
                        XAttribute caption = position.Attributes("Caption").First();
                        XAttribute code = position.Attributes("Code").First();
                        XAttribute units = position.Attributes("Units").First();
                        count++;
                        XElement quantity = position.Elements("Quantity").First();
                        XAttribute quantity1 = quantity.Attributes("Fx").First();
                        string str = quantity1.Value.ToString();
                        str = str.Replace(",", ".");
                        decimal result = Convert.ToDecimal(new DataTable().Compute(str, null));
                        result = Decimal.Round(result, 3);
                        var positions = new Position(count, code.Value, caption.Value, units.Value, result.ToString());
                        chapter.Positions.Add(positions);



                        foreach (XElement resources in position.Elements("Resources"))
                        {
                            foreach (XElement tzm in resources.Elements("Tzm"))
                            {
                                XAttribute tzm1 = tzm.Attributes("Caption").First();
                                XAttribute tzmMchCode = tzm.Attributes("Code").First();
                                XAttribute tzmMchUnits = tzm.Attributes("Units").First();
                                XAttribute tzmMchQuantity = tzm.Attributes("Quantity").First();

                                
                                positions.TzmMchs.Add(new TzmMch(tzmMchCode.Value, tzm1.Value, tzmMchUnits.Value, tzmMchQuantity.Value));
                            }
                            foreach (XElement mch in resources.Elements("Mch"))
                            {
                                XAttribute mch1 = mch.Attributes("Caption").First();
                                XAttribute tzmMchCode = mch.Attributes("Code").First();
                                XAttribute tzmMchUnits = mch.Attributes("Units").First();
                                XAttribute tzmMchQuantity = mch.Attributes("Quantity").First();
                                positions.TzmMchs.Add(new TzmMch(tzmMchCode.Value, mch1.Value, tzmMchUnits.Value, tzmMchQuantity.Value));
                            }

                        }

                    }
                    Chapters.Add(chapter);


                }
            }
        }

        private void Export(object param)
        {
            Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            ex.Visible = true;
            ex.SheetsInNewWorkbook = 1;
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            ex.DisplayAlerts = false;
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            sheet.Name = "XML";
            int i = 1; // Номер строки в exel 
            int c = 0; // индекс элементов первого уровня
            int k = 0; // индекс элементов второго уровня
            int m = 0; // индекс элементов третьего уровня
            while (c < Chapters.Count)
            {
                sheet.Cells[i, 1] = Chapters[c].Name;
                i++;
                
                k = 0;
                m = 0;
                while (k < Chapters[c].Positions.Count)
                {
                    sheet.Cells[i, 1] = Chapters[c].Positions[k].Number;
                    sheet.Cells[i, 2] = Chapters[c].Positions[k].Code;
                    sheet.Cells[i, 3] = Chapters[c].Positions[k].Name;
                    sheet.Cells[i, 4] = Chapters[c].Positions[k].Units;
                    sheet.Cells[i, 5] = Chapters[c].Positions[k].Quantity;

                    i++;
                    m = 0;
                    while (m < Chapters[c].Positions[k].TzmMchs.Count)
                    {
                        sheet.Cells[i, 2] = Chapters[c].Positions[k].TzmMchs[m].Code;
                        sheet.Cells[i, 3] = Chapters[c].Positions[k].TzmMchs[m].Name;
                        sheet.Cells[i, 4] = Chapters[c].Positions[k].TzmMchs[m].Quantity;
                        m++;
                        i++;
                    }
                    k++;
                }
                c++;
            }
            sheet.Columns.AutoFit();

            
            ex.Application.ActiveWorkbook.SaveAs("doc.xlsx", Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }
}
