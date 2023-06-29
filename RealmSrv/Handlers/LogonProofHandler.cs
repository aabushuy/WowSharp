using MediatR;
using RealmSrv.Entity;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;
using RealmSrv.Enums;
using RealmSrv.Services;

namespace RealmSrv.Handlers
{
    internal class LogonProofHandler : IRequestHandler<LogonProofRequest, LogonProofResponse>
    {
        public Task<LogonProofResponse> Handle(LogonProofRequest request, CancellationToken cancellationToken)
        {
            UserSession session = request.Session;
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
