# VS Code & F# Shortcut Keys Cheat Sheet

---

## General VS Code Navigation

| Shortcut         | Action                                    |
| ---------------- | ----------------------------------------- |
| `Ctrl+P`         | Quick Open — find any file by name        |
| `Ctrl+Shift+P`   | Command Palette — run any VS Code command |
| `Ctrl+,`         | Open Settings                             |
| `Ctrl+`` `       | Toggle integrated terminal                |
| `Ctrl+B`         | Toggle sidebar visibility                 |
| `Ctrl+Shift+E`   | Focus Explorer (file tree)                |
| `Ctrl+Shift+F`   | Global search across files                |
| `Ctrl+Shift+G`   | Open Source Control (Git) panel           |
| `Ctrl+Shift+X`   | Open Extensions panel                     |
| `Ctrl+\`         | Split editor                              |
| `Ctrl+1 / 2 / 3` | Focus editor group 1 / 2 / 3              |
| `Ctrl+W`         | Close current tab                         |
| `Ctrl+Shift+T`   | Reopen last closed tab                    |
| `Ctrl+Tab`       | Switch between open tabs                  |
| `Ctrl+K Z`       | Zen mode (distraction-free)               |

---

## Code Editing

| Shortcut              | Action                                 |
| --------------------- | -------------------------------------- |
| `Ctrl+D`              | Select next occurrence of current word |
| `Ctrl+Shift+L`        | Select all occurrences of current word |
| `Alt+Up / Down`       | Move line up / down                    |
| `Shift+Alt+Up / Down` | Duplicate line up / down               |
| `Ctrl+Shift+K`        | Delete current line                    |
| `Ctrl+/`              | Toggle line comment                    |
| `Shift+Alt+A`         | Toggle block comment                   |
| `Ctrl+Z`              | Undo                                   |
| `Ctrl+Shift+Z`        | Redo                                   |
| `Ctrl+X`              | Cut line (no selection needed)         |
| `Tab / Shift+Tab`     | Indent / outdent selection             |
| `Ctrl+Shift+[`        | Fold (collapse) code block             |
| `Ctrl+Shift+]`        | Unfold (expand) code block             |
| `Ctrl+K Ctrl+0`       | Fold all regions                       |
| `Ctrl+K Ctrl+J`       | Unfold all regions                     |
| `Shift+Alt+F`         | Format document                        |

---

## IntelliSense & Code Intelligence

| Shortcut           | Action                                         |
| ------------------ | ---------------------------------------------- |
| `Ctrl+Space`       | Trigger IntelliSense / suggestions             |
| `Ctrl+Shift+Space` | Trigger parameter hints                        |
| `F12`              | Go to Definition                               |
| `Alt+F12`          | Peek Definition (inline)                       |
| `Shift+F12`        | Show all References                            |
| `F2`               | Rename symbol                                  |
| `Ctrl+.`           | Quick Fix / Code Actions                       |
| `Ctrl+Shift+O`     | Go to symbol in current file                   |
| `Ctrl+T`           | Go to symbol in workspace                      |
| `Ctrl+G`           | Go to line number                              |
| `Ctrl+Shift+M`     | Open Problems panel                            |
| `F8`               | Go to next error or warning                    |
| `Shift+F8`         | Go to previous error or warning                |
| `K` (hover)        | Show hover info (while hovering over a symbol) |
| `Ctrl+K Ctrl+I`    | Show hover info at cursor                      |

---

## Search & Replace

| Shortcut        | Action                               |
| --------------- | ------------------------------------ |
| `Ctrl+F`        | Find in current file                 |
| `Ctrl+H`        | Find and Replace in current file     |
| `Ctrl+Shift+F`  | Find across all files                |
| `Ctrl+Shift+H`  | Replace across all files             |
| `F3 / Shift+F3` | Find next / previous match           |
| `Alt+Enter`     | Select all occurrences of find match |

---

## Multi-Cursor Editing

| Shortcut             | Action                                     |
| -------------------- | ------------------------------------------ |
| `Alt+Click`          | Add cursor at click position               |
| `Ctrl+Alt+Up / Down` | Add cursor above / below                   |
| `Ctrl+D`             | Add cursor at next occurrence of selection |
| `Escape`             | Dismiss multiple cursors                   |

---

## F# Specific (Ionide Extension)

| Shortcut               | Action                                 |
| ---------------------- | -------------------------------------- |
| `F12`                  | Go to Definition of F# symbol          |
| `Shift+F12`            | Find all References of F# symbol       |
| `F2`                   | Rename F# symbol across files          |
| `Ctrl+.`               | Show F# quick fixes and suggestions    |
| `Ctrl+K Ctrl+I`        | Show F# type signature at cursor       |
| `Alt+Enter`            | Send selection to F# Interactive (FSI) |
| `Ctrl+Alt+Enter`       | Send entire file to FSI                |
| `Ctrl+Shift+P` → `FSI` | Access all Ionide FSI commands         |
| `Ctrl+Shift+P` → `F#`  | Access all Ionide F# commands          |

> **Tip:** Hover over any F# binding to see its inferred type — essential in this codebase where Elmish model/message types are inferred.

---

## Terminal

| Shortcut         | Action                   |
| ---------------- | ------------------------ |
| `Ctrl+`` `       | Toggle terminal          |
| `Ctrl+Shift+`` ` | New terminal instance    |
| `Ctrl+C`         | Kill running process     |
| `Up / Down`      | Navigate command history |
| `Ctrl+L`         | Clear terminal           |

---

## Git & Source Control

| Shortcut                      | Action                                   |
| ----------------------------- | ---------------------------------------- |
| `Ctrl+Shift+G`                | Open Source Control panel                |
| `Ctrl+Enter`                  | Commit staged changes (in SCM input box) |
| `Ctrl+Shift+P` → `Git: Stage` | Stage current file                       |
| `Ctrl+Shift+P` → `Git: Diff`  | View file diff                           |

---

## Useful Commands for This Project (via `Ctrl+Shift+P`)

| Command                    | Purpose                                           |
| -------------------------- | ------------------------------------------------- |
| `Tasks: Run Task`          | Run build/watch tasks defined in `Build.fsproj`   |
| `F#: Start F# Interactive` | Open FSI REPL for quick experiments               |
| `F#: Load project`         | Load current project into FSI                     |
| `Developer: Reload Window` | Restart VS Code window (fixes Ionide glitches)    |
| `Fable: ...`               | Any Fable-related commands if extension installed |

---

## Tips for This Fable/Elmish Project

- Use `Ctrl+P` and type a filename like `Route.fs` or `Layout.fs` to jump instantly to any source file.
- Use `Ctrl+T` and type a function name (e.g. `update`, `view`, `init`) to locate it across modules.
- Use `Shift+F12` on a `Msg` discriminated union case to trace all dispatch sites.
- Use `Alt+Enter` to test small F# expressions in FSI without running the full app.
- Use `F8` to cycle through build errors after running `dotnet build`.
