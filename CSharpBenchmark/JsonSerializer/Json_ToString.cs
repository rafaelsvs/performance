using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBenchmark.JsonSerializer
{
    [GenericTypeArguments(typeof(LoginViewModel))]
    [GenericTypeArguments(typeof(Location))]
    [GenericTypeArguments(typeof(IndexViewModel))]
    [GenericTypeArguments(typeof(MyEventsListerViewModel))]
    [GenericTypeArguments(typeof(CollectionsOfPrimitives))]
    [MemoryDiagnoser]
    [BenchmarkCategory(Categories.JsonSerializer)]
    public class Json_ToString<T>
    {
        private T value;

        [GlobalSetup]
        public void Setup() => value = DataGenerator.Generate<T>();

        [BenchmarkCategory(Categories.ThirdParty)]
        [Benchmark(Description = "Jil")]
        public string Jil_() => Jil.JSON.Serialize<T>(value, Jil.Options.ISO8601);

        [BenchmarkCategory(Categories.Libraries, Categories.ThirdParty)]
        [Benchmark(Description = "JSON.NET")]
        public string JsonNet_() => Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [BenchmarkCategory(Categories.ThirdParty)]
        [Benchmark(Description = "Utf8Json")]
        public string Utf8Json_() => Utf8Json.JsonSerializer.ToJsonString(value);
    }
}
