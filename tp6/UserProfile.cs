using AutoMapper;
using tp6.Models;
using tp6.DTOs;

namespace tp6
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Categorie, CategorieDto>();
            CreateMap<CategorieDto, Categorie>();
        }
    }
}
