using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrbitMapper.Tessellations;


namespace OrbitMapper
{
    /// <summary>
    /// Contains events to subscribe to.
    /// </summary>
    public static class EventSource
    {
        /// <summary>
        /// Subscribe to this when you want to know when text has been logged
        /// </summary>
        public static event TextChanged textChanged;
        /// <summary>
        /// Subscribe to this when you want to have some custom behavior prior to the Undefined being filled in. (Could have done this just by ordering the subscribers correctly)
        /// </summary>
        public static event Tessellate tessellate;
        /// <summary>
        /// Subscribe to this when you want to do something after the collision simulation finishes from tessellation (like update the bounces text)
        /// </summary>
        public static event FinishedDrawTess finishedTessellate;
        /// <summary>
        /// Subscribe to this when you want know when the EVERYTHING has finished from either inputs (to account for undefined behavior)
        /// </summary>
        public static event FinishedDrawShape finishedShape;
        /// <summary>
        /// Subscribe to this when you want to have custom behavior when a tab is determined to be removed. (like actually removing it)
        /// </summary>
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
