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

        /// <summary>
        /// This method is to trigger output to the debug console string. This is what appears when a crash happens + exception information tagged on
        /// Triggers the methods subscribed to the textChanged event
        /// </summary>
        /// <param name="output"></param>
        public static void output(string output)
        {
            console += output + Environment.NewLine;
            if(textChanged != null){
                textChanged(new object(), new Events("New text was entered to the debug console."));
            }
        }

        /// <summary>
        /// Triggers the methods subscribed to the tessellate event
        /// </summary>
        /// <param name="newTess"></param>
        public static void updateTess(Tessellation newTess){
            tessellate(newTess, new Events("Tessellation parameters have been changed."));
        }

        /// <summary>
        /// Triggers the methods subscribed to the finishedTessellate event
        /// </summary>
        /// <param name="bounces"></param>
        public static void finishedDrawTess(int bounces){
            finishedTessellate(bounces, new Events("Draw finished with " + bounces + " bounces."));
        }

        /// <summary>
        /// Triggers the methods subscribed to the finishedShape event
        /// </summary>
        public static void finishedDrawShape()
        {
            finishedShape("", new Events("Collision detection and Shape draw has finished."));
        }

        /// <summary>
        /// Triggers the methods subscribed to the tabRemove event
        /// </summary>
        /// <param name="name"></param>
        public static void removeTab(string name)
        {
            tabRemove(name, new Events("Tab " + name + " was removed."));
        }

        /// <summary>
        /// This is used when there is a crash to get the console information before the crash.
        /// </summary>
        /// <returns></returns>
        public static string getText()
        {
            return console;
        }
    }
}
