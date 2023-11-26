using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/books")]
public class BookApiController : ControllerBase
{
    private readonly IGenericRepository<Book> _bookRepository;

    public BookApiController(IGenericRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet(Name = "GetBookListIndex")]
    public async Task<ActionResult<IEnumerable<BookViewModel>>> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            var books = await _bookRepository.GetAllAsync();

            var totalItems = books.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var bookViewModels = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(book => new BookViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    PublicationDate = book.PublicationDate,
                    Genre = book.Genre,
                    AuthorId = book.AuthorId,
                    LibraryBranchId = book.LibraryBranchId
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return Ok(new
            {
                Books = bookViewModels,
                PaginationInfo = paginationInfo
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost(Name = "CreateBook")]
    public async Task<IActionResult> Create([FromBody] BookViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var book = new Book
            {
                Title = vm.Title,
                ISBN = vm.ISBN,
                PublicationDate = vm.PublicationDate,
                Genre = vm.Genre,
                AuthorId = vm.AuthorId,
                LibraryBranchId = vm.LibraryBranchId,
                CreatedAt = DateTime.UtcNow
            };
            _bookRepository.Add(book);
            await _bookRepository.SaveAsync();

            return Ok(vm);
        }

        return BadRequest(ModelState);
    }

    [HttpGet("{id}", Name = "GetBookById")]
    public async Task<ActionResult<BookViewModel>> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest("ID cannot be null");
        }

        var book = await _bookRepository.GetByIdAsync(id.Value);
        if (book == null)
        {
            return NotFound();
        }

        var bookViewModel = new BookViewModel
        {
            BookId = book.BookId,
            Title = book.Title,
            ISBN = book.ISBN,
            PublicationDate = book.PublicationDate,
            Genre = book.Genre,
            AuthorId = book.AuthorId,
            LibraryBranchId = book.LibraryBranchId,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };

        return Ok(bookViewModel);
    }

    [HttpPut("{id}", Name = "EditBook")]
    public async Task<IActionResult> Edit(int id, [FromBody] BookViewModel vm)
    {
        if (id != vm.BookId)
        {
            return BadRequest("ID in the URL does not match ID in the request body");
        }

        if (ModelState.IsValid)
        {
            var book = new Book
            {
                BookId = id,
                Title = vm.Title,
                ISBN = vm.ISBN,
                PublicationDate = vm.PublicationDate,
                Genre = vm.Genre,
                AuthorId = vm.AuthorId,
                LibraryBranchId = vm.LibraryBranchId,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = DateTime.Now
            };
            _bookRepository.Update(book);
            await _bookRepository.SaveAsync();

            return NoContent();
        }

        return BadRequest(ModelState);
    }
}
