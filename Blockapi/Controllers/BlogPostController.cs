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
        [HttpGet("{id}")]
        public object GetBlogPost(int id)
        {
            blogpost blogpost = null;

            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT * FROM `blogpost` WHERE id = @id;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                blogpost = new blogpost
                {
                    Id = data.GetInt32("id"),
                    Title = data.GetString("title"),
                    Content = data.GetString("content"),
                    postTime = data.GetDateTime("postTime"),
                    updateTime = data.GetDateTime("updateTime"),
                    blogId = data.GetInt32("blogId")
                };
            }

            connection.Close();

            if (blogpost == null)
            {
                return NotFound("Nincs ilyen blogpost.");
            }

            return blogpost;
        }
    
    [HttpPost]
        public object CreateBlogPost(blogpost blogpost)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"
                INSERT INTO blogpost
                (title, content, postTime, updateTime, blogId)
                VALUES
                (@title, @content, @postTime, @updateTime, @blogId);
            ";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@title", blogpost.Title);
            cmd.Parameters.AddWithValue("@content", blogpost.Content);
            cmd.Parameters.AddWithValue("@postTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@blogId", blogpost.blogId);

            cmd.ExecuteNonQuery();

            blogpost.Id = (int)cmd.LastInsertedId;
            blogpost.postTime = DateTime.Now;
            blogpost.updateTime = DateTime.Now;

            connection.Close();

            return Ok(blogpost);
        }
    }
}
