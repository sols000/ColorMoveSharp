using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMoveUI
{
    class Step
    {
        Step Parent;
        List<Step> NextSteps;
        int ColumnFrom;
        int ColumnTo;
        int Number;
        int Score;
    }
}
