using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IGenericRepository<Book> bookRepository, ILogger<HomeController> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var books = _bookRepository.GetAllAsync();
        return View(books);
    }

    public async Task<IActionResult> Search(string searchBook, int page = 1, int pageSize = 5)
    {
        var books = await _bookRepository.GetAllAsync();
        if (!string.IsNullOrEmpty(searchBook))
        {
            books = books.Where(s => s.Title!.Contains(searchBook)).ToList();
        }

        var totalItems = books.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var bookViewModels = books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(book => new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                PublicationDate = book.PublicationDate
            }).ToList();

        var paginationInfo = new PaginationInfoViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };

        var viewModel = new BookIndexViewModel
        {
            Book = bookViewModels,
            PaginationInfo = paginationInfo
        };

        return View("SearchResults", viewModel);
    }
}