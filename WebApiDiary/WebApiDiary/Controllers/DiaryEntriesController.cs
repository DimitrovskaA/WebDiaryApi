using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDiary.Models;
using WebApiDiary.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApiDiary.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DiaryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries() 
        {
            return await _context.DiaryEntries.ToListAsync();
        }

        [HttpGet("{id}")] 
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntryById(int id)
        {
           
            var diaryEntry = await _context.DiaryEntries.FindAsync(id);

            if (diaryEntry == null)
            {
                return NotFound();
            }

            return diaryEntry;
        }
        
        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
        {     
            diaryEntry.Id = 0;
            
            _context.DiaryEntries.Add(diaryEntry);
            await _context.SaveChangesAsync();

            var resourceUrl=Url.Action(nameof(GetDiaryEntries), new { id = diaryEntry.Id });

           return Created(resourceUrl, diaryEntry);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DiaryEntry>> UpdateDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry) 
        {           
            if(id != diaryEntry.Id)  
            {
                return BadRequest();
            }
            _context.Entry(diaryEntry).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryEntityExists(id))
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
        private bool DiaryEntityExists(int id)
        {
            return _context.DiaryEntries.Any(e => e.Id == id);
        }            
    }
}
