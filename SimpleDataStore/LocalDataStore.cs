using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.Text;

namespace SimpleDataStore
{
    public class LocalDataStore : IDataStore
    {
        private readonly string DefaultDataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimpleDataStore");

        // TODO: probably move into a nested config class
        // TODO: maybe config provider? Hopefully overkill.
        public string DataRoot => DefaultDataPath;
        public string RecordFileExtension => ".json";

        private string DataPath<T>()
        {
            // TODO: would this be better as FullName? Configurable?
            return Path.Combine(DataRoot, typeof (T).Name);
        }

        private void VerifyDataPathExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private string ReadKey<T>(T item)
        {
            // TODO: configure which property is key
            return item.GetType().GetProperty("Id").GetValue(item, null).ToString();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var files = Directory.GetFiles(path).Where(x => x.EndsWith(RecordFileExtension));
            return files
                .Select(File.ReadAllText)
                .Select(text => text.FromJson<T>());
        }

        public T Get<T>(object id)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var file = Directory.GetFiles(path)
                .SingleOrDefault(x => Path.GetFileName(x) == string.Format("{0}{1}", id, RecordFileExtension));

            // TODO: configure to throw instead of null/default?
            if (file == null) return default(T);
            return File.ReadAllText(file).FromJson<T>();
        }

        public void Save<T>(T item)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            var id = ReadKey(item);
            var serialisedItem = item.ToJson();
            var fileName = Path.Combine(DataPath<T>(), string.Format("{0}{1}", id, RecordFileExtension));

            File.WriteAllText(fileName, serialisedItem);
        }

        public void Delete<T>(object key)
        {
            var path = DataPath<T>();
            VerifyDataPathExists(path);

            File.Delete(path);
        }

        public void Configure<T>(string folderName, Func<T, object> key)
        {
            throw new NotImplementedException();
        }
    }
}