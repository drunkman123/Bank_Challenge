using Desafio_Nubank;
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
                    var taxes = OperationsCalcs.CalculateTaxes(operation);

                    Console.WriteLine(JsonSerializer.Serialize(taxes));
                }

            }

            Console.ReadLine(); // aguarda a entrada do usuário para encerrar o console
        }
    }
}