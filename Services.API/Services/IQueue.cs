using Services.API.Models;
using Afrinet.API.Contracts;

namespace Services.API.Services;

public interface IQueue
{
    Task  SendMessageAsync( OrderDetails orderDetails);
}

