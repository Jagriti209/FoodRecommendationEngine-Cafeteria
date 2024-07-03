﻿using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

public static class ChefOperations
{
    static RecommendationService recommendationService = new RecommendationService();
    public static void ViewRecommendation(NetworkStream stream)
    {
        var responseData = recommendationService.GenerateRecommendation();
        string responseDataJson = JsonConvert.SerializeObject(responseData);
        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        stream.Flush();
    }

    public static void CreateMenuForNextDay(NetworkStream stream, MenuItem menuData)
    {
        try
        {
            ChefMenuRepository.CreateMenuForNextDay(menuData);
            CustomData message = new CustomData
            {
                Message = $"{menuData.MenuID} added successfully to Menu"
            };
            ClientHandler.SendResponse(stream, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating menu for next day: {ex.Message}");
            CustomData message = new CustomData
            {
             Message = $"Error creating menu for next day: {ex.Message}"
            };
            ClientHandler.SendResponse(stream, message);
        }
    }
}