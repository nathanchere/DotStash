SimpleDataStore
==========

A ridiculously simple data store intended for rapid prototyping or extremely light-weight projects where a full DB isn't required.

Why bother? The main itches driving this project were wanting:

* serverless
* minimal config required. No creating databases, setting user permissions etc
* minimal external dependencies
* data stored in a simple readable format (i.e. JSON) for ease of both
  * rapid prototyping (edit documents in any text editor); and
  * easily read and verify results in automated tests

It offers no concept of relationships, transactions, indexing or any of the other things you probably take for granted when using a proper
database.

Example
---------------

TODO: put an example of usage here