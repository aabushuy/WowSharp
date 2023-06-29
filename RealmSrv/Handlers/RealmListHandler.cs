using MediatR;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;
using RealmSrv.Repository;

namespace RealmSrv.Handlers
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
            var session = request.Session;

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
