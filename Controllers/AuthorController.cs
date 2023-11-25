using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;
public class AuthorController : Controller
{
    private readonly IGenericRepository<Author> _authorRepository;

    public AuthorController(IGenericRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)//Defaul value to be shown on Index = 10
    {
        var authors = await _authorRepository.GetAllAsync();
        var totalItems = authors.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var authorViewModels = authors
        .Skip((page - 1) * pageSize)
        .Take(pageSize)//Values for pages = 10
        .Select(author => new AuthorViewModel
        {
            AuthorId = author.AuthorId,
            LastName = author.LastName,
            FirstName = author.FirstName,
            BirthPlace = author.BirthPlace,
            NobelPrize = author.NobelPrize
        }).ToList();

        var paginationInfo = new PaginationInfoViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };

        var viewModel = new AuthorIndexViewModel
        {
            Author = authorViewModels,
            PaginationInfo = paginationInfo
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AuthorViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var author = new Author
            {
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                BirthPlace = vm.BirthPlace,
                NobelPrize = vm.NobelPrize,
                CreatedAt = DateTime.Now,
            };

            _authorRepository.Add(author);
            await _authorRepository.SaveAsync();
            return RedirectToAction("Index");
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            throw new ArgumentException("ID cannot be null or not found");
        }

        var author = await _authorRepository.GetByIdAsync(id.Value);

        if (author == null)
        {
            throw new ArgumentException($"Author with ID {id} not found");
        }

        var authorViewModel = new AuthorViewModel
        {
            AuthorId = author.AuthorId,
            LastName = author.LastName,
            FirstName = author.FirstName,
            BirthPlace = author.BirthPlace,
            NobelPrize = author.NobelPrize,
            CreatedAt = author.CreatedAt,
            UpdatedAt = DateTime.Now
        };

        return View(authorViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AuthorViewModel vm)
    {
        if (id == 0)
        {
            throw new ArgumentException("ID cannot be null or not found");
        }

        if (ModelState.IsValid)
        {
            var author = new Author
            {
                AuthorId = id,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                BirthPlace = vm.BirthPlace,
                NobelPrize = vm.NobelPrize,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            _authorRepository.Update(author);
            await _authorRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _authorRepository.GetByIdAsync(id.Value);

        if (author == null)
        {
            return NotFound();
        }

        var authorViewModel = new AuthorViewModel
        {
            AuthorId = author.AuthorId,
            LastName = author.LastName,
            FirstName = author.FirstName,
            BirthPlace = author.BirthPlace,
            NobelPrize = author.NobelPrize,
            CreatedAt = author.CreatedAt,
            UpdatedAt = author.UpdatedAt

        };

        return View(authorViewModel);
    }
}
