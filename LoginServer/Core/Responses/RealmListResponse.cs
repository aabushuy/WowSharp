using LoginServer.Entity;
using System.Buffers.Binary;
using System.Text;

namespace LoginServer.Core.Responses
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

            byte[] bodyLength = new byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(bodyLength, (ushort)responseBodyLength);

            //header
            await UserContext.Writer.WriteByteAsync((byte)OperationCode.AuthRealmList);
            await UserContext.Writer.WriteByteArrayAsync(bodyLength);

            await UserContext.Writer.WriteZeroByte(4);
            
            byte[] realmListCount = new byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(realmListCount, (ushort)RealmList.Count);
            await UserContext.Writer.WriteByteArrayAsync(realmListCount);

            foreach (var realm in RealmList)
            {
                await UserContext.Writer.WriteByteAsync((byte)realm.Icon);
                await UserContext.Writer.WriteByteAsync((byte)(realm.IsLocked ? 1 : 0));
                await UserContext.Writer.WriteZeroByte(1);

                await UserContext.Writer.WriteByteArrayAsync(Encoding.UTF8.GetBytes(realm.Name));
                await UserContext.Writer.WriteZeroByte(1);

                await UserContext.Writer.WriteByteArrayAsync(Encoding.UTF8.GetBytes($"{realm.Address}:{realm.Port}"));
                await UserContext.Writer.WriteZeroByte(1);

                await UserContext.Writer.WriteFloat(realm.Population);
                await UserContext.Writer.WriteByteAsync((byte)realm.NumberOfChars);
                await UserContext.Writer.WriteByteAsync((byte)realm.Timezone);
                await UserContext.Writer.WriteByteAsync(1);
            }

            await UserContext.Writer.WriteByteAsync(2);
            await UserContext.Writer.WriteByteAsync(0);
        }
    }
}
