SimpleDataStore
==========

[![Send me a tweet](http://nathanchere.github.io/twitter_tweet.png)](https://twitter.com/intent/tweet?screen_name=nathanchere "Send me a tweet") [![Follow me](http://nathanchere.github.io/twitter_follow.png)](https://twitter.com/intent/user?screen_name=nathanchere "Follow me")

An extremely simple data store intended for rapid prototyping or extremely light-weight projects where simple data persistence is required but a full DB is overkill.

Why bother? The main itches driving this project were wanting:

* serverless
* minimal config required. No creating databases, setting user permissions etc
* minimal external dependencies
* data stored in a simple readable format (i.e. JSON) for ease of both
  * rapid prototyping (edit documents in any text editor); and
  * easily read and verify results in automated tests

Limitations
----------

It is NOT a full fledged database by any stretch of the imagination. It does not currently, nor is it ever likely to, support things like:

* Querying
* Relationships
* Schemaless collections
* Indexing
* Composite keys
* Logging
* Analytics

It is not going to happen in the immediately foreseeable future but it may, depending on my needs, one day support:

* Transactions
* Auto-number / auto-generated IDs (only for *int* and *GUID* types)

One more time just in case you missed it earlier - ***THIS IS NOT A DATABASE!***

Examples
---------------

#### Basic usage

##### Data format

```cs
private class Widget
{
    public int Id { get; set; }
    public string Value { get; set; }
}
```
##### Storage and retrieval

```cs
var db = new LocalDataStore("WidgetSoftInventory");            

// Storage
db.Save(new Widget {Id = 1337, Value = value});

// Retrieval by ID
var result = db.Get<Widget>(1337);

// Retrieval of all items of given type
var results = db.GetAll<Widget>();
```

#### Configuration

##### Primary key settings

```cs
var db = new LocalDataStore("AlbatrossAirlines");

// Change default key property from "Id"
db.Config.DefaultKeyProperty = "AAID";

// Change key property only for specific types
db.Configure<VipCustomer>(keyName: "FrequentFlyerNumber");
```

##### Disk persistence settings

```cs
// Changing data storage root path from the default (in %AppData%\SimpleDataStore)
db.Config.DataStoreRootPath = Environment.GetCommandLineArgs()[0];
db.Config.DataStoreRootPath = @"D:\CI\Albatross\TestData";

// Changing the file extension for each document (default is ".json")
db.Config.RecordFileExtension = ".txt";

// Use full class name for storing documents. This is to avoid conflicts in similarly
// named classes (e.g. "Foo.Contact" and "Widget.Domain.Contact")
db.Config.UseFullNameAsFolder = true;

// Override the class name entirely for the data storage folder
db.Configure<CustomerViewmodel>(folderName: "customers");
```

##### Info which might be useful for integration tests
```cs
// Get folder where all instances of a specific class will be persisted
var widgetJsonFolder = db.DataPath<Widget>();

// Get location on disk where a specific instance will be persisted
var widget = new Widget{ Id = 1337, Value = "o hai" };
var widgetJsonPath = db.GetFileName(widget);
```

##### Clean up after yourself

```cs
// Deleting all of a single type (roughly equivalent to "DELETE FROM Widget;")
db.DeleteFolder<Widget>();

// Deleting the entire data store
db.DeleteDataStore();

// Automatically delete data store when instance is disposed
db.Config.CleanupOnExit = true;
using(var db = new LocalDataStore("SandwichFactory")
{
    db.Save(new Order {Id = 1, Comments = "Slice off crusts please"});
    db.Save(new Order {Id = 2, Comments = "Don't forget the salt and pepper"});

    // TODO: Get by ID 2 and assert that result is as expected

} // Data store will be deleted here
```


Miscellaneous notes
---------------

Default configuration is intended to be as quick and painless as possible to get up and running. This means making some small assumptions:

* Classes use an *Id* property as a unique identifier / primary key
* Data store folders (i.e. 'tables' in RDBMS land) are named after the class they contain

There is no distinction between `Create` or `Update` in a traditional CRUD sense because - that's right - this is not a database. Anything you save
overwrites anything else in that collection which has the same key.

License
-------

NServiceKit.Text: [view license](https://github.com/NServiceKit/NServiceKit/blob/master/LICENSE)

Other than for that, the rest of this code is licensed under do-whatever-you-want license.