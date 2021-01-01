using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stl.CommandR;
using Stl.CommandR.Configuration;
using Stl.DependencyInjection;

namespace Stl.Tests.CommandR.Services
{
    [Service, AddCommandHandlers]
    public class LogCommandHandler : ServiceBase, ICommandHandler<LogCommand>
    {
        public LogCommandHandler(IServiceProvider services) : base(services) { }

        public Task OnCommandAsync(
            LogCommand command, CommandContext context,
            CancellationToken cancellationToken)
        {
            Log.LogInformation(command.Message);
            return Task.CompletedTask;
        }
    }
}
