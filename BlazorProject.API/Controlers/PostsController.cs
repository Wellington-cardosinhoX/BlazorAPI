using BlazorProject.API.Data;
using BlazorProject.API.DTO;
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
                OrderByDescending(p => p.CreatedAt)
                .Select(p => new ResponsePostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Tag = p.Tag,
                    AuthorName = p.AuthorName,
                    CreatedAt = p.CreatedAt
                }).ToListAsync();


            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post =
                await _db.Posts.FindAsync(id);

            if (post is null)
            {
                return NotFound("Post não encontrado");
            }

            var dto = new ResponsePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Tag = post.Tag,
                AuthorName = post.AuthorName,
                CreatedAt = post.CreatedAt
            };

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest("Título não pode ser vazio");
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return BadRequest("Conteúdo não pode estar vazio");
            }

            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                Tag = dto.Tag,
                AuthorName = dto.AuthorName 
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();


            var reponse = new ResponsePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Tag = post.Tag,
                AuthorName = post.AuthorName,
                CreatedAt = post.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = post.Id}, reponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>? Update(int id, [FromBody] UpdatePostDTO dto)
        {
            var post = await _db.Posts.FindAsync(id);

            if (post is null)
            {
                return NotFound("Não foi encontrado este ID");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Título não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Conteúdo não pode ser vazio.");

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.Tag = dto.Tag;

            await _db.SaveChangesAsync();


            var response = new ResponsePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Tag = post.Tag,
                AuthorName = post.AuthorName,
                CreatedAt = post.CreatedAt
            };


            return Ok(response);
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
