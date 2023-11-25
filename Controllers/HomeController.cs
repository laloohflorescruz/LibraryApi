using LibraryApi.Models;
using LibraryApi.Repo;
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

}