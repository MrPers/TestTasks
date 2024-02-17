using System.Data;

namespace Calculator
{
    class Context : State
    {
        private State state;
        public int x, y;
        public string op;

        public Context() => clear();

        public void clear()
        {
            state = new StateFirst(this);
            x = 0;
            y = 0;
            op = "";
        }

        public void ChangeState(State newState) => state = newState;

        public void digit(string key) => state.digit(key);

        public void equal() => state.equal();

        public void opers(string key) => state.opers(key);

        public string scren() => $"X: {x} Y: {y}  Op: {op} {state.ToString().Split(".")[1]} " +
            $"\n Screen: {state.scren()}";
    }
}
