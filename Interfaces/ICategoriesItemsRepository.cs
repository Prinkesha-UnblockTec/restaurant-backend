using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface ICategoriesItemsRepository
    {
        ICollection<CategoriesItems> GetAllCategoriesItemsData();
        ICollection<ActiveCategoriesItems> GetAllActiveCategoriesItemsData();
        bool EditCategoriesItemsList(CategoriesItems model);
        bool DeleteCategoriesItem(int id);
        bool AddCategoriesItemsList(CategoriesItems categoryDto);
    }
}
