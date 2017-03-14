using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chernobyl_Relay_Chat
{
    interface ICRCSendable
    {
        void AddInformation(string message);
        void AddError(string message);
    }
}
