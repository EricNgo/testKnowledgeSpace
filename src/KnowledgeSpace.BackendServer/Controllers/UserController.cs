using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

       
        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreateRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
            Dob = request.Dob,
            UserName = request.UserName,
            LastName = request.LastName,
            FirstName = request.FirstName,  
            PhoneNumber = request.PhoneNumber


            };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, request);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;

            var uservms = await users.Select(r => new UserVm()
            {
                Id = r.Id,
                UserName = r.UserName,
                Dob=r.Dob,  
                Email = r.Email,    
                PhoneNumber = r.PhoneNumber ,
                LastName = r.LastName,
                FirstName=r.FirstName
                
            }).ToListAsync();

            return Ok(uservms);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetUsersPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Id.Contains(filter) 
                    || x.UserName.Contains(filter)
                    || x.PhoneNumber.Contains(filter)
                    || x.Email.Contains(filter)
                    || x.FirstName.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(r => new UserVm()
                {
                    Id = r.Id,
                    UserName = r.UserName,
                    Dob = r.Dob,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    LastName = r.LastName,
                    FirstName = r.FirstName
                })
                .ToListAsync();

            var pagination = new Pagination<UserVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userVm = new UserVm()
            {
                Id = user.Id,
                UserName = user.UserName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LastName = user.LastName,
                FirstName = user.FirstName
            };
            return Ok(userVm);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] UserCreateRequest request)
        {
         
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.FirstName =request.FirstName;
            user.LastName = request.LastName;
            user.Dob = request.Dob;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var uservm = new UserVm()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Dob = user.Dob,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LastName = user.LastName,
                    FirstName = user.FirstName
                };
                return Ok(uservm);
            }
            return BadRequest(result.Errors);
        }
    }
}
