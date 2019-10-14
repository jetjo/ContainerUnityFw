

using Microsoft.Practices.Unity.GuardSupport;

namespace Microsoft.Practices.Unity.GuardSupport
{
    public class ObjectWithTwoConstructorParameters
    {
        private string connectionString;
        private ILogger logger;

        public ObjectWithTwoConstructorParameters(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public ILogger Logger
        {
            get { return logger; }
        }
    }
}
