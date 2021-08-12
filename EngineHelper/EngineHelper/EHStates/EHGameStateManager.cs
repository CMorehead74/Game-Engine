using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EngineHelper.EHManagers
{
    public interface EHILoadingScreen
    {
        void Update();
        void Draw(float loadPercentage);
    }

    public interface EHIGameState
    {
        float LoadPercentage { get; set; }

        bool IsLoading { get; }

        void StartLoad();

        void LoadContent();

        void OnEnter(string perviousState);

        void OnExit(string nextState);

        void Input();

        void Update();

        void DrawRenderTargets();

        void Draw();

        void OnOverride(string nextState);

        void OnResume(string previousState);

        string Name { get; set; }
    }

    public sealed class EHGameStateManager
    {
        public string _invalidState = "Invalid State";

        private enum GameStateCmdType
        {
            PUSH, CHANGE, POP, POPALL, FIRST
        }

        private struct GameStateCmd
        {
            public GameStateCmdType cmd;
            public string stateName;
            public float delay;
        }

        private List<EHIGameState> stateStack = new List<EHIGameState>();
        private Dictionary<string, EHIGameState> registeredGameStates = new Dictionary<string, EHIGameState>();
        private Queue<GameStateCmd> cmdQueue = new Queue<GameStateCmd>();

        private float currentTime = 0.0f;

        //  For loading
        private ContentManager content;
        private EHILoadingScreen loadingScreen;
        private float loadPercentage;
        private int currentlyLoadingState;
        static bool loadingState = false;

        private List<EHIGameState> statesToLoad = new List<EHIGameState>();

        public void Initialize(ContentManager content, EHILoadingScreen loadScreen)
        {
            this.content = content;
            this.loadingScreen = loadScreen;
        }

        public void Shutdown()
        {
            for (int i = stateStack.Count - 1; i >= 0; --i)
            {
                stateStack[i].OnExit(_invalidState);
            }
            stateStack.Clear();
        }

        private void SendCmd(GameStateCmdType cmdType, string stateName, float delay)
        {
            stateName = stateName.ToLower();

            GameStateCmd gSCmd;
            gSCmd.cmd = cmdType;
            gSCmd.stateName = stateName;
            gSCmd.delay = delay;

            cmdQueue.Enqueue(gSCmd);
        }

        // Register a state object and associate it with a string identifier
        public bool RegisterState(string stateName, EHIGameState state)
        {
            stateName = stateName.ToLower();

            //Check to see if state was already registered
            EHIGameState value;
            if (registeredGameStates.TryGetValue(stateName, out value))
            {
                //If the string ID already exists in the state map, return an error
                return false;
            }

            //Assign the object pointer to the ID location in the map
            registeredGameStates[stateName] = state;
            state.Name = stateName;

            //If this is the first state registered, put it on the stack as the first state
            if (registeredGameStates.Count == 1)
            {
                SendCmd(GameStateCmdType.PUSH, stateName, 0);
            }
            return true;
        }

        private void LoadStates()
        {
            if (!loadingState)
            {
                statesToLoad[currentlyLoadingState].StartLoad();
                loadingState = true;
            }
            else
            {
                loadPercentage = statesToLoad[currentlyLoadingState].LoadPercentage;

                if (loadPercentage == 100.0f)
                {
                    loadingState = false;
                    stateStack.Add(statesToLoad[currentlyLoadingState]);

                    //Once state is loaded
                    string previousStateName = CurrentStateName;

                    if (stateStack.Count == 1)
                        previousStateName = _invalidState;

                    CurrentState.OnEnter(previousStateName);
                    currentlyLoadingState++;

                    if (currentlyLoadingState >= statesToLoad.Count)
                    {
                        //All states loaded
                        statesToLoad.Clear();
                    }
                }
            }
        }

        //Checks if the current state will change on the next update cycle
        public bool IsStateChangePending
        {
            get
            {
                return (cmdQueue.Count == 0) ? false : true;
            }
        }

        //Returns the current state
        public string CurrentStateName
        {
            get
            {
                if (stateStack.Count == 0)
                    return _invalidState;
                return stateStack[stateStack.Count - 1].Name;
            }
        }

        // Get the state object based on the string ID
        public EHIGameState GetState(string stateName)
        {
            stateName = stateName.ToLower();

            if (registeredGameStates.ContainsKey(stateName))
                return registeredGameStates[stateName];
            else
                throw new KeyNotFoundException("State not found: " + stateName);
        }

        //Get the state object on top of the current state stack
        public EHIGameState CurrentState
        {
            get
            {
                int index = stateStack.Count - 1;

                if (index > -1 && index < stateStack.Count)
                    return stateStack[index];
                else
                    return null;
            }
        }

        //Returns the size of the state stack
        public int StateStackSize
        {
            get { return stateStack.Count; }
        }

        public void ChangeState(string stateName, float delay, bool shouldFlush)
        {
            //Clear the queue
            if (shouldFlush)
            {
                cmdQueue.Clear();
            }

            //Push a "change state" command onto the queue
            SendCmd(GameStateCmdType.CHANGE, stateName, currentTime + delay);
        }

        public void ChangeState(string stateName)
        {
            ChangeState(stateName, 0.0f, false);
        }

        public void PushState(string stateName, float delay, bool shouldFlush)
        {
            // Clear the queue
            if (shouldFlush)
            {
                cmdQueue.Clear();
            }

            //Push a "push state" command onto the queue
            SendCmd(GameStateCmdType.PUSH, stateName, currentTime + delay);
        }

        //Pushes a new state on top of the existing one on the next update cycle.
        public void PushState(string stateName)
        {
            PushState(stateName, 0.0f, false);
        }

        //Pops off the current state or states to reveal a stored state underneath.  You may not pop off the last state
        public void PopState()
        {
            PopState(0, 0.0f, false);
        }

        //NumStates to pop is EXTRA states to pop. It will pop off the current state REGARDLESS
        public void PopState(int numExtraStatesToPop, float delay, bool shouldFlush)
        {
            //Clear the queue
            if (shouldFlush)
            {
                cmdQueue.Clear();
            }

            //Push a "pop state" command onto the queue
            SendCmd(GameStateCmdType.POP, "", currentTime + delay);

            //Pop more states if needed
            while (numExtraStatesToPop > 0)
            {
                //Only the first state command can have a non-zero value for delay.
                SendCmd(GameStateCmdType.POP, "", 0.0f);
                numExtraStatesToPop--;
            }
        }

        //Pops all but the last state.
        public void PopAllStates()
        {
            PopAllStates(0.0f, false);
        }

        public void PopAllStates(float delay, bool shouldFlush)
        {
            //Clear the queue
            if (shouldFlush)
            {
                cmdQueue.Clear();
            }

            //Push a "pop all states" command onto the queue
            SendCmd(GameStateCmdType.POPALL, "", currentTime + delay);
        }

        public void ProcessCmdQueue()
        {
            // Empty our command queue, consisting of commands to either
            // push new states onto the stack, pop states off the stack, or
            // to switch the states on the top of the stack.  In each case
            // we transmit the new state to the old one, and vice-versa.
            GameStateCmd cmd;
            while ((cmdQueue.Count > 0) && (cmdQueue.Peek().delay <= currentTime))
            {
                string previousStateName = CurrentStateName;
                cmd = cmdQueue.Dequeue();

                switch (cmd.cmd)
                {
                    case GameStateCmdType.PUSH:
                        {
                            if (previousStateName != cmd.stateName)
                            {
                                if (CurrentState != null)
                                    CurrentState.OnOverride(cmd.stateName);

                                AddStateToLoad(GetState(cmd.stateName));
                            }
                        }
                        break;

                    case GameStateCmdType.POP:
                        {
                            if (stateStack.Count > 1)
                            {
                                CurrentState.OnExit(stateStack[stateStack.Count - 2].Name);
                                stateStack.RemoveAt(stateStack.Count - 1);
                                CurrentState.OnResume(previousStateName);
                            }
                        }
                        break;

                    case GameStateCmdType.POPALL:
                        {
                            while (stateStack.Count > 1)
                            {
                                CurrentState.OnExit(stateStack[stateStack.Count - 2].Name);
                                stateStack.RemoveAt(stateStack.Count - 1);
                                CurrentState.OnResume(previousStateName);
                            }
                        }
                        break;

                    case GameStateCmdType.CHANGE:
                        {
                            //This prevented changing from/to the same state (but sometimes you JUST WANT TO!)
                            CurrentState.OnExit(cmd.stateName);
                            stateStack.RemoveAt(stateStack.Count - 1);
                            AddStateToLoad(GetState(cmd.stateName));
                        }
                        break;
                }
            }

            if (statesToLoad.Count > 0)
            {
                currentlyLoadingState = 0;
            }
        }

        //Updates the state machine internal mechanism.  This function is called once by the main update loop and should not be called by anyone else.
        public void Update(float dt)
        {
            if (statesToLoad.Count == 0)
            {
                ProcessCmdQueue();

                //Update the total run time
                currentTime += dt;

                //Check input only for the top
                if (CurrentState != null)
                    CurrentState.Input();

                //After all state transitions are finished, do the current state stack updates
                //NOTE: Updates from TOP to BOTTOM
                for (int i = stateStack.Count - 1; i >= 0; i--)
                    stateStack[i].Update();
            }
            else
            {
                if (loadingScreen != null)
                    loadingScreen.Update();

                LoadStates();
            }
        }

        private void AddStateToLoad(EHIGameState stateToLoad)
        {
            statesToLoad.Add(stateToLoad);
        }

        public void Draw()
        {
            if (statesToLoad.Count == 0)
            {
                for (int i = 0; i < stateStack.Count; i++)
                    stateStack[i].DrawRenderTargets();

                //After all state transitions are finished, do the current state stack updates
                //NOTE: Draws from BOTTOM to TOP
                for (int i = 0; i < stateStack.Count; i++)
                    stateStack[i].Draw();
            }
            else
            {
                if (loadingScreen != null)
                    loadingScreen.Draw(loadPercentage);
            }
        }
    }
}
