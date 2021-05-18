using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace Extraction.UI
{
	public partial class ChatPanel : Panel
	{
		static ChatPanel Current;

		public Panel Canvas { get; protected set; }
		public TextEntry Input { get; protected set; }

		public ChatPanel()
		{
			Current = this;

			StyleSheet.Load( "/UI/Game/ChatPanel.scss" );

			Canvas = Add.Panel( "chat_canvas" );

			Input = Add.TextEntry( "" );
			Input.AddEvent( "onsubmit", () => Submit() );
			Input.AddEvent( "onblur", () => Blur() );
			Input.AcceptsFocus = true;
			Input.AllowEmojiReplace = true;

			Chat.OnOpenChat += Open;
		}

		void Blur()
		{
			Input.Text = "";
			Close();
		}


		void Open()
		{
			AddClass( "open" );
			Input.Focus();
		}

		void Close()
		{
			RemoveClass( "open" );
			Input.Blur();
		}

		void Submit()
		{
			Close();

			string msg = Input.Text.Trim();
			Input.Text = "";

			if ( string.IsNullOrWhiteSpace( msg ) )
				return;

			Say( msg );
		}

		public void AddEntry( string name, string message, string avatar )
		{
			var entry = Canvas.AddChild<ChatEntry>();
			//e.SetFirstSibling();
			entry.Message.Text = message;
			entry.NameLabel.Text = name;
			entry.Avatar.SetTexture( avatar );

			entry.SetClass( "noname", string.IsNullOrEmpty( name ) );
			entry.SetClass( "noavatar", string.IsNullOrEmpty( avatar ) );
		}


		[ClientCmd( "chat_add", CanBeCalledFromServer = true )]
		public static void AddChatEntry( string name, string message, string avatar = null )
		{
			Current?.AddEntry( name, message, avatar );

			// Only log clientside if we're not the listen server host
			if ( !Global.IsListenServer )
			{
				Log.Info( $"{name}: {message}" ); 
			}
		}

		[ClientCmd( "chat_addinfo", CanBeCalledFromServer = true )]
		public static void AddInformation( string message, string avatar = null )
		{
			Current?.AddEntry( null, message, avatar );
		}

		[ServerCmd( "say" )]
		public static void Say( string message )
		{
			Assert.NotNull( ConsoleSystem.Caller );

			// todo - reject more stuff
			if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
				return;

			Log.Info( $"{ConsoleSystem.Caller}: {message}" );
			AddChatEntry( To.Everyone, ConsoleSystem.Caller.Name, message, $"avatar:{ConsoleSystem.Caller.SteamId}" );
		}

	}
}

namespace Sandbox.Hooks
{
	public static partial class Chat
	{
		public static event Action OnOpenChat;

		[ClientCmd( "openchat" )]
		internal static void MessageMode()
		{
			OnOpenChat?.Invoke();
		}

	}
}
