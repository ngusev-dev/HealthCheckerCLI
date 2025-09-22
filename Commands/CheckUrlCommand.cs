using HealthCheckerCLI.Abstracts;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace HealthCheckerCLI.Commands
{
    public class CheckUrlCommand : BaseCommand
    {
        public override void InitializeCommand(RootCommand rootCommand)
        {
            Command checkedLinkCommand = new("checked", "Checked response stutus code of website");

            Argument<string> linkArgument = new("link")
            {
                Description = "The link for which need to get response status code",
                Arity = ArgumentArity.ExactlyOne,
            };

            checkedLinkCommand.AddArgument(linkArgument);

            checkedLinkCommand.Handler = CommandHandler.Create<string>(async (link) =>
            {
                HttpClient client = new();

                var responseMessage = await client.GetAsync(new Uri(link)).ConfigureAwait(false);

                Console.WriteLine($"Status code for '{link}' = [{(int)responseMessage.StatusCode}] {responseMessage.StatusCode}");
            });

            rootCommand.AddCommand(checkedLinkCommand);
        }
    }
}
