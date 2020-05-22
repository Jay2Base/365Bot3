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



        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            try 
            {
                var messageDeets = new MessageRecord();
                messageDeets.MessageTime = turnContext.Activity.Timestamp.ToString();
                messageDeets.MesageText = turnContext.Activity.Text;
                messageDeets.MessageFrom = turnContext.Activity.From.Name;
                messageDeets.MessageTo = turnContext.Activity.Recipient.Name;

                var changes = new Dictionary<string, object>();
                {
                    changes.Add("MessageRecords", messageDeets);
                }


                await MyStorage.WriteAsync(changes, cancellationToken);
                
                await turnContext.SendActivityAsync($"Your message was saved");

            }
            
            catch
            {
                await turnContext.SendActivityAsync($"Your message was all fucked up");
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
