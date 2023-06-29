﻿using MediatR;
using RealmSrv.Entity.Responses;

namespace RealmSrv.Entity.Requests
{
    internal class RealmListRequest : Request, IRequest<RealmListResponse>
    {
        public byte[] Unk { get; private set; }

        public RealmListRequest(UserSession session) : base(session)
        {
        }

        public override async Task Read()
        {
            Unk = await Session.ReadByteArrayAsync(4);
        }
    }
}
