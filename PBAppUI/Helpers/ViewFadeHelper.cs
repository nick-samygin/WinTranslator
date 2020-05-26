using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PasswordBoss.Helpers
{
	public class ViewFadeHelper
	{
		private readonly Action showFader = () => { };
		private readonly Action hideFader = () => { };
		
		public ViewFadeHelper(Action showFader, Action hideFader)
		{
			if (showFader != null)
				this.showFader = showFader;

			if (hideFader != null)
				this.hideFader = hideFader;
		}

		public void FadeAction(Action action)
		{
			showFader();
			try
			{
				PerformAction(action);
			}
			catch
			{
				throw;
			}
			finally
			{
				hideFader();
			}
		}

		protected virtual void PerformAction(Action action)
		{
			action();
		}
	}

	// guarantees that app will be faded for certain time, so user will be able to read info message even if it very fast
	public class ViewFadeTimerHelper : ViewFadeHelper
	{
		private readonly int minimumFadeTimeMilliseconds = 0;

		// never raise it, just autopulse on timeout
		AutoResetEvent autoEvent = new AutoResetEvent(false);


		public ViewFadeTimerHelper(Action showFader, Action hideFader, int minimumFadeTimeMilliseconds) 
			: base(showFader, hideFader)
		{
			if (minimumFadeTimeMilliseconds < 0)
				throw new ArgumentException("minimumFadeTimeMilliseconds negative");

			this.minimumFadeTimeMilliseconds = minimumFadeTimeMilliseconds;
		}

		protected override void PerformAction(Action action)
		{
			var actions = new Action[]
			{
				() => autoEvent.WaitOne(minimumFadeTimeMilliseconds, false),
				() => base.PerformAction(action)
            };

			Task.WaitAll(actions.Select(a => Task.Factory.StartNew(a)).ToArray());
		}
	}
}
