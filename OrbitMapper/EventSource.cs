using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OrbitMapper
{
    public static class EventSource
    {
        //Event handling method definitions for when an event is triggered
        public static event TextChanged textChanged;
        public static event Tessellate tessellate;
        public static event FinishedDrawTess finishedTessellate;
        public static event FinishedDrawShape finishedShape;
        public static event RemoveTab tabRemove;
        private static string console = "";

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

        public static void finishedDrawTess(int bounces){
            finishedTessellate(bounces, new Events("Draw finished with " + bounces + " bounces."));
        }

        public static void finishedDrawShape()
        {
            finishedShape("", new Events("Collision detection and Shape draw has finished."));
        }

        public static void removeTab(string name)
        {
            tabRemove(name, new Events("Tab " + name + " was removed."));
        }

        public static string getText()
        {
            return console;
        }
    }
}
