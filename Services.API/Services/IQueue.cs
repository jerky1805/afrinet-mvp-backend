using Services.API.Models;
using Afrinet.API.Contracts;
using Afrinet.Models;

namespace Services.API.Services;

public interface IQueue
{
   // Task  SendMessageAsync( OrderDetails orderDetails);
    Task  SendMessageAsync( ActivationRequest activationRequest);
    Task  SendMessageAsync( ConfirmationRequest confirmationRequest);

}

