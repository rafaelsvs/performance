using BenchmarkDotNet.Running;
using CSharpBenchmark.JsonSerializer;
using CSharpBenchmark.Linq;
using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace CSharpBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            BenchmarkRunner.Run<LinqBenchmarks>();
            //BenchmarkRunner.Run<Json_FromStream<MyEventsListerViewModel>>();
            //BenchmarkRunner.Run<Json_FromString<MyEventsListerViewModel>>();
            //BenchmarkRunner.Run<Json_ToStream<MyEventsListerViewModel>>();
            //BenchmarkRunner.Run<Json_ToString<MyEventsListerViewModel>>();
#endif
#if DEBUG
            RunWhere1($"\n\n Where 1 \n");

            RunWhere2($"\n\n Where 2 \n");

            RunJsonFromStream($"\n\n Json From Stream \n");

            RunJsonFromString($"\n\n Json From String \n");

            RunJsonToStream($"\n\n Json To Stream \n");

            RunJsonToString($"\n\n Json To String \n");

            Console.Read();
#endif
        }

        private static void RunWhere1(string methodName)
        {
            int repetitions = 1000000;
            LinqBenchmarks linqBenchmarks = new LinqBenchmarks();
            Stopwatch rel;
            ICollection result = null;
            Console.WriteLine(methodName);

            // warmup
            result = linqBenchmarks.Where1LinqQueryX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqQueryX();
            rel.Stop();
            Console.WriteLine($"Linq: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqLambdaX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqLambdaX();
            rel.Stop();
            Console.WriteLine($"Lambda: \t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqForX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqForX();
            rel.Stop();
            Console.WriteLine($"For: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqUnrolledForX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqUnrolledForX();
            rel.Stop();
            Console.WriteLine($"Unrolled for: \t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqForeachSortedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqForeachSortedX();
            rel.Stop();
            Console.WriteLine($"Foreach sorted:  {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqKeyedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqKeyedX();
            rel.Stop();
            Console.WriteLine($"Dictionary: \t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where1LinqPositionalX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where1LinqPositionalX();
            rel.Stop();
            Console.WriteLine($"Array: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");
        }

        private static void RunWhere2(string methodName)
        {
            int repetitions = 1000000;
            LinqBenchmarks linqBenchmarks = new LinqBenchmarks();
            Stopwatch rel;
            ICollection result;
            Console.WriteLine(methodName);

            // warmup
            result = linqBenchmarks.Where2LinqQueryX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqQueryX();
            rel.Stop();
            Console.WriteLine($"Linq: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqLambdaX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqLambdaX();
            rel.Stop();
            Console.WriteLine($"Lambda: \t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqForX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqForX();
            rel.Stop();
            Console.WriteLine($"For: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqForSortedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqForSortedX();
            rel.Stop();
            Console.WriteLine($"For sorted:  {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqKeyedSortedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqKeyedSortedX();
            rel.Stop();
            Console.WriteLine($"Dictionary: \t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqPositionalSortedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqPositionalSortedX();
            rel.Stop();
            Console.WriteLine($"Array: \t\t {rel.ElapsedMilliseconds} - {result.Count} lines");

            // warmup
            result = linqBenchmarks.Where2LinqPositionalSortedVectorizedX();
            rel = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
                result = linqBenchmarks.Where2LinqPositionalSortedVectorizedX();
            rel.Stop();
            Console.WriteLine($"Array vectorized:{rel.ElapsedMilliseconds} - {result.Count} lines");

        }

        private static void RunJsonFromStream(string methodName)
        {
            var jsonFromStream = new Json_FromStream<MyEventsListerViewModel>();
            Stopwatch rel = new Stopwatch();

            Console.WriteLine(methodName);

            jsonFromStream.SetupJil_();
            rel.Start();
            jsonFromStream.Jil_();
            rel.Stop();
            Console.WriteLine($"Jil: \t\t {rel.ElapsedMilliseconds}");

            jsonFromStream.SetupJsonNet_();
            rel = new Stopwatch();
            rel.Start();
            jsonFromStream.JsonNet_();
            rel.Stop();
            Console.WriteLine($"JsonNet: \t\t {rel.ElapsedMilliseconds}");

            jsonFromStream.SetupUtf8Json_();
            rel = new Stopwatch();
            rel.Start();
            jsonFromStream.Utf8Json_();
            rel.Stop();
            Console.WriteLine($"Utf8Json: \t\t {rel.ElapsedMilliseconds}");

            jsonFromStream.SetupDataContractJsonSerializer_();
            rel = new Stopwatch();
            rel.Start();
            jsonFromStream.DataContractJsonSerializer_();
            rel.Stop();
            Console.WriteLine($"Json Serializer: \t\t {rel.ElapsedMilliseconds}");

            jsonFromStream.Cleanup();
        }

        private static void RunJsonFromString(string methodName)
        {
            var jsonFromString = new Json_FromString<MyEventsListerViewModel>();
            Stopwatch rel = new Stopwatch();

            Console.WriteLine(methodName);

            jsonFromString.SetupJil();
            rel.Start();
            jsonFromString.Jil_();
            rel.Stop();
            Console.WriteLine($"Jil: \t\t {rel.ElapsedMilliseconds}");

            jsonFromString.SerializeJsonNet();
            rel = new Stopwatch();
            rel.Start();
            jsonFromString.JsonNet_();
            rel.Stop();
            Console.WriteLine($"JsonNet: \t\t {rel.ElapsedMilliseconds}");

            jsonFromString.SerializeUtf8Json_();
            rel = new Stopwatch();
            rel.Start();
            jsonFromString.Utf8Json_();
            rel.Stop();
            Console.WriteLine($"Utf8Json: \t\t {rel.ElapsedMilliseconds}");
        }

        private static void RunJsonToStream(string methodName)
        {
            var jsonToStream = new Json_ToStream<MyEventsListerViewModel>();
            Stopwatch rel = new Stopwatch();

            Console.WriteLine(methodName);

            jsonToStream.Setup();

            rel.Start();
            jsonToStream.Jil_();
            rel.Stop();
            Console.WriteLine($"Jil: \t\t {rel.ElapsedMilliseconds}");

            rel = new Stopwatch();
            rel.Start();
            jsonToStream.JsonNet_();
            rel.Stop();
            Console.WriteLine($"JsonNet: \t\t {rel.ElapsedMilliseconds}");

            rel = new Stopwatch();
            rel.Start();
            jsonToStream.Utf8Json_();
            rel.Stop();
            Console.WriteLine($"Utf8Json: \t\t {rel.ElapsedMilliseconds}");

            jsonToStream.Cleanup();
        }

        private static void RunJsonToString(string methodName)
        {
            var jsonToString = new Json_ToString<MyEventsListerViewModel>();
            Stopwatch rel = new Stopwatch();

            Console.WriteLine(methodName);

            jsonToString.Setup();

            rel.Start();
            var t = jsonToString.Jil_();
            rel.Stop();
            Console.WriteLine($"Jil: \t\t {rel.ElapsedMilliseconds}");

            rel = new Stopwatch();
            rel.Start();
            jsonToString.JsonNet_();
            rel.Stop();
            Console.WriteLine($"JsonNet: \t\t {rel.ElapsedMilliseconds}");

            rel = new Stopwatch();
            rel.Start();
            jsonToString.Utf8Json_();
            rel.Stop();
            Console.WriteLine($"Utf8Json: \t\t {rel.ElapsedMilliseconds}");
        }
    }
}