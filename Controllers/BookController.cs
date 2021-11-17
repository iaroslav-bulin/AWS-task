using DynamoDBApi.Models;
using DynamoDBApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DynamoDBApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookManager;
        private readonly ILogger<BookController> _logger;
        private readonly Func<string, string> _logMsg = methodName => String.Format("{0} method is called", methodName);

        public BookController(IBookService bookManager, ILogger<BookController> logger)
        {
            _bookManager = bookManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            _logger.LogInformation(_logMsg("GetBooks"));
            return Ok(await _bookManager.GetBooks());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute]string id)
        {
            _logger.LogInformation(_logMsg("GetBook"));
            return Ok(await _bookManager.GetBook(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody]Book book)
        {
            _logger.LogInformation(_logMsg("AddBook"));
            return Ok(await _bookManager.AddBook(book));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            _logger.LogInformation(_logMsg("DeleteBook"));
            await _bookManager.DeleteBook(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditBook([FromBody]Book book)
        {
            _logger.LogInformation(_logMsg("EditBook"));
            return Ok(await _bookManager.EditBook(book));
        }
    }
}
