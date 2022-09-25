using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.Api.Repositories;
using BookStoreApp.Api.Models;

namespace BookStoreApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<VirtualizeResponse<AuthorReadOnlyDto>>> GetAuthors([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                return await authorRepository.GetAllAsync<AuthorReadOnlyDto>(queryParameters);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Server error...");
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<AuthorReadOnlyDto>>> GetAuthors()
        {
            try
            {
                var authors = await authorRepository.GetAllAsync();
                var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authors);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Server error...");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
            try
            {
                var author = await authorRepository.GetAuthorDetailsAsync(id);

                if (author == null)
                {
                    return NotFound();
                }


                return Ok(author);

            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorUpdateDto)
        {

            if (id != authorUpdateDto.Id)
            {
                return BadRequest();
            }

            var author = await authorRepository.GetAsync(id);

            if (author == null)
                return NotFound();

            _mapper.Map(authorUpdateDto, author);

            try
            {
                await authorRepository.UpdateAsync(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await authorRepository.AddAsync(author);

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await authorRepository.GetAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                await authorRepository.DeleteAsync(author.Id);
                return NoContent();
            }
            catch (Exception)
            {

                return StatusCode(500, "Server error.");
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await authorRepository.Exists(id);
        }
    }
}
