using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class SentimentAnalysisService
{
    private readonly List<WordScore> wordScores;

    public SentimentAnalysisService()
    {
        string jsonFilePath = "C:\\Users\\jagriti.anchalia\\OneDrive - InTimeTec Visionsoft Pvt. Ltd.,\\Desktop\\sentiemntWords.json";

        try
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            wordScores = JsonConvert.DeserializeObject<List<WordScore>>(jsonData) ?? new List<WordScore>();
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: JSON file not found at path {jsonFilePath}. {ex.Message}");
            wordScores = new List<WordScore>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error: JSON deserialization failed. {ex.Message}");
            wordScores = new List<WordScore>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while loading JSON data. {ex.Message}");
            wordScores = new List<WordScore>();
        }
    }

    public double CalculateAverageScore(List<FeedbackData> feedbacks)
    {
        double totalScore = 0;
        int count = 0;

        if (feedbacks == null)
        {
            Console.WriteLine("Error: Feedback list is null.");
            return 0;
        }

        foreach (var feedback in feedbacks)
        {
            try
            {
                double feedbackScore = CalculateFeedbackScore(feedback.Feedback);
                if (feedbackScore != 0)
                {
                    totalScore += feedbackScore;
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating score for feedback '{feedback.Feedback}': {ex.Message}");
            }
        }

        return count > 0 ? totalScore / count : 0;
    }

    private double CalculateFeedbackScore(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            return 0;
        }

        string[] words = comment.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double score = 0;

        foreach (string word in words)
        {
            try
            {
                WordScore match = wordScores.Find(ws => ws.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    score += match.Score;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating score for word '{word}': {ex.Message}");
            }
        }

        return score;
    }
}
