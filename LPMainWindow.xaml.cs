using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Autodesk.Revit.DB;

namespace LinePatternMacro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LPMainWindow : Window
    {
        public System.Collections.ObjectModel.ObservableCollection<LinePattern> TheCollection { get { return data; } }
        System.Collections.ObjectModel.ObservableCollection<LinePattern> data = null;

        public LPMainWindow(System.Collections.ObjectModel.ObservableCollection<LinePattern> _data)
        {
            InitializeComponent();
            data = _data;
        }
    }
}
