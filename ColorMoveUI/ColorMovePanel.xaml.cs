using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorMoveUI
{
    /// <summary>
    /// ColorMovePanel.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class ColorMovePanel : UserControl
    {
        const int NCol = 5;
        const int NRow = 8;
        int[] ListData1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataTable ColorTable = new DataTable();
        public ColorMovePanel()
        {
            InitializeComponent();
            InitData();
            ColorDataGrid.DataContext = ColorTable;
        }

        private void InitData()
        {
            //DataColumn Column1 = new DataColumn("红色", typeof(Int32));
            ColorTable.Columns.Add("红色");
            ColorTable.Columns.Add("橙色");
            ColorTable.Columns.Add("黄色");
            ColorTable.Columns.Add("绿色");
            ColorTable.Columns.Add("蓝色");
            ColorTable.Rows.Add(new int[NCol] { 1, 2, 3, 4, 5 });
            ColorTable.Rows.Add(new int[NCol] { 1, 2, 3, 4, 5 });
            ColorTable.Rows.Add(new int[NCol] { 1, 2, 3, 4, 5 });
            ColorTable.Rows.Add(new int[NCol] { 1, 2, 3, 4, 5 });
            ColorTable.Rows.Add(new int[NCol] { 1, 2, 3, 4, 5 });
            ColorTable.Rows.Add(new int[NCol] { 0, 0, 0, 0, 0 });
            ColorTable.Rows.Add(new int[NCol] { 0, 0, 0, 0, 0 });
            ColorTable.Rows.Add(new int[NCol] { 0, 0, 0, 0, 0 });
        }
    }
}
