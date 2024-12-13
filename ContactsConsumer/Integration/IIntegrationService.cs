using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration
{
    public interface IIntegrationService
    {
        Task<T?> SendRequestWithPolicy<T>(Func<Task<T>> call);
    }
}
