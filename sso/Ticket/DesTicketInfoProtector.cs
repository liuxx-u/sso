using sso.Utils;

namespace sso.Ticket
{
    /// <summary>
    /// 票据加密方式
    /// Base64(DES(ticket))+Base64(DES(_salt))
    /// </summary>
    internal class DesTicketInfoProtector : ITicketInfoProtector
    {
        public const string DefaultDesKey = "bode1234";
        public const string DefaultSalt = "salt";

        private readonly string _salt;
        private readonly string _desKey;

        public DesTicketInfoProtector(string desKey = DefaultDesKey, string salt = DefaultSalt)
        {
            desKey.Length.CheckGreaterThan(nameof(desKey.Length), 8, true);

            _salt = salt;
            if (desKey.Length > 8 && desKey.Length < 24)
            {
                desKey = desKey.Left(8);
            }
            else if (desKey.Length > 24)
            {
                desKey = desKey.Left(24);
            }
            _desKey = desKey;
        }

        public string Protect(TicketInfo ticket)
        {
            ticket.CheckNotNull(nameof(ticket));

            var json = JsonHelper.ToJson(ticket);
            return DesHelper.Encrypt(json, _desKey) + DesHelper.Encrypt(_salt, _desKey);
        }

        public TicketInfo UnProtect(string token)
        {
            token.CheckNotNullOrEmpty(nameof(token));

            var salt = DesHelper.Encrypt(_salt, _desKey);
            if (!token.EndsWith(salt)) return null;

            var json = token.Substring(0, token.Length - salt.Length);
            return JsonHelper.FromJson< TicketInfo >(DesHelper.Decrypt(json, _desKey));
        }
    }
}