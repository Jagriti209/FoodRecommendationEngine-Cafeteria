public class FeedbackData
{
    public string Feedback { get; set; }
    public int Rating { get; set; }
    public int MenuID { get; set; }
    public int UserID { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;


}