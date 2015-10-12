using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataStore.Example
{
    public class Example
    {

        public void LoadOneRecord()
        {
        }

        public void KeyExamples()
        {
            var db = new LocalDataStore();
            db.Configure<Contact>("contacts", c => c.Id);

            var result = db.Get<Contact>(14);
        }

        public void LoadAllRecords()
        {

        }

        public void RelatedRecords()
        {

        }

    }
}
