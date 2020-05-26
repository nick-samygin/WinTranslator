using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.BusinessLayer
{
    public class BussinessLayerMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public BussinessLayerMessageEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
