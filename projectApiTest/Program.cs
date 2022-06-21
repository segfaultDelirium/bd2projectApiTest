using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace projectApiTest
{
    class Program
    {
        static string connectionString = @"Data Source=.;" +
               "INITIAL CATALOG=projekttry0;" +
               "Integrated Security=SSPI";

        static string sqlcommand = @"use Lab6db
select * from  sys.databases";

        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                List<bool> testResults = new List<bool>()
                {
                    testTableWithPoints(connection),
                    testGetX(connection),
                    testGetY(connection),
                    testCalculateDistance(connection),
                    testCalculateArea(connection),
                    testIsPointInArea(connection),

                };

                if(testResults.Where(result => result == false).ToArray().Length == 0)
                {
                    Console.WriteLine("all tests passed");
                }
                else
                {
                    Console.WriteLine("some tests have failed");
                }
                
            }

                Console.ReadKey();
        }

        static bool testTableWithPoints(SqlConnection connection)
        {
            Console.WriteLine("start of testTableWithPoints");
            bool result = true;
            List<string> points = new List<string>()
            {
                "2; 4",
                "0; 0",
                "2.24; 4.123",
                "-2.24; -4.123"
            };
            List<string> lines = points.Select(point => $"('{point}')").ToList();
            
            string stringCommand = $@"
use projekttry0;
create table PointsUnitTest(p Point);
insert into PointsUnitTest values
{String.Join(",\n", lines)};
select p.ToString() as p from PointsUnitTest;

drop table PointsUnitTest;
";
            SqlCommand command = new SqlCommand(stringCommand, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int lineCounter = 0;
                while (reader.Read())
                {

                    if(points[lineCounter] != reader["p"].ToString())
                    {
                        Console.WriteLine($"test failed: {points[lineCounter]} is not equal to {reader["p"]}");
                        result = false;
                    }
                    else
                    {
                        Console.WriteLine("OK");
                    }
                    lineCounter += 1;
                }
            }
            if(result) Console.WriteLine("testTableWithPoints unit test passed");
            return result;
        }

        static bool testGetX(SqlConnection connection)
        {
            Console.WriteLine("start of testGetX");
            bool result = true;
            List<string> points = new List<string>()
            {
                "2; 4",
                "0; 0",
                "2.24; 4.123",
                "-2.24; -4.123"
            };
            List<string> lines = points.Select(point => $"('{point}')").ToList();
            string stringCommand = $@"
                use projekttry0;
                create table PointsUnitTest(p Point);
                insert into PointsUnitTest values
                {String.Join(",\n", lines)};
                select p.getX() as getX from PointsUnitTest;

                drop table PointsUnitTest;
                ";
            //Console.WriteLine(stringCommand);
            SqlCommand command = new SqlCommand(stringCommand, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int lineCounter = 0;
                while (reader.Read())
                {
                    string expectedX = points[lineCounter].Split(';')[0].Trim();
                    string readX = reader["getX"].ToString().Trim();
                    if (expectedX != readX)
                    {
                        Console.WriteLine($"test failed: {expectedX} is not equal to {readX}");
                        result = false;
                    }
                    else
                    {
                        Console.WriteLine("OK");
                    }
                    lineCounter += 1;
                }
            }
            if (result) Console.WriteLine("testGetX unit test passed");
            return true;
        }
        static bool testGetY(SqlConnection connection)
        {
            Console.WriteLine("start of testGetY");
            bool result = true;
            List<string> points = new List<string>()
            {
                "2; 4",
                "0; 0",
                "2.24; 4.123",
                "-2.24; -4.123"
            };
            List<string> lines = points.Select(point => $"('{point}')").ToList();
            string stringCommand = $@"
                use projekttry0;
                create table PointsUnitTest(p Point);
                insert into PointsUnitTest values
                {String.Join(",\n", lines)};
                select p.getY() as getY from PointsUnitTest;

                drop table PointsUnitTest;
                ";
            //Console.WriteLine(stringCommand);
            SqlCommand command = new SqlCommand(stringCommand, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                int lineCounter = 0;
                while (reader.Read())
                {
                    string expectedY = points[lineCounter].Split(';')[1].Trim();
                    string readY = reader["getY"].ToString().Trim();
                    if (expectedY != readY)
                    {
                        Console.WriteLine($"test failed: {expectedY} is not equal to {readY}");
                        result = false;
                    }
                    else
                    {
                        Console.WriteLine("OK");
                    }
                    lineCounter += 1;
                }
            }
            if (result) Console.WriteLine("testGetY unit test passed");
            return true;
        }

        static bool testCalculateDistance(SqlConnection connection)
        {
            Console.WriteLine("start of testCalculateDistance");
            bool result = true;
            List<Tuple<string, string, double>> pointsAndDistance = new List<Tuple<string, string, double>>()
            {
                new Tuple<string, string, double>("0;0", "0;15", 15),
                new Tuple<string, string, double>("0;15", "0;0", 15),
                new Tuple<string, string, double>("-15;0", "0;0", 15),
                new Tuple<string, string, double>("15;0", "0;15", 15 * Math.Sqrt(2)),
                new Tuple<string, string, double>("-15;0", "15;0", 30),
            };

            List<string> lines = pointsAndDistance.Select(tuple => $"('{tuple.Item1}', '{tuple.Item2}')").ToList();
            for(int i = 0; i < pointsAndDistance.Count; i++)
            {
                string stringCommand = $@"
                use projekttry0;
                select dbo.calculateDistance{lines[i]} as distance;
                ";
                SqlCommand command = new SqlCommand(stringCommand, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double expectedDistance = Convert.ToDouble(pointsAndDistance[i].Item3);
                        double distance = Convert.ToDouble(reader["distance"]);
                        if (Math.Abs(expectedDistance - distance) > 0.0005)
                        {
                            Console.WriteLine($"test failed: {expectedDistance} is not equal to {distance}");
                            result = false;
                        }
                        else
                        {
                            Console.WriteLine("OK");
                        }
                    }
                    
                }
            }
            if (result) Console.WriteLine("testCalculateDistance unit test passed");
            return true;
        }

        static bool testCalculateArea(SqlConnection connection)
        {
            Console.WriteLine("start of testCalculateArea");
            bool result = true;
            List<Tuple<string, string, string, double>> pointsAndDistance = new List<Tuple<string, string, string, double>>()
            {
                new Tuple<string, string, string, double>("0;0", "0;15", "15;0", 15 * 15 /2.0),
                new Tuple<string, string, string, double>("-15;0", "0;-15", "0;15", 30 * 15.0/2.0),
            };

            List<string> lines = pointsAndDistance.Select(tuple => $"('{tuple.Item1}', '{tuple.Item2}', '{tuple.Item3}')").ToList();
            for (int i = 0; i < pointsAndDistance.Count; i++)
            {
                string stringCommand = $@"
                use projekttry0;
                select dbo.calculateArea{lines[i]} as area;
                ";
                SqlCommand command = new SqlCommand(stringCommand, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double expectedArea = Convert.ToDouble(pointsAndDistance[i].Item4);
                        double area = Convert.ToDouble(reader["area"]);
                        if (Math.Abs(expectedArea - area) > 0.0005)
                        {
                            Console.WriteLine($"test failed: {expectedArea} is not equal to {area}");
                            result = false;
                        }
                        else
                        {
                            Console.WriteLine("OK");
                        }
                    }

                }
            }
            if (result) Console.WriteLine("testCalculateArea unit test passed");
            return result;
        }

        static bool testIsPointInArea(SqlConnection connection)
        {
            Console.WriteLine("start of testIsPointInArea");
            bool result = true;
            List<Tuple<string, string, string, string, bool>> pointsAndDistance = new List<Tuple<string, string, string, string, bool>>()
            {
                new Tuple<string, string, string, string, bool>("7;7", "0;0", "0;15", "15;0", true),
                new Tuple<string, string, string, string, bool>("-1;3", "-15;0", "0;-15", "0;15", true),
                new Tuple<string, string, string, string, bool>("-34;3", "-15;0", "0;-15", "0;15", false),
            };

            List<string> lines = pointsAndDistance.Select(tuple => $"('{tuple.Item1}', '{tuple.Item2}', '{tuple.Item3}', '{tuple.Item4}')").ToList();
            for (int i = 0; i < pointsAndDistance.Count; i++)
            {
                string stringCommand = $@"
                use projekttry0;
                select dbo.isPointInArea{lines[i]} as isPointInArea;
                ";
                SqlCommand command = new SqlCommand(stringCommand, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int expectedIsInArea = pointsAndDistance[i].Item5 ? 1 : 0;
                        int isInArea = Convert.ToInt32(reader["isPointInArea"]);
                        if (expectedIsInArea != isInArea)
                        {
                            Console.WriteLine($"test failed: {expectedIsInArea} is not equal to {isInArea}");
                            result = false;
                        }
                        else
                        {
                            Console.WriteLine("OK");
                        }
                    }
                }
            }
            if (result) Console.WriteLine("testIsPointInArea unit test passed");
            return result;
        }
    }
}
