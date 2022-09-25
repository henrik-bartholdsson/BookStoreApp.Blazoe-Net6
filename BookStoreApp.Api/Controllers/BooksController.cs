
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Book;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.Api.Repositories;

namespace BookStoreApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(IBooksRepository context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            booksRepository = context;
            _mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            var booksDto = await booksRepository.GetAllBooksAsync();

            return Ok(booksDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            var bookDetailsDto = await booksRepository.GetBookAsync(id);

            if (bookDetailsDto == null)
            {
                return NotFound();
            }

            return bookDetailsDto;
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await booksRepository.GetAsync(id);

            if (book == null)
                return NotFound();

            if (String.IsNullOrEmpty(bookDto.ImageData) == false)
            {
                bookDto.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);

                var picName = Path.GetFileName(book.Image);
                var path = $"{_webHostEnvironment.WebRootPath}\\bookcoverimages\\{picName}";


                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            _mapper.Map(bookDto, book);


            try
            {
                await booksRepository.UpdateAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
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
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            if(String.IsNullOrEmpty(bookDto.ImageData) == false || String.IsNullOrEmpty(bookDto.OriginalImageName) == false)
            {
                book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
            }
            
            await booksRepository.AddAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await booksRepository.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await booksRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await booksRepository.Exists(id);
        }

        private string CreateFile(string imageBase64, string imageName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid().ToString()}{ext}";

            var path = $"{_webHostEnvironment.WebRootPath}\\bookcoverimages\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();


            return $"https://{url}/bookcoverimages/{fileName}";
        }
    }
}
