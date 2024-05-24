using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface ICategoriesItemsRepository
    {
        ICollection<CategoriesItems> GetAllCategoriesItemsData();
        ICollection<ActiveCategoriesItems> GetAllActiveCategoriesItemsData();
        bool EditCategoriesItemsList(CategoriesItems model);
        bool UpdateCalculationItems(UpdateCalculation model);
        bool EditUpdateBalanceQuantityList(List<UpdateBalanceQuantity>  model);
        bool DeleteCategoriesItem(int id);
        bool AddCategoriesItemsList(CategoriesItems categoryDto);
    }
}
