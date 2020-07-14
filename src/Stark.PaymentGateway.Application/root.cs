using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Stark.PaymentGateway")]
namespace Stark.PaymentGateway.Application
{
    internal class ApplicationLayer
    {
        /* This class is needed by MediatR so it can register the assembly and scan for Request Handlers (command handlers) */
    }
}
