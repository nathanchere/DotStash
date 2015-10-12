using System;
using Xunit;

namespace DotStash
{
    public class SimplestExamples
    {
        private class KeyExampleClass
        {
            public int Id { get; set; }
            public int RegistrationNumber { get; set; }
            public string Value { get; set; }
        }

        private class GuidKeyExampleClass
        {
            public Guid Id { get; set; }            
            public string Value { get; set; }
        }

        [Fact]
        public void DefaultKeyExample()
        {
            const int id = 6133;
            const string value = "norfen pls";

            var db = new SimpleDataStore("keyExamples");            
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

            var db = new SimpleDataStore("keyExamples");
            db.Configure<KeyExampleClass>("keyExample1", "RegistrationNumber");

            var input = new KeyExampleClass {Id = id, RegistrationNumber = registrationNumber, Value = value};
            db.Save(input);

            var result = db.Get<KeyExampleClass>(id);
            Assert.Null(result);

            result = db.Get<KeyExampleClass>(registrationNumber);
            Assert.Equal(result.Value, value);
        }

        [Fact]
        public void NonIntegerKeyNoConfiguration()
        {
            var id = Guid.NewGuid();
            const string value = "dat not candel in yuor pokit";

            var db = new SimpleDataStore("keyExamples");

            var input = new GuidKeyExampleClass {Id = id, Value = value};
            db.Save(input);

            var result = db.Get<GuidKeyExampleClass>(id);
            Assert.Equal(result.Value, value);
        }

    }
}
