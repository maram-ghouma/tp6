using tp6.DTOs;
namespace tp6.Repositories.CategorieRepository
{
    public interface ICategorieRepository 
    {
        IEnumerable<CategorieDto> GetAll();
        CategorieDto GetById(int id);
        CategorieDto Add(CategorieDto categorieDto);
        CategorieDto Update(CategorieDto categorieDto, int id);
        void Delete(int id);
    }

}
