using Humanizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UserStory
{
    public class Story<T> where T : class
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        
        public TimeSpan Duration { get; private set; }
        public Result Result { get; private set; }

        private readonly List<Scenario<T>> _scenarios;
        private bool _continueOnFailed = false;
        private string _asA;
        private string _iWantTo;
        private string _soThatICan;

        private static readonly SequentialIdGenerator idGeneratorForStory = new SequentialIdGenerator();
        private readonly SequentialIdGenerator idGeneratorForScenario = new SequentialIdGenerator();

        public Story(T testObject, string title = null, string id = null, string description = null, string asA = null, string iWantTo = null, string soThatICan = null, bool continueOnFailed = false)
        {
            Title = title ?? testObject.GetType().Name.Humanize();

            Id = id ?? $"{idGeneratorForStory.NewId()}";

            Description = description ?? testObject.GetType().Namespace.Humanize();

            AsA = asA;
            IWantTo = iWantTo;
            SoThatICan = soThatICan;

            _continueOnFailed = continueOnFailed;

            _scenarios = new List<Scenario<T>>();
        }

        public string AsA
        {
            get { return _asA; }
            set { _asA = $"As a {value}"; }
        }

        public string IWantTo
        {
            get { return _iWantTo; }
            set { _iWantTo = $"I want to {value}"; }
        }

        public string SoThatICan
        {
            get { return _soThatICan; }
            set { _soThatICan = $"So that I can {value}"; }
        }

        public Story<T> AddScenario(Scenario<T> scenario)
        {
            if (string.IsNullOrEmpty(scenario.Id))
                scenario.Id = $"{idGeneratorForScenario.NewId()}";

            _scenarios.Add(scenario);

            return this;
        }

        public Result Run()
        {
            Result = Result.Running;
            Duration = TimeSpan.Zero;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine($"Story {Id}: {Title}");
                Console.WriteLine($"Description: {Description}");

                if (!string.IsNullOrEmpty(AsA))
                    Console.WriteLine($"\t{AsA}");

                if (!string.IsNullOrEmpty(IWantTo))
                    Console.WriteLine($"\t{IWantTo}");

                if (!string.IsNullOrEmpty(SoThatICan))
                    Console.WriteLine($"\t{SoThatICan}");

                foreach (var scenario in _scenarios)
                {
                    Console.WriteLine();
                    try
                    {
                        scenario.Run();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"{scenario.Title} {scenario.Result.ToString().ToLower()}");

                        if (!_continueOnFailed)
                            throw;
                    }
                    finally
                    {
                        //Console.WriteLine($"{scenario.Title} {scenario.Result.ToString().ToLower()}");
                    }
                }

                if (_scenarios.All(x => x.Result == Result.Success))
                    Result = Result.Success;

                else
                    Result = Result.Failed;
            }
            catch (Exception)
            {
                Result = Result.Failed;
                throw;
            }
            finally
            {
                sw.Stop();
                Duration = sw.Elapsed;
                Console.WriteLine($"\nResult: {Result.ToString().ToUpper()}");
            }

            return Result;
        }
    }
}
