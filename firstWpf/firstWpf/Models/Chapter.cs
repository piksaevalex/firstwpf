using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace firstWpf.Models
{
    public class Chapter
    {
        public string Chaptername { get; set; }

        public int PositionNumber { get; set; }
        public string PositionCode { get; set; }
        public string Positionname { get; set; }
        public string PositionUnits { get; set; }
        public string PositionQuantity { get; set; }

        public string TzmMchCode { get; set; }
        public string TzmMchname { get; set; }
        public string TzmMchQuantity { get; set; }
    }
    public class Chapters: ObservableCollection<Chapter>
    {
        public Chapters()
        {
            string path = "../../LS.xml";
            string text = File.ReadAllText(path, Encoding.GetEncoding("windows-1251"));
            XDocument xdoc = XDocument.Parse(text);
            

            foreach (XElement Chapterss in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement Chapter in Chapterss.Elements("Chapter"))
                {
                    int Count = 0;
                    XAttribute Chapter1 = Chapter.Attributes("Caption").First();
                    //Chapters newChapter;
                    //Chapter0.Add(newChapter = new Chapters() { Caption = Chapter1.Value.ToString() });

                    foreach (XElement Position in Chapter.Elements("Position"))
                    {
                        XAttribute Caption = Position.Attributes("Caption").First();
                        XAttribute Code = Position.Attributes("Code").First();
                        XAttribute units = Position.Attributes("Units").First();
                        Count++;
                        XElement Quantity = Position.Elements("Quantity").First();
                        XAttribute Quantity1 = Quantity.Attributes("Fx").First();
                        //Position0.Add(new Positions(Count, Code.Value.ToString(), Caption.Value.ToString(), units.Value.ToString(), Quantity1.Value.ToString()));




                        foreach (XElement Resources in Position.Elements("Resources"))
                        {
                            foreach (XElement Tzm in Resources.Elements("Tzm"))
                            {
                                XAttribute Tzm1 = Tzm.Attributes("Caption").First();
                                //TzmMch0.Add(new TzmMch() { Caption = Tzm1.Value.ToString() });
                                this.Add(new Chapter()
                                {
                                    Chaptername = Chapter1.Value.ToString(),
                                    PositionNumber = Count,
                                    PositionCode = Code.Value.ToString(),
                                    Positionname = Caption.Value.ToString(),
                                    PositionUnits = units.Value.ToString(),
                                    PositionQuantity = Quantity1.Value.ToString(),

                                    TzmMchname = Tzm1.Value.ToString()
                                });
                            }
                            foreach (XElement Mch in Resources.Elements("Mch"))
                            {
                                XAttribute Mch1 = Mch.Attributes("Caption").First();
                                //TzmMch0.Add(new TzmMch() { Caption = Mch1.Value.ToString() });
                                this.Add(new Chapter()
                                {
                                    Chaptername = Chapter1.Value.ToString(),
                                    PositionNumber = Count,
                                    PositionCode = Code.Value.ToString(),
                                    Positionname = Caption.Value.ToString(),
                                    PositionUnits = units.Value.ToString(),
                                    PositionQuantity = Quantity1.Value.ToString(),

                                    TzmMchname = Mch1.Value.ToString()
                                });
                            }

                        }

                    }


                }
            }
        }
    }
}
