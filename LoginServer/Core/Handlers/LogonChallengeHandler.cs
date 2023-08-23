using MediatR;
using LoginServer.Entity;
using LoginServer.Repository;
using LoginServer.Services;
using System.Globalization;
using System.Text;
using LoginServer.Core.Responses;
using LoginServer.Core.Requests;

namespace LoginServer.Core.Handlers
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
            UserContext session = request.UserContext;
            IAuthEngine auth = session.Auth;

            session.AccountInfo = await _accountRepository.GetAccount(request.AccountName)
                ?? throw new InvalidOperationException("No such user name");

            auth.CalculateX(
                Encoding.UTF8.GetBytes(session.AccountInfo.Username),
                GetBytesFromPasswordHash(session.AccountInfo.PasswordHash));

            return new LogonChallengeResponse(session)
            {
                B = auth.PublicB,
                Generator = auth.G,
                PrimeNumber = auth.N,
                Salt = auth.Salt,
                VersionChallenge = auth.CrcSalt
            };
        }

        private byte[] GetBytesFromPasswordHash(string passwordHash)
        {
            var hash = new byte[20];

            for (var i = 0; i < 40; i += 2)
                hash[i / 2] = byte.Parse(passwordHash.AsSpan(i, 2), NumberStyles.HexNumber);

            return hash;
        }
    }
}
