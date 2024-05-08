using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface ICategoriesRepository
    {
        ICollection<Categories> GetAllCategoriesData();
        ICollection<ActiveCategories> GetAllActiveCategoriesData();
        bool EditCategoriesList(NewCategories model);
        bool DeleteCategories(int id);
        bool AddCategoriesList(NewCategories categoryDto);
    }
}