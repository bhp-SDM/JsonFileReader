using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace JsonFileReader
{
    class Program
    {
        private const string FILE_NAME = "../../../../../ratings.json";  // Change the path

        static void Main(string[] args)
        {
            List<BEReview> ratings = new ();

            Console.Write("Converting Json file to objects... ");

            Stopwatch sw = Stopwatch.StartNew();

            using (StreamReader streamReader = new StreamReader(FILE_NAME))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                try
                {
                    ratings = serializer.Deserialize<List<BEReview>>(reader);
                    //    while (reader.Read())
                    //    {
                    //        if (reader.TokenType == JsonToken.StartObject)
                    //        {
                    //            BEReview mr = ReadOneBEReview(reader);
                    //            ratings.Add(mr);
                    //        }
                    //    }
                }
                catch (JsonReaderException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            sw.Stop();
            Console.WriteLine($"Done. Time = {sw.Elapsed.TotalSeconds:f4} sec.");

            Dictionary<int, List<BEReview>> Reviewers = new Dictionary<int, List<BEReview>>();
            Dictionary<int, List<BEReview>> Movies = new Dictionary<int, List<BEReview>>();

            Console.Write("Indexing Reviews... ");
            sw.Restart();
            foreach (BEReview m in ratings)
            {
                if (!Reviewers.ContainsKey(m.Reviewer))
                    Reviewers[m.Reviewer] = new List<BEReview>();
                Reviewers[m.Reviewer].Add(m);

                if (!Movies.ContainsKey(m.Movie))
                    Movies[m.Movie] = new List<BEReview>();
                Movies[m.Movie].Add(m);
            }
            sw.Stop();
            Console.WriteLine($"Done. Time = {sw.Elapsed.TotalSeconds:f4} sec.");

            foreach (KeyValuePair<int, List<BEReview>> kv in Reviewers)
            {
                Console.WriteLine("Reviewer: {0,4} has reviewed {1,6} movies.", kv.Key, kv.Value.Count);
            }
            Console.ReadKey();
        }

        private static BEReview ReadOneBEReview(JsonTextReader reader)
        {
            BEReview m = new BEReview();
            for (int i = 0; i < 4; i++)
            {
                reader.Read();
                switch (reader.Value)
                {
                    case "Reviewer": m.Reviewer = (int)reader.ReadAsInt32(); break;
                    case "Movie": m.Movie = (int)reader.ReadAsInt32(); break;
                    case "Grade": m.Grade = (int)reader.ReadAsInt32(); break;
                    case "Date": m.ReviewDate = (DateTime)reader.ReadAsDateTime(); break;
                    default: throw new InvalidDataException("no such token: " + reader.Value);
                }
            }
            return m;
        }
    }
}
