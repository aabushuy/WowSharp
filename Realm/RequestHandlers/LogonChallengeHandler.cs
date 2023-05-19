using DataAccess.Repositories;
using Domain.Entity.Account;
using MediatR;
using Realm.Entity.Requests;
using Realm.Entity.Responses;
using Realm.Services;

namespace Realm.RequestHandlers
{
    internal class LogonChallengeHandler : IRequestHandler<LogonChallengeRequest, LogonChallengeResponse>
    {
        private readonly IAccountInfoRepository _accountInfoRepository;
        private readonly IAuthEngine _authEngine;

        public LogonChallengeHandler(IAccountInfoRepository accountInfoRepository, IAuthEngine authEngine)
        {
            _accountInfoRepository = accountInfoRepository;
            _authEngine = authEngine;
        }

        public async Task<LogonChallengeResponse> Handle(LogonChallengeRequest request, CancellationToken cancellationToken)
        {
            var accountInfo = await _accountInfoRepository.GetAccountInfo(request.AccountName);

            _authEngine.Init(accountInfo.Username, accountInfo.PasswordHash);

            request.UserContext[nameof(AccountInfo)] = accountInfo;
            request.UserContext[nameof(IAuthEngine)] = _authEngine;

            return new LogonChallengeResponse()
            { 
                B = _authEngine.PublicB,
                Generator = _authEngine.Generator,
                PrimeNumber = _authEngine.PrimeNumber,
                Salt = _authEngine.Salt,
                VersionChallenge = _authEngine.VersionChallenge
            };
        }
    }
}
