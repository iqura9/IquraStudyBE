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
using Microsoft.AspNetCore.Authorization;

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AnswerController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Answer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
          if (_context.Answers == null)
          {
              return NotFound();
          }
            return await _context.Answers.ToListAsync();
        }

        // GET: api/Answer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
          if (_context.Answers == null)
          {
              return NotFound();
          }
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        // PUT: api/Answer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answer/{QuestionId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{QuestionId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<List<Answer>>> PostAnswer(int QuestionId, [FromBody] List<CreateAnswerDto> answers)
        {
            try
            {
                // Ensure the question exists
                var question = await _context.Questions.FindAsync(QuestionId);
                if (question == null)
                {
                    return NotFound("Question not found");
                }

                // Check if answers are empty or null
                if (answers == null || !answers.Any())
                {
                    return BadRequest("Answers cannot be empty");
                }

                // Check if any answer has a null or empty title
                if (answers.Any(a => string.IsNullOrWhiteSpace(a.Title)))
                {
                    return BadRequest("Answer title cannot be empty");
                }

                // Map DTOs to Answer entities
                var answerEntities = answers.Select(dto => new Answer
                {
                    QuestionId = QuestionId,
                    Title = dto.Title,
                    IsCorrect = dto.IsCorrect
                }).ToList();

                // Add answers to the context
                _context.Answers.AddRange(answerEntities);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAnswer", answerEntities);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            if (_context.Answers == null)
            {
                return NotFound();
            }
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnswerExists(int id)
        {
            return (_context.Answers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
