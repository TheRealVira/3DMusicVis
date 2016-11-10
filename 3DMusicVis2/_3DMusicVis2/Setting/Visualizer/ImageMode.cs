using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DMusicVis2.Setting.Visualizer
{
    [Flags]
    enum ImageMode
    {
        None = 0,
        Vibrate = 1,
        Rotate = (1<<1),
        ReverseOnBeat = (1<<2)
    }
}
