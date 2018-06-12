using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace firstWpf
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isFileLoaded;

        public MainViewModel()
        {
            Chapters = new ObservableCollection<Models>();
            LoadCommand = new RelayCommand(Load);
            ExportCommand = new RelayCommand(Export, CanExport);
        }

        public RelayCommand LoadCommand { get; }

        public RelayCommand ExportCommand { get; }

        public ObservableCollection<Models> Chapters { get; }

        public string GetPath()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true) return dialog.FileName;
            return null;
        }


        private async void Load(object param)
        {
            try
            {
                var path = GetPath();
                if (path == null) return;
                string text;

                using (var str = new StreamReader(path, Encoding.Default))
                {
                    IsBusy = true;
                    text = await str.ReadToEndAsync();

                    await Task.Delay(1000);
                    IsBusy = false;
                }

                var xdoc = XDocument.Parse(text);

                foreach (var chapterss in xdoc.Root.Elements("Chapters"))
                foreach (var chapter in chapterss.Elements("Chapter"))
                {
                    var count = 0;
                    var chapterCaption = chapter.Attributes("Caption").First();
                    var newchapter = new Models(chapterCaption.Value);

                    foreach (var position in chapter.Elements("Position"))
                    {
                        var caption = position.Attributes("Caption").First();
                        var code = position.Attributes("Code").First();
                        var units = position.Attributes("Units").First();
                        count++;
                        var quantity = position.Elements("Quantity").First();
                        var quantityFx = quantity.Attributes("Fx").First();
                        var result = CalcExpression(quantityFx.Value);
                        var positions = new Position(count, code.Value, caption.Value, units.Value, result.ToString());
                        newchapter.Positions.Add(positions);


                        foreach (var resources in position.Elements("Resources"))
                        {
                            foreach (var tzm in resources.Elements("Tzm"))
                            {
                                var tzm1 = tzm.Attributes("Caption").First();
                                var tzmMchCode = tzm.Attributes("Code").First();
                                var tzmMchUnits = tzm.Attributes("Units").First();
                                var tzmMchQuantity = tzm.Attributes("Quantity").First();


                                positions.TzmMchs.Add(new TzmMch(tzmMchCode.Value, tzm1.Value, tzmMchUnits.Value,
                                    tzmMchQuantity.Value));
                            }

                            foreach (var mch in resources.Elements("Mch"))
                            {
                                var mch1 = mch.Attributes("Caption").First();
                                var tzmMchCode = mch.Attributes("Code").First();
                                var tzmMchUnits = mch.Attributes("Units").First();
                                var tzmMchQuantity = mch.Attributes("Quantity").First();
                                positions.TzmMchs.Add(new TzmMch(tzmMchCode.Value, mch1.Value, tzmMchUnits.Value,
                                    tzmMchQuantity.Value));
                            }
                        }
                    }

                    Chapters.Add(newchapter);
                }

                _isFileLoaded = true;
                ExportCommand.OnCanExecuteChanged();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void Export(object param)
        {
            try
            {
                IsBusy = true;
                await ExportAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task ExportAsync()
        {
            return Task.Run(() =>
            {
                var ex = new Application();
                ex.Visible = true;
                ex.SheetsInNewWorkbook = 1;
                var workBook = ex.Workbooks.Add(Type.Missing);
                ex.DisplayAlerts = false;
                var sheet = (Worksheet) ex.Worksheets.get_Item(1);
                sheet.Name = "XML";
                var i = 1; // Номер строки в exel 
                var c = 0; // индекс элементов первого уровня
                var k = 0; // индекс элементов второго уровня
                var m = 0; // индекс элементов третьего уровня
                while (c < Chapters.Count)
                {
                    sheet.Cells[i, 1] = Chapters[c].Name;
                    i++;

                    k = 0;
                    m = 0;
                    while (k < Chapters[c].Positions.Count)
                    {
                        sheet.Cells[i, 2] = Chapters[c].Positions[k].Number;
                        sheet.Cells[i, 3] = Chapters[c].Positions[k].Code;
                        sheet.Cells[i, 4] = Chapters[c].Positions[k].Name;
                        sheet.Cells[i, 5] = Chapters[c].Positions[k].Units;
                        sheet.Cells[i, 6] = Chapters[c].Positions[k].Quantity;

                        i++;
                        m = 0;
                        while (m < Chapters[c].Positions[k].TzmMchs.Count)
                        {
                            sheet.Cells[i, 7] = Chapters[c].Positions[k].TzmMchs[m].Code;
                            sheet.Cells[i, 8] = Chapters[c].Positions[k].TzmMchs[m].Name;
                            sheet.Cells[i, 9] = Chapters[c].Positions[k].TzmMchs[m].Quantity;
                            m++;
                            i++;
                        }

                        k++;
                    }

                    c++;
                }

                sheet.Columns.AutoFit();


                ex.Application.ActiveWorkbook.SaveAs("doc.xlsx", Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            });
        }

        private bool CanExport(object param)
        {
            return _isFileLoaded;
        }

        private decimal CalcExpression(string str)
        {
            str = str.Replace(",", ".");
            var result = Convert.ToDecimal(new DataTable().Compute(str, null));
            return decimal.Round(result, 3);
        }
    }
}