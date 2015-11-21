using UnityEngine;
using System.Collections;

namespace ModularFSM
{
	public abstract class FSMAction
	{
		public abstract void PerformAction (Transform player);
		protected abstract bool OkToAct ();
		public abstract void Enter ();
		public abstract void Exit ();

	}
}
