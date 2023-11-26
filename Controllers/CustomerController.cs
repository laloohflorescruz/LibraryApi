using LibraryApi.Models;
using LibraryApi.Repo;
using LibraryApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;
[ApiController]
[Route("api/customers")]
public class CustomerApiController : ControllerBase
{
    private readonly IGenericRepository<Customer> _repo;

    public CustomerApiController(IGenericRepository<Customer> repo)
    {
        _repo = repo;
    }

    [HttpGet(Name = "GetCustomerListIndex")]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            var customers = await _repo.GetAllAsync();

            var totalItems = customers.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customerViewModels = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(customer => new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Birthday = customer.Birthday,
                    Student = customer.Student,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    MembershipSince = customer.MembershipSince,
                    Genre = customer.Genre
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
                Customers = customerViewModels,
                PaginationInfo = paginationInfo
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost(Name = "CreateCustomer")]
    public async Task<IActionResult> Create([FromBody] CustomerViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var customer = new Customer
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Birthday = viewModel.Birthday,
                Student = viewModel.Student,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Address = viewModel.Address,
                MembershipSince = viewModel.MembershipSince,
                Genre = viewModel.Genre,
                CreatedAt = DateTime.Now
            };

            _repo.Add(customer);
            await _repo.SaveAsync();

            return Ok(viewModel);
        }

        return BadRequest(ModelState);
    }

    [HttpGet("{id}", Name = "GetCustomerDetailsById")]
    public async Task<ActionResult<CustomerViewModel>> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest("ID cannot be null");
        }

        var customer = await _repo.GetByIdAsync(id.Value);
        if (customer == null)
        {
            return NotFound();
        }

        var customerViewModel = new CustomerViewModel
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Birthday = customer.Birthday,
            Student = customer.Student,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            MembershipSince = customer.MembershipSince,
            Genre = customer.Genre,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        return Ok(customerViewModel);
    }

    [HttpPut("{id}", Name = "EditCustomer")]
    public async Task<IActionResult> Edit(int id, [FromBody] CustomerViewModel viewModel)
    {
        if (id != viewModel.CustomerId)
        {
            return BadRequest("ID in the URL does not match ID in the request body");
        }

        if (ModelState.IsValid)
        {
            var customer = new Customer
            {
                CustomerId = id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Birthday = viewModel.Birthday,
                Student = viewModel.Student,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Address = viewModel.Address,
                MembershipSince = viewModel.MembershipSince,
                Genre = viewModel.Genre,
                CreatedAt = viewModel.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            _repo.Update(customer);
            await _repo.SaveAsync();

            return NoContent();
        }

        return BadRequest(ModelState);
    }
}
