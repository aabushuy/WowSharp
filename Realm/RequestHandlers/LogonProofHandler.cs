using Domain.Entity.Account;
using MediatR;
using Realm.Entity.Requests;
using Realm.Entity.Responses;
using Realm.Services;

namespace Realm.RequestHandlers
{
    internal class LogonProofHandler : IRequestHandler<LogonProofRequest, LogonProofResponse>
    {
        public async Task<LogonProofResponse> Handle(LogonProofRequest request, CancellationToken cancellationToken)
        {
            var accountInfo = (AccountInfo)request.UserContext[nameof(AccountInfo)];
            var authEngine = (IAuthEngine)request.UserContext[nameof(IAuthEngine)];
            
            authEngine.CalculateSessionKey(request.A);
            authEngine.HashSessionKey();

            authEngine.CalculateM1(accountInfo.Username);
            authEngine.CalculateM2();

            var response = new LogonProofResponse
            {
                AccountState = authEngine.M1.SequenceEqual(request.M1)
                    ? AccountStates.LoginOK
                    : AccountStates.LoginIncorrectPassword,
                M2 = authEngine.M2
            };

            return response;
        }
    }
}
