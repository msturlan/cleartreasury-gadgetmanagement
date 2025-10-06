using ClearTreasury.GadgetManagement.Api.Data;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public class GadgetsController(AppDbContext dbContext)
    : AppBaseController
{
    [HttpGet("{id}", Name = "GetGadgetById")]
    public async Task<ActionResult> Get(Guid id)
    {
        var entity = await dbContext
            .Set<Gadget>()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id, AbortToken);

        if (entity is null)
        {
            return NotFound();
        }

        var dto = Map(entity);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody]CreateGadgetRequest request)
    {
        var nameTaken = await dbContext
            .Set<Gadget>()
            .AnyAsync(x => x.Name == request.Name, AbortToken);

        if (nameTaken)
        {
            return CreateProblem(StatusCodes.Status409Conflict);
        }

        var entity = new Gadget(request.Name, request.Quantity);

        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(AbortToken);

        var categories = await dbContext
            .Set<Category>()
            .Where(x => request.CategoryIds.Contains(x.Id))
            .ToArrayAsync(AbortToken);

        entity.SetCategories(categories);
        await dbContext.SaveChangesAsync(AbortToken);

        var details = Map(entity);
        var uri = Url.Link("GetGadgetById", new { id = entity.Id });

        return Created(uri, details);
    }

    private GadgetDto Map(Gadget entity)
    {
        return new GadgetDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Quantity = entity.Quantity,
            DateCreated = entity.DateCreated,
            DateModified = entity.DateModified,
            RowVersion = entity.RowVersion,
            Categories = [.. entity.Categories.Select(x => new GadgetDto.GadgetCategoryDto(x.Id, x.Name))]
        };
    }
}
