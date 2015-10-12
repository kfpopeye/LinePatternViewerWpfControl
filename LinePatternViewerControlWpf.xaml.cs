using System;
using System.ComponentModel;
using System.Windows;
using WShapes = System.Windows.Shapes;
using System.Windows.Media;
using Autodesk.Revit.DB;

namespace LinePatternViewer
{
    /// <summary>
    /// Interaction logic for LinePatternViewerControlWpf.xaml
    /// </summary>
    public partial class LinePatternViewerControlWpf : INotifyPropertyChanged
    {
        #region LinePattern DependencyProperty
        public static readonly DependencyProperty
            LinePatternProperty = DependencyProperty
            .RegisterAttached("LinePattern",
                              typeof(LinePattern),
                              typeof(LinePatternViewerControlWpf),
                              new UIPropertyMetadata(null, OnLinePatternChanged));


        private static void OnLinePatternChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var linePatternViewerControl = d as LinePatternViewerControlWpf;
            if (linePatternViewerControl == null) return;
            linePatternViewerControl.OnPropertyChanged("LinePattern");
            linePatternViewerControl.CreateLinePatternOnCanvas();
        }

        public LinePattern LinePattern
        {
            get
            {
                return (LinePattern)GetValue(LinePatternProperty);
            }
            set
            {
                SetValue(LinePatternProperty, value);
            }
        }

        public LinePattern GetLinePattern(
            DependencyObject obj)
        {
            return (LinePattern)obj.GetValue(
                LinePatternProperty);
        }

        public void SetLinePattern(
            DependencyObject obj,
            LinePattern value)
        {
            obj.SetValue(LinePatternProperty, value);
        }
        #endregion

        public System.Collections.Generic.List<string> colorCheckList = null;

        public LinePatternViewerControlWpf()
        {
            InitializeComponent();
            this.Foreground = System.Windows.Media.Brushes.Black;
            theCanvas.Background = this.Background;
        }

        public event PropertyChangedEventHandler
            PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this,
                        new PropertyChangedEventArgs(propertyName));
        }

        private void CreateLinePatternOnCanvas()
        {
            var width =
                (ActualWidth == 0 ? Width : ActualWidth) == 0
                ? 100
                : (ActualWidth == 0 ? Width : ActualWidth);

            if (double.IsNaN(width))
                width = 100;

            var height =
                (ActualHeight == 0 ? Height : ActualHeight) == 0
                ? 30
                : (ActualHeight == 0 ? Height : ActualHeight);

            if (double.IsNaN(height))
                height = 30;

            System.Collections.Generic.IList<LinePatternSegment> segments = LinePattern.GetSegments();
            if (segments.Count > 0)
            {
                double x1 = 0, x2 = 0;
                while ((x2 * 96 * 12) <= width)
                {
                    foreach (LinePatternSegment lps in segments)
                    {
                        x2 += lps.Length;
                        WShapes.Line l = new WShapes.Line();
                        l.StrokeThickness = 3d;
                        l.StrokeEndLineCap = PenLineCap.Square;
                        l.StrokeStartLineCap = PenLineCap.Square;
                        l.X1 = x1 * 96 * 12; // 96px per inch X 12 inches per foot
                        l.Y1 = height / 2;
                        l.X2 = x2 * 96 * 12;
                        l.Y2 = height / 2;
                        switch (lps.Type)
                        {
                            case LinePatternSegmentType.Dash:
                                l.Stroke = Foreground;
                                break;
                            case LinePatternSegmentType.Dot:
                                l.StrokeThickness = 2.5d;
                                l.StrokeEndLineCap = PenLineCap.Round;
                                l.StrokeStartLineCap = PenLineCap.Round;
                                l.Stroke = Foreground;
                                break;
                            case LinePatternSegmentType.Space:
                                l.Stroke = System.Windows.Media.Brushes.Transparent;
                                break;
                            default:
                                throw new ArgumentException("Invalid segment type");
                        }
                        theCanvas.Children.Add(l);
                        x1 += lps.Length;
                    }
                }
            }
            else
            {
                //solid line
                WShapes.Line l = new WShapes.Line();
                l.StrokeThickness = 5;
                l.StrokeEndLineCap = PenLineCap.Square;
                l.StrokeStartLineCap = PenLineCap.Square;
                l.X1 = 0 * 96 * 12; // 96px per inch X 12 inches per foot
                l.Y1 = height / 2;
                l.X2 = width;
                l.Y2 = height / 2;
                l.Stroke = System.Windows.Media.Brushes.Black;
                theCanvas.Children.Add(l);
            }
        }

        //OnPropertyChanged("LinePatternImage");
    }
}
