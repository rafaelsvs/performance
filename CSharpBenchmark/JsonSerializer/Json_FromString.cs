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
    public class Json_FromString<T>
    {
        private string serialized;

        [GlobalSetup(Target = nameof(Jil_))]
        public void SetupJil() => serialized = Jil.JSON.Serialize<T>(DataGenerator.Generate<T>(), Jil.Options.ISO8601);

        [BenchmarkCategory(Categories.ThirdParty)]
        [Benchmark(Description = "Jil")]
        public T Jil_() => Jil.JSON.Deserialize<T>(serialized, Jil.Options.ISO8601);

        [GlobalSetup(Target = nameof(JsonNet_))]
        public void SerializeJsonNet() => serialized = Newtonsoft.Json.JsonConvert.SerializeObject(DataGenerator.Generate<T>());

        [BenchmarkCategory(Categories.Libraries, Categories.ThirdParty)]
        [Benchmark(Description = "JSON.NET")]
        public T JsonNet_() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);

        [GlobalSetup(Target = nameof(Utf8Json_))]
        public void SerializeUtf8Json_() => serialized = Utf8Json.JsonSerializer.ToJsonString(DataGenerator.Generate<T>());

        [BenchmarkCategory(Categories.ThirdParty)]
        [Benchmark(Description = "Utf8Json")]
        public T Utf8Json_() => Utf8Json.JsonSerializer.Deserialize<T>(serialized);

        [GlobalSetup(Target = nameof(SystemTextJson_))]
        public void SerializeSystemTextJsons_() => serialized = System.Text.Json.JsonSerializer.Serialize(DataGenerator.Generate<T>());

        [BenchmarkCategory(Categories.Libraries)]
        [Benchmark(Description = "SystemTextJson")]
        public T SystemTextJson_() => System.Text.Json.JsonSerializer.Deserialize<T>(serialized);
    }
}
