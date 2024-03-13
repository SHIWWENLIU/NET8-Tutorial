using Microsoft.AspNetCore.Mvc;
using AuctionService.Entities;
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;


[ApiController]
[Route("api/[controller]")]

public class AuctionsController : ControllerBase
{
    private readonly DataContext _context;

    public AuctionsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]   
    public async Task<ActionResult<List<SuperHero>>> GetAllHeroes(){
        var list = await _context.SuperHeros.ToListAsync();
        return Ok(list);

    }

    [HttpGet("{id}")]   
    public async Task<ActionResult<SuperHero>> GetHero(int id){
        var list = await _context.SuperHeros.FindAsync(id);

        if(list == null) return NotFound();

        return Ok(list);

    }

    [HttpPost]   
    public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero){
        _context.SuperHeros.Add(hero);

        await _context.SaveChangesAsync();

        return Ok(await _context.SuperHeros.ToListAsync());

    }

    [HttpPut]
    public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updatedHero){
        var hero = await _context.SuperHeros.FindAsync(updatedHero.Id);

        if(hero == null) return NotFound();

        hero.Name = updatedHero.Name;
        hero.FirstName = updatedHero.FirstName;
        hero.LastName = updatedHero.LastName;
        hero.Place = updatedHero.Place;

        await _context.SaveChangesAsync();

        return  Ok(await _context.SuperHeros.ToListAsync());
    }

    [HttpDelete]
    public async Task<ActionResult<SuperHero>> DeleteHero(int id){
        var hero = await _context.SuperHeros.FindAsync(id);

        if(hero == null) return NotFound();
        _context.SuperHeros.Remove(hero);
        await _context.SaveChangesAsync();

        return Ok();
    }
}