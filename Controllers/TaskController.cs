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

          List<GroupTask> groupTask = await _context.GroupTasks.Where(gt => gt.GroupId == id).Include(t => t.GroupTaskQuizzes).ThenInclude(t => t.Quiz).ToListAsync();
            
            if (groupTask == null)
            {
                return NotFound();
            }

            return groupTask;
        }
        
        // GET: api/Task/view-grade/5
        [HttpGet("view-grade/{id}")]
        public async Task<ActionResult<List<object>>> GetTaskGrade(int id)
        {
            if (_context.QuizSubmittions == null)
            {
                return NotFound();
            }

            List<QuizSubmittion> quizSubmittions = await _context.QuizSubmittions
                .Where(gt => gt.GroupTaskId == id)
                .Include(t => t.Quiz)
                .Include(t => t.User)
                .ToListAsync();
    
            if (quizSubmittions == null)
            {
                return NotFound();
            }

            var groupedByUser = quizSubmittions.GroupBy(qs => qs.User.Id);

            var result = new List<object>();

            foreach (var group in groupedByUser)
            {
                var user = group.First().User;
                var scores = group.Select(qs => new
                {
                    createdDate = qs.CreatedAt,
                    taskTitle = qs.Quiz.Title,
                    maxScore = 100,
                    received = qs.Score
                }).ToList();

                var userResult = new
                {
                    key = user.Id,
                    name = user.UserName,
                    overall_score = scores.Average(s => s.received),
                    scores = scores
                };

                result.Add(userResult);
            }

            return result;
        }


        
        // GET: api/Task/Quiz?taskId=5
        [HttpGet("Quiz")]
        [Authorize]
        public async Task<ActionResult<GetGroupTaskQuiz>> GetGroupTaskQuiz([FromQuery] int taskId)
        {
            if (_context.GroupTasks == null)
            {
                return NotFound();
            }

            var myUserId = _tokenService.GetUserIdFromToken();
            
            var groupTask = await _context.GroupTasks
                .Where(gt => gt.Id == taskId)
                .Include(t => t.GroupTaskQuizzes)
                    .ThenInclude(t => t.Quiz)
                .Include(qt => qt.CreatedByUser)
                .Include(qt => qt.QuizSubmittions.Where(qs => qs.UserId == myUserId))
                .Select(qt => new GetGroupTaskQuiz
                {
                    Id = qt.Id,
                    CreateByUserId = qt.CreateByUserId,
                    CreatedByUser = qt.CreatedByUser,
                    Description = qt.Description,
                    GroupId = qt.GroupId,
                    CreatedAt = qt.CreatedAt,
                    GroupTaskQuizzes = qt.GroupTaskQuizzes.Select(qtq => new GroupTaskQuizzesDto
                    {
                        Id = qtq.Id,
                        GroupTaskId = qtq.GroupTaskId,
                        QuizId = qtq.QuizId,
                        Quiz = qtq.Quiz,
                        Score = qt.QuizSubmittions.Where(qs => qs.UserId == myUserId && qs.QuizId == qtq.QuizId && qs.GroupTaskId == qt.Id).FirstOrDefault().Score
                    }).ToList(),
                    AverageScore = qt.QuizSubmittions
                        .Where(qs => qs.UserId == myUserId && qs.GroupTaskId == qt.Id)
                        .Select(qs => (double?)qs.Score)
                        .Average() ?? 0.0,
                    Title = qt.Title,
                })
                .FirstOrDefaultAsync();
                

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
        
        // DELETE: api/Task/Quiz
        [HttpDelete("Quiz")]
        public async Task<IActionResult> DeleteGroupTaskQuizzes([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No IDs provided for deletion.");
            }

            foreach (var id in ids)
            {
                var groupTaskQuiz = await _context.GroupTaskQuizzes.FindAsync(id);

                if (groupTaskQuiz != null)
                {
                    var quizSubmittions = await _context.QuizSubmittions
                        .Where(q => q.GroupTaskId == groupTaskQuiz.GroupTaskId && q.QuizId == groupTaskQuiz.QuizId)
                        .ToListAsync(); // Execute the query

                    _context.GroupTaskQuizzes.Remove(groupTaskQuiz);

                    if (quizSubmittions.Any()) // Check if any QuizSubmittions exist
                    {
                        _context.QuizSubmittions.RemoveRange(quizSubmittions); // Remove all found QuizSubmittions
                    }
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
