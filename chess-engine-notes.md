# Building a Chess Engine in .NET — Conversation Notes

## 1. AI vs Machine Learning

- **Artificial Intelligence (AI)** is the broad field of making machines do things that seem intelligent (play chess, recognize speech, drive cars).
- **Machine Learning (ML)** is a subset of AI where the machine learns patterns from data rather than being explicitly programmed.
- **Deep Learning** is a further subset of ML, using neural networks.

```
AI ⊃ ML ⊃ Deep Learning
```

A rule-based expert system is AI but not ML. A neural network trained on data is both.

## 2. Where Does a Minimax Chess Engine Fit?

**Minimax + alpha-beta pruning + hand-written heuristics = classic AI, not ML.**

You're hand-coding the intelligence:
- The search algorithm explores the game tree.
- A human-designed evaluation function ("a queen = 9, center control = X") scores positions.
- The machine isn't learning — it's executing your logic very fast.

This is **Good Old-Fashioned AI (GOFAI)** / symbolic AI. Historically how Stockfish worked (it now mixes in a small neural net called NNUE for evaluation).

**Optimizations in this tradition:**
- Alpha-beta pruning
- Iterative deepening
- Transposition tables (cache positions reached via different move orders)
- Quiescence search (keep evaluating through capture sequences)
- Move ordering (try likely-good moves first → better pruning)
- Opening books and endgame tablebases

**It becomes ML when** you replace the hand-written eval with one *learned* from data — e.g., a neural network trained on millions of positions. AlphaZero/Leela do this, combined with Monte Carlo Tree Search instead of minimax.

## 3. Where Does "Pre-training" Fit?

"Pre-training" is an ML concept:
1. First train a model on a huge generic dataset to learn broad patterns.
2. Then **fine-tune** it on a smaller, task-specific dataset.

Examples:
- **LLMs**: pre-trained on the internet, then fine-tuned for chat/instructions.
- **Chess (AlphaZero)**: "pre-trained" itself via self-play (millions of games), learning a general position-evaluation network.

For a classical minimax engine, **pre-training doesn't apply** — there are no learnable weights. It only enters if you later add a neural net for position evaluation or move selection.

## 4. Will a Classical Engine Beat a 1800 Lichess Player?

**Yes, easily — if implemented competently.**

| What you build | Approx. Lichess strength |
|---|---|
| Plain minimax, depth 3–4, material-only eval | ~1200–1500 |
| + Alpha-beta pruning (depth 5–6) | ~1600–1800 |
| + Move ordering, quiescence, iterative deepening | ~2000–2200 |
| + Transposition tables, decent positional eval | ~2300–2500 |
| + Opening book, endgame tablebases, null-move pruning, LMR | ~2700+ |

**Key insight:** the big jump past 1800 is **quiescence search**. Without it, the engine "blunders" by stopping mid-capture and thinking it just won a queen.

**Common pitfalls keeping engines weak:**
- Python without bitboards → search-speed-limited.
- Buggy move generation (en passant, castling, repetition). Validate against **Perft** numbers early. #1 source of mysterious losses.
- Forgetting quiescence → engine hangs pieces despite "deep" search.

**Reference engines** to look at: **Sunfish** (~120 lines Python, ~2000 Elo), **TSCP** (~2000 lines C, ~2000–2100 Elo).

## 5. .NET-Specific Advice

**Language/runtime:**
- **C# on .NET 8+**. JIT is excellent for tight numeric code.
- Release mode, `ServerGarbageCollection=true`.
- Expect ~1–5M nodes/sec with bitboards and care.

**Key .NET features to lean on:**
- `ulong` for bitboards (64-bit = 64 squares, perfect fit).
- `System.Numerics.BitOperations` — `TrailingZeroCount`, `PopCount`, `Log2` compile to single CPU instructions (BMI1/POPCNT).
- `Span<T>` and `stackalloc` for move lists — avoid heap allocations in the search hot path.
- `readonly struct` for `Move`.
- `[MethodImpl(MethodImplOptions.AggressiveInlining)]` on tiny hot methods.
- Avoid LINQ, `IEnumerable` `foreach`, and lambdas in the search loop.

**Protocol: implement UCI** (Universal Chess Interface):
- Don't build a GUI. Plug into:
- **Arena**, **Cute Chess**, **Banksia** (free GUIs)
- **Lichess** via [lichess-bot](https://github.com/lichess-bot-devs/lichess-bot)
- Engine-vs-engine tournaments via Cute Chess CLI
- ~15 commands. A weekend to get right.

## 6. Suggested Build Order

1. **Board representation** + move generation (bitboards).
2. **Perft testing** — must match known values exactly. Catches 95% of bugs.
3. **UCI loop** — talk to Arena/Cute Chess.
4. **Negamax + alpha-beta** + material eval. Playable engine.
5. **Quiescence search**. Massive strength jump.
6. **Iterative deepening + move ordering** (MVV-LVA, killer moves, history heuristic).
7. **Transposition table** with Zobrist hashing.
8. **Better eval**: piece-square tables, pawn structure, king safety.
9. **Null-move pruning, late-move reductions**.

## 7. Project Structure (React frontend + .NET backend)

```
┌─────────────┐ HTTP/WebSocket ┌──────────────────┐
│ React │ ◄─────────────────────► │ ASP.NET Core │
│ (chess UI) │ JSON / FEN │ Web API │
└─────────────┘ │ ┌────────────┐ │
│ │ Engine Core│ │
│ └────────────┘ │
└──────────────────┘
```

- **Engine.Core** — class library. Pure engine, no I/O. Board, moves, search, eval. Unit-testable in isolation.
- **Engine.Api** — ASP.NET Core Web API. Hosts the engine, exposes REST/SignalR endpoints.
- **Engine.Uci** — *keep this* as a separate console app. Critical for testing against Arena/Cute Chess, running engine-vs-engine tournaments to measure Elo, and deploying as a Lichess bot.
- **Engine.Tests** (xUnit/NUnit) — Perft, tactical suites, eval sanity checks.
- **Engine.Tools** (optional) — tuning, self-play tournaments, profiling.
- **react-frontend/** — separate folder/tooling (Vite/Next/CRA).

**Architecture concepts to explore (engine side):**
- **Threading**: search on a background thread; main thread stays responsive to cancellation. Use `CancellationToken` throughout.
- **Time management**: per-move time allocation. Its own minor art.

**Architecture concepts to explore (web layer):**
- **API style**:
- REST: `POST /api/analyze` with `{ fen, depth, timeMs }` → `{ bestMove, eval, pv, nodes }`. Stateless, simple.
- SignalR / WebSocket: stream `info depth N score cp X pv ...` updates as the engine thinks deeper. Live eval bar + best line in React.
- Hybrid is common.
- **Engine hosting (the tricky part)**:
- Transposition tables can be hundreds of MB — don't allocate per request.
- Options: singleton + lock, engine pool (N instances), or per-session engine (cached by session ID, evicted on idle). Per-session is best for analysis UX (TT stays warm between moves).
- **Cancellation**: wire `HttpContext.RequestAborted` / SignalR connection lifetime into the engine's `CancellationToken`.
- **Threading vs ASP.NET**: long CPU-bound searches will starve the thread pool. Use dedicated threads or `Task.Run` carefully, and enforce hard time/depth limits at the API layer.
- **Resource limits**: cap depth and time server-side. A `depth=30` request will pin a CPU for minutes.

**Frontend libraries to explore:**
- **[chess.js](https://github.com/jhlywa/chess.js)** — JS move generation/validation. Use client-side for legal-move highlighting, UI state, draw detection.
- **[react-chessboard](https://github.com/Clariity/react-chessboard)** or **[chessground](https://github.com/lichess-org/chessground)** (Lichess's board) — drag-and-drop board UI. Don't build from scratch.
- Wire format: **FEN** for positions, **UCI move notation** (`e2e4`, `e7e8q`) for moves. Both libraries speak them natively.

**Deployment notes:**
- Engine is CPU-bound, benefits from CPU intrinsics (POPCNT, BMI). Avoid burstable cloud VMs (Azure B-series, AWS t-series — they throttle). Use compute-optimized tiers.
- Consider **AOT compilation** (`PublishAot`) for faster startup; test if steady-state is faster too.

**Why still keep the UCI app:**
1. **Testing infrastructure**: Cute Chess CLI runs thousands of self-play games to measure if a change is +20 Elo or -5 Elo. Without this you're flying blind.
2. **Lichess bot**: trivial to deploy as a Lichess bot once UCI works → free Elo testing vs real humans.

## 8. Board & Move Representation

**Bitboards (core idea):**
- A `ulong` where each bit = one square (a1 = bit 0, h8 = bit 63 — "Little-Endian Rank-File mapping").
- 12 piece bitboards (6 types × 2 colors) + occupancy bitboards (white, black, all).
- Operations become bitwise: white pawn single push = `(whitePawns << 8) & ~allPieces`.

**Topics to dig into:**
- **Magic bitboards** for sliding pieces (rook/bishop/queen). Don't derive your own magics — use published ones.
- **PEXT bitboards** as an alternative using BMI2 (`Bmi2.X64.ParallelBitDeposit`) — simpler if targeting modern x64 only.
- **Zobrist hashing** — XOR-based incremental position hashing for the transposition table.
- **Make/Unmake vs Copy/Make** — two philosophies. With .NET structs, copy/make is surprisingly competitive; try it first.

**Move encoding:**
- 16 bits is enough: 6 bits `from`, 6 bits `to`, 4 bits flags (promotion, castle, EP, capture). Pack into a `ushort` in a `readonly struct Move`.
- 32-bit option: embed captured/moved piece for faster unmake. Trade-off: memory bandwidth vs CPU.
- Move lists: pre-allocated `Span<Move>` on stack. Upper bound ~256 (proven max 218).

**Position state beyond piece bitboards:**
- Side to move, castling rights (4 bits), en passant square, halfmove clock (50-move rule), fullmove number, Zobrist hash, repetition history.

## 9. Reference .NET Engines to Study

- **[Leorik](https://github.com/lithander/Leorik)** — C#, well-documented, ~2700 Elo. Author wrote an excellent blog series.
- **[MinimalChess](https://github.com/lithander/MinimalChess)** — same author, smaller and educational.
- **[Cosette](https://github.com/Tearth/Cosette)** — another solid C# engine.

## 10. Recommended Reading Order

1. [Chess Programming Wiki](https://www.chessprogramming.org) — "Bitboards", "Square Mapping Considerations", "Magic Bitboards", "Perft Results".
2. Leorik's blog series (start at part 1) — walks through these decisions specifically in C#.
3. Write move generator → validate with Perft → only then touch search.