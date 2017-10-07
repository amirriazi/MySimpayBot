using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "longitude", Order = 1)]
        public double Longitude { set; get; }

        [DataMember(Name = "latitude", Order = 2)]
        public double Latitude { set; get; }
    }
}