using System.Diagnostics;
using KnapsackProblem.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// Summary description for CalculatorTests
    /// </summary>
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine(Calculator.Choose(10, 5));

        }
    }
}
