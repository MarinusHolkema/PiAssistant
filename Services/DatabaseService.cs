using Microsoft.Data.Sqlite;
using PiAssistant.Models;

namespace PiAssistant.Services;

public class DatabaseService
{
    private readonly string _connectionString =
        "Data Source=assistant.db";

    public DatabaseService()
    {
        Initialize();
    }

    private void Initialize()
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
@"
CREATE TABLE IF NOT EXISTS Messages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Role TEXT,
    Content TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Memories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Content TEXT,
    Embedding TEXT
);
";

        command.ExecuteNonQuery();
    }

    public void SaveMessage(ChatMessage message)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        INSERT INTO Messages(Role, Content)
        VALUES($role, $content)
        ";

        command.Parameters.AddWithValue(
            "$role",
            message.Role);

        command.Parameters.AddWithValue(
            "$content",
            message.Content);

        command.ExecuteNonQuery();
    }

    private double CosineSimilarity(List<float> vectorA, List<float> vectorB)
    {
        double dotProduct = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (int i = 0; i < vectorA.Count; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];
            magnitudeA += Math.Pow(vectorA[i], 2);
            magnitudeB += Math.Pow(vectorB[i], 2);
        }

        magnitudeA = Math.Sqrt(magnitudeA);
        magnitudeB = Math.Sqrt(magnitudeB);

        if (magnitudeA == 0 || magnitudeB == 0)
            return 0;

        return dotProduct / (magnitudeA * magnitudeB);
    }

    public List<ChatMessage> GetMessages()
    {
        var messages = new List<ChatMessage>();

        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        SELECT Role, Content
        FROM Messages
        ORDER BY Id
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            messages.Add(new ChatMessage
            {
                Role = reader.GetString(0),
                Content = reader.GetString(1)
            });
        }

        return messages;
    }

    public void SaveMemory(
    string content,
    List<float> embedding)
{
    using var connection =
        new SqliteConnection(_connectionString);

    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText =
    @"
    INSERT INTO Memories(Content, Embedding)
    VALUES($content, $embedding)
    ";

    command.Parameters.AddWithValue(
        "$content",
        content);

    command.Parameters.AddWithValue(
        "$embedding",
        string.Join(",", embedding));

    command.ExecuteNonQuery();
}

public List<string> GetRelevantMemories(List<float> queryEmbedding, int top = 5)
    {
        var results = new List<(string Content, double score)>();
        using var connection =
            new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText =
        @"
        SELECT Content, Embedding
        FROM Memories
        ";
        using var reader = command.ExecuteReader();
        while (reader.Read())        {
            var content = reader.GetString(0);
            var embeddingStr = reader.GetString(1);
            var embedding = embeddingStr.Split(',').Select(float.Parse).ToList();
            var score = CosineSimilarity(queryEmbedding, embedding);
            results.Add((content, score));      


        }
        return results
            .OrderByDescending(r => r.score)
            .Take(top)
            .Select(r => r.Content)
            .ToList();  
    }
  
}