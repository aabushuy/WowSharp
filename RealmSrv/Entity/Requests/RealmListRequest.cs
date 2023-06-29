using MediatR;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Entity.Requests
{
    internal class RealmListRequest : Request, IRequest<RealmListResponse>
    {
        public RealmListRequest(UserSession session) : base(session)
        {
        }

        public override Task Read()
        {
            throw new NotImplementedException();
        }
    }
}
