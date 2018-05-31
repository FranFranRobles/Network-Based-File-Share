using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NE_UI
{
    public class HomePage : UI_Interface
    {
        /// <summary>
        /// Tells Logic Engine To Create A Server
        /// </summary>
        public event System.EventHandler CreateServer;
        /// <summary>
        /// Tells The Logic Engine To Turn Off the Server
        /// </summary>
        public event System.EventHandler TurnOffServer;
    }
}