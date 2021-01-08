using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
//using Dag_11;

namespace Dag_11.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			//Arrange
			var stoelen = new Stoelen(new List<string>
			{
				"###",
				"...",
				"###"
			});

			//Act
			var buren = stoelen.VindBuren(stoelen.stoelen[0]);
			//Assert
			Assert.Equal(1, buren);
		}

		[Fact]
		public void Test2()
		{
			//Arrange
			var stoelen = new Stoelen(new List<string>
			{
				"###",
				"...",
				"###"
			});

			//Act
			var buren = stoelen.VindBuren(stoelen.stoelen[4]);
			//Assert
			//Assert.Equal(1, buren1);
			Assert.Equal(6, buren);
		}
	}
}