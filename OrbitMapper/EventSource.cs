﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitMapper
{
    public static class EventSource
    {
        public static event TextChanged textChanged;
        public static event Tessellate tessellate;
        public static event FinishedDraw finishedTessellate;
        public static event RemoveTab tabRemove;
        private static string console = "";
        private static bool newText = false;

        public static void output(string output)
        {
            console += output + Environment.NewLine;
            if(textChanged != null){
                textChanged(new object(), new Events("New text was entered to the debug console."));
            }
        }
        public static void updateTess(Tessellation newTess){
            tessellate(newTess, new Events("Tessellation parameters have been changed."));
        }

        public static void finishedDraw(int bounces){
            finishedTessellate(bounces, new Events("Draw finished with " + bounces + " bounces."));
        }

        public static void removeTab(string name)
        {
            tabRemove(name, new Events("Tab " + name + " was removed."));
        }

        public static string getText()
        {
            newText = false;
            return console;
        }
    }
}
