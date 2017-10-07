using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id", Order = 1)]
        public int ID { set; get; }

        [DataMember(Name = "first_name", Order = 2)]
        public string FirstName { set; get; }

        [DataMember(Name = "last_name", Order = 3)]
        public string LastName { set; get; }

        [DataMember(Name = "username", Order = 4)]
        public string UserName { set; get; }

        [DataMember(Name = "language_code", Order = 4)]
        public string LanguageCode { set; get; }

        public override string ToString()
        {
            return $@"[User:{UserName}, FirstName:{FirstName}, LastName:{LastName}, ID:{ID}]";
        }
    }
}