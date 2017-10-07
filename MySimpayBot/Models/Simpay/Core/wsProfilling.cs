namespace Models.Core
{
    public class wsProfilling
    {
        public class SendActivationCode_Input
        {
            public SendActivationCode_Input_Parameters Parameters { get; set; }
        }

        public class SendActivationCode_Input_Parameters
        {
            public string MobileNumber { get; set; }
        }

        public class SendActivationCode_Output
        {
            public wsInterface.Status Status { get; set; }
            public SendActivationCode_Output_Parameters Parameters { get; set; }

        }

        public class SendActivationCode_Output_Parameters
        {
            public bool TwoPhaseAuthentication { get; set; }
        }

        //********************************

        public class CreateSession_Input
        {
            public CreateSession_Input_Parameters Parameters { get; set; }
        }

        public class CreateSession_Input_Parameters
        {
            public string ActivationCode { get; set; }
            public string ApplicationType { get; set; }
            public string ApplicationVersion { get; set; }
            public string MobileNumber { get; set; }
            public string ProjectName { get; set; }
        }

        public class CreateSession_Output
        {
            public wsInterface.Status Status { get; set; }
            public CreateSession_Output_Parameters Parameters { get; set; }

        }

        public class CreateSession_Output_Parameters
        {
            public string JsonWebToken { get; set; }
        }




    }
}