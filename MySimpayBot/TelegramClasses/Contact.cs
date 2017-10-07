using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Contact
    {
        [DataMember(Name = "phone_number", Order = 1)]
        public string PhoneNumber { set; get; }

        [DataMember(Name = "first_name", Order = 2)]
        public string FirstName { set; get; }

        [DataMember(Name = "last_name", Order = 3)]
        public string LastName { set; get; }

        [DataMember(Name = "user_id", Order = 4)]
        public long UserID { set; get; }
    }
}