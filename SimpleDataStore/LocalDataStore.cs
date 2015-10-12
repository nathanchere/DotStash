using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataStore
{
    public class LocalDataStore : IDataStore
    {
        public void Dispose()
        {
            
        }

        public IEnumerable<T> GetAll<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Configure<T>(string folderName, Func<T, object> key)
        {
            throw new NotImplementedException();
        }
    }
}
