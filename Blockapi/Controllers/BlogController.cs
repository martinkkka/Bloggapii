using Blockapi.Models;
using Blockapi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blockapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public string connectionString = "server=localhost;uid=root;password=;databse=blog";
        [HttpGet]
        public object GetBloggers()
        {
            List<Blogger> bloggers = new List<Blogger>();
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT*FROM blogger;";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();
            while (data.Read())
            {
                var blogger = new Blogger
                {
                    Id = data.GetInt32("id"),
                    Name = data.GetString("name"),
                    Email = data.GetString("email"),
                    Age = data.GetInt32("age"),
                    password = data.GetString("password"),
                    RegistrationTime = data.GetDateTime("RegistrationTime")
                };
                bloggers.Add(blogger);
            }
            connection.Close();
            return bloggers;

        }
        [HttpPost]
        public object AddNemBlogger([FromBody]AddNewBloggerDTOs addNewBloggerDTOs)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = @"INSERT INTO `blogger` (`name`, `email`, `age`, `password`, `registrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@name", addNewBloggerDTOs.Name);
            cmd.Parameters.AddWithValue("@email", addNewBloggerDTOs.Email);
            cmd.Parameters.AddWithValue("@age", addNewBloggerDTOs.Age);
            cmd.Parameters.AddWithValue("@password", addNewBloggerDTOs.password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres felvétel.", result = addNewBloggerDTOs }; ;
            
        }
        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"DELETE FROM `blogger` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery ();
            connection.Close();

            return new { message = "Sikeres Törlés", result = "" };
        }

        [HttpPut]

        public object UpdateBlogger([FromQuery] int id, UpdateBloggerDto updateBloggerDto)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            var sql = @"UPDATE `blogger` SET `name`=@name,`email`=@email,`age`=@age,`password`=@password WHERE `id`=@id";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDto.password);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Close();
            return new { message = "Sikeres frissités", result = "" };
        }

        [HttpGet("byId")]

        public object GetBloggerByid(int id)
        {
            var connection = new MySqlConnection(connectionString);
            string sql = @"SELECT *FROM `blogger` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connection);
            var datareader = cmd.ExecuteReader();
            object? data = null;
            if (datareader.Read() == true)
            {
                var blogger = new Blogger()
                {
                    Id = datareader.GetInt32("id"),
                    Name = datareader.GetString("name"),
                    Email = datareader.GetString("email"),
                    Age = datareader.GetInt32("age"),
                    password = datareader.GetString("password"),
                    RegistrationTime = datareader.GetDateTime(5)
                };
                data = new { message = "siker", result = blogger };
            }
            else
            {
              data = new { message = "nem siker", result = "" };
            }
            connection.Close();
            return data;
            

            

        }
    }
}
