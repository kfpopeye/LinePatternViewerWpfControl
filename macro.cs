using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LinePatternMacro
{
	[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
	[Autodesk.Revit.DB.Macros.AddInId("E06D5119-8BFF-4870-B19D-B546347DBD47")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		
		public void LinePatternViewer()
		{
			LPMainWindow main_win = null;
			try
            {
                Document theDoc = this.ActiveUIDocument.Document;
                System.Collections.ObjectModel.ObservableCollection<LinePattern> data = 
                	new System.Collections.ObjectModel.ObservableCollection<LinePattern>();

                //Collect all line pattern elements
                FilteredElementCollector collector = new FilteredElementCollector(theDoc);
                IList<Element> linepatternelements = collector.WherePasses(new ElementClassFilter(typeof(LinePatternElement))).ToElements();
                foreach (LinePatternElement lpe in linepatternelements)
                {
                    data.Add(lpe.GetLinePattern());
                }
                //start main window
                main_win = new LinePatternMacro.LPMainWindow(data);
                System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(main_win);
                x.Owner = Process.GetCurrentProcess().MainWindowHandle;
                main_win.ShowDialog();
            }
            catch (Exception err)
            {
                Debug.WriteLine(new string('*', 100));
                Debug.WriteLine(err.ToString());
                Debug.WriteLine(new string('*', 100));
                if (main_win != null && main_win.IsActive)
                    main_win.Close();
            }
		}
	}
}
