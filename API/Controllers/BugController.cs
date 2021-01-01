using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BugController : ControllerBase
    {
        private readonly DataContext _context;

        public BugController(DataContext context)
        {
            _context = context;
        }
    }
}