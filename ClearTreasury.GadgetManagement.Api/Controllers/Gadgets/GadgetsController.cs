using ClearTreasury.GadgetManagement.Api.Data;
using ClearTreasury.GadgetManagement.Api.Dtos;
using ClearTreasury.GadgetManagement.Api.Infrastructure;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

[Authorize(AuthPolicies.CanViewGadgets)]
public class GadgetsController(AppDbContext dbContext)
    : AppBaseController
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] GadgetSearchDto dto)
    {
        var query = dbContext
            .Set<Gadget>()
            .AsNoTracking();

        if (!String.IsNullOrWhiteSpace(dto.NameFilter))
        {
            var term = await dbContext.PrepareContainsTerm(dto.NameFilter, AbortToken);
            query = query.Where(x => EF.Functions.Contains(x.NameGrams, term));
        }
        if (dto.DateFromFilter.HasValue)
        {
            query = query.Where(x => x.DateCreated >= dto.DateFromFilter.Value);
        }
        if (dto.DateToFilter.HasValue)
        {
            query = query.Where(x => x.DateCreated <= dto.DateToFilter.Value);
        }

        var count = await query.CountAsync(AbortToken);

        var finalQuery = query
            .Include(x => x.Categories)
            .OrderBy(x => x.Name)
            .Skip(dto.PageSize * dto.PageIndex)
            .Take(dto.PageSize);

        var pageDto = new PageDto<GadgetDto>(count);

        await foreach(var item in finalQuery.AsAsyncEnumerable())
        {
            AbortToken.ThrowIfCancellationRequested();
            pageDto.Add(Map(item));
        }
        
        return Ok(pageDto);
    }

    [HttpGet("{id}", Name = "GetGadgetById")]
    public async Task<ActionResult> Get(Guid id)
    {
        var entity = await dbContext
            .Set<Gadget>()
            .Include(x => x.Categories)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, AbortToken);

        if (entity is null)
        {
            return NotFound();
        }

        var dto = Map(entity);
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(AuthPolicies.CanManageGadgets)]
    public async Task<ActionResult> Create([FromBody] GadgetSubmitDto dto)
    {
        var nameTaken = await dbContext
            .Set<Gadget>()
            .AnyAsync(x => x.Name == dto.Name, AbortToken);

        if (nameTaken)
        {
            return ConflictProblem($"The name '{dto.Name}' is already taken.");
        }

        var entity = new Gadget(dto.Name, dto.Quantity);

        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(AbortToken);

        var categories = await dbContext
            .Set<Category>()
            .Where(x => dto.CategoryIds.Contains(x.Id))
            .ToArrayAsync(AbortToken);

        entity.SetCategories(categories);

        // TODO: handle name confict race condition
        await dbContext.SaveChangesAsync(AbortToken);

        var details = Map(entity);
        var uri = Url.Link("GetGadgetById", new { id = entity.Id });

        return Created(uri, details);
    }

    [HttpPatch("{id}")]
    [Authorize(AuthPolicies.CanManageGadgets)]
    public async Task<ActionResult> IncreaseStock(Guid id,
        [FromIfMatchHeader] byte[] etag, [FromQuery] bool increase)
    {
        var entity = await dbContext
            .Set<Gadget>()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id, AbortToken);

        if (entity is null)
        {
            return NotFound();
        }

        dbContext.SetOriginalRowVersion(entity, etag);

        if (increase)
        {
            entity.IncreaseStock();
        }
        else
        {
            entity.DecreaseStock();
        }

        await dbContext.SaveChangesAsync(AbortToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(AuthPolicies.CanManageGadgets)]
    public async Task<ActionResult> Update(Guid id,
        [FromIfMatchHeader] byte[] etag, [FromBody] GadgetSubmitDto dto)
    {
        var entity = await dbContext
            .Set<Gadget>()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id, AbortToken);

        if (entity is null)
        {
            return NotFound();
        }

        dbContext.SetOriginalRowVersion(entity, etag);

        if (dto.Name != entity.Name)
        {
            var nameTaken = await dbContext
                .Set<Gadget>()
                .AnyAsync(x => x.Name == dto.Name && x.Id != id, AbortToken);

            if (nameTaken)
            {
                return ConflictProblem($"The name '{dto.Name}' is already taken.");
            }
        }

        entity.Update(dto.Name, dto.Quantity);

        var categories = await dbContext
            .Set<Category>()
            .Where(x => dto.CategoryIds.Contains(x.Id))
            .ToArrayAsync(AbortToken);

        entity.UpdateCategories(categories);

        // TODO: handle name confict race condition
        await dbContext.SaveChangesAsync(AbortToken);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(AuthPolicies.CanManageGadgets)]
    public async Task<ActionResult> Delete([FromBody] GadgetIdTagDto[] idDtos)
    {
        var results = new List<GadgetDeleteResultDto>();
        var idsToFetch = idDtos.Select(x => x.Id).ToArray();

        var entities = await dbContext
            .Set<Gadget>()
            .Where(x => idsToFetch.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x, AbortToken);

        foreach (var dto in idDtos)
        {
            GadgetDeleteResultDto result;

            if (entities.TryGetValue(dto.Id, out var currentEntity))
            {
                dbContext.SetOriginalRowVersion(currentEntity, dto.ETag);
                dbContext.Remove(currentEntity);

                try
                {
                    await dbContext.SaveChangesAsync(AbortToken);
                    result = GadgetDeleteResultDto.Ok(dto.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    result = GadgetDeleteResultDto.Fail(dto.Id, "ETag mismatch");
                }
                catch
                {
                    result = GadgetDeleteResultDto.Fail(dto.Id, "Unexpected error");
                }
            }
            else
            {
                result = GadgetDeleteResultDto.Fail(dto.Id, "Not found");
            }

            results.Add(result);
        }

        return Ok(results);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthPolicies.CanManageGadgets)]
    public async Task<ActionResult> Delete(Guid id, [FromIfMatchHeader] byte[] version)
    {
        var entity = await dbContext
            .Set<Gadget>()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id, AbortToken);

        if (entity is null)
        {
            return NotFound();
        }

        dbContext.SetOriginalRowVersion(entity, version);
        dbContext.Remove(entity);

        await dbContext.SaveChangesAsync(AbortToken);
        return NoContent();
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
