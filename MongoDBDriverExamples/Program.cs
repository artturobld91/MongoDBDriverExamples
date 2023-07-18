// See https://aka.ms/new-console-template for more information
using MongoDBDriverExamples;

Console.WriteLine("Hello, World!");
MongoDBExamples examples = new MongoDBExamples();
Console.WriteLine("----------------------------------------------");
examples.ListDatabases();
Console.WriteLine("----------------------------------------------");
//examples.InsertDocument();
//await examples.InsertAsyncDocument();
//await examples.InsertManyAsyncDocument();
//await examples.FindAsyncDocument();
Console.WriteLine("----------------------------------------------");
examples.FindListDocument();
Console.WriteLine("----------------------------------------------");
//examples.UpdateDocument();
//Console.WriteLine("----------------------------------------------");
//examples.DeleteDocument();
//Console.WriteLine("----------------------------------------------");
examples.AggregationMatch();
Console.WriteLine("----------------------------------------------");
examples.AggregationGroup();
Console.WriteLine("----------------------------------------------");
examples.AggregationSort();
Console.WriteLine("----------------------------------------------");
examples.AggregationProjection();
