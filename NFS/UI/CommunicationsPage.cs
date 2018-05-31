using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NE_UI
{
    public class CommunicationsPage : UI_Interface
    {
        /// <summary>
        /// Recieves data From the logic Engine
        /// </summary>
        public event System.EventHandler RecieveData;
        /// <summary>
        /// Sends data to the logic Engine
        /// </summary>
        public event System.EventHandler SendData;
    }
}