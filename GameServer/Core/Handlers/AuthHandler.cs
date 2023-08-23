using GameServer.Core.Requests;
using GameServer.Core.Responses;
using GameServer.Enums;
using MediatR;
using Microsoft.VisualBasic;
using System.Buffers.Binary;
using System.Text;

namespace GameServer.Core.Handlers
{
    internal class AuthHandler : IRequestHandler<AuthRequest, AuthResponse>
    {
        public Task<AuthResponse> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            return request.OperationCode switch
            {
                OperationCode.CMSG_AUTH_SESSION => DoAuth(request, cancellationToken),

                _ => throw new InvalidOperationException($"No handler for {request.OperationCode}")
            };
        }

        private async Task<AuthResponse> DoAuth(AuthRequest request, CancellationToken cancellationToken)
        {
            uint build = request.GetUInt32(0);
            uint serverId = request.GetUInt32(4);
            
            string username = request.GetString(8);
            int offset = username.Length;

            uint clientSeed = request.GetUInt32(offset);            
            offset += 4;

            byte[] clientProof = request.GetArray(offset, 20);
            offset += clientProof.Length;

            uint decompressedAddonInfoSize = request.GetUInt32(offset);
            offset += 4;

            uint compressed_addon_info = request.GetUInt32(offset);

            AuthResponse response = new(OperationCode.SMSG_AUTH_RESPONSE);
            
            response.AddUIntLittleEndian(13); // AUTH_OK
            response.AddUIntLittleEndian(0); //billing_time
            response.AddByteLittleEndian(2); //BillingPlanFlags
            response.AddUIntLittleEndian(0U); //billing_rested

            InitEncryption(request, clientSeed, username);

            await Task.Delay(500, cancellationToken);

            return response;
        }

        private static void InitEncryption(AuthRequest request, uint clientSeed, string username)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(request.Session.Key, clientSeed);

            for (int i = 0, loopTo = username.Length - 1; i <= loopTo; i += 2)
            {
                request.Session.Hash[i / 2] = (byte)Conversion.Val("&H" + Strings.Mid(username, i + 1, 2));
            }

            request.Session.IsEncrypt = true;
        }
    }
}
