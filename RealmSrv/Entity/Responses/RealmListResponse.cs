using RealmSrv.Enums;
using System.Text;

namespace RealmSrv.Entity.Responses
{
    internal class RealmListResponse : Response
    {
        public byte[] Unk { get; init; }

        public List<Realm> RealmList { get; }

        public RealmListResponse(UserSession userContext) : base(userContext)
        {
            RealmList = new();
        }

        public override async Task Write()
        {
            int realmListLength = RealmList.Sum(x =>
                    3 +
                    x.Name.Length + 1 +
                    x.Address.Length + 1 + x.Port.ToString().Length + 1 +
                    7);

            int responseBodyLength = 6 + (short)realmListLength + 2;

            await Session.WriteByteAsync((byte)OperationCode.AuthRealmList);            
            await Session.WriteByteAsync((byte)(responseBodyLength % 256));
            await Session.WriteByteAsync((byte)(responseBodyLength / 256));

            await Session.WriteByteArrayAsync(Unk);
            await Session.WriteByteAsync((byte)RealmList.Count);
            await Session.WriteZeroByte(1);

            foreach (var realm in RealmList)
            {
                await Session.WriteByteAsync((byte)realm.Icon);
                await Session.WriteByteAsync((byte)(realm.IsLocked ? 1 : 0));
                await Session.WriteByteAsync((byte)realm.RealmFlags);

                await Session.WriteByteArrayAsync(Encoding.UTF8.GetBytes(realm.Name));
                await Session.WriteZeroByte(1);
                
                await Session.WriteByteArrayAsync(Encoding.UTF8.GetBytes($"{realm.Address}:{realm.Port}"));
                await Session.WriteZeroByte(1);

                await Session.WriteFloat(realm.Population);
                await Session.WriteByteAsync((byte)realm.CharCount);
                await Session.WriteByteAsync((byte)realm.Timezone);
                await Session.WriteZeroByte(1);
            }

            await Session.WriteByteAsync(2);
        }
    }
}
