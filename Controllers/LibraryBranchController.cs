using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;
[ApiController]
[Route("api/library-branches")]
public class LibraryBranchApiController : ControllerBase
{
    private readonly IGenericRepository<LibraryBranch> _libRep;

    public LibraryBranchApiController(IGenericRepository<LibraryBranch> libRep)
    {
        _libRep = libRep;
    }

    [HttpGet(Name = "GetLibraryBranchIndex")]
    public async Task<ActionResult<IEnumerable<LibraryBranchViewModel>>> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            var branches = await _libRep.GetAllAsync();

            var totalItems = branches.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var branchViewModels = branches
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(branch => new LibraryBranchViewModel
                {
                    LibraryBranchId = branch.LibraryBranchId,
                    BranchName = branch.BranchName,
                    ZipCode = branch.ZipCode,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    City = branch.City,
                    Email = branch.Email,
                    OpeningHours = branch.OpeningHours,
                    CreatedAt = branch.CreatedAt,
                    UpdatedAt = branch.UpdatedAt
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
                LibraryBranches = branchViewModels,
                PaginationInfo = paginationInfo
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost(Name = "CreateLibraryBranch")]
    public async Task<IActionResult> Create([FromBody] LibraryBranchViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var branch = new LibraryBranch
            {
                BranchName = viewModel.BranchName,
                ZipCode = viewModel.ZipCode,
                Address = viewModel.Address,
                Phone = viewModel.Phone,
                City = viewModel.City,
                Email = viewModel.Email,
                OpeningHours = viewModel.OpeningHours,
                CreatedAt = DateTime.Now
            };

            _libRep.Add(branch);
            await _libRep.SaveAsync();

            return Ok(viewModel);
        }

        return BadRequest(ModelState);
    }

    [HttpGet("{id}", Name = "GetLibraryDetailsById")]
    public async Task<ActionResult<LibraryBranchViewModel>> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest("ID cannot be null");
        }

        var branch = await _libRep.GetByIdAsync(id.Value);
        if (branch == null)
        {
            return NotFound();
        }

        var viewModel = new LibraryBranchViewModel
        {
            LibraryBranchId = branch.LibraryBranchId,
            BranchName = branch.BranchName,
            ZipCode = branch.ZipCode,
            Address = branch.Address,
            Phone = branch.Phone,
            City = branch.City,
            Email = branch.Email,
            OpeningHours = branch.OpeningHours,
            CreatedAt = branch.CreatedAt,
            UpdatedAt = branch.UpdatedAt
        };

        return Ok(viewModel);
    }

    [HttpPut("{id}", Name = "EditLibraryBranch")]
    public async Task<IActionResult> Edit(int id, [FromBody] LibraryBranchViewModel viewModel)
    {
        if (id != viewModel.LibraryBranchId)
        {
            return BadRequest("ID in the URL does not match ID in the request body");
        }

        if (ModelState.IsValid)
        {
            var branch = new LibraryBranch
            {
                LibraryBranchId = id,
                BranchName = viewModel.BranchName,
                ZipCode = viewModel.ZipCode,
                Address = viewModel.Address,
                Phone = viewModel.Phone,
                City = viewModel.City,
                Email = viewModel.Email,
                OpeningHours = viewModel.OpeningHours,
                CreatedAt = viewModel.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            _libRep.Update(branch);
            await _libRep.SaveAsync();

            return NoContent();
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id}", Name = "DeleteLibraryBranch")]
    public IActionResult Delete(int id)
    {
        var branch = _libRep.GetByIdAsync(id);
        if (branch == null)
        {
            return NotFound("ID not found!");
        }
        _libRep.Remove(id);
        return Ok("Deleted successfully");
    }
}
