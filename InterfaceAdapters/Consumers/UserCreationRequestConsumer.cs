using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using MassTransit;

namespace InterfaceAdapters.Consumers
{
    public class CreateUserConsumer : IConsumer<UserForCollabCommandMessage>
    {
        private readonly IUserService _userService;
        private readonly IPublishEndpoint _publishEndpoint;
        public CreateUserConsumer(IUserService userService, IPublishEndpoint publishEndpoint)
        {
            _userService = userService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<UserForCollabCommandMessage> context)
        {
            var command = context.Message;
            Console.WriteLine($"[user-cmd] Recebido CreateUser para o email {command.Email} com CorrelationId {command.Id}");

         
                var newUser = await _userService.CreateSagaUserAsync(
                    command.Names,
                    command.Surnames,
                    command.Email,
                    command.FinalDate
                );


        }
    }
}