using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stl.CommandR.Configuration;
using Stl.CommandR.Internal;

namespace Stl.CommandR
{
    public readonly struct CommandRBuilder
    {
        public IServiceCollection Services { get; }
        public ICommandHandlerRegistry Handlers { get; }

        internal CommandRBuilder(IServiceCollection services)
        {
            Services = services;

            Services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
            Services.TryAddSingleton<ICommandHandlerRegistry>(new CommandHandlerRegistry());
            Services.TryAddSingleton<ICommandHandlerResolver, CommandHandlerResolver>();

            var handlers = (ICommandHandlerRegistry?) null;
            foreach (var descriptor in Services) {
                if (descriptor.ServiceType == typeof(ICommandHandlerRegistry)) {
                    handlers = (ICommandHandlerRegistry?) descriptor.ImplementationInstance
                        ?? throw Errors.CommandHandlerRegistryMustBeRegisteredAsInstance();
                    break;
                }
            }
            Handlers = handlers ?? throw Errors.CommandHandlerRegistryInstanceIsNotRegistered();
        }

        public CommandRBuilder AddHandler<TCommand, THandlerService>(double priority = 0)
            where TCommand : class, ICommand
            where THandlerService : ICommandHandler<TCommand>
            => AddHandler(CommandHandler.New<TCommand, THandlerService>(priority));

        // Low-level methods

        public CommandRBuilder AddHandler(CommandHandler handler)
        {
            Handlers.Add(handler);
            return this;
        }

        public CommandRBuilder ClearHandlers()
        {
            Handlers.Clear();
            return this;
        }
    }
}
