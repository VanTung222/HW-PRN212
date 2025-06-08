using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DelegatesLinQ.Homework
{
    // Data models for the homework
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; } // 0-4.0 scale
        public string Semester { get; set; }
        public string Instructor { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// Homework 3: LINQ Data Processor
    /// 
    /// This is the most challenging homework requiring students to:
    /// 1. Use complex LINQ operations with multiple data sources
    /// 2. Implement custom extension methods
    /// 3. Create generic delegates and expressions
    /// 4. Handle advanced scenarios like pivot operations, statistical analysis
    /// 5. Implement caching and performance optimization
    /// 
    /// Advanced Requirements:
    /// - Custom LINQ extension methods
    /// - Expression trees and dynamic queries
    /// - Performance comparison between different approaches
    /// - Statistical calculations and reporting
    /// - Data transformation and pivot operations
    /// </summary>
    public class LinqDataProcessor
    {
        private List<Student> _students;

        public LinqDataProcessor()
        {
            _students = GenerateSampleData();
        }

        // BASIC REQUIREMENTS (using techniques from sample codes)
        public void BasicQueries()
        {
            // TODO: Implement basic LINQ queries similar to 6_8_LinQObject

            // 1. Find all students with GPA > 3.5
            // 2. Group students by major
            // 3. Calculate average GPA per major
            // 4. Find students enrolled in specific courses
            // 5. Sort students by enrollment date

            var highGPAStudents = _students.Where(s => s.GPA > 3.5).ToList();
            var studentsByMajor = _students.GroupBy(s => s.Major)
                                           .Select(g => new { Major = g.Key, Students = g.ToList() })
                                           .ToList();
            var averageGPAByMajor = _students.GroupBy(s => s.Major)
                                             .Select(g => new { Major = g.Key, AverageGPA = g.Average(s => s.GPA) })
                                             .ToList();
            var enrolledInCourses = _students.SelectMany(s => s.Courses)
                                            .Where(c => c.Name.Contains("Programming"))
                                            .Select(c => c.Name)
                                            .Distinct()
                                            .ToList();
            var sortedByEnrollment = _students.OrderBy(s => s.EnrollmentDate).ToList();

            Console.WriteLine("=== BASIC LINQ QUERIES ===");
            Console.WriteLine("Students with GPA > 3.5:");
            highGPAStudents.ForEach(s => Console.WriteLine($"{s.Name}: {s.GPA:F2}"));
            Console.WriteLine("\nStudents by Major:");
            studentsByMajor.ForEach(g => Console.WriteLine($"{g.Major}: {g.Students.Count} students"));
            Console.WriteLine("\nAverage GPA by Major:");
            averageGPAByMajor.ForEach(g => Console.WriteLine($"{g.Major}: {g.AverageGPA:F2}"));
            Console.WriteLine("\nProgramming Courses:");
            enrolledInCourses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nSorted by Enrollment Date:");
            sortedByEnrollment.ForEach(s => Console.WriteLine($"{s.Name}: {s.EnrollmentDate:yyyy-MM-dd}"));
        }

        // Challenge 1: Create custom extension methods
        public void CustomExtensionMethods()
        {
            Console.WriteLine("=== CUSTOM EXTENSION METHODS ===");

            // TODO: Implement extension methods
            // 1. CreateAverageGPAByMajor() extension method
            // 2. FilterByAgeRange(int min, int max) extension method  
            // 3. ToGradeReport() extension method that creates formatted output
            // 4. CalculateStatistics() extension method

            var highPerformers = _students.FilterByAgeRange(20, 25).Where(s => s.GPA > 3.5);
            Console.WriteLine("High GPA students aged 20-25:");
            foreach (var s in highPerformers)
                Console.WriteLine($"{s.Name}, Age: {s.Age}, GPA: {s.GPA:F2}");

            var avgByMajor = _students.AverageGPAByMajor();
            Console.WriteLine("\nAverage GPA by major:");
            foreach (var kvp in avgByMajor)
                Console.WriteLine($"{kvp.Key}: {kvp.Value:F2}");

            Console.WriteLine("\nGrade Reports:");
            foreach (var s in _students)
                Console.WriteLine($"\n{s.ToGradeReport()}");

            var stats = _students.CalculateStatistics();
            Console.WriteLine("\nGPA Statistics:");
            Console.WriteLine($"Mean: {stats.MeanGPA:F2}, Median: {stats.MedianGPA:F2}, StdDev: {stats.StdDeviation:F2}, Min: {stats.MinGPA}, Max: {stats.MaxGPA}");
        }

        // Challenge 2: Dynamic queries using Expression Trees
        public void DynamicQueries()
        {
            Console.WriteLine("=== DYNAMIC QUERIES ===");

            // TODO: Research Expression Trees
            // Implement a method that builds LINQ queries dynamically based on user input
            // Example: BuildDynamicFilter("GPA", ">", "3.5")
            // This requires understanding of Expression<Func<T, bool>>
            var results = DynamicFilter(_students, "GPA", ">", 3.5);
            foreach (var s in results)
                Console.WriteLine($"{s.Name} - GPA: {s.GPA}");
            // Students should implement:
            // 1. Dynamic filtering based on property name and value
            // 2. Dynamic sorting by any property
            // 3. Dynamic grouping operations
        }

        // Challenge 3: Statistical Analysis with Complex Aggregations
        public void StatisticalAnalysis()
        {
            Console.WriteLine("=== STATISTICAL ANALYSIS ===");

            // TODO: Implement complex statistical calculations
            // 1. Calculate median GPA (requires custom logic)
            // 2. Calculate standard deviation of GPAs
            // 3. Find correlation between age and GPA
            // 4. Identify outliers using statistical methods
            // 5. Create percentile rankings
            var medianGPA = _students.Select(s => s.GPA).Median();
            var stdDevGPA = _students.Select(s => s.GPA).StandardDeviation();
            var correlation = CalculateCorrelation(_students.Select(s => s.Age), _students.Select(s => s.GPA));
            Console.WriteLine($"Median GPA: {medianGPA:F2}, Standard Deviation: {stdDevGPA:F2}, Correlation (Age vs GPA): {correlation:F2}");

            // This requires research into statistical formulas and advanced LINQ usage
        }

        // Challenge 4: Data Pivot Operations
        public void PivotOperations()
        {
            Console.WriteLine("=== PIVOT OPERATIONS ===");

            // TODO: Research pivot table concepts
            // Create pivot tables showing:
            // 1. Students by Major vs GPA ranges (3.0-3.5, 3.5-4.0, etc.)
            // 2. Course enrollment by semester and major
            // 3. Grade distribution across instructors
            var pivot = _students
                .SelectMany(s => s.Courses, (student, course) => new { student.Major, course.Grade })
                .GroupBy(x => new { x.Major, Range = GetGradeRange(x.Grade) })
                .Select(g => new { g.Key.Major, g.Key.Range, Count = g.Count() })
                .ToList();

            foreach (var row in pivot)
                Console.WriteLine($"{row.Major} - {row.Range}: {row.Count} students");
            // This requires understanding of GroupBy with multiple keys and complex projections
        }

        // Sample data generator
        private List<Student> GenerateSampleData()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1, Name = "Alice Johnson", Age = 20, Major = "Computer Science",
                    GPA = 3.8, EnrollmentDate = new DateTime(2022, 9, 1),
                    Email = "alice.j@university.edu",
                    Address = new Address { City = "Seattle", State = "WA", ZipCode = "98101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS101", Name = "Intro to Programming", Credits = 3, Grade = 3.7, Semester = "Fall 2022", Instructor = "Dr. Smith" },
                        new Course { Code = "MATH201", Name = "Calculus II", Credits = 4, Grade = 3.9, Semester = "Fall 2022", Instructor = "Prof. Johnson" }
                    }
                },
                new Student
                {
                    Id = 2, Name = "Bob Wilson", Age = 22, Major = "Mathematics",
                    GPA = 3.2, EnrollmentDate = new DateTime(2021, 9, 1),
                    Email = "bob.w@university.edu",
                    Address = new Address { City = "Portland", State = "OR", ZipCode = "97201" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "MATH301", Name = "Linear Algebra", Credits = 3, Grade = 3.3, Semester = "Spring 2023", Instructor = "Dr. Brown" },
                        new Course { Code = "STAT101", Name = "Statistics", Credits = 3, Grade = 3.1, Semester = "Spring 2023", Instructor = "Prof. Davis" }
                    }
                },
                new Student
                {
                    Id = 3, Name = "Carol Davis", Age = 19, Major = "Computer Science",
                    GPA = 3.9, EnrollmentDate = new DateTime(2023, 9, 1),
                    Email = "carol.d@university.edu",
                    Address = new Address { City = "San Francisco", State = "CA", ZipCode = "94101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS102", Name = "Data Structures", Credits = 4, Grade = 4.0, Semester = "Fall 2023", Instructor = "Dr. Smith" },
                        new Course { Code = "CS201", Name = "Algorithms", Credits = 3, Grade = 3.8, Semester = "Fall 2023", Instructor = "Prof. Lee" }
                    }
                }
            };
        }

        // Helper methods
        private string GetGradeRange(double grade)
        {
            if (grade >= 3.5) return "3.5-4.0";
            if (grade >= 3.0) return "3.0-3.5";
            if (grade >= 2.5) return "2.5-3.0";
            return "Below 2.5";
        }

        private IEnumerable<Student> DynamicFilter(IEnumerable<Student> students, string property, string operation, double value)
        {
            var parameter = Expression.Parameter(typeof(Student), "s");
            var propertyExpression = Expression.Property(parameter, property);
            var constant = Expression.Constant(value);
            Expression comparison;

            switch (operation)
            {
                case ">":
                    comparison = Expression.GreaterThan(propertyExpression, constant);
                    break;
                case "<":
                    comparison = Expression.LessThan(propertyExpression, constant);
                    break;
                case "=":
                    comparison = Expression.Equal(propertyExpression, constant);
                    break;
                default:
                    throw new ArgumentException("Invalid operation");
            }

            var lambda = Expression.Lambda<Func<Student, bool>>(comparison, parameter);
            return students.AsQueryable().Where(lambda);
        }

        private double CalculateCorrelation(IEnumerable<int> x, IEnumerable<double> y)
        {
            var xList = x.Select(i => (double)i).ToList();
            var yList = y.ToList();
            int n = xList.Count;
            double sumX = xList.Sum();
            double sumY = yList.Sum();
            double sumXY = xList.Zip(yList, (a, b) => a * b).Sum();
            double sumX2 = xList.Sum(a => a * a);
            double sumY2 = yList.Sum(b => b * b);

            return (n * sumXY - sumX * sumY) /
                   Math.Sqrt((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY));
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 3: LINQ DATA PROCESSOR ===");
            Console.WriteLine();

            Console.WriteLine("BASIC REQUIREMENTS:");
            Console.WriteLine("1. Implement basic LINQ queries (filtering, grouping, sorting)");
            Console.WriteLine("2. Use SelectMany for course data");
            Console.WriteLine("3. Calculate averages and aggregations");
            Console.WriteLine();

            Console.WriteLine("ADVANCED REQUIREMENTS:");
            Console.WriteLine("1. Create custom LINQ extension methods");
            Console.WriteLine("2. Implement dynamic queries using Expression Trees");
            Console.WriteLine("3. Perform statistical analysis (median, std dev, correlation)");
            Console.WriteLine("4. Create pivot table operations");
            Console.WriteLine();

            LinqDataProcessor processor = new LinqDataProcessor();

            // Students should implement all these methods
            processor.BasicQueries();
            processor.CustomExtensionMethods();
            processor.DynamicQueries();
            processor.StatisticalAnalysis();
            processor.PivotOperations();

            Console.ReadKey();
        }
    }

    // TODO: Implement these extension methods
    public static class StudentExtensions
    {
        // Challenge: Implement custom extension methods
        public static IEnumerable<Student> FilterByAgeRange(this IEnumerable<Student> students, int minAge, int maxAge)
        {
            return students.Where(s => s.Age >= minAge && s.Age <= maxAge);
        }

        public static Dictionary<string, double> AverageGPAByMajor(this IEnumerable<Student> students)
        {
            return students.GroupBy(s => s.Major)
                          .ToDictionary(g => g.Key, g => g.Average(s => s.GPA));
        }

        public static string ToGradeReport(this Student student)
        {
            var report = $"Student: {student.Name}\nMajor: {student.Major}\nGPA: {student.GPA:F2}\nCourses:\n";
            foreach (var course in student.Courses)
            {
                report += $"  {course.Name} ({course.Semester}): {course.Grade:F2}\n";
            }
            return report;
        }

        public static StudentStatistics CalculateStatistics(this IEnumerable<Student> students)
        {
            var gpas = students.Select(s => s.GPA).ToList();
            return new StudentStatistics
            {
                MeanGPA = gpas.Average(),
                MedianGPA = gpas.Median(),
                StdDeviation = gpas.StandardDeviation(),
                MinGPA = gpas.Min(),
                MaxGPA = gpas.Max()
            };
        }

        public static double Median(this IEnumerable<double> source)
        {
            var sorted = source.OrderBy(x => x).ToList();
            int count = sorted.Count;
            if (count == 0) return 0;
            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
            return sorted[count / 2];
        }

        public static double StandardDeviation(this IEnumerable<double> source)
        {
            var values = source.ToList();
            if (!values.Any()) return 0;
            double avg = values.Average();
            double sum = values.Sum(x => Math.Pow(x - avg, 2));
            return Math.Sqrt(sum / values.Count);
        }
    }

    // TODO: Define this class for statistical operations
    public class StudentStatistics
    {
        // Properties for mean, median, mode, standard deviation, etc.
        public double MeanGPA { get; set; }
        public double MedianGPA { get; set; }
        public double StdDeviation { get; set; }
        public double MinGPA { get; set; }
        public double MaxGPA { get; set; }
    }
}