using Polly;
using Refit;
using System.Net;

namespace Integration
{
    public class IntegrationService : IIntegrationService
    {
        public async Task<T?> SendRequestWithPolicy<T>(Func<Task<T>> call)
        {
            var retypolicy = Policy.HandleInner<ApiException>(ex => ex.StatusCode == HttpStatusCode.ServiceUnavailable)
            .WaitAndRetryAsync(2, _ => { return TimeSpan.FromMilliseconds(2000); });

            var result = await retypolicy.ExecuteAndCaptureAsync(call);

            if (result.Outcome == OutcomeType.Failure)
                return default;

            return result.Result;
        }
    }
}
