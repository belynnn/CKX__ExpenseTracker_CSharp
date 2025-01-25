using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger; // Logger pour enregistrer des messages liés au contrôle de l'application

		private readonly ExpenseTrackerDbContext _context; // Contexte de base de données pour accéder aux entités

		// Constructeur injectant le logger et le contexte de base de données
		public HomeController(ILogger<HomeController> logger, ExpenseTrackerDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		// Affiche la page d'accueil
		public IActionResult Index()
		{
			return View();
		}

		// Affiche la liste des dépenses et calcule le total des dépenses
		public IActionResult Expenses()
		{
			// Récupère toutes les dépenses depuis la base de données
			var allExpenses = _context.Expenses.ToList();

			// Calcule le total des dépenses
			var totalExpenses = allExpenses.Sum(expense => expense.Value);

			// Envoie le total à la vue via ViewBag
			ViewBag.Expenses = totalExpenses;

			// Passe la liste des dépenses à la vue
			return View(allExpenses);
		}

		// Gère la création ou la modification d'une dépense
		public IActionResult CreateEditExpense(int? id) // id nullable pour différencier création et édition
		{
			if (id != null)
			{
				// Charge une dépense existante à partir de son ID pour l'éditer
				var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

				return View(expenseInDb);
			}

			// Renvoie une vue vierge pour créer une nouvelle dépense
			return View();
		}

		// Supprime une dépense existante
		public IActionResult DeleteExpense(int id)
		{
			// Recherche la dépense correspondante dans la base de données
			var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

			// Supprime la dépense si elle est trouvée
			_context.Expenses.Remove(expenseInDb);

			// Sauvegarde les changements dans la base de données
			_context.SaveChanges();

			// Redirige vers la liste des dépenses après la suppression
			return RedirectToAction("Expenses");
		}

		// Gère la sauvegarde d'une dépense (création ou modification)
		public IActionResult CreateEditExpenseForm(Expense model)
		{
			if (model.Id == 0)
			{
				// Ajoute une nouvelle dépense à la base de données
				_context.Expenses.Add(model);
			}
			else
			{
				// Met à jour une dépense existante
				_context.Expenses.Update(model);
			}

			// Sauvegarde les changements dans la base de données
			_context.SaveChanges();

			// Redirige vers la liste des dépenses après la sauvegarde
			return RedirectToAction("Expenses");
		}

		// Gère les erreurs et affiche une page d'erreur
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			// Envoie un modèle d'erreur avec l'ID de la requête pour aider au débogage
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}