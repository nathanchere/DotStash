﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleDataStore.Example
{
    public class RecordKeyExamples
    {
        private class KeyExampleClass
        {
            public int Id { get; set; }
            public int RegistrationNumber { get; set; }
            public string Value { get; set; }
        }

        [Fact]
        public void DefaultKeyExample()
        {
            const int id = 6133;
            const string value = "norfen pls";

            var db = new LocalDataStore();            
            var input = new KeyExampleClass {Id = id, Value = value};
            db.Save(input);

            var result = db.Get<KeyExampleClass>(id);
            Assert.Equal(result.Value, value);
        }

        [Fact]
        public void OverrideKeyExample()
        {
            const int id = 164;
            const int registrationNumber = 171339811;
            const string value = "acualy is beef";

            var db = new LocalDataStore();
            db.Configure<KeyExampleClass>("keyExample1", c => c.RegistrationNumber);

            var input = new KeyExampleClass {Id = id, RegistrationNumber = registrationNumber, Value = value};
            db.Save(input);

            var result = db.Get<KeyExampleClass>(id);
            Assert.Null(result);

            db.Get<KeyExampleClass>(registrationNumber);
            Assert.Equal(result.Value, value);
        }

    }
}