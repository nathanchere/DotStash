using System;
using System.Collections.Generic;

namespace DotStash
{
    public interface IDataStore : IDisposable
    {
        void Configure<T>(string folderName, string keyName);
                
        T Get<T>(object id);
        IEnumerable<T> GetAll<T>();
        void Save<T>(T item);
        void Delete<T>(object key);        
        void DeleteFolder<T>();
        void DeleteDataStore();        
    }
}