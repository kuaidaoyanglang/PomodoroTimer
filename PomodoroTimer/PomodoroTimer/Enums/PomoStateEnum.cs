using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    /// <summary>
    /// 番茄时钟状态枚举
    /// </summary>
    public enum PomoStateEnum
    {
        Init,
        Working,
        WorkDone,
        ShortResting,
        LongResting,
        RestDone
    }
}
