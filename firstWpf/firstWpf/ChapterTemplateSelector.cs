﻿using System.Windows;
using System.Windows.Controls;

namespace firstWpf
{
    public class ChapterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChapterTemplate { get; set; }
        public DataTemplate PositionTemplate { get; set; }
        public DataTemplate TzmMchTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case Models _:
                    return ChapterTemplate;
                case Position _:
                    return PositionTemplate;
                case TzmMch _:
                    return TzmMchTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}