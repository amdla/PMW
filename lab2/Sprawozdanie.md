
### Sprawozdanie LAB2

#### Michał Jagodziński i Maciej Karabin

---

## Projekt

Wykonaliśmy trzy aplikacje:
1. Kalkulator dodający dwie liczby (MVC).
2. Lista "TODO" z przechowywaniem danych w sesji  (MVC).
3. Aplikacja do zarządzania ulubionymi cytatami z wykorzystaniem Local Storage.

---

## 1. Kalkulator

Aplikacja działa jako prosty kalkulator, który dodaje dwie liczby wprowadzone w formsie przez użytkownika.

- **Model**: Prosty model deklarujący dwie liczby oraz wynik.
- **Widok**: Formularz HTML umożliwiający wprowadzenie dwóch liczb oraz przycisk "Dodaj", który za pomocą post'a przesyła dane do kontrolera.
- **Kontroler**: Odbiera dane z formularza, dodaje liczby do siebie, a następnie zwraca wynik do widoku.

### Kod

#### Model
```csharp
// Kod modelu
public class CalculatorModel
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
    public int Result { get; set; }
}
```

#### Kontroler
```csharp
// Kod kontrolera, odpowiedzialny za dodanie liczb
public IActionResult AddNumbers(int number1, int number2)
{
    var result = number1 + number2;
    ViewData["Result"] = result;
    return View();
}
```

#### Widok
```csharp
// Kod widoku, form z post'em
<h2>Calculator - Add Two Numbers</h2>

<form method="post">
    <div class="form-group">
        <label for="number1">Number 1</label>
        <input type="number" class="form-control" id="number1" name="Number1" step="any" value="@Model.Number1" required>
    </div>
    <div class="form-group">
        <label for="number2">Number 2</label>
        <input type="number" class="form-control" id="number2" name="Number2" step="any" value="@Model.Number2" required>
    </div>
    <button type="submit" class="btn btn-primary">Add</button>
</form>

@if (Model.Result != 0)
{
    <h3>Result: @Model.Result</h3>
}

```
---

## 2. Aplikacja TODO list z sesją

### Opis funkcjonalności

Aplikacja umożliwia tworzenie i zarządzanie listą TODO gdzie wszystkie dane przechowywane są po stronie serwera w sesji. Użytkownik może dodawać nowe zadania, usuwać istniejące oraz oznaczać zadania jako ukończone.

- **Model**: Przechowuje dane dotyczące zadania
- **Widok**: Wyświetla listę zadań i pozwala na dodawanie, usuwanie i aktualizację statusu zadań za pomocą formsów z post'ami.
- **Kontroler**: Zarządza stanem sesji, w której przechowywane są zadania użytkownika.

### Kod

#### Model
```csharp
// Kod modelu, reprezentujący pojedyncze zadanie
public class TodoItem
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}
```

#### Kontroler
```csharp
// Kontroler zarządzający listą TODO z użyciem sesji
public IActionResult AddTodoItem(string description)
{
    var todoList = HttpContext.Session.Get<List<TodoItem>>("TodoList") ?? new List<TodoItem>();
    todoList.Add(new TodoItem { Description = description, IsCompleted = false });
    HttpContext.Session.Set("TodoList", todoList);
    return RedirectToAction("Index");
}
```

#### Widok
```csharp
// Widok pozwalający wpisywać nowe zadania i wyświetlać już wpisane
<h2>TODO List</h2>

<form method="post" asp-action="AddTask">
    <div class="form-group">
        <label for="task">New Task</label>
        <input type="text" class="form-control" id="task" name="task" placeholder="Enter task" required />
    </div>
    <button type="submit" class="btn btn-primary">Add Task</button>
</form>

<hr />

@if (Model.Count == 0)
{
    <p>No tasks added yet!</p>
}
else
{
    <ul class="list-group">
        @for (int i = 0; i < Model.Count; i++)
        {
            var todo = Model[i];
            <li class="list-group-item">
                <span style="@(todo.IsCompleted ? "text-decoration: line-through;" : "")">@todo.Task</span>

                @if (todo.IsCompleted)
                {
                    <!-- Unmark as Completed Button -->
                    <form method="post" asp-action="UnmarkAsCompleted" class="d-inline">
                        <input type="hidden" name="index" value="@i" />
                        <button type="submit" class="btn btn-sm btn-warning">Unmark as Completed</button>
                    </form>
                }
                else
                {
                    <!-- Mark as Completed Button -->
                    <form method="post" asp-action="MarkAsCompleted" class="d-inline">
                        <input type="hidden" name="index" value="@i" />
                        <button type="submit" class="btn btn-sm btn-success">Mark as Completed</button>
                    </form>
                }

                <!-- Remove Task Button -->
                <form method="post" asp-action="RemoveTask" class="d-inline">
                    <input type="hidden" name="index" value="@i" />
                    <button type="submit" class="btn btn-sm btn-danger">Remove</button>
                </form>
            </li>
        }
    </ul>
}
```
---

## 3. Aplikacja do zarządzania cytatami z Local Storage

### Opis funkcjonalności

Aplikacja do przechowywania cytatów użytkownika. Cytaty są przechowywane w Local Storage, co umożliwia ich dostępność po odświeżeniu strony.

- **Widok**: Formularz do dodawania, edytowania oraz usuwania cytatów.
- **Local Storage**: Przechowuje dane na urządzeniu użytkownika, umożliwiając ich dostępność bez konieczności ponownego ładowania z serwera.

### Widok

```csharp
/// Widok pozwalający na przesłanie cytatu z autorem oraz odpowiedzialny za wyświeletnie wszystkich cytatów
<h2>Favorite Quotes</h2>

<form id="quoteForm">
    <div class="form-group">
        <label for="quoteText">Quote</label>
        <input type="text" class="form-control" id="quoteText" placeholder="Enter quote" required />
    </div>
    <div class="form-group">
        <label for="quoteAuthor">Author</label>
        <input type="text" class="form-control" id="quoteAuthor" placeholder="Enter author" required />
    </div>
    <button type="submit" class="btn btn-primary">Add Quote</button>
</form>

<hr />

<h3>Your Quotes</h3>

<!-- Container for displaying quotes -->
<ul id="quoteList" class="list-group"></ul>
```

### Kod (JavaScript)

```javascript

// Funkcja pomocnicza do pobrania wszystkich cytatów z localStorage
function getQuotes() {
    let quotes = localStorage.getItem('quotes');
    return quotes ? JSON.parse(quotes) : [];
}

// Funkcja pomocnicza do zapisana cytatów w localStorage
function saveQuotes(quotes) {
    localStorage.setItem('quotes', JSON.stringify(quotes));
}

// Funkcja odpowiedzialna za wyświetlenie listy cytatów poprzez dodanie ich do listy w widoku o id = "quoteList"
function renderQuotes() {
    let quotes = getQuotes();
    let quoteList = document.getElementById('quoteList');
    quoteList.innerHTML = ''; 

    quotes.forEach((quote, index) => {
        let li = document.createElement('li');
        li.className = 'list-group-item d-flex justify-content-between align-items-center';
        
        let quoteText = document.createElement('span');
        quoteText.innerHTML = `"${quote.text}" - ${quote.author}`;
        
        let buttons = document.createElement('div');
        
        // Edit button
        let editButton = document.createElement('button');
        editButton.className = 'btn btn-sm btn-warning';
        editButton.innerText = 'Edit';
        editButton.onclick = function() {
            editQuote(index);
        };
        
        // Delete button
        let deleteButton = document.createElement('button');
        deleteButton.className = 'btn btn-sm btn-danger';
        deleteButton.innerText = 'Delete';
        deleteButton.onclick = function() {
            deleteQuote(index);
        };
        
        buttons.appendChild(editButton);
        buttons.appendChild(deleteButton);
        
        li.appendChild(quoteText);
        li.appendChild(buttons);
        quoteList.appendChild(li);
    });
}

// Funkcja dodająca nowy cytat
function addQuote(event) {
    event.preventDefault();
    
    let quoteText = document.getElementById('quoteText').value;
    let quoteAuthor = document.getElementById('quoteAuthor').value;
    
    if (quoteText && quoteAuthor) {
        let quotes = getQuotes();
        quotes.push({ text: quoteText, author: quoteAuthor });
        saveQuotes(quotes);
        renderQuotes();
        
        // Wyczyść formsa
        document.getElementById('quoteForm').reset();
    }
}

// Funkcja do edycji już istniejącego cytatu za pomocą prompta
function editQuote(index) {
    let quotes = getQuotes();
    let quote = quotes[index];
    
    let newQuoteText = prompt('Edit the quote text:', quote.text);
    let newQuoteAuthor = prompt('Edit the quote author:', quote.author);
    
    if (newQuoteText && newQuoteAuthor) {
        quotes[index] = { text: newQuoteText, author: newQuoteAuthor };
        saveQuotes(quotes);
        renderQuotes();
    }
}

// Funkcja usuwająca cytat z localStorage
function deleteQuote(index) {
    let quotes = getQuotes();
    quotes.splice(index, 1);
    saveQuotes(quotes);
    renderQuotes();
}

// Zainicjowanie widoku z wszystkimi cytatami jakie się znajdują w localStorage
document.getElementById('quoteForm').addEventListener('submit', addQuote);
document.addEventListener('DOMContentLoaded', renderQuotes);
---


