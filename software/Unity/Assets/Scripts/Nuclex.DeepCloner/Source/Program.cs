using System;
using System.Collections.Generic;

using Nuclex.Support.Cloning;

namespace DeepCloneDemo {

	#region class ComplicatedStuff

	/// <summary>Class doing some complicated stuff</summary>
	public class ComplicatedStuff {

		/// <summary>Initializes a new instance of the complicated class</summary>
		/// <param name="nonDefaultConstructorArg">
		///   A value that will be stored in a somewhat creative way
		/// </param>
		public ComplicatedStuff(int nonDefaultConstructorArg) {
			this.values = new List<int>(nonDefaultConstructorArg);

			this.texts = new string[2, 2];
			this.texts[0, 1] = "Hello";
			this.texts[1, 0] = "World";
		}

		/// <summary>Returns a message stored in a complicated way</summary>
		/// <returns>The message stored by the instance</returns>
		public string SaySomething() {
			return
				this.texts[0, 1] + " " +
				this.texts[1, 0] + " " +
				this.values.Capacity.ToString();
		}

		/// <summary>Stores a series of values that are not used</summary>
		private List<int> values;
		/// <summary>Stores some texts in a multi-dimensional array</summary>
		private string[,] texts;

	}

	#endregion // class ComplicatedStuff

	/// <summary>Contains the application's startup code</summary>
	class Program {

		/// <summary>Main entry point for the application</summary>
		/// <param name="args">
		///   Parameters passed to the application on the command line
		/// </param>
		static void Main(string[] args) {
			var original = new ComplicatedStuff(42);
			Console.WriteLine("Original: " + original.SaySomething());

			ComplicatedStuff clone1 = SerializationCloner.DeepFieldClone(original);
			Console.WriteLine("SerializationCloner: " + clone1.SaySomething());

			ComplicatedStuff clone2 = ReflectionCloner.DeepFieldClone(original);
			Console.WriteLine("ReflectionCloner: " + clone1.SaySomething());
		}

	}

} // namespace DeepCloneDemo
