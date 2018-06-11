using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using firstWpf.Models;

namespace firstWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Chapters = new ObservableCollection<Chapter>();
            LoadCommand = new RelayCommand(Load);
        }

        public ICommand LoadCommand { get; }

        public ICommand ExportCommand { get; }

        public ObservableCollection<Chapter> Chapters { get; }

        private async void Load(object param)
        {
            string path = "../../LS.xml";
            string text;

            using (var reader = File.OpenText(path))
            {
                IsBusy = true;
                text = await reader.ReadToEndAsync();
                await Task.Delay(1000);
                IsBusy = false;
            }
            
            XDocument xdoc = XDocument.Parse(text);

            foreach (XElement chapterss in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement Chapter in chapterss.Elements("Chapter"))
                {
                    int Count = 0;
                    XAttribute chapter1 = Chapter.Attributes("Caption").First();
                    //Chapters newChapter;
                    //Chapter0.Add(newChapter = new Chapters() { Caption = Chapter1.Value.ToString() });
                    var chapter = new Chapter(chapter1.Value);

                    foreach (XElement position in Chapter.Elements("Position"))
                    {
                        XAttribute caption = position.Attributes("Caption").First();
                        XAttribute code = position.Attributes("Code").First();
                        XAttribute units = position.Attributes("Units").First();
                        Count++;
                        XElement quantity = position.Elements("Quantity").First();
                        XAttribute quantity1 = quantity.Attributes("Fx").First();
                        string str = quantity1.Value.ToString();
                        str = str.Replace(",", ".");
                        var result = new DataTable().Compute(str, null);

                        var positions = new Position(Count, code.Value, caption.Value, units.Value, result.ToString());
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
    }
}
