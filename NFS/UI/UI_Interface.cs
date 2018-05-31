using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NE_UI
{
    public interface UI_Interface
    {
        /// <summary>
        /// Tells the Logic Engine To Connect To An Exsiting Server
        /// </summary>
        event System.EventHandler ConnectToServer;
        /// <summary>
        /// Tells the Logic Engine To Connect To Another Existing User
        /// </summary>
        event System.EventHandler ConnectToUser;
        /// <summary>
        /// Tells The Logic engine To Disconnect From The connected user
        /// </summary>
        event System.EventHandler DisconectFromUser;
    }
}