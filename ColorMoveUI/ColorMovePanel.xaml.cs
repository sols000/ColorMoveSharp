using System;
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
            m_HistoryStep.Clear();
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

        bool bAutoRun = false;

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            RandomStep();
        }

        //随机步骤
        private void RandomStep()
        {
            int FormC = GRandom.Next(0, 5);
            int FormT = GRandom.Next(0, 5);
            int Count = GRandom.Next(1, 3);
            MoveTo(FormC, FormT, Count);
            UpdateView();
        }

        //自动随机步骤
        private void AutoRandomStep()
        {
            bAutoRun = true;
            Task.Run(new Action(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    Thread.Sleep(10);
                    Dispatcher.Invoke(RandomStep);
                    if (bAutoRun == false)
                    {
                        return;
                    }
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

        private bool MoveTo(Step step, bool record = true)
        {
            bool Res = MoveTo(step.ColumnFrom,step.ColumnTo,step.Number);
            if(Res && record)
            {
                m_HistoryStep.Add(step);
            }
            return Res;
        }

        private void PulseBtn_Click(object sender, RoutedEventArgs e)
        {
            bAutoRun = false;
        }

        private void NextStepBtn_Click(object sender, RoutedEventArgs e)
        {
            Step();
        }

        private void PreStepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (m_HistoryStep.Count <= 0)
            {
                return;
            }
            Step Temp = m_HistoryStep[m_HistoryStep.Count-1];
            m_HistoryStep.Remove(Temp);
            //m_HistoryStep.RemoveAt(m_HistoryStep.Count-1);
            Temp.inverse();
            MoveTo(Temp,false);
        }

        //获取顶点颜色值
        int GetTopColor(int column)
        {
            int Count = GetColumnCount(column);
            if(Count == 0)
            {
                return column+1;
            }
            else
            {
                return (int)CSet.Rows[Count-1][column];
            }
        }

        Step GetLestMoveTo(int Column)
        {
            int TopColor = GetTopColor(Column);
            int ColumnCount = GetColumnCount(Column);
            Step Res = new Step
            {
                ColumnFrom = -1,
                ColumnTo = Column,
                Score = 0
            };
            int MinMove = 8;
            for (int i=0; i< NCol;i++)
            {
                if(i==Column)
                {
                    continue;
                }
                int CountNeedMove = 0;
                for(int j= NRow-1 ; j>=0;j--)
                {
                    if((int)CSet.Rows[j][i] == 0)
                    {
                        continue;
                    }else if((int)CSet.Rows[j][i] == TopColor)
                    {
                        for (int k = j; k >= 0; k--)
                        {
                            if ((int)CSet.Rows[k][i] == TopColor)
                            {
                                CountNeedMove++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if( ColumnCount + CountNeedMove > NRow )
                        {
                            break;
                        }
                        //固定对应色，
                        if( i+1 == TopColor && CountNeedMove == GetColumnCount(i))
                        {
                            break;
                        }

                        if (MinMove > CountNeedMove)
                        {
                            MinMove = CountNeedMove;
                            Res.ColumnFrom = i;
                            Res.Number = MinMove;
                        }
                        break;
                    }
                    else
                    {
                        CountNeedMove++;
                    }
                }
            }
            return Res;
        }

        private bool IsGameSuccess()
        {
            for(int i= 0;i<5 ;i++)
            {
                for(int j=0;j<5 ;j++)
                {
                    if ((int)CSet.Rows[i][j] != j+1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        ///////////////////算法部分/////////////////////////
        Step AnOtherBreakStep = new Step();
        private void Step()
        {
            if (AnOtherBreakStep.isValid())
            {
                MoveTo(AnOtherBreakStep);
                AnOtherBreakStep.Reset();
                Console.WriteLine("额外打破");
            }

            Step TempStep = ScanStepList();
            if(TempStep.ColumnFrom == -1 || TempStep.ColumnTo == -1)
            {//移不动了呀
                //判断是否已经完成
                if(IsGameSuccess())
                {
                    Console.WriteLine("游戏成功了呀");
                }
                else
                {
                    Console.WriteLine("移不动了呀，进入解锁过程");
                    BreakStep();
                }
                //未完成则进入解锁阶段
            }
            else
            {
                MoveTo(TempStep);
            }
            Console.WriteLine("================");
        }

        //判断某一行是否完全正确
        private bool IsPerfect(int column)
        {
            for(int i=0; i<5;i++)
            {
                if((int)CSet.Rows[i][column] != column + 1)
                {
                    return false;
                }
            }
            for (int i = 5; i < NRow; i++)
            {
                if ((int)CSet.Rows[i][column] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        bool CanBreak(int column)
        {
            for (int i = 0; i < NRow; i++)
            {
                if((int)CSet.Rows[i][column] == 0)
                {
                    return false;
                }
                else if ((int)CSet.Rows[i][column] != column + 1)
                {
                    return true;
                }

            }
            return false;
        }

        bool IsSingleColor(int column)
        {
            int ResColor = (int)CSet.Rows[0][column];
            for (int i = 1; i < NRow; i++)
            {
                if ((int)CSet.Rows[i][column] == 0)
                {
                    return true;
                }
                else if ((int)CSet.Rows[i][column] != ResColor)
                {
                    return false;
                }

            }
            return true;
        }

        int GetSapceCount(int column)
        {
            int count = 0;
            for (int i = NRow-1; i >=0 ; i--)
            {
                if ((int)CSet.Rows[i][column] == 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        private int TopColorCount(int column)
        {
            int TopColor = GetTopColor(column);
            int Count = 0;
            for (int i = NRow - 1; i >= 0; i--)
            {
                if ((int)CSet.Rows[i][column] == TopColor)
                {
                    Count++;
                }else if((int)CSet.Rows[i][column] == 0)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return Count;
        }

        //获取指定列的BreakStats
        BreakState GetBreakStats(int column)
        {
            BreakState res = new BreakState();
            res.Column = column;
            res.isPerFect = IsPerfect(column);
            res.canBreak = CanBreak(column);
            res.EmptySpace = GetSapceCount(column);
            res.isSingleColor = IsSingleColor(column);
            res.TopColorCount = TopColorCount(column);

            return res;
        }

        BreakState[] breakStats = new BreakState[NCol];
        //这里可能会设置 AnOtherBreakStep 下一步打破步骤，避免回测
        private void BreakStep()
        {
            //遍历容器，设置breakStats
            for(int i = 0; i<NCol;i++)
            {
                breakStats[i] = GetBreakStats(i);
            }
            Console.WriteLine("初始化打破死锁状态完毕");
            //查找需要打破的列数，
            int BreakColumn = GetBreakColumn();

            Console.WriteLine("很麻烦呀"+ BreakColumn);

        }

        //获取尝试打破的列数
        private int GetBreakColumn()
        {
            //尝试找最小单色可破列
            int Column = -1;
            List<BreakState> TempList = new List<BreakState>();
            for (int i = 0; i < NCol; i++)
            {
                if (breakStats[i].canBreak && breakStats[i].isSingleColor)
                {
                    TempList.Add(breakStats[i]);
                }
            }
            if(TempList.Count==1)
            {
                return TempList[0].Column;
            }
            else if(TempList.Count > 1)
            {
                return TempList.Max().Column;
            }
            //
            //尝试打破最
            List<BreakState> TempNoSingle = new List<BreakState>();
            int TempTopColorCount = 6;

            for (int i =0; i<NCol;i++)
            {
                if (breakStats[i].canBreak && breakStats[i].TopColorCount< TempTopColorCount)
                {
                    TempTopColorCount = (int)breakStats[i].TopColorCount;
                    Column = i;
                }
            }
            return Column;
        }

        //历史记录
        List<Step> m_HistoryStep = new List<Step>();
        //找到一个步骤
        private Step ScanStepList()
        {
            Step MinStep = new Step();
            MinStep.Number = NRow;
            //目标地址为i的最佳步骤
            for (int i=0;i< NCol ;i++)
            {
                //查找该颜色的非锁定最高位置
                Step Temp = GetLestMoveTo(i);
                if(Temp.ColumnFrom != -1 && MinStep.Number > Temp.Number)
                {
                    MinStep = Temp;
                }
                Console.WriteLine(Temp);
            }
            return MinStep;
        }


    }
}
