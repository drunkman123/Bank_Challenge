namespace Desafio_Nubank
{
    public class OperationsCalcs
    {
        public static List<Tax> CalculateTaxes(IEnumerable<Operation> operations)
        {
            var taxes = new List<Tax>();
            int StocksBalance = 0;
            decimal Cost = 0;
            decimal OperationValue = 0.00m;
            decimal Tax = 0.00m;
            decimal TaxOffset = 0.00m;
            decimal AveragePrice = 0.00m;
            decimal Profit = 0.00m;
            foreach (var operation in operations)
            {
                if (StocksBalance == 0)
                {
                    ClearCostAndAveragePrice(ref AveragePrice, ref Cost);
                }
                OperationValue = operation.UnitCost * operation.Quantity;
                if (operation.Type == OperationType.Buy)
                {
                    BuyOperation(ref StocksBalance, operation.Quantity, ref Cost, ref OperationValue, ref AveragePrice, ref taxes);
                    continue;
                }
                if (OperationValue < 20000)
                {
                    SellOperationLessThan20k(operation.UnitCost, ref AveragePrice, ref TaxOffset, operation.Quantity, ref StocksBalance, ref Cost, ref OperationValue, ref taxes);
                    continue;
                }
                else if (operation.UnitCost > AveragePrice)
                {
                    SellOperationWithProfit(ref Tax, ref Profit, ref AveragePrice, ref TaxOffset, operation.Quantity, ref StocksBalance, ref OperationValue, ref taxes);
                    continue;
                }
                else if (operation.UnitCost < AveragePrice)
                {
                    SellOperationWithLoss(ref Profit, ref AveragePrice, ref TaxOffset, operation.UnitCost, operation.Quantity, ref StocksBalance, ref OperationValue, ref taxes);
                    continue;
                }
                else if (operation.UnitCost == AveragePrice)
                {
                    SellNoProfitNoLoss(operation.Quantity, ref StocksBalance, ref taxes);
                }
            }
            return taxes;
        }
        public static void ClearCostAndAveragePrice(ref decimal AveragePrice, ref decimal Cost)
        {
            AveragePrice = 0;
            Cost = 0;
        }
        public static void BuyOperation(ref int StocksBalance, int OperationQuantity, ref decimal Cost, ref decimal OperationValue, ref decimal AveragePrice, ref List<Tax> taxes)
        {
            StocksBalance += OperationQuantity;
            Cost -= OperationValue;
            AveragePrice = -1 * Cost / StocksBalance;
            taxes.Add(new Tax() { tax = 0.00m });
        }        
        public static void SellOperationLessThan20k(decimal OperationUnitCost, ref decimal AveragePrice, ref decimal TaxOffset, int OperationQuantity, ref int StocksBalance, ref decimal Cost, ref decimal OperationValue, ref List<Tax> taxes)
        {
            if (OperationUnitCost < AveragePrice)
            {
                TaxOffset -= (AveragePrice - OperationUnitCost) * OperationQuantity;
            }
            StocksBalance -= OperationQuantity;
            Cost += OperationValue;
            taxes.Add(new Tax() { tax = 0.00m });
        }
        public static void SellOperationWithProfit(ref decimal Tax,ref decimal Profit, ref decimal AveragePrice, ref decimal TaxOffset, int OperationQuantity, ref int StocksBalance, ref decimal OperationValue, ref List<Tax> taxes)
        {
            Profit = OperationValue - OperationQuantity * AveragePrice;
            if (TaxOffset < 0 && Profit > 0)
            {
                if (Profit > TaxOffset * -1) Profit += TaxOffset;
                else
                {
                    TaxOffset += Profit;
                    Profit = 0;
                };
            }
            StocksBalance -= OperationQuantity;
            if (Profit > 0)
            {
                Tax = Profit * 20 / 100;
            }
            taxes.Add(new Tax() { tax = Tax });
        }
        public static void SellOperationWithLoss(ref decimal Profit, ref decimal AveragePrice, ref decimal TaxOffset,decimal OperationUnitCost, int OperationQuantity, ref int StocksBalance, ref decimal OperationValue, ref List<Tax> taxes)
        {
            Profit = OperationValue - OperationQuantity * AveragePrice;
            if (TaxOffset < 0 && Profit > 0)
            {
                if (Profit > TaxOffset * -1)
                    Profit += TaxOffset;
                else 
                    TaxOffset -= Profit;
            }
            if (OperationUnitCost < AveragePrice) 
                TaxOffset += Profit;
            StocksBalance -= OperationQuantity;
            taxes.Add(new Tax() { tax = 0.00m });
        }
        public static void SellNoProfitNoLoss(int OperationQuantity, ref int StocksBalance, ref List<Tax> taxes)
        {
            StocksBalance -= OperationQuantity;
            taxes.Add(new Tax() { tax = 0.00m });
        }
    }
}
