namespace Calculator
{
    internal class StateResult : State
    {
        private Context context;

        public StateResult(Context context)
        {
            this.context = context;
        }

        public void digit(string key)
        {
            context.ChangeState(new StateFirst(context));
            context.digit(key);
        }

        public void equal() => calculate();

        public void opers(string key)
        {
            calculate();
            context.ChangeState(new StateOpers(context));
            context.opers(key);
        }

        public string scren() => $"= {context.x}";

        void calculate()
        {
            int x = context.x;
            int y = context.y;

            switch (context.op)
            {
                case "+": x += y; break;
                case "-": x -= y; break;
                case "*": x *= y; break;
                case "/": if (y != 0) x /= y; break;
            }

            context.x = x;
        }
    }
}