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
    public class TaskController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;

        public TaskController(MyDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupTask>>> GetGroupTasks()
        {
          if (_context.GroupTasks == null)
          {
              return NotFound();
          }
            return await _context.GroupTasks.ToListAsync();
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GroupTask>>> GetGroupTask(int id)
        {
          if (_context.GroupTasks == null)
          {
              return NotFound();
          }

          List<GroupTask> groupTask = await _context.GroupTasks.Where(gt => gt.GroupId == id).ToListAsync();
            
            if (groupTask == null)
            {
                return NotFound();
            }

            return groupTask;
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupTask(int id, GroupTask groupTask)
        {
            if (id != groupTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupTaskExists(id))
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

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<GroupTask>> PostGroupTask(CreateGroupTaskDTO data)
        {
          if (_context.GroupTasks == null)
          {
              return Problem("Entity set 'MyDbContext.GroupTasks'  is null.");
          }
            
          var groupTask = new GroupTask
          {
              Title = data.Title,
              Description = data.Description,
              GroupId  = data.GroupId,
              CreateByUserId = _tokenService.GetUserIdFromToken(),
          };
            _context.GroupTasks.Add(groupTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupTask", new { id = groupTask.Id }, groupTask);
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupTask(int id)
        {
            if (_context.GroupTasks == null)
            {
                return NotFound();
            }
            var groupTask = await _context.GroupTasks.FindAsync(id);
            if (groupTask == null)
            {
                return NotFound();
            }

            _context.GroupTasks.Remove(groupTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupTaskExists(int id)
        {
            return (_context.GroupTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
