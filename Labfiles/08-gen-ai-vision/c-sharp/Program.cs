using Azure;
using Azure.AI.Inference;
using Azure.AI.Projects;
using Azure.Core;
using Azure.Core.Pipeline;
// Add references
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chat_app
{
    class Program
    {
        static void Main(string[] args)
        {
            // Clear the console
            Console.Clear();
            
            try
            {
                // Get configuration settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string project_connection = configuration["PROJECT_CONNECTION"];
                string model_deployment = configuration["MODEL_DEPLOYMENT"];

                

                // Initialize the project client
                //var projectClient = new AIProjectClient(project_connectionuri,
                //    new DefaultAzureCredential());

                //using Azure;
                //using Azure.Identity;
                //using Azure.AI.Inference;
                //using Azure.Core;
                //using Azure.Core.Pipeline;

                //var endpointUrl = project_connection; // Replace with your actual endpoint
                var credential = new DefaultAzureCredential();

                Uri endpointUrl = new Uri(project_connection);

                AzureAIInferenceClientOptions clientOptions = new AzureAIInferenceClientOptions();
                BearerTokenAuthenticationPolicy tokenPolicy = new BearerTokenAuthenticationPolicy(
                    credential,
                    new string[] { "https://cognitiveservices.azure.com/.default" }
                );
                clientOptions.AddPolicy(tokenPolicy, HttpPipelinePosition.PerRetry);

                var projectClient = new ChatCompletionsClient(
                    endpointUrl,
                    credential,
                    clientOptions
                );

                // Get a chat client
                // Get a chat client
                // ChatCompletionsClient chat = projectClient.GetChatCompletionsClient();




                // Initialize prompts
                string system_message = "You are an AI assistant in a grocery store that sells fruit.";
                string prompt = "";

                // Loop until the user types 'quit'
                while (prompt.ToLower() != "quit")
                {
                    // Get user input
                    Console.WriteLine("\nAsk a question about the image\n(or type 'quit' to exit)\n");
                    prompt = Console.ReadLine().ToLower();
                    if (prompt == "quit")
                    {
                        break;
                    }
                    else if (prompt.Length < 1)
                    {
                        Console.WriteLine("Please enter a question.\n");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Getting a response ...\n");


                        // Get a response to image input
                        // Get a response to image input
                        string imageUrl = "https://github.com/MicrosoftLearning/mslearn-ai-vision/raw/refs/heads/main/Labfiles/08-gen-ai-vision/orange.jpeg";
                        ChatCompletionsOptions requestOptions = new ChatCompletionsOptions()
                        {
                            Messages = {
                            new ChatRequestSystemMessage(system_message),
                            new ChatRequestUserMessage([
                            new ChatMessageTextContentItem(prompt),
                            new ChatMessageImageContentItem(new Uri(imageUrl))
                            ]),
                        },
                            Model = model_deployment
                        };
                        var response = projectClient.Complete(requestOptions);
                        Console.WriteLine(response.Value.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

