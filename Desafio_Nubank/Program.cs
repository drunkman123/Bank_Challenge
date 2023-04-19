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
                string[] inputao;
                input = input.Replace("] [", "]] [[");
                inputao = (input.Split("] ["));
                List<List<Operation>> operationList = new List<List<Operation>>();
                foreach (string line in inputao)
                {
                    operationList.Add(JsonSerializer.Deserialize<List<Operation>>(line, options));
                }

                //List<Operation> operations = JsonSerializer.Deserialize<List<Operation>>(input, options);
                foreach (var operation in operationList)
                {
                    var taxes = CalculateTaxes(operation);

                    Console.WriteLine(JsonSerializer.Serialize(taxes));
                }

            }

            Console.ReadLine(); // aguarda a entrada do usuário para encerrar o console
        }

        static List<Tax> CalculateTaxes(List<Operation> operations)
        {
            var taxes = new List<Tax>();
            double QuantidadeDeAcoes = 0;
            double Custo = 0;
            double ValorOperacao = 0;
            double Imposto = 0;
            double ImpostoAbater = 0;
            double PrecoMedio = 0;
            double Lucro = 0;
            foreach (var operation in operations)
            {
                if (QuantidadeDeAcoes == 0)
                {
                    PrecoMedio = 0;
                    Custo = 0;
                }
                ValorOperacao = operation.UnitCost * operation.Quantity;
                if (operation.Type == OperationType.Buy)
                {
                    QuantidadeDeAcoes += operation.Quantity;
                    Custo -= ValorOperacao;
                    PrecoMedio = -1*Custo/ QuantidadeDeAcoes;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }
                if(ValorOperacao < 20000)
                {
                    if(operation.UnitCost < PrecoMedio)
                    {
                        ImpostoAbater -= (PrecoMedio-operation.UnitCost)*operation.Quantity;
                    }
                    QuantidadeDeAcoes -= operation.Quantity;
                    Custo += ValorOperacao;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }
                else if (operation.UnitCost > PrecoMedio)
                {
                    //Custo += ValorOperacao;
                    Lucro = ValorOperacao-operation.Quantity*PrecoMedio;
                    if(ImpostoAbater < 0 && Lucro > 0)
                    {
                        if (Lucro > ImpostoAbater*-1) Lucro += ImpostoAbater;
                        else 
                        {
                            ImpostoAbater += Lucro;
                            Lucro = 0;
                        };
                    }
                    QuantidadeDeAcoes -= operation.Quantity;
                    if(Lucro > 0)
                    {
                        Imposto = Lucro*20/100;
                    }
                    taxes.Add(new Tax() { tax = Imposto });
                    continue;
                }
                else if (operation.UnitCost < PrecoMedio)
                {
                    Lucro = ValorOperacao - operation.Quantity * PrecoMedio;
                    if (ImpostoAbater < 0 && Lucro > 0)
                    {
                        if (Lucro > ImpostoAbater*-1) Lucro += ImpostoAbater;
                        else ImpostoAbater -= Lucro;
                    }
                    if (operation.UnitCost < PrecoMedio)
                    {
                        ImpostoAbater += Lucro;
                    }
                    QuantidadeDeAcoes -= operation.Quantity;
                    taxes.Add(new Tax() { tax = 0 });
                    continue;
                }else if (operation.UnitCost == PrecoMedio)
                {
                    QuantidadeDeAcoes -= operation.Quantity;
                    taxes.Add(new Tax() { tax = 0 });
                }
            }
            return taxes;
        }
    }

    class Operation
    {
        [JsonPropertyName("operation")]
        public OperationType Type { get; set; }

        [JsonPropertyName("unit-cost")]
        public double UnitCost { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    enum OperationType
    {
        Buy,
        Sell
    }

    class Tax
    {
        public double tax { get; set; }
    }
}