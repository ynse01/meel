using System;

namespace Meel
{
    public sealed class ConnectionContext : Metadata, IDisposable
    {
        public ConnectionContext(long id)
        {
            Id = id;
            State = SessionState.NotAuthenticated;
        }

        public long Id { get; private set; }
       
        public string Username { get; set; }

        public SessionState State { get; set; }

        public Mailbox SelectedMailbox { get; private set; }

        public bool ExpectLiteral { get; set; }

        public void SetSelectedMailbox(Mailbox newSelected)
        {
            if (newSelected == null)
            {
                State = SessionState.Authenticated;
            }
            SelectedMailbox = newSelected;
        }

        public void Dispose()
        {
            Foreach((key, value) =>
            {
                if (value is IDisposable)
                {
                    ((IDisposable)value).Dispose();
                }
            });
        }
    }
}
