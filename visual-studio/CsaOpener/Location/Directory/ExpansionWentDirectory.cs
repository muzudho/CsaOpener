namespace Grayscale.CsaOpener.Location
{
    public class ExpansionWentDirectory
    {
        private static ExpansionWentDirectory thisInstance;

        protected ExpansionWentDirectory()
        {

        }

        public static ExpansionWentDirectory Instance
        {
            get
            {
                if (thisInstance == null)
                {
                    thisInstance = new ExpansionWentDirectory();
                }

                return thisInstance;
            }
        }

        /*
        public string Path
        {
            get
            {
                return 
            }
        }
        */
    }
}
