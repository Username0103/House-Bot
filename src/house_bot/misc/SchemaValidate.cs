
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;

namespace root.src.house_bot.Misc
{
    public class SchemaValidate(JsonModels models)
    {
        private readonly JsonSchema schema = models.MapSchema;

        public async Task<string?> ValidateJson(string input)
        {
            return await Task.Run(() =>
            {
                JsonNode? inputNode;
                try
                {
                    inputNode = JsonNode.Parse(input);
                }
                catch (JsonException ex)
                {
                    return $"Invalid JSON syntax: {ex.Message}";
                }
                EvaluationResults result = schema.Evaluate(inputNode);
                if (!result.IsValid)
                {
                    return "Invalid JSON input. Please consult the /help_admin command.";
                }
                return null;
            }
            );
        }
    }
}