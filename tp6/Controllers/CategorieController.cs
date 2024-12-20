using Microsoft.AspNetCore.Mvc;
using tp6.Models;
using tp6.Repositories.CategorieRepository;
using tp6.DTOs;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class CategorieController : ControllerBase
{
    private readonly ICategorieRepository _catRepo;

    public CategorieController(ICategorieRepository catRepo)
    {
        _catRepo = catRepo;
    }

    // GET: api/Categories
    [HttpGet]
    public ActionResult<IEnumerable<CategorieDto>> GetCategories()
    {
        var categories = _catRepo.GetAll();
        return Ok(categories);
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public ActionResult<CategorieDto> GetCategorie(int id)
    {
        var categorie = _catRepo.GetById(id);

        if (categorie == null)
        {
            return NotFound();
        }

        return Ok(categorie);
    }

    // POST: api/Categories
    [HttpPost]
    public ActionResult<CategorieDto> PostCategorie(CategorieDto categorieDto)
    {
        var createdCategorie = _catRepo.Add(categorieDto);
        return CreatedAtAction(nameof(GetCategorie), new { id = createdCategorie.Id }, createdCategorie);
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public IActionResult PutCategorie(int id, CategorieDto categorieDto)
    {
        if (id != categorieDto.Id)
        {
            return BadRequest();
        }

        var existingCategorie = _catRepo.GetById(id);
        if (existingCategorie == null)
        {
            return NotFound();
        }

        _catRepo.Update(categorieDto, id);
        return NoContent();
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public IActionResult DeleteCategorie(int id)
    {
        var categorie = _catRepo.GetById(id);
        if (categorie == null)
        {
            return NotFound();
        }

        _catRepo.Delete(id);
        return NoContent();
    }
}
