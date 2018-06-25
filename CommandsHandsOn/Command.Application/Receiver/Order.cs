using System;
using System.Threading.Tasks;
using Command.Application.Enums;
using Command.Application.Interfaces;

namespace Command.Application.Receiver
{
    public class Order : IReceiver
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string WaitressName { get; set; }


        public Task<bool> Cook(CookEnum action)
        {
            switch (action)
            {
                    case CookEnum.Create:
                        return Create();
                    case CookEnum.Dismiss:
                        return Delete();
                default:
                    throw new NotImplementedException();
            }
        }

        private Task<bool> Create()
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(WaitressName) || WaitressName.Length < 3)
                    return false;
                if (string.IsNullOrEmpty(Type) || Type.Length < 3)
                    return false;

                return true;
            });
        }

        private Task<bool> Delete()
        {
            return Task.Run(() => Id != default(int));
        }
    }
}   