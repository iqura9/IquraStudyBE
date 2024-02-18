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
    public class QuizController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;
        
        public QuizController(MyDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Quiz
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
          if (_context.Quizzes == null)
          {
              return NotFound();
          }
            return await _context.Quizzes
                .Include(q => q.CreatedByUser)
                .ToListAsync();
        }

        // GET: api/Quiz/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
          if (_context.Quizzes == null)
          {
              return NotFound();
          }
            var quiz = await _context.Quizzes
                .Include(q => q.CreatedByUser)
                .Include(q => q.Questions)
                    .ThenInclude(question => question.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        // GET: api/Quiz/WithoutAnswers/5
        [HttpGet("WithoutAnswers/{id}")]
        public async Task<ActionResult<Quiz>> GetQuizWithoutAnswers(int id)
        {
            if (_context.Quizzes == null)
            {
                return NotFound();
            }
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(question => question.Answers)
                .Where(q => q.Id == id)
                .Select(q => new Quiz
                {
                    Id = q.Id,
                    CreatedAt = q.CreatedAt,
                    CreatedByUser = q.CreatedByUser,
                    CreatedByUserId = q.CreatedByUserId,
                    Title = q.Title,
                    Questions = q.Questions.Select(question => new Question
                    {
                        Id = question.Id,
                        CreatedAt = question.CreatedAt,
                        UpdatedAt = question.UpdatedAt,
                        isMultiSelect = question.isMultiSelect,
                        QuizId = question.QuizId,
                        Title = question.Title,
                        Answers = question.Answers.Select(answer => new Answer
                        {
                            Id = answer.Id,
                            Title = answer.Title,
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }
        
        // PUT: api/Quiz/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quiz
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Quiz>> PostQuiz([FromBody] CreateQuizDto data)
        {
          if (_context.Quizzes == null)
          {
              return Problem("Entity set 'MyDbContext.Quizzes'  is null.");
          }

          var quiz = new Quiz
          {
              Title = data.Title,
              CreatedByUserId = _tokenService.GetUserIdFromToken(),
              CreatedAt = DateTime.UtcNow,
          };
          
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz);
        }
        
        // POST: api/Quiz/verify
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("verify")]
        public async Task<ActionResult<double>> VerifyQuizAnswers(QuizVerificationRequest request)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(question => question.Answers)
                .FirstOrDefaultAsync(q => q.Id == request.QuizId);

            if (quiz == null)
            {
                return NotFound();
            }
            
            double score = CalculateScore(quiz, request.Questions);

            return score;
        }

        private double CalculateScore(Quiz quiz, List<QuestionAnswer> userAnswers)
        {
            int totalQuestions = quiz.Questions.Count;
            int correctAnswers = 0;

            foreach (var userAnswer in userAnswers)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
                if (question != null && userAnswer.Answers.OrderBy(a => a).SequenceEqual(question.Answers.Where(a => a.IsCorrect).Select(s => s.Id).OrderBy(a => a)))
                {
                    correctAnswers++;
                }
            }

            // Calculate the score as a percentage
            double score = (double)correctAnswers / totalQuestions * 100;
            return Math.Round(score, 2);
        }
     
        
        // POST: api/QuizTask
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("QuizTask")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> PostQuizTasks([FromBody] CreateQuizTaskDto data)
        {
            if (_context.Quizzes == null)
            {
                return Problem("Entity set 'MyDbContext.Quizzes' is null.");
            }

            if (data.QuizIds == null || data.QuizIds.Length == 0)
            {
                return BadRequest("QuizIds array is null or empty.");
            }

            foreach (var quizId in data.QuizIds)
            {
                // Check if a GroupTaskQuiz with the same QuizId and GroupTaskId already exists
                if (!_context.GroupTaskQuizzes.Any(gtq => gtq.QuizId == quizId && gtq.GroupTaskId == data.GroupTasksId))
                {
                    var quiz = new GroupTaskQuiz()
                    {
                        GroupTaskId = data.GroupTasksId,
                        QuizId = quizId,
                    };

                    _context.GroupTaskQuizzes.Add(quiz);
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Successfully added quizzes to task");
        }


        // DELETE: api/Quiz/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            if (_context.Quizzes == null)
            {
                return NotFound();
            }
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return (_context.Quizzes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
