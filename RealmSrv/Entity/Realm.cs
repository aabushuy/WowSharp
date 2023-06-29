using RealmSrv.Enums;

namespace RealmSrv.Entity
{
    internal class Realm
    {
        public string Name { get; }
        public string Address { get; }
        public int Port { get; }

        public RealmIcon Icon { get; set; }        
        public bool IsLocked { get; set; }

        public RealmTimezone Timezone { get; set; }
               
        public RealmFlag RealmFlags { get; set; }

        /// <summary>
        /// Population value.
        ///200 is always green "Recommended",
        ///400 is always red "Full",
        ///600 is always blue "Recommended".
        ///Low/Medium/High are calculated based on the population values sent.
        /// </summary>
        public float Population { get; set; }
        
        public int CharCount { get; set; }

        public Realm(string name, string address, int port)
        {
            Name = name;
            Address = address;
            Port = port;

            Timezone = RealmTimezone.Russian;
            Icon = RealmIcon.Normal;
            Population = 200f;
        }
    }
}
