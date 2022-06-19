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
                    testGetY(connection)
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
            return true;
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

        //static bool 
    }
}
