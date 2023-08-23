namespace LoginServer.Entity
{
    internal class Realm
    {
        public string Name { get; }
        public string Address { get; }
        public int Port { get; }

        public RealmIcon Icon { get; set; } = RealmIcon.Normal;
        
        public bool IsLocked { get; set; } = false;

        public RealmTimezone Timezone { get; set; } = RealmTimezone.Russian;

        public RealmFlag RealmFlags { get; set; } = RealmFlag.ForceNewPlayers;

        /// <summary>
        /// Population value.
        ///200 is always green "Recommended",
        ///400 is always red "Full",
        ///600 is always blue "Recommended".
        ///Low/Medium/High are calculated based on the population values sent.
        /// </summary>
        public float Population { get; set; } = 200f;

        public int NumberOfChars { get; set; } = 0;

        public Realm(string name, string address, int port)
        {
            Name = name;
            Address = address;
            Port = port;
        }
    }
}
