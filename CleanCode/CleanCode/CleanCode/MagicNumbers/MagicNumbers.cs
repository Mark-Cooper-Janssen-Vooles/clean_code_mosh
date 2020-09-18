
namespace CleanCode.MagicNumbers
{
    public enum DocumentStatus
    {
        Draft = 1,
        Lodged = 2
    }

    public class MagicNumbers
    {

        public void ApproveDocument(DocumentStatus status)
        {
            if (status == DocumentStatus.Draft)
            {
                // ...
            }
            else if (status == DocumentStatus.Lodged)
            {
                // ...
            }
        }

        public void RejectDoument(DocumentStatus status)
        {
            switch (status)
            {
                case DocumentStatus.Draft: //a magic string
                    // ...
                    break;
                case DocumentStatus.Lodged:
                    // ...
                    break;
            }
        }
    }
}
