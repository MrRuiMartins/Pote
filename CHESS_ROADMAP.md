# Chess Game 1-Day Roadmap

This roadmap is designed to get you from a blank folder to a live, Azure-hosted chess game by the time you log off today. We are using the Code-to-Cloud path because it's the fastest way to leverage your C# skills without getting bogged down in Docker networking.

## The "Expert" Tech Stack for Your MVP

- **Infrastructure**: Azure Developer CLI (azd) using the todo-csharp-sql template.
- **Backend**: .NET 8 Web API + Geras1mleo.Chess (NuGet).
- **Frontend**: React + react-chessboard + chess.js.
- **Data Format**: FEN (Forsyth-Edwards Notation). This is a single string that represents the entire board state (e.g., `rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1`).

## Phase 1: The Infrastructure "Plumbing" (Morning)

### Task 1.1: Create a new project folder and run azd init

```bash
azd init -t todo-csharp-sql
```

### Task 1.2: Run azd up

Why? This provisions your Azure App Service and SQL Database immediately. By starting here, you ensure your "hosting" is solved before you write a single line of chess logic.

```bash
azd up
```

### Task 1.3: Verify the sample "Todo" app is live

Check the URL provided in your terminal to confirm the app is deployed.

## Phase 2: The .NET "Random Mover" Brain (Mid-Day)

### Task 2.1: Install the Chess NuGet package

```bash
dotnet add package Chess
```

### Task 2.2: Create a ChessController.cs

Create a new controller in your .NET project.

### Task 2.3: Write one POST endpoint: Move(string fen)

It should take the current FEN, load it into a ChessBoard object, get all legal moves, pick one at random, and return the new FEN or the move in UCI format (e.g., "e2e4").

#### Code Snippet:

```csharp
[HttpPost("move")]
public IActionResult Move([FromBody] MoveRequest request)
{
    var board = new ChessBoard(request.Fen);
    var moves = board.Moves();
    board.Move(moves[Random.Shared.Next(moves.Length)]);
    return Ok(new { nextFen = board.ToFen() });
}

public class MoveRequest
{
    public string Fen { get; set; }
}
```

## Phase 3: The React UI (Afternoon)

### Task 3.1: Install React chess libraries

```bash
npm install react-chessboard chess.js
```

### Task 3.2: Replace the Todo UI with the Chessboard component

Replace the default UI with the `<Chessboard/>` component.

### Task 3.3: Use chess.js locally to validate moves

Validate the user's moves so they can't move a Pawn like a Queen.

### Task 3.4: Send moves to the API

Use `fetch` to send the FEN to your .NET API after the user moves and update the board with the computer's response.

## Phase 4: The Final Push (Late Afternoon)

### Task 4.1: Deploy to Azure

```bash
azd deploy
```

This pushes your new Chess code to the infrastructure you built in Phase 1.

### Task 4.2: Share and play

Send the link to yourself or a friend and play your first game.

## Why This Makes You Look Like an "AI/Cloud Expert"

By using this specific roadmap, you are hitting the "Trifecta" of modern Microsoft engineering:

- **Infrastructure as Code (IaC)**: You used Bicep files (inside the `/infra` folder) instead of clicking buttons in a portal.
- **Managed Identity**: Your app talks to the SQL database securely without you ever typing a password into a config file.
- **Agent-Ready Architecture**: Because your logic is cleanly separated into "Board State (FEN) → API → Next Move," it is very easy to ask an AI tool later to "replace the random mover with a Minimax search" without breaking the rest of the app.

## Key Vocabulary for Your Endpoints

- **FEN String**: Use this as the "Source of Truth" for your board.
- **UCI (Universal Chess Interface)**: When the computer moves, have the API return the move in UCI format (like `g1f3`). This is what professional engines use.
