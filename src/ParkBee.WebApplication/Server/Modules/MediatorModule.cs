using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using MediatR;
using ParkBee.Domain.Exceptions;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Server.Validators;

namespace ParkBee.WebApplication.Server.Modules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler)
            // in assembly holding the Commands
            builder.RegisterAssemblyTypes(
                    typeof(SaveDoorCommandHandler).GetTypeInfo().Assembly).
                AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(
                    typeof(SaveGarageCommandHandler).GetTypeInfo().Assembly).
                AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder
                .RegisterAssemblyTypes(typeof(SaveGarageValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();
            // Other types registration
            //...
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).
                As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).
                As(typeof(IPipelineBehavior<,>));
        }

        public class LoggingBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
        {
            private readonly ILoggerManager _loggerManager;
            public LoggingBehavior(ILoggerManager loggerManager)
            {
                _loggerManager = loggerManager;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _loggerManager.Info($"Handling {typeof(TRequest).Name}");
                var response = await next();
                _loggerManager.Info($"Handled {typeof(TResponse).Name}");
                return response;
            }
        }

        public class ValidatorBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
        {
            private readonly IValidator<TRequest>[] _validators;
            public ValidatorBehavior(IValidator<TRequest>[] validators) =>
                _validators = validators;

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                var failures = _validators
                    .Select(v => v.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(error => error != null)
                    .ToList();

                if (failures.Any())
                {
                    throw new ParkBeeBaseException($"Command Validation Errors for type {typeof(TRequest).Name}", 
                        new AggregateException(failures.Select(failure => new ValidationException(failure.ErrorMessage))));
                }

                var response = await next();
                return response;
            }
        }
    }
}
