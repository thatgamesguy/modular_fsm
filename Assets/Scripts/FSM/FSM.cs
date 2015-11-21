using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModularFSM
{
	/// <summary>
	/// FSM transistion.
	/// </summary>
	public enum FSMTransistion
	{
		None = 0
	}


	/// <summary>
	/// Eash state requires a unique id.
	/// </summary>
	public enum FSMStateID
	{
		None = 0
	}
	
	public class FSM : MonoBehaviour
	{

		private static readonly string SCRIPT_NAME = typeof(FSM).Name;

		private List <FSMState> fsmStates = new List<FSMState> ();

		//private FSMStateID previousStateID;
		public FSMStateID PreviousStateID {
			get {	
				return previousState.ID;
			}
		}

		//private FSMStateID currentStateID;
		public FSMStateID CurrentStateID {
			get {
				return currentState.ID;
			}
		}

		private FSMState previousState;
		public FSMState PreviousState {
			get {
				return previousState;
			}
		}

		private FSMState currentState;
		public FSMState CurrentState {
			get {
				return currentState;
			}
		}
				
		private FSMState defaultState;

				

		void OnDisable ()
		{
			if (currentState != null)
				currentState.Exit ();
		}

		public void AddState (FSMState state)
		{

			if (state == null) {
				Debug.LogWarning (SCRIPT_NAME + ": null state not allowed");
				return;
			}

			// First State inserted is also the Initial state
			//   the state the machine is in when the simulation begins
			if (fsmStates.Count == 0) {
				fsmStates.Add (state);
				currentState = state;
				defaultState = state;
				return;
			}

			// Add the state to the List if it´s not inside it
			foreach (FSMState tmpState in fsmStates) {
				if (state.ID == tmpState.ID) {
					Debug.LogError (SCRIPT_NAME + ": Trying to add a state that was already inside the list, " + state.ID);
					return;
				}
			}

			//If no state in the current then add the state to the list
			fsmStates.Add (state);
		}

		public void DeleteState (FSMStateID stateID)
		{
		
			if (stateID == FSMStateID.None) {
				Debug.LogWarning (SCRIPT_NAME + ": no state id");
				return;
			}

			
			// Search the List and delete the state if it´s inside it
			foreach (FSMState state in fsmStates) {
				if (state.ID == stateID) {
					fsmStates.Remove (state);
					return;
				}
			}
			
			Debug.LogError (SCRIPT_NAME + ": The state passed was not on the list");

		}

		public void PerformTransition (FSMTransistion trans, Transform extraData)
		{
			// Check for NullTransition before changing the current state
			if (trans == FSMTransistion.None) {
				Debug.LogError (SCRIPT_NAME + ": Null transition is not allowed");
				return;
			}
			
			// Check if the currentState has the transition passed as argument
			FSMStateID id = currentState.GetOutputState (trans);
			if (id == FSMStateID.None) {
				Debug.LogError (SCRIPT_NAME + ": Current State does not have a target state for this transition");
				return;
			}

			
			// Update the currentStateID and currentState		
			//currentStateID = id;
			foreach (FSMState state in fsmStates) {
				if (state.ID == id) {
					// Store previous state and call exit method.
					previousState = currentState;
					previousState.Exit ();

					// Update current state and call enter method.
					currentState = state;
					currentState.Enter ();

					break;
				}
			}
		}
		
		public void ClearStates ()
		{
			fsmStates.Clear ();
		}
				
		public void Reset ()
		{
			currentState = defaultState;
			if (currentState != null) {
				currentState.Enter ();
			}
		}


	}
}
