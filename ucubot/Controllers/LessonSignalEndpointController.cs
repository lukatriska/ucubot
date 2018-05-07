using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        public LessonSignalEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string queryCommand = "SELECT student_id AS Id, timestamp AS Timestamp, signal_type AS Type, " +
                                        "student_id AS UserId FROM lesson_signal LEFT JOIN student " +
                                        "ON lesson_signal.student_id = student.id";

            try
            {
                connection.Open();
                var lessonSignalDtos = connection.Query<LessonSignalDto>(queryCommand).AsList();
                connection.Close();
                return lessonSignalDtos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string queryCommand = "SELECT student_id AS Id, timestamp AS Timestamp, signal_type AS Type, " +
                                        "student_id AS UserId FROM lesson_signal LEFT JOIN student " +
                                        "ON lesson_signal.student_id = student.id WHERE lesson_signal.id = @id";

            try
            {
                connection.Open();
                var lessonSignalDtos = connection.Query<LessonSignalDto>(queryCommand, new {id = id}).SingleOrDefault();
                connection.Close();
                return lessonSignalDtos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }        
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();

            
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            const string selectText = "SELECT id FROM student WHERE user_id = @userId";
            const string insertText = "INSERT INTO lesson_signal (user_id, signal_type) VALUES (@user_id, @signal_type)";
            
            try
            {
                connection.Open();
                var details = connection.Query(selectText, new{userId = userId}).SingleOrDefault();
                if (details != null) connection.Execute(insertText, new
                {
                    user_id = details.id, 
                    signal_type = signalType
                });
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            
            const string deleteText = "DELETE FROM lesson_signal WHERE student_id = @id;";
            
            try
            {
                connection.Open();
                connection.Execute(deleteText, new {id = id});
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

            return Accepted();
        }
    }
}