using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitMapper
{
    public delegate void TextChanged(object source, Events e);
    public delegate void Tessellate(object source, Events e);
    public delegate void FinishedDraw(object source, Events e);
    public delegate void RemoveTab(object source, Events e);

    public class Events : EventArgs
    {
        private string EventInfo;
        public Events(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }
    }
}