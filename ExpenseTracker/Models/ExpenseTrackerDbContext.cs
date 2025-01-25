using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
	public class ExpenseTrackerDbContext : DbContext
	{
		// Représente la table "Expenses" dans la base de données
		public DbSet<Expense> Expenses { get; set; }

		// Constructeur acceptant les options de configuration pour le contexte
		public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
		{
			// Appelle le constructeur de la classe parente DbContext avec les options
		}
	}
}