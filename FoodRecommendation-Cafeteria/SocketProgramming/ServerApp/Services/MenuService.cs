using System.Collections.Generic;

public class MenuService
{
    private readonly MenuRepository menuRepository;

    public MenuService(MenuRepository menuRepository)
    {
        this.menuRepository = menuRepository;
    }

    public List<MenuItem> GetMenuItems()
    {
        return menuRepository.FetchMenuItems();
    }
}
