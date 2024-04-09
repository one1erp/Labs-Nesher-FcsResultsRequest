namespace FcsResultsRequest
{
    public partial class SendToMSB
    {
        public class responseReq
        {
            public bool success { get; set; }
            public string str { get; set; }

            public responseReq(bool success, string str)
            {
                this.success = success;
                this.str = str;

            }
        }
    }
}


