using tp6.DTOs;
using tp6.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using tp6.Data;
namespace tp6.Repositories.CategorieRepository
{
    public class CategorieRepository :  ICategorieRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _imapper;
        public CategorieRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _imapper = mapper;
        }
        public CategorieDto Add(CategorieDto categorie)
        {
            
            var cat = _imapper.Map<Categorie>(categorie);
            _context.Categorie.Add(cat);
            _context.SaveChanges();
            return categorie;
        }
        public CategorieDto Update(CategorieDto c, int id)
        {
            var CatInDb = _context.Categorie.SingleOrDefault(c => c.Id == id);
            CatInDb.Name = c.Name;
            _context.SaveChanges();
            return _imapper.Map<Categorie, CategorieDto>(CatInDb); ;
        }
        public IEnumerable<CategorieDto> GetAll()
        {
            var cat = _context.Categorie.ToList();
            var catDTO = _imapper.Map<List<CategorieDto>>(cat);
            return catDTO;
        }
        public void Delete(int id)
        {
            var categorie = _context.Categorie.Find(id);
            if (categorie != null)
            {
                _context.Categorie.Remove(categorie);
                _context.SaveChanges();
            }
        }
        public CategorieDto GetById(int id)
        {
            var categorie = _context.Categorie.Find(id);
            return categorie == null ? null : _imapper.Map<CategorieDto>(categorie);
        }
    }

}

