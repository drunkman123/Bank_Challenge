using System.Linq;

namespace Desafio_Nubank.Tests
{
    public class OperationsCalcsTests
    {
        [Fact]
        public void CalculateTaxes_Case1()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 100),
            new Operation(OperationType.Sell,15.00m, 50),
            new Operation(OperationType.Sell,15.00m, 50)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case2()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell,20.00m, 5000),
            new Operation(OperationType.Sell,5.00m, 5000)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 10000.00m },
            new Tax { tax = 0.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case1Plus2()
        {
            // Arrange
            var twoOperations = new List<IEnumerable<Operation>>();
            var TestTaxes = new List<List<Tax>>();

            var case1 = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 100),
            new Operation(OperationType.Sell,15.00m, 50),
            new Operation(OperationType.Sell,15.00m, 50)
        };
            var case2 = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell,20.00m, 5000),
            new Operation(OperationType.Sell,5.00m, 5000)
        };
            twoOperations.Add(case1);
            twoOperations.Add(case2);

            var expectedTaxes1 = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m }
        };
            var expectedTaxes2 = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 10000.00m },
            new Tax { tax = 0.00m }
        };
            TestTaxes.Add(expectedTaxes1);
            TestTaxes.Add(expectedTaxes2);

            // Act
            var outputedTaxes = new List<List<Tax>>();
            foreach (var operation in twoOperations)
            {
                outputedTaxes.Add(OperationsCalcs.CalculateTaxes(operation));
            }

            // Assert
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.Equal(TestTaxes[i][j].tax, outputedTaxes[i][j].tax);
                }
            }
        }

        [Fact]
        public void CalculateTaxes_Case3()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell,5.00m, 5000),
            new Operation(OperationType.Sell,20.00m, 3000)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 1000.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case4()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Buy, 25.00m, 5000),
            new Operation(OperationType.Sell,15.00m, 10000)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case5()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Buy, 25.00m, 5000),
            new Operation(OperationType.Sell,15.00m, 10000),
            new Operation(OperationType.Sell,25.00m, 5000)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 10000.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case6()
        {
            // Arrange
            var operations = new List<Operation>
        {

            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell, 2.00m, 5000),
            new Operation(OperationType.Sell,20.00m, 2000),
            new Operation(OperationType.Sell,20.00m, 2000),
            new Operation(OperationType.Sell,25.00m, 1000),
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 3000.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case7()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell, 2.00m, 5000),
            new Operation(OperationType.Sell,20.00m, 2000),
            new Operation(OperationType.Sell,20.00m, 2000),
            new Operation(OperationType.Sell,25.00m, 1000),
            new Operation(OperationType.Buy, 20.00m, 10000),
            new Operation(OperationType.Sell,15.00m, 5000),
            new Operation(OperationType.Sell,30.00m, 4350),
            new Operation(OperationType.Sell,30.00m, 650)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 3000.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 3700.00m },
            new Tax { tax = 0.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }

        [Fact]
        public void CalculateTaxes_Case8()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 10.00m, 10000),
            new Operation(OperationType.Sell,50.00m, 10000),
            new Operation(OperationType.Buy, 20.00m, 10000),
            new Operation(OperationType.Sell,50.00m, 10000)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 80000.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 60000.00m }
        };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(expectedTaxes.Count, taxes.Count);
            for (int i = 0; i < expectedTaxes.Count; i++)
            {
                Assert.Equal(expectedTaxes[i].tax, taxes[i].tax);
            }
        }
    }
}