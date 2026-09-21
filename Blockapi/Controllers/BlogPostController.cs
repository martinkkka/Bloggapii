using Blockapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Blockapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {

        public string connectionString =
            "server=localhost;uid=root;password=;database=blog";

        [HttpGet]
        public object GetBlogPosts()
        {

            List<blogpost> blogposts = new List<blogpost>();

            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT * FROM `blogpost`;";

            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var blogpost = new blogpost
                {
                    Id = data.GetInt32("id"),
                    Title = data.GetString("title"),
                    Content = data.GetString("content"),
                    postTime = data.GetDateTime("postTime"),
                    updateTime = data.GetDateTime("updateTime"),
                    blogId = data.GetInt32("blogId")
                };

                blogposts.Add(blogpost);
            }

            connection.Close();

            return blogposts;
        }
    }
}
