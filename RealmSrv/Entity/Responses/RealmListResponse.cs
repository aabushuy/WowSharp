namespace RealmSrv.Entity.Responses
{
    internal class RealmListResponse : Response
    {
        public RealmListResponse(UserSession userContext) : base(userContext)
        {
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}
