using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceKit.Text;

namespace SimpleDataStore
{
    public class LocalDataStore : IDataStore
    {
        // TODO: maybe config provider? Hopefully overkill.
        public class ConfigurationModel
        {
            internal ConfigurationModel() {
                TypeKeyProperties = new TypeDictionary<string>();
                TypeFolderNames = new TypeDictionary<string>();
            }

            private static readonly string DefaultDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimpleDataStore");

            public string DataRootPath { get; set; } = DefaultDataPath;

            /// <summary>
            /// Roughly equivalent of "Database Name" in MSSQL land
            /// </summary>
            public string DataStoreName { get; set; }

            public bool UseFullNameAsFolder { get; set; } = false;

            public string RecordFileExtension { get; set; } = ".json";

            public string DefaultKeyProperty { get; set; } = "Id";     

            internal readonly TypeDictionary<string> TypeKeyProperties;
            internal readonly TypeDictionary<string> TypeFolderNames;
        }

        public readonly LocalDataStore.ConfigurationModel Config = new ConfigurationModel();

        public LocalDataStore(string dataStoreName)
        {
            Config.DataStoreName = dataStoreName;
        }

        private string DataPath<T>()
        {
            return Path.Combine(
                Config.DataRootPath,
                Config.DataStoreName,
                (Config.TypeFolderNames.SafeGet<T>() ?? typeof(T).Name)
            );
        }

        private void VerifyDataPathExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private string GetKeyProperty<T>(T item)
        {
            var key = Config.TypeKeyProperties.SafeGet<T>() ?? Config.DefaultKeyProperty;
            return item.GetType().GetProperty(key).GetValue(item, null).ToString();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var files = Directory.GetFiles(path).Where(x => x.EndsWith(Config.RecordFileExtension));
            return files
                .Select(File.ReadAllText)
                .Select(text => text.FromJson<T>());
        }

        public T Get<T>(object id)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var file = Directory.GetFiles(path)
                .SingleOrDefault(x => Path.GetFileName(x) == string.Format("{0}{1}", id, Config.RecordFileExtension));

            // TODO: configure to throw instead of null/default?
            if (file == null) return default(T);
            return File.ReadAllText(file).FromJson<T>();
        }

        public void Save<T>(T item)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var id = GetKeyProperty(item);
            var serialisedItem = item.ToJson();
            var fileName = Path.Combine(DataPath<T>(), string.Format("{0}{1}", id, Config.RecordFileExtension));

            File.WriteAllText(fileName, serialisedItem);
        }

        public void Delete<T>(object key)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            File.Delete(path);
        }

        public void Configure<T>(string folderName, string keyPropertyName)
        {
            if(folderName != null)
                Config.TypeFolderNames[typeof(T)] = folderName;

            if(keyPropertyName != null) 
                Config.TypeKeyProperties[typeof(T)] = keyPropertyName;
        }

        // TODO
        // something like this might be nice, will see...
        //public void Configure<T>(string folderName, Func<T, object> key)
        // usage: db.Configure<InternalStaff>("people", p=> string.Format($"{p.FirstName}_{p.SecondName}")
        
    }
}