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

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;
        public GroupController(MyDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups([FromQuery] bool myGroups = false)
        {
            IQueryable<Group> query = _context.Groups.Include(g => g.CreatedByUser);
            var userId = _tokenService.GetUserIdFromToken();
           
            if (myGroups)
            {
                if (User.IsInRole("Teacher") && !string.IsNullOrEmpty(userId))
                {
                    // Apply filter1: return only my groups (created by the teacher)
                    query = query.Where(g => g.CreatedByUserId == userId);
                }
                else if (User.IsInRole("Student") && !string.IsNullOrEmpty(userId))
                {
                    // Apply filter2: return only groups that the student is enrolled in
                    query = query.Where(g => g.GroupPeople.Any(gp => gp.UserId == userId));
                }
                else
                {
                    // Handle unauthorized access or invalid user ID
                    return Unauthorized();
                }
            }
            
            var groups = await query.ToListAsync();

            if (groups == null)
            {
                return NotFound();
            }

            return groups;
        }


        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
          if (_context.Groups == null)
          {
              return NotFound();
          }
            var @group = await _context.Groups.FindAsync(id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        // PUT: api/Group/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Group
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Group>> PostGroup([FromBody] CreateGroupDTO model)
        {
          if (_context.Groups == null)
          {
              return Problem("Entity set 'MyDbContext.Groups'  is null.");
          }

          var group = new Group
          {
              CreatedAt = DateTime.UtcNow,
              CreatedByUserId = _tokenService.GetUserIdFromToken(),
              Name = model.Name,
          };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = group.Id }, group);
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
