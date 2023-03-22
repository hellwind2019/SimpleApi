using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]


namespace SimpleApi;

public class Functions
{
    private static readonly UpdateService updateService;

    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions()
    {
    }

    private static readonly HttpClient client = new HttpClient();


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The API Gateway response.</returns>
    public APIGatewayProxyResponse Get(JObject request, ILambdaContext context)
    {
        context.Logger.LogInformation("Get Request\n");
        string botToken = "bot5995864096:AAFvvRBUzfgmGuUeI0CMA10W1FMq2Ec72iQ";
        string chatId = "933138922";
        client.GetStringAsync(
                $"https://api.telegram.org/bot5995864096:AAFvvRBUzfgmGuUeI0CMA10W1FMq2Ec72iQ/sendMessage?&chat_id={chatId}&text={request.GetValue("body")}")
            .Wait();
        try
        {
            var body = request.GetValue("body");
            var Jbody = JObject.Parse(body.ToString());
            Update? update = Jbody.ToObject<Update>();
            updateService.EchoAsync(update);
            client.GetStringAsync(
                  $"https://api.telegram.org/bot5995864096:AAFvvRBUzfgmGuUeI0CMA10W1FMq2Ec72iQ/sendMessage?&chat_id={update.Message.Chat.Id}&text=Будь здоров, {update.Message.Chat.FirstName}")
            .Wait();
        }
        catch (Exception e)
        {
            context.Logger.LogInformation("error Request\n");
        }
       


        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = "something",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }
}