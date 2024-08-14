public class DiscardedMenuItemFeedback
{
    public int menuID { get; set; }
    public int userID { get; set; }
    public string momsRecipe { get; set; }
    public string dislikedReason { get; set; }
    public string preferedTaste { get; set; }
    public DateTime feedbackAddedDate { get; set; } = DateTime.Today;


}