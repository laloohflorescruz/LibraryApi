using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorApiController : ControllerBase
{
    private readonly IGenericRepository<Author> _authorRepository;

    public AuthorApiController(IGenericRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [HttpGet(Name = "GetAuthorListIndex")]
    public async Task<ActionResult<IEnumerable<AuthorViewModel>>> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            var authors = await _authorRepository.GetAllAsync();
            var totalItems = authors.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var authorViewModels = authors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            return Ok(new
            {
                Authors = authorViewModels,
                PaginationInfo = paginationInfo
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost(Name = "CreateAuthor")]
    public async Task<IActionResult> Create([FromBody] AuthorViewModel vm)
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
            
            return Ok(vm);
        }

        return BadRequest(ModelState);
    }

    [HttpGet("{id}", Name = "GetAuthorDetailsById")]
    public async Task<ActionResult<AuthorViewModel>> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest("ID cannot be null");
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

        return Ok(authorViewModel);
    }

    [HttpPut("{id}", Name = "EditAuthor")]
    public async Task<IActionResult> Edit(int id, [FromBody] AuthorViewModel vm)
    {
        if (id != vm.AuthorId)
        {
            return BadRequest("ID in the URL does not match ID in the request body");
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

            return NoContent();
        }

        return BadRequest(ModelState);
    }
}
