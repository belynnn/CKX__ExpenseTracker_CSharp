using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger; // Logger pour enregistrer des messages li�s au contr�le de l'application

		private readonly ExpenseTrackerDbContext _context; // Contexte de base de donn�es pour acc�der aux entit�s

		// Constructeur injectant le logger et le contexte de base de donn�es
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

		// Affiche la liste des d�penses et calcule le total des d�penses
		public IActionResult Expenses()
		{
			// R�cup�re toutes les d�penses depuis la base de donn�es
			var allExpenses = _context.Expenses.ToList();

			// Calcule le total des d�penses
			var totalExpenses = allExpenses.Sum(expense => expense.Value);

			// Envoie le total � la vue via ViewBag
			ViewBag.Expenses = totalExpenses;

			// Passe la liste des d�penses � la vue
			return View(allExpenses);
		}

		// G�re la cr�ation ou la modification d'une d�pense
		public IActionResult CreateEditExpense(int? id) // id nullable pour diff�rencier cr�ation et �dition
		{
			if (id != null)
			{
				// Charge une d�pense existante � partir de son ID pour l'�diter
				var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

				return View(expenseInDb);
			}

			// Renvoie une vue vierge pour cr�er une nouvelle d�pense
			return View();
		}

		// Supprime une d�pense existante
		public IActionResult DeleteExpense(int id)
		{
			// Recherche la d�pense correspondante dans la base de donn�es
			var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

			// Supprime la d�pense si elle est trouv�e
			_context.Expenses.Remove(expenseInDb);

			// Sauvegarde les changements dans la base de donn�es
			_context.SaveChanges();

			// Redirige vers la liste des d�penses apr�s la suppression
			return RedirectToAction("Expenses");
		}

		// G�re la sauvegarde d'une d�pense (cr�ation ou modification)
		public IActionResult CreateEditExpenseForm(Expense model)
		{
			if (model.Id == 0)
			{
				// Ajoute une nouvelle d�pense � la base de donn�es
				_context.Expenses.Add(model);
			}
			else
			{
				// Met � jour une d�pense existante
				_context.Expenses.Update(model);
			}

			// Sauvegarde les changements dans la base de donn�es
			_context.SaveChanges();

			// Redirige vers la liste des d�penses apr�s la sauvegarde
			return RedirectToAction("Expenses");
		}

		// G�re les erreurs et affiche une page d'erreur
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			// Envoie un mod�le d'erreur avec l'ID de la requ�te pour aider au d�bogage
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}