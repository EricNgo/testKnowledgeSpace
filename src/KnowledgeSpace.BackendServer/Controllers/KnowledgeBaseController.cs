using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class KnowledgeBaseController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public KnowledgeBaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostKnowledgeBase([FromBody] KnowledgeBaseCreateRequest request)
        {
            //var dbKnowledgeBase = await _context.KnowledgeBases.FindAsync(request.Id);
            //if (dbKnowledgeBase != null)
            //    return BadRequest($"KnowledgeBase with id {request.Id} is existed.");
            var function = new KnowledgeBase()
            {
         CategoryId = request.CategoryId,
        Title = request.Title,
        SeoAlias = request.SeoAlias,
        Description = request.Description,
        Environment=request.Environment,
        Problem=request.Problem,
        StepToReproduce = request.StepToReproduce,
        ErrorMessage = request.ErrorMessage,
        Workaround = request.Workaround,
        Note = request.Note,
        Labels =  request.Labels,

           };
            _context.KnowledgeBases.Add(function);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = function.Id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetKnowledgeBases()
        {
            var knowledgeBase = _context.KnowledgeBases;

            var functionvms = await knowledgeBase.Select(u => new KnowledgeBaseQuickVm()
            {
        Id = u.Id,
        CategoryId = u.CategoryId,
        Title  = u.Title,
        SeoAlias = u.SeoAlias,
        Description =u.Description,
            }).ToListAsync();

            return Ok(functionvms);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetKnowledgeBasesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _context.KnowledgeBases.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Title.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(u => new KnowledgeBaseQuickVm()
                {
                    Id = u.Id,
                    CategoryId = u.CategoryId,
                    Title = u.Title,
                    SeoAlias = u.SeoAlias,
                    Description = u.Description,
                }).ToListAsync();

            var pagination = new Pagination<KnowledgeBaseQuickVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _context.KnowledgeBases.FindAsync(id);
            if (request == null)
                return NotFound();

            var requestVm = new KnowledgeBaseVm()
            {
            

         CategoryId = request.CategoryId,
         
         Title = request.Title,
         SeoAlias = request.SeoAlias,
         Description = request.Description,

         Environment = request.Environment,
         Problem = request.Problem,
         StepToReproduce = request.StepToReproduce,
         ErrorMessage = request.ErrorMessage,
         Workaround =   request.Workaround,
         Note =request.Note,
         OwnerUserId = request.OwnerUserId,
         Labels = request.Labels, 
         CreateDate = request.CreateDate,
        LastModifiedDate = request.LastModifiedDate,
        NumberOfComments = request.NumberOfComments,
        NumberOfVotes = request.NumberOfVotes,
        NumberOfReports = request.NumberOfReports,
              };
            return Ok(requestVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnowledgeBase(int id, [FromBody] KnowledgeBaseCreateRequest request)
        {
            var knowledgeBase= await _context.KnowledgeBases.FindAsync(id);
            if (knowledgeBase == null)
                return NotFound();

            knowledgeBase.CategoryId = request.CategoryId;

            knowledgeBase.Title = request.Title;

            knowledgeBase.SeoAlias = request.SeoAlias;

            knowledgeBase.Description = request.Description;

            knowledgeBase.Environment = request.Environment;

            knowledgeBase.Problem = request.Problem;

            knowledgeBase.StepToReproduce = request.StepToReproduce;

            knowledgeBase.ErrorMessage = request.ErrorMessage;

            knowledgeBase.Workaround = request.Workaround;

            knowledgeBase.Note = request.Note;

            knowledgeBase.Labels = request.Labels;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnowledgeBase(int id)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(id);
            if (knowledgeBase == null)
                return NotFound();

            _context.KnowledgeBases.Remove(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var knowledgeBasevm = new KnowledgeBaseVm()
                {
                    Id = knowledgeBase.CategoryId,

                    CategoryId = knowledgeBase.CategoryId,

                    Title = knowledgeBase.Title,

                    SeoAlias = knowledgeBase.SeoAlias,

                    Description = knowledgeBase.Description,

                    Environment = knowledgeBase.Environment,

                    Problem = knowledgeBase.Problem,

                    StepToReproduce = knowledgeBase.StepToReproduce,

                    ErrorMessage = knowledgeBase.ErrorMessage,

                    Workaround = knowledgeBase.Workaround,

                    Note = knowledgeBase.Note,

                    OwnerUserId = knowledgeBase.OwnerUserId,

                    Labels = knowledgeBase.Labels,

                    CreateDate = knowledgeBase.CreateDate,

                    LastModifiedDate = knowledgeBase.LastModifiedDate,

                    NumberOfComments = knowledgeBase.CategoryId,

                    NumberOfVotes = knowledgeBase.CategoryId,

                    NumberOfReports = knowledgeBase.CategoryId,
                };
                return Ok(knowledgeBasevm);
            }
            return BadRequest();
        }
    }
}
