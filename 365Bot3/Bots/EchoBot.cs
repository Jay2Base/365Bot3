// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.1

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Azure;
using System.Linq;




namespace _365Bot3.Bots
{
    public class EchoBot : ActivityHandler
    {


        private static CosmosDbPartitionedStorage MyStorage =

            new CosmosDbPartitionedStorage(new CosmosDbPartitionedStorageOptions
           {
               CosmosDbEndpoint = "https://365db.documents.azure.com:443/",
               AuthKey = "wprejZdl9ybqxTdb3SxESe1C6qTfNjPqsOiCFwU4E4YM5qf4GxDSCP2UJ6aFZGiwC3lY5RTDrZpVpqYY3THmQQ==",
               DatabaseId = "365db",
               ContainerId = "365db-container",
               CompatibilityMode = false
           });


        

        public class UtteranceLog : IStoreItem
        {
            public List<deets> UtteranceList { get; } =  new List<deets>();
            public int TurnNumber{get; set;} = 0;
            public string ETag { get; set; } = "*";

        }

        
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            deets utterance = new deets();
            utterance.MessageText = turnContext.Activity.Text;
            utterance.MessageRecipient = turnContext.Activity.From.ToString();
            utterance.MessageDate = turnContext.Activity.LocalTimestamp.ToString();
            UtteranceLog logItems = null;

            try
            {

                string[] utteranceList = { "UtteranceLog" };
                logItems = MyStorage.ReadAsync<UtteranceLog>(utteranceList).Result?.FirstOrDefault().Value;

            }
            catch
            {
                // Inform the user an error occured.
                await turnContext.SendActivityAsync("Sorry, something went wrong reading your stored messages!");

            }

            if (logItems is null)
            {
                logItems = new UtteranceLog();
                logItems.UtteranceList.Add(utterance);
                logItems.TurnNumber++;

                await turnContext.SendActivityAsync($"{logItems.TurnNumber}: The list is now: {logItems.UtteranceList.Count()} messages long");

                var changes = new Dictionary<string, object>();
                {
                    changes.Add("UtteranceLog", logItems);
                };

                try
                {
                    await MyStorage.WriteAsync(changes, cancellationToken);
                    await turnContext.SendActivityAsync($"Your message was saved");
                }


                catch
                {
                    await turnContext.SendActivityAsync($"Your message was all fucked up");
                }
            }
            else
            {
                logItems.UtteranceList.Add(utterance);
                logItems.TurnNumber++;

                await turnContext.SendActivityAsync($"{logItems.TurnNumber}: The list is now: {logItems.UtteranceList.Count()} messages long");

                var changes = new Dictionary<string, object>();
                {
                    changes.Add("UtteranceLog", logItems);

                }
                try
                {
                    // Save new list to your Storage.
                    await MyStorage.WriteAsync(changes, cancellationToken);
                    await turnContext.SendActivityAsync($"Your message was saved");
                }
                catch
                {
                    // Inform the user an error occured.
                    await turnContext.SendActivityAsync("Sorry, something went wrong storing your message!");
                }
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
