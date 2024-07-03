using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class SentimentAnalysisService
{
    private readonly List<WordScore> _wordScores;

    public SentimentAnalysisService()
    {
        string jsonFilePath = "C:\\Users\\jagriti.anchalia\\OneDrive - InTimeTec Visionsoft Pvt. Ltd.,\\Desktop\\sentiemntWords.json";
        string jsonData = File.ReadAllText(jsonFilePath);
        _wordScores = JsonConvert.DeserializeObject<List<WordScore>>(jsonData);
    }

    public double CalculateAverageScore(List<FeedbackData> feedbacks)
    {
        double totalScore = 0;
        int count = 0;

        foreach (var feedback in feedbacks)
        {
            double feedbackScore = CalculateFeedbackScore(feedback.Feedback);
            if (feedbackScore != 0)
            {
                totalScore += feedbackScore;
                count++;
            }
        }

        return count > 0 ? totalScore / count : 0;
    }

    private double CalculateFeedbackScore(string comment)
    {
        string[] words = comment.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double score = 0;

        foreach (string word in words)
        {
            WordScore match = _wordScores.Find(ws => ws.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                score += match.Score;
            }
        }

        return score;
    }
}
