using System;
using System.Collections.Generic;

namespace SimpleDataStore
{
    public interface IDataStore
    {
        IEnumerable<T> GetAll<T>();
        T Get<T>(object key);
        void Save<T>(T item);
        void Delete<T>(object key);
        
        void Configure<T>(string folderName, string keyPropertyName);

        //// Maybe todo:
        //void BackupData(string fileName);
        //void RestoreBackup(string fileName);        
    }
}