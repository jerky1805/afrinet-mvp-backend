using Services.API.Models;
using Afrinet.API.Contracts;

namespace Services.API.Services;

public interface IServiceBus
{
    Task  SendMessageAsync( OrderDetails orderDetails);
}

