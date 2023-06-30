using RealmSrv.Enums;
using System.Text;

namespace RealmSrv.Entity.Responses
{
    internal class RealmListResponse : Response
    {
        public byte[] Unk { get; init; }

        public List<Realm> RealmList { get; }

        public RealmListResponse(UserContext userContext) : base(userContext)
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

            await UserContext.Writer.WriteByteAsync((byte)OperationCode.AuthRealmList);            
            await UserContext.Writer.WriteByteAsync((byte)(responseBodyLength % 256));
            await UserContext.Writer.WriteByteAsync((byte)(responseBodyLength / 256));

            await UserContext.Writer.WriteByteArrayAsync(Unk);
            await UserContext.Writer.WriteByteAsync((byte)RealmList.Count);
            await UserContext.Writer.WriteZeroByte(1);

            foreach (var realm in RealmList)
            {
                await UserContext.Writer.WriteByteAsync((byte)realm.Icon);
                await UserContext.Writer.WriteByteAsync((byte)(realm.IsLocked ? 1 : 0));
                await UserContext.Writer.WriteByteAsync((byte)realm.RealmFlags);

                await UserContext.Writer.WriteByteArrayAsync(Encoding.UTF8.GetBytes(realm.Name));
                await UserContext.Writer.WriteZeroByte(1);
                
                await UserContext.Writer.WriteByteArrayAsync(Encoding.UTF8.GetBytes($"{realm.Address}:{realm.Port}"));
                await UserContext.Writer.WriteZeroByte(1);

                await UserContext.Writer.WriteFloat(realm.Population);
                await UserContext.Writer.WriteByteAsync((byte)realm.CharCount);
                await UserContext.Writer.WriteByteAsync((byte)realm.Timezone);
                await UserContext.Writer.WriteZeroByte(1);
            }

            await UserContext.Writer.WriteByteAsync(2);
            await UserContext.Writer.WriteByteAsync(0);
        }
    }
}
