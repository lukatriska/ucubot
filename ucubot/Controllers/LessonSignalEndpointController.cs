using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;

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

            var dataTable = new DataTable();
            var queryCommand = new MySqlCommand("select * from lesson_signal", connection);
            var adapter = new MySqlDataAdapter(queryCommand);

            try
            {
                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
            }
            catch (Exception e)
            {
<<<<<<< HEAD
                Console.WriteLine(e);
            }

            var lessonSignalDtos = new List<LessonSignalDto>();

            foreach (DataRow row in dataTable.Rows)
            {
                var lessonSignalDto = new LessonSignalDto
                {
                    Id = (int) row["id"],
                    UserId = (string) row["user_id"],
                    Type = (LessonSignalType) row["signal_type"],
                    Timestamp = Convert.ToDateTime(row["time_stamp"])
                };

                lessonSignalDtos.Add(lessonSignalDto);
=======
            	conn.Open();
                var adapter = new MySqlDataAdapter("SELECT * FROM lesson_signal", conn);
                
                var dataset = new DataSet();
                
                adapter.Fill(dataset, "lesson_signal");

                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    var signalDto = new LessonSignalDto
                    {
                        Id = (int) row["id"],
                        Timestamp = (DateTime) row["timestamp_"],
                        Type = (LessonSignalType)Convert.ToInt32(row["signal_type"]),
                        UserId = (string) row["user_id"]
                    };
                    yield return signalDto;
                }
>>>>>>> c772387e191624a570e1095f08074b93fb033588
            }

            return lessonSignalDtos;
        }
<<<<<<< HEAD

=======
        
>>>>>>> c772387e191624a570e1095f08074b93fb033588
        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            var dataTable = new DataTable();
            var queryCommand = new MySqlCommand("select * from lesson_signal", connection);
            var adapter = new MySqlDataAdapter(queryCommand);

            try
            {
<<<<<<< HEAD
                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
=======
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM lesson_signal WHERE id = @id", conn);
                command.Parameters.AddWithValue("id", id);
                var adapter = new MySqlDataAdapter(command);
                
                var dataset = new DataSet();
                
                adapter.Fill(dataset, "lesson_signal");
                if (dataset.Tables[0].Rows.Count < 1)
                    return null;
                
                var row = dataset.Tables[0].Rows[0];
                var signalDto = new LessonSignalDto
                {
                	Timestamp = (DateTime) row["timestamp_"],
                    Type = (LessonSignalType) row["signal_type"],
                    UserId = (string) row["user_id"]
                };
                return signalDto;
>>>>>>> c772387e191624a570e1095f08074b93fb033588
            }


	        if (table.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];

            return new LessonSignalDto
            {
                Id = (int) row["id"],
                UserId = (string) row["user_id"],
                Type = (LessonSignalType) row["signal_type"],
                Timestamp = Convert.ToDateTime(row["time_stamp"])
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();

            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);


            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO lesson_signal (user_id, signal_type) VALUES (@user_id, @signal_type)";
            command.Parameters.Add(new MySqlParameter("user_id", userId));
            command.Parameters.Add(new MySqlParameter("signal_type", signalType));

            try
            {
<<<<<<< HEAD
                connection.Open();
=======
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText =
                    "INSERT INTO lesson_signal (user_id, signal_type) VALUES (@userId, @signalType);";
                command.Parameters.AddRange(new[]
                {
                	new MySqlParameter("userId", userId),
                    new MySqlParameter("signalType", signalType)
                });
>>>>>>> c772387e191624a570e1095f08074b93fb033588
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Accepted();
        }
<<<<<<< HEAD

=======
        
>>>>>>> c772387e191624a570e1095f08074b93fb033588
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM lesson_signal WHERE ID = @id;";
            command.Parameters.Add(new MySqlParameter("id", id));

            try
            {
<<<<<<< HEAD
                connection.Open();
=======
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText =
                    "DELETE FROM lesson_signal WHERE ID = @id;";
            	command.Parameters.Add(new MySqlParameter("id", id));
>>>>>>> c772387e191624a570e1095f08074b93fb033588
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Accepted();
        }
    }
}