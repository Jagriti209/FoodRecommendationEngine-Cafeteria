using System.Collections.Generic;

public class FeedbackService
{
    private readonly FeedbackRepository feedbackRepository;

    public FeedbackService(FeedbackRepository feedbackRepository)
    {
        this.feedbackRepository = feedbackRepository;
    }

    public List<FeedbackData> GetFeedback(int MenuId)
    {
        return feedbackRepository.FetchFeedback(MenuId);
    }
}
