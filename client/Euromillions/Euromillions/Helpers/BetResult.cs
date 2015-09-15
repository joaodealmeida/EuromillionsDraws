using System;
using System.Linq;

namespace Euromillions
{
	public class BetResult
	{
		public  BetResult ()
		{
		}

		public bool checkPrize(Bet toCheck, Bet drawn){
			string[] numbersDrawn = drawn.Numbers.Split ('-');
			string[] starsDrawn = drawn.Stars.Split ('-');

			string[] numbersToCheck = toCheck.Numbers.Split ('-');
			string[] starsToCheck = toCheck.Stars.Split ('-');

			var commonNumbers = numbersDrawn.Intersect (numbersToCheck);
			var commonStars = starsDrawn.Intersect (starsToCheck);

			//0 estrelas minimo 2 numeros
			if (commonStars.Count() == 0)
			{
				if (commonNumbers.Count()>= 2) {
					return true;
				} else
				{
					return false;
				}
			}
			//1 estrela minimo 2 numeros
			if (commonStars.Count() == 1)
			{
				if (commonNumbers.Count() >= 2) {
					return true;
				} else
				{
					return false;
				}
				
			}
			//2 estrelas minimo 1 numero
			if (commonStars.Count() == 2)
			{
				if (commonNumbers.Count() >= 1) {
					return true;
				} else
				{
					return false;
				}

			}
			//1 numero minimo 2 estrelas
			if (commonNumbers.Count() == 1)
			{
				if (commonStars.Count() >= 2) {
					return true;
				} else
				{
					return false;
				}

			}

			return false;
		}
	}
}

