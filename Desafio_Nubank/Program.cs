using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GanhoCapital
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;

            while ((input = Console.ReadLine()) != null && !String.IsNullOrEmpty(input))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter());
                string[] ArrayInput;
                input = input.Replace("] [", "]] [[");
                ArrayInput = (input.Split("] ["));
                List<IEnumerable<Operation>> operationList = new List<IEnumerable<Operation>>();
                foreach (string line in ArrayInput)
                {
                    //If there is different block operations together (case 1 + case 2 e.g.)
                    operationList.Add(JsonSerializer.Deserialize<IEnumerable<Operation>>(line, options));
                }

                foreach (var operation in operationList)
                {
                    //Tax calculation for each single operation
                    var taxes = CalculateTaxes(operation);

                    Console.WriteLine(JsonSerializer.Serialize(taxes));
                }

            }

            Console.ReadLine(); // aguarda a entrada do usuário para encerrar o console
        }

        static List<Tax> CalculateTaxes(IEnumerable<Operation> operations)
        {

            var taxes = new List<Tax>();
            double StocksBalance = 0;
            double Cost = 0;
            double OperationValue = 0;
            double Tax = 0;
            double TaxOffset = 0;
            double AveragePrice = 0;
            double Profit = 0;
            foreach (var operation in operations)
            {
                if (StocksBalance == 0)
                {
                    AveragePrice = 0;
                    Cost = 0;
                }
                OperationValue = operation.UnitCost * operation.Quantity;
                if (operation.Type == OperationType.Buy)
                {
                    StocksBalance += operation.Quantity;
                    Cost -= OperationValue;
                    AveragePrice = -1* Cost / StocksBalance;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }
                if(OperationValue < 20000)
                {
                    if(operation.UnitCost < AveragePrice)
                    {
                        TaxOffset -= (AveragePrice - operation.UnitCost)*operation.Quantity;
                    }
                    StocksBalance -= operation.Quantity;
                    Cost += OperationValue;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }
                else if (operation.UnitCost > AveragePrice)
                {
                    Profit = OperationValue - operation.Quantity* AveragePrice;
                    if(TaxOffset < 0 && Profit > 0)
                    {
                        if (Profit > TaxOffset * -1) Profit += TaxOffset;
                        else 
                        {
                            TaxOffset += Profit;
                            Profit = 0;
                        };
                    }
                    StocksBalance -= operation.Quantity;
                    if(Profit > 0)
                    {
                        Tax = Profit * 20/100;
                    }
                    taxes.Add(new Tax() { tax = Tax });
                    continue;
                }
                else if (operation.UnitCost < AveragePrice)
                {
                    Profit = OperationValue - operation.Quantity * AveragePrice;
                    if (TaxOffset < 0 && Profit > 0)
                    {
                        if (Profit > TaxOffset * -1) Profit += TaxOffset;
                        else TaxOffset -= Profit;
                    }
                    if (operation.UnitCost < AveragePrice)
                    {
                        TaxOffset += Profit;
                    }
                    StocksBalance -= operation.Quantity;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }else if (operation.UnitCost == AveragePrice)
                {
                    StocksBalance -= operation.Quantity;
                    taxes.Add(new Tax() { tax = 0 });
                }
            }
            return taxes;
        }
    }
}