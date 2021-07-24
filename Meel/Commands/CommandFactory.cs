using System.Collections.Generic;

namespace Meel.Commands
{
    public class CommandFactory
    {
        private static readonly List<IImapCommand> commands =
            new List<IImapCommand>();
        
        public CommandFactory(IMailStation station)
        {
            if (commands == null)
            {
                var bad = new BadCommand();
                // This list needs to be in the same order as the ImapCommands enum.
                commands.Add(bad);
                commands.Add(new NoopCommand());
                commands.Add(new CapabilityCommand());
                commands.Add(new LoginCommand());
                commands.Add(new LogoutCommand());
                commands.Add(bad); // Authenticate (not implemented)
                commands.Add(bad); // StartTLS (Handled by ServerSession class)
                commands.Add(new SelectCommand(station));
                commands.Add(new SelectCommand(station));  // Examine
                commands.Add(new CreateCommand(station));
                commands.Add(new DeleteCommand(station));
                commands.Add(new RenameCommand(station));
                commands.Add(new SubscribeCommand(station));
                commands.Add(new UnsubscribeCommand(station));
                commands.Add(new ListCommand(station));
                commands.Add(new ListCommand(station)); // LSub
                commands.Add(new StatusCommand(station));
                commands.Add(new AppendCommand(station));
                commands.Add(new NoopCommand()); // Check
                commands.Add(new CloseCommand(station));
                commands.Add(new ExpungeCommand(station));
                commands.Add(new SearchCommand(station));
                commands.Add(new FetchCommand(station));
            }
        }

        public IImapCommand GetCommand(ImapCommands request)
        {
            IImapCommand imapCommand = commands[(int)request];
            return imapCommand;
        }
    }
}
