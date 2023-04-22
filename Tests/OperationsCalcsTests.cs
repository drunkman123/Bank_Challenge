namespace Desafio_Nubank.Tests
{
    public class OperationsCalcsTests
    {
        [Fact]
        public void CalculateTaxes_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var operations = new List<Operation>();

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Empty(taxes);
        }

        [Fact]
        public void CalculateTaxes_WithOneBuyOperation_ReturnsListWithZeroTax()
        {
            // Arrange
            var operations = new List<Operation> { new Operation(OperationType.Buy, 100, 10) };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(0.00m, taxes[0].tax);            
        }

        [Fact]
        public void CalculateTaxes_WithOneSellOperationLessThan20k_ReturnsListWithZeroTax()
        {
            // Arrange
            var operations = new List<Operation> { new Operation(OperationType.Sell, 90, 5) };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(0.00m, taxes[0].tax);
        }

        [Fact]
        public void CalculateTaxes_WithOneSellOperationWithProfit_ReturnsListWithExpectedTax()
        {
            // Arrange
            var operations = new List<Operation> { new Operation(OperationType.Buy, 100, 1000), new Operation(OperationType.Sell, 150, 500) };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(0.00m, taxes[0].tax);
            Assert.Equal(5000.00m, taxes[1].tax);
        }

        [Fact]
        public void CalculateTaxes_WithOneSellOperationWithLoss_ReturnsListWithZeroTax()
        {
            // Arrange
            var operations = new List<Operation> { new Operation(OperationType.Buy, 100, 5), new Operation(OperationType.Sell, 80, 5) };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(0.00m, taxes[1].tax);
        }

        [Fact]
        public void CalculateTaxes_WithOneSellOperationWithZeroProfitAndLoss_ReturnsListWithZeroTax()
        {
            // Arrange
            var operations = new List<Operation> { new Operation(OperationType.Buy, 100, 5), new Operation(OperationType.Sell, 100, 5) };

            // Act
            var taxes = OperationsCalcs.CalculateTaxes(operations);

            // Assert
            Assert.Equal(0.00m, taxes[0].tax);
            Assert.Equal(0.00m, taxes[1].tax);
        }
        [Fact]
        public void CalculateTaxes_WithSellOperationsAndTaxGreaterThan0()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 1000.00m, 100),
            new Operation(OperationType.Sell,2000.00m, 50),
            new Operation(OperationType.Sell,1000.00m, 50)
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
        public void CalculateTaxes_WithSellOperationsAndTaxOffsetGreaterThan0()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation(OperationType.Buy, 1000.00m, 100),
            new Operation(OperationType.Sell,50.00m, 50),
            new Operation(OperationType.Sell,2000.00m, 50)
        };
            var expectedTaxes = new List<Tax>
        {
            new Tax { tax = 0.00m },
            new Tax { tax = 0.00m },
            new Tax { tax = 500.00m }
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