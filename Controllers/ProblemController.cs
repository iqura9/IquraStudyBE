using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IquraStudyBE.Context;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using IquraStudyBE.ViewModal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        
        public ProblemController(MyDbContext context,ITokenService tokenService, UserManager<User> userManager)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        // GET: api/Problem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Problem>>> GetProblems()
        {
          if (_context.Problems == null)
          {
              return NotFound();
          }
          var problems = await _context.Problems.Include("User").ToListAsync();
          return Ok(problems);
        }

        // GET: api/Problem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Problem>> GetProblem(int id)
        {
          if (_context.Problems == null)
          {
              return NotFound();
          }

          var problem = await _context.Problems.Include(p => p.TestCases).FirstOrDefaultAsync(p => p.Id == id);
            
            if (problem == null)
            {
                return NotFound();
            }

            
            return problem;
        }

        // PUT: api/Problem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProblem(int id, Problem problem)
        {
            if (id != problem.Id)
            {
                return BadRequest();
            }

            _context.Entry(problem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemExists(id))
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

        // POST: api/Problem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Problem>> PostProblem([FromBody] ProblemViewModel problem)
        {
          if (_context.Problems == null)
          {
              return Problem("Entity set 'MyDbContext.Problems'  is null.");
          }

          var userId = _tokenService.GetUserIdFromToken();
          
          var newProblem = new Problem
          {
              Title = problem.Title,
              Description = problem.Description,
              CreatedAt = DateTime.UtcNow,
              UpdatedAt = DateTime.UtcNow,
              UserId = userId,
              InitFunc = problem.InitFunc, 
          };
          _context.Problems.Add(newProblem);
          
          await _context.SaveChangesAsync();
          
          foreach (var testCase in problem.TestCases)
          {
              var testCaseEntity = new TestCase
              {
                  ExpectedResult = testCase.ExpectedResult,
                  Input = testCase.Input,
                  Problem = newProblem,
                  ProblemId = newProblem.Id,
                  
              };

              _context.TestCases.Add(testCaseEntity);
          }
          
          await _context.SaveChangesAsync();
        
          return CreatedAtAction("GetProblem", new { id = newProblem.Id }, problem);
        }
        
        
        // POST: api/Problem/Submittion
// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/Problems/Submittion")]
        public async Task<ActionResult<ProblemSubmittion>> PostProblemSubmittion([FromBody] PostProblemSubmittionDTO problem)
        {
            if (_context.ProblemSubmittions == null)
            {
                return Problem("Entity set 'MyDbContext.Problems' is null.");
            }

            var userId = _tokenService.GetUserIdFromToken();

            // Check if a problem submission with the same problem ID and user ID already exists
            var existingSubmission = await _context.ProblemSubmittions.FirstOrDefaultAsync(ps =>
                ps.UserId == userId && ps.ProblemId == problem.ProblemId && ps.GroupTaskId == problem.GroupTaskId);

            if (existingSubmission != null)
            {
                // Update the existing problem submission
                existingSubmission.SourceCode = problem.SourceCode;
                existingSubmission.Score = problem.Score;
                existingSubmission.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingSubmission);
            }
                // Create a new problem submission
                var newProblem = new ProblemSubmittion()
                {
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId,
                    SourceCode = problem.SourceCode,
                    Score = problem.Score,
                    ProblemId = problem.ProblemId,
                    GroupTaskId = problem.GroupTaskId
                };
                _context.ProblemSubmittions.Add(newProblem);

                await _context.SaveChangesAsync();

                return Ok(newProblem);
            
        }

        
        // GET: api/Problem/Submittion
        [HttpGet("Submittion")]
        public async Task<ActionResult<IEnumerable<ProblemSubmittion>>> GetProblemSubmittions(int groupTaskId, int problemId)
        {
            var userId = _tokenService.GetUserIdFromToken();
    
            // Retrieve problem submissions for the current user matching groupTaskId and problemId
            var problemSubmittions = await _context.ProblemSubmittions
                .Where(ps => ps.UserId == userId && ps.GroupTaskId == groupTaskId && ps.ProblemId == problemId)
                .ToListAsync();

            return Ok(problemSubmittions);
        }



        // DELETE: api/Problem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProblem(int id)
        {
            if (_context.Problems == null)
            {
                return NotFound();
            }
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProblemExists(int id)
        {
            return (_context.Problems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
