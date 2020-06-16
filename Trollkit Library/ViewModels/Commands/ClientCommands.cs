using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;

namespace Trollkit_Library.ViewModels.Commands
{
    public class ClientCommands
    {
        private Server _server;
        private string handler;
        public ClientCommands(Server server, string handler)
        {
            _server = server;
            this.handler = handler;
        }

        public ICommand Test { get { return new SendServerCommand(SendBeep); } }

        public void SendBeep()
        {
            //TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayBeep", Handler = handler, Value = "800,800" };
            BConsole.WriteLine("TESTESTEST");
            //_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
        }
    }
}
