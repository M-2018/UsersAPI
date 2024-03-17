using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly UsersDbContext usersDbContext;

        public UserController(UsersDbContext usersDbContext)
        {
            this.usersDbContext = usersDbContext;
        }

        //Get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await usersDbContext.Users.ToListAsync();
            return Ok(users);
        }

        //Get single user
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetUser")]

        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found");
        }

        //Add user
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            await usersDbContext.Users.AddAsync(user);
            await usersDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            //return CreatedAtAction(nameof(GetUser), user.Id, user);
        }

        //Update user
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] User user)
        {
            var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(existingUser != null)
            {
                existingUser.Id = user.Id;
                existingUser.FirstName = user.FirstName;
                existingUser.SecondName = user.SecondName;
                existingUser.FirstLastName = user.FirstLastName;
                existingUser.SecondLastName = user.SecondLastName;
                existingUser.BirthDate = user.BirthDate;
                existingUser.Salary = user.Salary;

                await usersDbContext.SaveChangesAsync();
                return Ok(existingUser);
            }

            return NotFound("User not found");
        }

        //Delete user
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser != null)
            {
                usersDbContext.Remove(existingUser);
                await usersDbContext.SaveChangesAsync();
                return Ok(existingUser);
            }

            return NotFound("User not found");
        }

        // Search users by first name or last name with pagination
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<User> query = usersDbContext.Users;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FirstName.Contains(searchTerm) || u.FirstLastName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Users = users
            };

            return Ok(response);
        }
    }
}