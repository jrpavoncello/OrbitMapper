using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitMapper
{
    /// <summary>
    /// Used for logging
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void TextChanged(object source, Events e);
    /// <summary>
    /// Used for updating the fields
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void Tessellate(object source, Events e);
    /// <summary>
    /// Used to update bounces
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void FinishedDrawTess(object source, Events e);
    /// <summary>
    /// Used to determine whether undefined behavior has occurred
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void FinishedDrawShape(object source, Events e);
    /// <summary>
    /// Used when right clicking a tab and selecting Remove
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void RemoveTab(object source, Events e);

    /// <summary>
    /// Entry point if we wanted to do something custom with the events
    /// </summary>
    public class Events : EventArgs
    {
        private string EventInfo;
        /// <summary>
        /// Grabs base Event text, does nothing special
        /// </summary>
        /// <param name="Text"></param>
        public Events(string Text)
        {
            EventInfo = Text;
        }
        /// <summary>
        /// Grabs base Event info, does nothing special
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            return EventInfo;
        }
    }
}