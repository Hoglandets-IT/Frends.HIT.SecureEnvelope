namespace Frends.Community.PaymentServices.Nordea.Helpers
{
    public class Enums
    {
        public enum Environment
        {
            TEST,
            PRODUCTION
        }
        // Possible statuses for files
        // NEW = New (not yet downloaded)
        // DOWNLOADED = Downloaded
        // ALL = Both new and downloaded files will be listed
        public enum Status
        {
            NEW,
            DOWNLOADED,
            ALL
        }
    }
}
