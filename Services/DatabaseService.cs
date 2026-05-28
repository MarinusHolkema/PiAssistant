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

public List<string> GetMemories()
{
    var memories = new List<string>();

    using var connection =
        new SqliteConnection(_connectionString);

    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText =
    @"
    SELECT Content
    FROM Memories
    ORDER BY Id DESC
    LIMIT 5
    ";

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        memories.Add(reader.GetString(0));
    }

    return memories;
}
}