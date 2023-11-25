using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApi.Controllers;
public class BookController : Controller
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IGenericRepository<Author> _authorRepository;
    private readonly IGenericRepository<LibraryBranch> _libraryBranchRepository;

    public BookController(IGenericRepository<Book> bookRepository,
                          IGenericRepository<Author> authorRepository,
                          IGenericRepository<LibraryBranch> libraryBranchRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _libraryBranchRepository = libraryBranchRepository;
    }

    [Authorize]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)//Defaul value to be shown on Index = 10
    {
        var books = await _bookRepository.GetAllAsync();

        var totalItems = books.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var bookViewModels = books
         .Skip((page - 1) * pageSize)
         .Take(pageSize)//Values for pages = 10
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

        var viewModel = new BookIndexViewModel
        {
            Book = bookViewModels,
            PaginationInfo = paginationInfo
        };
        return View(viewModel);
    }

    public async Task<IActionResult> CreateAsync()
    {
        ViewBag.AuthorItems = await GetAuthorsAsync();
        ViewBag.LibraryItems = await GetLibraryAsync();
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(BookViewModel vm)
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

            return RedirectToAction("Index");
        }

        ViewBag.AuthorItems = await GetAuthorsAsync();
        ViewBag.LibraryItems = await GetLibraryAsync();
        return View(vm);
    }
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            throw new ArgumentException("ID cannot be null or not found");
        }

        var book = await _bookRepository.GetByIdAsync(id.Value);

        if (book == null)
        {
            throw new ArgumentException($"Book with ID {id} not found");
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
            UpdatedAt = DateTime.Now
        };

        ViewBag.AuthorItems = await GetAuthorsAsync();
        ViewBag.LibraryItems = await GetLibraryAsync();
        return View(bookViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookViewModel vm)
    {
        if (id == 0)
        {
            throw new ArgumentException("ID cannot be null or not found");
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

            return RedirectToAction(nameof(Index));
        }

        ViewBag.AuthorItems = await GetAuthorsAsync();
        ViewBag.LibraryItems = await GetLibraryAsync();
        return View(vm);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
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

        ViewBag.AuthorItems = await GetAuthorsAsync();
        ViewBag.LibraryItems = await GetLibraryAsync();
        return View(bookViewModel);
    }

    private async Task<SelectList> GetAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();

        var authorItems = authors
        .OrderBy(author => author.FirstName) // Sort by first name
        .ThenBy(author => author.LastName)  // Then sort by last name
            .Select(author => new
            {
                Value = author.AuthorId,
                Text = $"{author.FirstName} {author.LastName}"
            })
            .ToList();

        return new SelectList(authorItems, "Value", "Text");
    }

    private async Task<SelectList> GetLibraryAsync()
    {
        var branches = await _libraryBranchRepository.GetAllAsync();

        var libraryItems = branches
            .OrderBy(branch => branch.BranchName)  // Sort branches by name in ascending order
            .Select(branch => new
            {
                Value = branch.LibraryBranchId,
                Text = branch.BranchName
            })
            .ToList();

        return new SelectList(libraryItems, "Value", "Text");
    }
}
