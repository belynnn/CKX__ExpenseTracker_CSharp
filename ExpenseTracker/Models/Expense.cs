using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
	public class Expense
	{
		// Identifiant unique de la dépense (clé primaire)
		public int Id { get; set; }

		// Montant de la dépense (en décimal pour supporter les valeurs monétaires précises)
		public decimal Value { get; set; }

		// Description de la dépense (obligatoire grâce à l'attribut [Required])
		[Required]
		public string Description { get; set; }
	}
}