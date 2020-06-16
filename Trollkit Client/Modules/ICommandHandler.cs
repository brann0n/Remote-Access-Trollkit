using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules
{
    interface ICommandHandler
    {
        bool HandleCommand(Socket s, TransferCommandObject obj);
    }
}
