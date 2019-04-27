using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMoveUI
{
    public class Step
    {
        public int ColumnFrom = -1;
        public int ColumnTo = -1;
        public int Number = 0;
        //计分原则：1 移动后连接的个数 + 相连的个数分 10 * 连接个数，
        //          2 冻结方块的冻结分 + 50分
        //          3 目标列剩余空格数 + 1*空格数
        public int Score = 0;
        public override string ToString()
        {
            return "F:" + ColumnFrom + " T:" + ColumnTo+" Num:"+Number;
        }
        public void inverse()
        {
            int Temp = ColumnFrom;
            ColumnFrom = ColumnTo;
            ColumnTo = Temp;
        }

        public bool isValid()
        {
            if(ColumnFrom >=0 && ColumnTo>=0 && Number>0)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            ColumnFrom = -1;
            ColumnTo = -1;
            Number = 0;
        }
    }


    public class BreakState: IComparable<BreakState>
    {
        public int Column;
        public bool canBreak;
        public bool isPerFect;
        public bool isSingleColor;
        public int EmptySpace;
        public int TopColorCount;

        public int CompareTo(BreakState other)
        {
            return EmptySpace.CompareTo(other.EmptySpace);
        }
    }

}
