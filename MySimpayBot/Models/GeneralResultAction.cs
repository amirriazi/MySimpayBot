namespace Models
{
    public class GeneralResultAction
    {
        public string actionName { get; set; }
        public bool hasError { get; set; }
        public string message { get; set; }
        public object Attachment { get; set; }

        public GeneralResultAction(string theActionName = "", bool theHasError = false, string theMessage = "")
        {
            actionName = theActionName;
            hasError = theHasError;
            message = theMessage;
        }

    }
}