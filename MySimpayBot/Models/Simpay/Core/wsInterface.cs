namespace Models.Core
{
    public class wsInterface
    {
        public class Identity
        {
            public string ServiceName { get; set; }
            public string ActionName { get; set; }

            public string JsonWebToken { get; set; }
        }
        public class Status
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }


    }
}