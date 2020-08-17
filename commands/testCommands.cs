using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;


namespace discBot.commands{


    public class testCommands : BaseCommandModule{
        [Command("ping")]
        [Description("Returns PAWNG")]
        public async Task Ping(CommandContext ctx){
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);

        }

        [Command("add")]
        [Description("adds two numbres")]
        public async Task Add(CommandContext ctx, [Description("First Number")] int numberOne, 
        [Description("Second Number")] int NumberTwo){
            await ctx.Channel.SendMessageAsync((numberOne + NumberTwo).ToString()).ConfigureAwait(false);

        }
        [Command("responsemessage")]
        public async Task responsemessage(CommandContext ctx){
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            
            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

          [Command("responseemoji")]
        public async Task ResponseEmoji(CommandContext ctx){
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            
            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("poll")]
        public async Task Poll(CommandContext ctx, string description, TimeSpan duration, params DiscordEmoji[] Emojioptions){
            var interactivity = ctx.Client.GetInteractivity();
            var options = Emojioptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder {
                Title = description,
                Description = string.Join(" ", options)
            };
            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);
            foreach(var option in Emojioptions){
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var Result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);
            var distinctResult = Result.Distinct();


            var Results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");

            await ctx.Channel.SendMessageAsync(String.Join("\n", Results)).ConfigureAwait(false);
        }
    }
}