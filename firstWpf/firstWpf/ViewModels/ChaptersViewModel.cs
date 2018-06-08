using System;
using firstWpf.Models;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace firstWpf.ViewModels
{
    class ChaptersViewModel
    {
        public ICollectionView ChaptersView { set; get; }
        public ChaptersViewModel()
        {
            IList<Chapter> chapter = new Chapters();
            ChaptersView = CollectionViewSource.GetDefaultView(chapter);
            ChaptersView.GroupDescriptions.Add(new PropertyGroupDescription("Chaptername"));
            ChaptersView.GroupDescriptions.Add(new PropertyGroupDescription("PositionNumber"));

        }

    }
}
