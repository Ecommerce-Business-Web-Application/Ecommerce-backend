using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class BaseController<TEntity, TContext> : ControllerBase
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseController(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    // GET: api/[Entity]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    // GET: api/[Entity]/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TEntity>> Get(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        return entity;
    }

    // POST: api/[Entity]
    [HttpPost]
    public async Task<ActionResult<TEntity>> Post(TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = entity.GetType().GetProperty("Id")?.GetValue(entity, null) }, entity);
    }

    // PUT: api/[Entity]/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, TEntity entity)
    {
        if (id != (int)entity.GetType().GetProperty("Id")?.GetValue(entity, null))
        {
            return BadRequest();
        }

        _context.Entry(entity).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntityExists(id))
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

    // DELETE: api/[Entity]/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EntityExists(int id)
    {
        return _dbSet.Find(id) != null;
    }
}
