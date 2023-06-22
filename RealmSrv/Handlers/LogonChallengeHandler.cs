using MediatR;
using RealmSrv.Entity;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;
using RealmSrv.Repository;

namespace RealmSrv.Handlers
{
    internal class LogonChallengeHandler : IRequestHandler<LogonChallengeRequest, LogonChallengeResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public LogonChallengeHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<LogonChallengeResponse> Handle(LogonChallengeRequest request, CancellationToken cancellationToken)
        {
            UserContext user = request.User;

            user.AccountInfo = await _accountRepository.GetAccount(request.AccountName)
                ?? throw new InvalidOperationException("No such user name");

            return new LogonChallengeResponse(user)
            {
            };
        }
    }
}
