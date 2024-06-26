public class FeedbackData
{
    public string Comment { get; set; }
    public int Rating { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
}
