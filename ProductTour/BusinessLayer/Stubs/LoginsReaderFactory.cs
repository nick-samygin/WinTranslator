using PasswordBoss;
using System.Linq;
namespace ProductTour.BusinessLayer.Stubs
{
    public static class LoginsReaderFactory
    {
        public static ILoginsReader CreateLoginsReader(string[] args, IPBData pbData)
        {
#if PRODUCTION
            return new LoginsReader(pbData);
#else

            if (args.Contains("/logins_reader_fake"))
            {
                return new LoginsReaderFake();
            }
            else if (args.Contains("/logins_reader_empty"))
            {
                return new LoginsReaderEmpty();
            }
            else
            {
                return new LoginsReader(pbData);
            }
#endif
        }
    }
}
