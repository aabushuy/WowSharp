using MediatR;
using RealmSrv.Entity.Requests;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Handlers
{
    internal class RealmListHandler : IRequestHandler<RealmListRequest, RealmListResponse>
    {
        public Task<RealmListResponse> Handle(RealmListRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
