using LibraryManagement.Infrastructure.Context;
using LibraryManagement.Domain.Entities; // Harf hatası DÜZELTİLDİ
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application; // Service katmanı için

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // NOT: İlerleyen aşamada burayı da IUserService'e çevirebilirsin (Hocan istediği için)
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // İPUCU: Burada normalde User listesini değil, UserDto listesi dönmelisin
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // NOT: Hocanın uyarısına göre burada doğrudan 'User' yerine 'UserCreateDto' kullanmalısın.
            // Ama şimdilik hata gitmesi için isimleri düzelttim.
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}