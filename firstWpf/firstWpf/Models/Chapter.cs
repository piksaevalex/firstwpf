using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            

            foreach (XElement chapterss in xdoc.Root.Elements("Chapters"))
            {
                foreach (XElement chapter in chapterss.Elements("Chapter"))
                {
                    int Count = 0;
                    XAttribute chapter1 = chapter.Attributes("Caption").First();
                    //Chapters newChapter;
                    //Chapter0.Add(newChapter = new Chapters() { Caption = Chapter1.Value.ToString() });

                    foreach (XElement position in chapter.Elements("Position"))
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
                        //var result = (double)new DataTable().Compute(quantity1.Value.ToString(), null);
                        //Position0.Add(new Positions(Count, Code.Value.ToString(), Caption.Value.ToString(), units.Value.ToString(), Quantity1.Value.ToString()));




                        foreach (XElement resources in position.Elements("Resources"))
                        {
                            foreach (XElement tzm in resources.Elements("Tzm"))
                            {
                                XAttribute tzm1 = tzm.Attributes("Caption").First();
                                XAttribute tzmMchCode = tzm.Attributes("Code").First();
                                XAttribute tzmMchQuantity = tzm.Attributes("Quantity").First();
                                
                                //TzmMch0.Add(new TzmMch() { Caption = Tzm1.Value.ToString() });
                                this.Add(new Chapter()
                                {
                                    Chaptername = chapter1.Value.ToString(),
                                    PositionNumber = Count,
                                    PositionCode = code.Value.ToString(),
                                    Positionname = caption.Value.ToString(),
                                    PositionUnits = units.Value.ToString(),
                                    PositionQuantity = result.ToString(),

                                    TzmMchname = tzm1.Value.ToString(),
                                    TzmMchCode = tzmMchCode.Value.ToString(),
                                    TzmMchQuantity = tzmMchQuantity.Value.ToString()
                                });
                            }
                            foreach (XElement mch in resources.Elements("Mch"))
                            {
                                XAttribute mch1 = mch.Attributes("Caption").First();
                                XAttribute tzmMchCode = mch.Attributes("Code").First();
                                XAttribute tzmMchQuantity = mch.Attributes("Quantity").First();                             
                                //TzmMch0.Add(new TzmMch() { Caption = Mch1.Value.ToString() });
                                this.Add(new Chapter()
                                {
                                    Chaptername = chapter1.Value.ToString(),
                                    PositionNumber = Count,
                                    PositionCode = code.Value.ToString(),
                                    Positionname = caption.Value.ToString(),
                                    PositionUnits = units.Value.ToString(),
                                    PositionQuantity = result.ToString(),

                                    TzmMchname = mch1.Value.ToString(),
                                    TzmMchCode = tzmMchCode.Value.ToString(),
                                    TzmMchQuantity = tzmMchQuantity.Value.ToString()
                                });
                            }

                        }

                    }


                }
            }
        }
    }
}
