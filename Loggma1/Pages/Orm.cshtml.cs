// Deneme.cshtml.cs dosyas�

using Loggma1.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loggma1.Models;

namespace Loggma1.Pages
{
    public class DenemeModel : PageModel
    {
      private readonly MyDbContext _dbContext;

    public DenemeModel(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

        public List<ClientInfo> Clients { get; set; }

        public async Task OnGetAsync()
        {
            // Entity Framework ile verileri �ekin
            Clients = await _dbContext.Clients.ToListAsync();
        }
    }
}
