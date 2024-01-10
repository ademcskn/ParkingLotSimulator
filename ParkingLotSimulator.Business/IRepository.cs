using System.Collections.ObjectModel;

namespace ParkingLotSimulator.Business
{
    internal interface IRepository<T, K> where T : class
    {
        T RetrieveById(K id);
        ReadOnlyCollection<T> RetrieveAll();
    }
}
