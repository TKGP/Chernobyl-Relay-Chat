using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chernobyl_Relay_Chat
{
    class CRCGameWrapper : ICRCSendable
    {
        public void AddError(string message)
        {
            CRCGame.AddError(message);
        }

        public void AddInformation(string message)
        {
            CRCGame.AddInformation(message);
        }
    }
}
