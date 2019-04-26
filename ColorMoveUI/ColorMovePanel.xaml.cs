﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class Pos
    {
        public int row;
        public int col;
        public override string ToString()
        {
            return "(" + row + "," + col + ")";
        }
    }


    public partial class ColorMovePanel : UserControl
    {
        const int NCol = 5;
        const int NRow = 8;

        const string strRed = "红色";
        const string strOri = "橙色";
        const string strYel = "黄色";
        const string strGre = "绿色";
        const string strBlu = "蓝色";

        DataTable CSet = new DataTable();
        public ColorMovePanel()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            //DataColumn Column1 = new DataColumn("红色", typeof(Int32));
            CSet.Columns.Add(strRed, typeof(int));
            CSet.Columns.Add(strOri, typeof(int));
            CSet.Columns.Add(strYel, typeof(int));
            CSet.Columns.Add(strGre, typeof(int));
            CSet.Columns.Add(strBlu, typeof(int));

            ReSetColorData();

        }

        //重置数据
        public void ReSetColorData()
        {
            CSet.Rows.Clear();
            for (int i = 0; i < 5; i++)
            {
                DataRow row = CSet.NewRow();
                row.ItemArray = new object[NCol] { 1, 2, 3, 4, 5 };
                CSet.Rows.Add(row);
            }
            for (int i = 0; i < 3; i++)
            {
                DataRow row = CSet.NewRow();
                row.ItemArray = new object[NCol] { 0, 0, 0, 0, 0 };
                CSet.Rows.Add(row);
            }
            UpdateView();
        }

        public void UpdateView()
        {
            ColorDataGrid.ItemsSource = null;
            ColorDataGrid.ItemsSource = CSet.DefaultView;
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(new Action(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(1000);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        int FormC = GRandom.Next(0, 5);
                        int FormT = GRandom.Next(0, 5);
                        int Count = GRandom.Next(1, 4);
                        MoveTo(FormC, FormT, Count);
                        UpdateView();
                    }
                    ));
                }
            }));
        }

        private void InitBtn_Click(object sender, RoutedEventArgs e)
        {
            ReSetColorData();
        }

        
        Random GRandom = new Random();

        //获取随机单元格
        private Pos GetRandomPoint(bool CheckValid = false)
        {
            //返回一个有效点
            Pos res =new Pos();
            do
            {
                res.col = GRandom.Next(0, 5);
                res.row = GRandom.Next(0, 8);
                if (CheckValid)
                {
                    if ( (int)CSet.Rows[res.row][res.col] != 0 )
                    {
                        CheckValid = false;
                    }
                }
                
            }
            while (CheckValid);
            return res;
        }

        //交换两个单元格的值
        public void SwapValue(Pos Pos1,Pos Pos2)
        {
            int temp = (int)CSet.Rows[Pos1.row][Pos1.col];
            CSet.Rows[Pos1.row][Pos1.col] = CSet.Rows[Pos2.row][Pos2.col];
            CSet.Rows[Pos2.row][Pos2.col] = temp;
        }

        //移动一个到空位置
        public bool MoveToEmptyPos(Pos PosFrom, Pos PosTo)
        {
            if((int)CSet.Rows[PosTo.row][PosTo.col] != 0)
            {
                Console.WriteLine("目标位置不为空");
                return false;
            }
            SwapValue(PosFrom, PosTo);
            return false;
        }



        //打乱一下
        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0; i<100;i++)
            {
                Pos tempPos1 = GetRandomPoint(true);
                Pos tempPos2 = GetRandomPoint(true);
                SwapValue(tempPos1, tempPos2);
            }
            UpdateView();
        }

        //获取某个柱子的方块个数
        private int GetColumnCount(int Colum)
        {
            for(int i=0; i< NRow; i++)
            {
                if((int)CSet.Rows[i][Colum]==0)
                {
                    return i;
                }
            }
            return NRow;
        }

        //移动多个方块到空位置
        private bool MoveTo(int ColumFrom,int ColumTo,int Count)
        {
            if(ColumFrom == ColumTo)
            {
                Console.WriteLine("不能移动到自己柱子上");
                return false;
            }
            int FromCount = GetColumnCount(ColumFrom);
            int ToCount = GetColumnCount(ColumTo);
            if(FromCount < Count)
            {//需要移动的方块不足
                Console.WriteLine("没有那么多方块可以移动");
                return false;
            }
            int AllCount = GetColumnCount(ColumTo) + Count;
            if(AllCount > NRow)
            {//超出上限
                Console.WriteLine("剩余空间不足");
                return false;
            }
            Pos posFrom = new Pos();
            Pos posTo = new Pos();
            posFrom.col = ColumFrom;
            posTo.col = ColumTo;
            for (int i=0; i<Count;i++)
            {
                posFrom.row = FromCount - Count + i;
                posTo.row = ToCount + i;
                MoveToEmptyPos(posFrom, posTo);
            }
            UpdateView();
            return true;
        }

    }
}
