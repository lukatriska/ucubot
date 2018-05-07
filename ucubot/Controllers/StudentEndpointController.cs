using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointCntroller : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentEndpointCntroller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string queryCommand = "SELECT id AS Id, first_name AS FirstName, last_name AS LastName " +
                                        "user_id AS UserId FROM student";

            try
            {
                connection.Open();
                var students = connection.Query<Student>(queryCommand).AsList();
                connection.Close();
                return students;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string queryCommand = "SELECT id AS Id, first_name AS FirstName, last_name AS LastName " +
                                        "user_id AS UserId FROM student WHERE id = @id";

            try
            {
                connection.Open();
                var stud = connection.Query<Student>(queryCommand, new {id = id}).SingleOrDefault();
                connection.Close();
                return stud;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }        
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string insertText = "INSERT INTO student (first_name, last_name, user_id) " +
                                      "VALUES (@first_name, @last_name, @user_id)";
            
            try
            {
                connection.Open();
                connection.Execute(insertText, new{
                    first_name = student.FirstName, 
                    last_name = student.LastName, 
                    user_id = student.UserId});
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }

            return Accepted();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string updateText = "UPDATE student SET first_name = @first_name, last_name = @last_name, " +
                                      "user_id = @user_id WHERE id = @id";

            try
            {
                connection.Open();
                connection.Execute(updateText, new
                {
                    first_name = student.FirstName,
                    last_name = student.LastName,
                    user_id = student.UserId,
                    id = student.Id
                });
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return Accepted();
        }
        
        
        
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            
            const string deleteText = "DELETE FROM student WHERE id = @id;";
            
            try
            {
                connection.Open();
                connection.Execute(deleteText, new {id = id});
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }

            return Accepted();
        }
    }
}