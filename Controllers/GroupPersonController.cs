using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IquraStudyBE.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IquraStudyBE.Context;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;



namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPersonController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;

        public GroupPersonController(MyDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
      
        // GET: api/GroupPerson
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPerson>>> GetGroupPeople()
        {
          if (_context.GroupPeople == null)
          {
              return NotFound();
          }
            return await _context.GroupPeople.ToListAsync();
        }

        // GET: api/GroupPerson/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GroupPerson>>> GetGroupPerson(int id)
        {
          if (_context.GroupPeople == null)
          {
              return NotFound();
          }

          var groupPeopleList = await _context.GroupPeople
              .Include(p => p.User)
              .Where(p => p.GroupId == id)
              .ToListAsync();

            if (groupPeopleList == null)
            {
                return NotFound();
            }

            return groupPeopleList;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> PutGroupPerson(int id, [FromBody] PutGroupPersonCreateDTO groupPerson)
        {
            var existingGroupPerson = await _context.GroupPeople.FindAsync(id);

            if (existingGroupPerson == null)
            {
                return NotFound();
            }
            
            var userId = _tokenService.GetUserIdFromToken();
            var group = await _context.Groups.FindAsync(groupPerson.GroupId);
            if (group.CreatedByUserId != userId)
            {
                return Forbid("User does not have permission to modify this group");
            }
            
            if (group == null || group.CreatedByUserId == groupPerson.UserId)
            {
                return BadRequest();
            }
            
            existingGroupPerson.UserId = groupPerson.UserId;
            existingGroupPerson.UserStatus = groupPerson.UserStatus;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "GroupPerson updated successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupPersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        
        // POST: api/GroupPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GroupPerson>> PostGroupPerson(GroupPersonCreateDTO groupPerson)
        {
            if (_context.GroupPeople == null)
          {
              return Problem("Entity set 'MyDbContext.GroupPeople'  is null.");
          }

            var group = await _context.Groups.FindAsync(groupPerson.GroupId);
            
            if (group == null) return NotFound();
            
            var userId = _tokenService.GetUserIdFromToken();
            if (group.CreatedByUserId ==  userId) return Forbid();
            
            var newGroupPerson = new GroupPerson
            {
                GroupId = groupPerson.GroupId,
                UserId = userId,
                UserStatus = UserStatus.Pending,
            };
            _context.GroupPeople.Add(newGroupPerson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupPerson", new { id = newGroupPerson.Id }, newGroupPerson);
        }

        // DELETE: api/GroupPerson/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupPerson(int id)
        {
            if (_context.GroupPeople == null)
            {
                return NotFound();
            }
            var groupPerson = await _context.GroupPeople.FindAsync(id);
            if (groupPerson == null)
            {
                return NotFound();
            }

            _context.GroupPeople.Remove(groupPerson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupPersonExists(int id)
        {
            return (_context.GroupPeople?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
