using System;
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

        public ICommand Website { get { return new SendServerCommand(param => OpenWebsite(param));}}

        private void OpenWebsite(Object url)
        {
            System.Diagnostics.Process.Start(url.ToString());
        }

    }
}
