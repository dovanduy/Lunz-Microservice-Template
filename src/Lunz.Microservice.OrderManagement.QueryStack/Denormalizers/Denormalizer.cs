using Lunz.Data;

namespace Lunz.Microservice.OrderManagement.QueryStack.Denormalizers
{
    public abstract class DenormalizerBase
    {
        protected readonly IAmbientDatabaseLocator DatabaseLocator;

        protected DenormalizerBase(IAmbientDatabaseLocator databaseLocator)
        {
            DatabaseLocator = databaseLocator;
        }
    }
}