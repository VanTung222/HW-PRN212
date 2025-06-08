using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DelegatesLinQ.Homework
{
    // Delegate types for processing pipeline
    public delegate string DataProcessor(string input);
    public delegate void ProcessingEventHandler(string stage, string input, string output);

    /// <summary>
    /// Homework 2: Custom Delegate Chain
    /// Create a data processing pipeline using multicast delegates.
    /// 
    /// Requirements:
    /// 1. Create a processing pipeline that transforms text data through multiple steps
    /// 2. Use multicast delegates to chain processing operations
    /// 3. Add logging/monitoring capabilities using events
    /// 4. Demonstrate adding and removing processors from the chain
    /// 5. Handle errors in the processing chain
    /// 
    /// Techniques used: Similar to 6_2_MulticastDelegate
    /// - Multicast delegate chaining
    /// - Delegate combination and removal
    /// - Error handling in delegate chains
    /// </summary>
    public class DataProcessingPipeline
    {
        // TODO: Declare events for monitoring the processing
        // public event ProcessingEventHandler ProcessingStageCompleted;
        public event ProcessingEventHandler ProcessingStageCompleted;

        // Individual processing methods that students need to implement
        public static string RemoveSpaces(string input) => input.Replace(" ", "");

        public static string ToUpperCase(string input) => input.ToUpper();

        public static string AddTimestamp(string input) => $"[{DateTime.Now:HH:mm:ss}] {input}";

        public static string ReverseString(string input)
        {
            // TODO: Reverse the characters in the input string
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string EncodeBase64(string input)
        {
            // TODO: Encode the input string to Base64
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string ValidateInput(string input)
        {
            // TODO: Validate input (throw exception if null or empty)
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input cannot be null or empty.");
            }
            return input;
        }

        // Method to process data through the pipeline
        public string ProcessData(string input, DataProcessor pipeline)
        {
            // TODO: Process input through the pipeline and raise events
            // Handle any exceptions that occur during processing
            string currentInput = input;
            string currentOutput = input;

            foreach (DataProcessor handler in pipeline.GetInvocationList())
            {
                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    currentOutput = handler(currentInput);
                    stopwatch.Stop();

                    OnProcessingStageCompleted(handler.Method.Name, currentInput, currentOutput);
                    currentInput = currentOutput;
                }
                catch (Exception ex)
                {
                    OnProcessingStageCompleted(handler.Method.Name, currentInput, $"ERROR: {ex.Message}");
                    throw;
                }
            }

            return currentOutput;
        }

        // TODO: Add method to raise processing events
        // protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
        protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
        {
            ProcessingStageCompleted?.Invoke(stage, input, output);
        }
    }

    // Logger class to monitor processing
    public class ProcessingLogger
    {
        // TODO: Implement event handler to log processing stages
        // public void OnProcessingStageCompleted(string stage, string input, string output)
        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Stage: {stage}, Input: '{input}', Output: '{output}'");
        }
    }

    // Performance monitor class
    public class PerformanceMonitor
    {
        // TODO: Track processing times and performance metrics
        // public void OnProcessingStageCompleted(string stage, string input, string output)
        // public void DisplayStatistics()
        private readonly Dictionary<string, List<long>> performanceData = new();

        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            // In a real-world scenario, you would time the actual handler.
            // For demo purposes, just simulate a small random delay
            Random rand = new();
            long simulatedTime = rand.Next(1, 10); // simulate 1-10ms delay

            if (!performanceData.ContainsKey(stage))
                performanceData[stage] = new List<long>();

            performanceData[stage].Add(simulatedTime);
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== PERFORMANCE STATISTICS ===");
            foreach (var entry in performanceData)
            {
                long total = 0;
                foreach (var t in entry.Value)
                    total += t;

                double avg = entry.Value.Count > 0 ? (double)total / entry.Value.Count : 0;
                Console.WriteLine($"{entry.Key}: Count = {entry.Value.Count}, Avg = {avg:F2} ms");
            }
        }
    }

    public class DelegateChain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 2: CUSTOM DELEGATE CHAIN ===");
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Implement the DataProcessingPipeline class");
            Console.WriteLine("2. Implement all processing methods (RemoveSpaces, ToUpperCase, etc.)");
            Console.WriteLine("3. Create a multicast delegate chain for processing");
            Console.WriteLine("4. Add logging and monitoring capabilities");
            Console.WriteLine("5. Demonstrate adding/removing processors from the chain");
            Console.WriteLine();

            // TODO: Students should implement the following:
            DataProcessingPipeline pipeline = new DataProcessingPipeline();
            ProcessingLogger logger = new ProcessingLogger();
            PerformanceMonitor monitor = new PerformanceMonitor();

            // Subscribe to events
            pipeline.ProcessingStageCompleted += logger.OnProcessingStageCompleted;
            pipeline.ProcessingStageCompleted += monitor.OnProcessingStageCompleted;

            // Create processing chain
            DataProcessor processingChain = DataProcessingPipeline.ValidateInput;
            processingChain += DataProcessingPipeline.RemoveSpaces;
            processingChain += DataProcessingPipeline.ToUpperCase;
            processingChain += DataProcessingPipeline.AddTimestamp;

            // Test the pipeline
            string testInput = "Hello World from C#";
            Console.WriteLine($"Input: {testInput}");
            
            string result = pipeline.ProcessData(testInput, processingChain);
            Console.WriteLine($"Output: {result}");

            // Demonstrate adding more processors
            processingChain += DataProcessingPipeline.ReverseString;
            processingChain += DataProcessingPipeline.EncodeBase64;

            // Test again with extended pipeline
            result = pipeline.ProcessData("Extended Pipeline Test", processingChain);
            Console.WriteLine($"Extended Output: {result}");

            // Demonstrate removing a processor
            processingChain -= DataProcessingPipeline.ReverseString;
            result = pipeline.ProcessData("Without Reverse", processingChain);
            Console.WriteLine($"Modified Output: {result}");

            // Display performance statistics
            monitor.DisplayStatistics();

            // Error handling test
            try
            {
                result = pipeline.ProcessData(null, processingChain); // Should handle null input
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handled: {ex.Message}");
            }

            Console.WriteLine("Please implement the missing code to complete this homework!");

            // Example of what the complete implementation should demonstrate:
            Console.WriteLine("\nExpected behavior:");
            Console.WriteLine("1. Chain multiple text processing operations");
            Console.WriteLine("2. Log each processing stage");
            Console.WriteLine("3. Monitor performance of each operation");
            Console.WriteLine("4. Handle errors gracefully");
            Console.WriteLine("5. Allow dynamic modification of the processing chain");

            Console.ReadKey();
        }
    }
}