using MediatR;
using LoginServer.Repository;
using LoginServer.Core.Responses;
using LoginServer.Core.Requests;

namespace LoginServer.Core.Handlers
{
    internal class RealmListHandler : IRequestHandler<RealmListRequest, RealmListResponse>
    {
        private readonly IRealmRepository _realmRepository;

        public RealmListHandler(IRealmRepository realmRepository)
        {
            _realmRepository = realmRepository;
        }

        public async Task<RealmListResponse> Handle(RealmListRequest request, CancellationToken cancellationToken)
        {
            var session = request.UserContext;

            var realms = await _realmRepository.GetRealmListForAccount(session.AccountInfo);

            var resopnse = new RealmListResponse(session)
            {
                Unk = request.Unk
            };

            resopnse.RealmList.AddRange(realms);

            return resopnse;
        }
    }
}
