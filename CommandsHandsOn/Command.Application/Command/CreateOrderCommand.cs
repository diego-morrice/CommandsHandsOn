﻿using System.Threading.Tasks;
using Command.Application.Enums;
using Command.Application.Interfaces;

namespace Command.Application.Command
{
    public class CreateOrderCommand : IOrderCommand
    {
        private readonly IReceiver Receiver;
        public bool IsPrepared { get; set; }

        public CreateOrderCommand(IReceiver receiver)
        {
            Receiver = receiver;
            IsPrepared = false;
        }

        public Task<bool> Prepare()
        {
            IsPrepared = true;
            return Receiver.Cook(CookEnum.Create);
        }
    }
}