# Copilot Instructions for PiAssistant

## Build, Test, and Lint Commands

- **Build:**
  - Use Visual Studio or run:
    ```sh
    dotnet build
    ```
- **Run:**
  - Start the app with:
    ```sh
    dotnet run
    ```
- **Test:**
  - No test project or test commands detected in this repository.

## High-Level Architecture

- **Framework:** .NET 10, Blazor Server (Razor Components)
- **Main entry:** `Program.cs` sets up services and configures the HTTP pipeline.
- **Components:**
  - UI is in `Components/` (e.g., `Pages/`, `Layout/`, `Tools/`).
  - Main chat UI: `Components/Pages/Home.razor`.
- **Services:**
  - `DatabaseService`: Handles SQLite storage for chat messages and vector memories.
  - `OllamaService`: Streams LLM completions from a local Ollama server.
  - `EmbeddingService`: Calls Ollama for text embeddings.
  - `SystemPromptService`: Provides the system prompt and tool usage rules.
  - `ToolRegistry`/`ToolExecutor`: Registers and executes tools (e.g., shell, time, file write).
- **Models:**
  - `ChatMessage`: Represents a chat message.

## Key Conventions

- **Tool Use:**
  - All tool invocations must return valid JSON as described in `SystemPromptService`.
  - Tools are registered in `ToolRegistry` and must implement the `ITool` interface.
  - Available tools: `time`, `shell`, `write_file`.
- **Memory System:**
  - User prompts are embedded and stored in SQLite (`Memories` table).
  - Retrieval uses cosine similarity for context injection.
- **UI:**
  - Main chat interface is styled and managed in `Home.razor`.
  - Error and not-found pages are in `Components/Pages/`.

## Integration with Other AI Assistant Configs

- No other AI assistant config files (Claude, Cursor, Windsurf, etc.) detected.

---

This file was generated to help Copilot and other AI agents understand the structure, build/run commands, and conventions of this repository. Adjust or expand as needed for future changes.
