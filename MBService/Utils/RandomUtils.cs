using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueMBService.Utils
{
    public  class RandomUtils
    {
		public static List<int> RandomNumbers(int n, int min, int max, List<int> excludes)
		{
			List<int> list = new List<int>();
			Random random = new Random();
			while (list.Count != n)
			{
				int item = random.Next(min, max);
				bool flag = !list.Contains(item) && !excludes.Contains(item);
				if (flag)
				{
					list.Add(item);
				}
			}
			list.Sort();
			return list;
		}
	}
}
