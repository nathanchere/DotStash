using System;
using System.Collections.Generic;

namespace SimpleDataStore
{
    public interface IDataStore : IDisposable
    {
        IEnumerable<T> GetAll<T>(string key);
        T Get<T>(object id);
        void Save<T>(T item);
        void Delete(object id);
        
        void Configure<T>(string folderName, Func<T, object> key);

        //// Maybe todo:
        //void BackupData(string fileName);
        //void RestoreBackup(string fileName);        
    }
}