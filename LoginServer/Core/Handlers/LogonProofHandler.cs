using MediatR;
using LoginServer.Entity;
using LoginServer.Services;
using LoginServer.Core.Responses;
using LoginServer.Core.Requests;

namespace LoginServer.Core.Handlers
{
    internal class LogonProofHandler : IRequestHandler<LogonProofRequest, LogonProofResponse>
    {
        public Task<LogonProofResponse> Handle(LogonProofRequest request, CancellationToken cancellationToken)
        {
            UserContext session = request.UserContext;
            IAuthEngine auth = session.Auth;

            auth.CalculateU(request.A);
            auth.CalculateM1();

            AccountStates accountState = auth.M1.SequenceEqual(request.M1)
                ? AccountStates.LoginOK
                : AccountStates.LoginIncorrectPassword;

            if (accountState == AccountStates.LoginOK)
                auth.CalculateM2(request.M1);

            LogonProofResponse response = new(session)
            {
                AccountState = accountState,
                M2 = auth.M2
            };

            return Task.FromResult(response);
        }
    }
}
