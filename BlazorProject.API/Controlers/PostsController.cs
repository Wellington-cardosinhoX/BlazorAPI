using BlazorProject.API.Data;
using BlazorProject.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlazorProject.API.Controlers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PostsController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/posts

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _db.Posts.
                OrderByDescending(p => p.CreatedAt).ToListAsync();


            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _db.Posts.FindAsync(id);

            if (post is null)
            {
                return NotFound("Post não encontrado");
            }

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title))
            {
                return BadRequest("Título não pode ser vazio");
            }

            if (string.IsNullOrWhiteSpace(post.Content))
            {
                return BadRequest("Conteúdo não pode estar vazio");
            }

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = post.Id}, post);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var removePost = await _db.Posts.FindAsync(id);

            if (removePost is null)
            {
                return NotFound("Não foi encontrado");
            }

            _db.Posts.Remove(removePost);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
